using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VMAT.Services;

namespace VMAT.Models
{
	public class VirtualMachineRepository : IVirtualMachineRepository
	{
		private DataEntities dataDB;

		public VirtualMachineRepository() : this(new DataEntities()) { }

		public VirtualMachineRepository(DataEntities db)
		{
			dataDB = db;

			if (dataDB.Projects == null || dataDB.Projects.Count() <= 0 ||
				dataDB.VirtualMachines == null || dataDB.VirtualMachines.Count() <= 0)
				InitializeDataContext();
		}

		private void InitializeDataContext()
		{
			var registeredImages = RegisteredVirtualMachineService.GetRegisteredVMImagePaths();

			foreach (var image in registeredImages)
			{
				int startIndex = image.IndexOf("] ") + "] ".Length;
				int length = image.IndexOf('/', startIndex) - startIndex;
				string projectName = image.Substring(startIndex, length);
				var service = new RegisteredVirtualMachineService(image);

				if (!dataDB.Projects.Select(p => p.ProjectName).Contains(projectName))
				{
					var project = new Project(projectName);
					dataDB.Projects.Add(project);
					dataDB.SaveChanges();
				}

				startIndex = image.LastIndexOf('/') + 1;
				length = image.LastIndexOf('.') - startIndex;
				string machineName = image.Substring(startIndex, length);

				bool vmExists = true;
				RegisteredVirtualMachine vm = dataDB.VirtualMachines.Single(
					v => v.MachineName == machineName) as RegisteredVirtualMachine;

				if (vm == null)
				{
					vmExists = false;
					vm = new RegisteredVirtualMachine();
				}

				vm.MachineName = machineName;
				vm.ImagePathName = image;
				vm.IsAutoStarted = false;
				vm.Status = service.GetStatus();
				vm.Hostname = service.GetHostname();
				vm.IP = service.GetIP();
				vm.Project = dataDB.Projects.Single(p => p.ProjectName == projectName);

				if (!vmExists)
					dataDB.VirtualMachines.Add(vm);

				dataDB.SaveChanges();
			}
		}

		public void CreateProject(Project proj)
		{
			dataDB.Projects.Add(proj);
		}

		public Project GetProject(int id)
		{
			return dataDB.Projects.Single(p => p.ProjectId == id);
		}

		public Project GetProjectWithVirtualMachines(int id)
		{
			return dataDB.Projects.Include("VirtualMachines").Single(p => p.ProjectId == id);
		}

		public IEnumerable<Project> GetAllProjects()
		{
			return dataDB.Projects.ToList();
		}

		public IEnumerable<Project> GetAllProjectsWithVirtualMachines()
		{
			return dataDB.Projects.Include("VirtualMachines").ToList();
		}

		public IEnumerable<VirtualMachine> GetAllVirtualMachines()
		{
			return dataDB.VirtualMachines as IEnumerable<VirtualMachine>;
		}

		private IEnumerable<RegisteredVirtualMachine> GetAllRegisteredVirtualMachines()
		{
			var imagePathNames = RegisteredVirtualMachineService.GetRegisteredVMImagePaths();
			var vmList = new List<RegisteredVirtualMachine>();

			foreach (var image in imagePathNames)
			{
				RegisteredVirtualMachine vm;

				try
				{
					vm = dataDB.VirtualMachines.OfType<PendingArchiveVirtualMachine>().
						Single(d => d.ImagePathName == image);
				}
				catch (InvalidOperationException)
				{
					try
					{
						vm = dataDB.VirtualMachines.OfType<RegisteredVirtualMachine>().
							Single(d => d.ImagePathName == image);
					}
					catch (Exception)
					{
						int startIndex = image.IndexOf("] ") + "] ".Length;
						int length = image.IndexOf('\\', startIndex) - startIndex;
						string projectName = image.Substring(startIndex, length);

						startIndex = image.LastIndexOf('\\');
						length = image.LastIndexOf('.') - startIndex;
						string machineName = image.Substring(startIndex, length);

						vm = new RegisteredVirtualMachine
						{
							MachineName = machineName,
							ImagePathName = image,
							IsAutoStarted = false,
							Project = dataDB.Projects.Single(p => p.ProjectName == image)
						};

						dataDB.VirtualMachines.Add(vm);
					}
				}

				var service = new RegisteredVirtualMachineService(image);

				if (service.GetStatus() == VMStatus.Running)
				{
					vm.IP = service.GetIP();
				}

				vmList.Add(vm);
			}

			dataDB.SaveChanges();
			return vmList;
		}

