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

				VirtualMachine vm;

				try
				{
					vm = dataDB.VirtualMachines.Single(v => v.MachineName == machineName);
				}
				catch (Exception)
				{
					vm = new VirtualMachine();
					dataDB.VirtualMachines.Add(vm);
				}

				vm.MachineName = machineName;
				vm.ImagePathName = image;
				vm.Status = service.GetStatus();
				vm.Hostname = service.GetHostname();
				vm.IP = service.GetIP();
				vm.Project = dataDB.Projects.Single(p => p.ProjectName == projectName);

				/*if (vm.IP == "ip_error" || vm.Hostname == "hostname_error")
				{
					service.PowerOn();
					vm.IP = service.GetIP();
					vm.Hostname = service.GetHostname();
					service.PowerOff();
				}*/

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

		public IEnumerable<VirtualMachine> GetAllPendingVirtualMachines()
		{
			return dataDB.VirtualMachines.Where(v => v.Status == VMStatus.Pending);
		}

		private IEnumerable<VirtualMachine> GetAllRegisteredVirtualMachines()
		{
			var imagePathNames = RegisteredVirtualMachineService.GetRegisteredVMImagePaths();
			var vmList = new List<VirtualMachine>();

			foreach (var image in imagePathNames)
			{
				VirtualMachine vm;

				try
				{
					vm = dataDB.VirtualMachines.Single(d => d.ImagePathName == image);
				}
				catch (Exception)
				{
					int startIndex = image.IndexOf("] ") + "] ".Length;
					int length = image.IndexOf('\\', startIndex) - startIndex;
					string projectName = image.Substring(startIndex, length);

					startIndex = image.LastIndexOf('\\');
					length = image.LastIndexOf('.') - startIndex;
					string machineName = image.Substring(startIndex, length);

					vm = new VirtualMachine
					{
						MachineName = machineName,
						ImagePathName = image,
						IsAutoStarted = false,
						Project = dataDB.Projects.Single(p => p.ProjectName == image)
					};

					dataDB.VirtualMachines.Add(vm);
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
			var vm = dataDB.VirtualMachines.Single(v => v.VirtualMachineId == id);
			vm.IsPendingArchive = true;

			dataDB.SaveChanges();
		}

		public void ScheduleArchiveProject(int id)
		{
			var project = GetProject(id);

			foreach (var vm in project.VirtualMachines)
			{
				ScheduleArchiveVirtualMachine(vm.VirtualMachineId);
			}
		}

		public void UndoScheduleArchiveVirtualMachine(int id)
		{
			var archiveVm = dataDB.VirtualMachines.Single(v => v.VirtualMachineId == id);
			archiveVm.IsPendingArchive = false;

			dataDB.SaveChanges();
		}

		public void CreatePendingVirtualMachine(VirtualMachine vm)
		{
			long freeSpace = 0;
			long fileSize = 0;
			long currentlyPendingSize = 0;
			int fudgeFactor = (int) Math.Pow(2, 22); //windows has 4 kilobyte partitions, so a given file is at least 4 kilobytes on disk, while this just gives us the base size
			//try
			//{
				string[] a = Directory.GetFiles(vm.ImagePathName);
				foreach (string name in a)
				{
					FileInfo info = new FileInfo(name);
					fileSize += info.Length + fudgeFactor;
				}
				foreach (VirtualMachine pvm in GetAllPendingVirtualMachines())
				{
					a = Directory.GetFiles(pvm.ImagePathName);
					foreach (string name in a)
					{
						FileInfo info = new FileInfo(name);
						currentlyPendingSize += info.Length + fudgeFactor;
					}
				}
				DriveInfo dI = new DriveInfo("Z:");
				freeSpace = dI.TotalSize;
				freeSpace = dI.AvailableFreeSpace;
			//}
			//catch (IOException ex)
			//{
			//    //drive not ready or exist
			//    throw new IOException("Drive not ready or does not exist");
			//}
			//catch (ArgumentException ex)
			//{
			//    //drive does not exist
			//    throw new 
			//}
			if (freeSpace > fileSize + currentlyPendingSize)
			{
				dataDB.VirtualMachines.Add(vm);
				dataDB.SaveChanges();
			}
			else
			{
				//error message
				throw new Exception("Not enough free space");
			}
		}

		public VMStatus ToggleVMStatus(int id)
		{
			VirtualMachine vm = dataDB.VirtualMachines.Single(d => d.VirtualMachineId == id);
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
			ipList = dataDB.VirtualMachines.Select(v => v.IP).
				ToList<string>();

			ipList.AddRange(dataDB.VirtualMachines.Select(v => v.IP).ToList<string>());

			ipList.AddRange(dataDB.VirtualMachines.Select(v => v.IP).ToList<string>());

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
				catch (FormatException)
				{
					// Ignore if a stored IP address is invalid
				}
			}

			for (int index = 0; index < ipUsed.Length; index++)
			{
				if (!ipUsed[index])
					return "192.168.1." + index.ToString();
			}

			return null;
		}

		public void PowerOn(VirtualMachine vm, RegisteredVirtualMachineService service)
		{
			vm.Status = VMStatus.PoweringOn;
			dataDB.SaveChanges();
			service.PowerOn();
			vm.Status = VMStatus.Running;
			vm.LastStarted = DateTime.Now;
			dataDB.SaveChanges();
		}

		public void PowerOff(VirtualMachine vm, RegisteredVirtualMachineService service)
		{
			vm.Status = VMStatus.PoweringOff;
			dataDB.SaveChanges();
			service.PowerOff();
			vm.Status = VMStatus.Stopped;
			vm.LastStopped = DateTime.Now;
			dataDB.SaveChanges();
		}
	}
}