		public void CreateVirtualMachine(VirtualMachine vm)
		{
			dataDB.VirtualMachines.Add(vm);
		}

		public VirtualMachine GetVirtualMachine(int id)
		{
			return dataDB.VirtualMachines.Single(v => v.VirtualMachineId == id);
		}

		public void DeleteVirtualMachine(int id)
		{
			VirtualMachine vm = dataDB.VirtualMachines.Single(v => v.VirtualMachineId == id);
			dataDB.VirtualMachines.Remove(vm);
			dataDB.SaveChanges();
		}

		public void ScheduleArchiveVirtualMachine(int id)
		{
			var vm = dataDB.VirtualMachines.Single(v => v.VirtualMachineId == id) 
				as RegisteredVirtualMachine;
			var archiveVm = new PendingArchiveVirtualMachine(vm);
			
			try
			{
				dataDB.VirtualMachines.Remove(vm);
				dataDB.VirtualMachines.Add(archiveVm);
			}
			catch (Exception)
			{
				// Do not save changes if error occurs
				return;
			}

			dataDB.SaveChanges();
		}

		public void UndoScheduleArchiveVirtualMachine(int id)
		{
			var archiveVm = dataDB.VirtualMachines.Single(v => v.VirtualMachineId == id)
				as PendingArchiveVirtualMachine;
			var vm = new RegisteredVirtualMachine(archiveVm);

			try
			{
				dataDB.VirtualMachines.Remove(archiveVm);
			}
			catch (Exception)
			{
				// Do not save changes if error occurs
				return;
			}

			dataDB.SaveChanges();

			try
			{
				dataDB.VirtualMachines.Add(vm);
			}
			catch (Exception)
			{
				// Do not save changes if error occurs
				return;
			}

			dataDB.SaveChanges();
		}

		public void ScheduleArchiveProject(int id)
		{
			var project = GetProjectWithVirtualMachines(id);

			foreach (var vm in project.VirtualMachines)
			{
				ScheduleArchiveVirtualMachine(vm.VirtualMachineId);
			}
		}

		public VMStatus ToggleVMStatus(int id)
		{
			RegisteredVirtualMachine vm = dataDB.VirtualMachines.
				OfType<RegisteredVirtualMachine>().Single(d => d.VirtualMachineId == id);
			var service = new RegisteredVirtualMachineService(vm.ImagePathName);

			if (service.IsRunning())
				PowerOff(vm, service);
			else
				PowerOn(vm, service);

			return vm.Status;
		}

		public string GetNextAvailableIP()
		{
			List<string> ipList = new List<string>();
			ipList = dataDB.VirtualMachines.OfType<RegisteredVirtualMachine>().Select(v => v.IP).
				ToList<string>();

			ipList.AddRange(dataDB.VirtualMachines.OfType<PendingVirtualMachine>().
				Select(v => v.IP).ToList<string>());

			ipList.AddRange(dataDB.VirtualMachines.OfType<PendingArchiveVirtualMachine>().
				Select(v => v.IP).ToList<string>());

			bool[] ipUsed = new bool[256];
			ipUsed[0] = true;

			foreach (var ip in ipList)
			{
				try
				{
					string longIP = ip;
					int ipTail = int.Parse(longIP.Substring(longIP.LastIndexOf('.') + 1));
					ipUsed[ipTail] = true;
				}
				catch (NullReferenceException)
				{
					// Ignore if a stored IP address is NULL
				}
			}

			for (int index = 0; index < ipUsed.Length; index++)
			{
				if (!ipUsed[index])
					return "192.168.1." + index.ToString();
			}

			return null;
		}

		public void PowerOn(RegisteredVirtualMachine vm, RegisteredVirtualMachineService service)
		{
			vm.Status = VMStatus.PoweringOn;
			service.PowerOn();
			vm.Status = VMStatus.Running;
			vm.LastStarted = DateTime.Now;
			dataDB.SaveChanges();
		}

		public void PowerOff(RegisteredVirtualMachine vm, RegisteredVirtualMachineService service)
		{
			vm.Status = VMStatus.PoweringOff;
			service.PowerOff();
			vm.Status = VMStatus.Stopped;
			vm.LastStopped = DateTime.Now;
			dataDB.SaveChanges();
		}
	}
}
