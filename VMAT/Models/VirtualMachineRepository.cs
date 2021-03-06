﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using VMAT.Models.VMware;
using Vestris.VMWareLib;
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
		public VirtualMachineRepository(bool skip)
		{
			
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

				//PowerOn(vm, service);
				vm.MachineName = machineName;
				vm.ImagePathName = image;
				vm.Status = service.GetStatus();

                //TODO reset
			    //vm.Hostname = "test-hostname";
				vm.Hostname = service.GetHostname();
				vm.IP = service.GetIP();
				vm.Project = dataDB.Projects.Single(p => p.ProjectName == projectName);

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

		public ICollection<Project> GetAllProjects()
		{
			foreach (var vm in dataDB.VirtualMachines.Where(v => v.Status != VirtualMachine.ARCHIVED &&
				v.Status != VirtualMachine.PENDING))
			{
				RegisteredVirtualMachineService service = null;

				try
				{
					service = new RegisteredVirtualMachineService(vm.ImagePathName);
				}
				catch (Vestris.VMWareLib.VMWareException)
				{
					dataDB.VirtualMachines.Remove(vm);
					continue;
				}

				vm.Status = service.GetStatus();

				if (vm.IP == null && vm.Status == VirtualMachine.RUNNING)
					vm.IP = service.GetIP();

				if (vm.Hostname == null && vm.Status == VirtualMachine.RUNNING)
					vm.Hostname = service.GetHostname();
			}

			dataDB.SaveChanges();
			return dataDB.Projects.ToList();
		}

		public ICollection<VirtualMachine> GetAllVirtualMachines()
		{
			return dataDB.VirtualMachines as ICollection<VirtualMachine>;
		}

		public ICollection<VirtualMachine> GetAllPendingVirtualMachines()
		{
			return dataDB.VirtualMachines.Where(v => v.Status == VirtualMachine.PENDING).ToList();
		}

		public ICollection<VirtualMachine> GetAllRegisteredVirtualMachines()
		{
			return dataDB.VirtualMachines.Where(v => v.Status != VirtualMachine.ARCHIVED &&
				v.Status != VirtualMachine.PENDING).ToList();
		}

		public void ReinitializeAllRegisteredVirtualMachines()
		{
			var imagePathNames = RegisteredVirtualMachineService.GetRegisteredVMImagePaths();

			foreach (var image in imagePathNames)
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

				if (!dataDB.VirtualMachines.Select(v => v.MachineName).Contains(machineName))
				{
					VirtualMachine vm = new VirtualMachine();
					
					vm.MachineName = machineName;
					vm.ImagePathName = image;
					vm.Status = service.GetStatus();
					vm.Hostname = service.GetHostname();
					vm.IP = service.GetIP();
					vm.Project = dataDB.Projects.Single(p => p.ProjectName == projectName);

					dataDB.VirtualMachines.Add(vm);
					dataDB.SaveChanges();
				}
			}
		}

		public void CreateVirtualMachine(VirtualMachine vm, string projectName)
		{
			try
			{
				vm.Project = dataDB.Projects.Single(p => p.ProjectName == projectName);
			}
			catch (InvalidOperationException)
			{
				var project = new Project(projectName);
				dataDB.Projects.Add(project);
				vm.Project = project;
			}

			dataDB.VirtualMachines.Add(vm);
			dataDB.SaveChanges();
		}

		public VirtualMachine GetVirtualMachine(int id)
		{
			return dataDB.VirtualMachines.Single(v => v.VirtualMachineId == id);
		}

		public void DeleteVirtualMachine(int id)
		{
			VirtualMachine vm = dataDB.VirtualMachines.Single(v => v.VirtualMachineId == id);
			Project project = vm.Project;
			dataDB.VirtualMachines.Remove(vm);

			if (project.VirtualMachines.Count <= 0)
				dataDB.Projects.Remove(project);

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
			int fudgeFactor = (int)Math.Pow(2, 12); //windows has 4 kilobyte partitions, so a given file is at least 4 kilobytes on disk, while this just gives us the base size
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

		public int ToggleVMStatus(int id)
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

			return GetNextAvailableIP(ipList);
		}

		public string GetNextAvailableIP(List<string> ipList )
		{
			//TODO: get the correct HostConfiguration
			ConfigurationRepository configRepo = new ConfigurationRepository();
			HostConfiguration config = configRepo.GetHostConfiguration();

			//remove low and high end IP's from being available
			string minIp = config.MinIP;
			string maxIp = config.MaxIP;

			do
			{
				if (ipList.Contains(minIp))
				{
					string[] bytes = minIp.Split('.');

					for (int i = bytes.Length - 1; i > 0; i--)
					{
						bytes[i] = (int.Parse(bytes[i]) + 1).ToString();

						if (int.Parse(bytes[i]) < 256)
							break;
						else
							bytes[i] = "1";
					}

					minIp = bytes[0];

					foreach (var nibble in bytes.Skip(1))
						minIp += "." + nibble;
				}
				else
					return minIp;
			} while (!minIp.Equals(maxIp));

			return null;
		}

		public void CreateSnapshot(VirtualMachine vm, string name, string description)
		{
            var virtualHost = RegisteredVirtualMachineService.GetVirtualHost();

            //// connect to a local VMWare Workstation virtual host
            //virtualHost.ConnectToVMWareWorkstation();
			// open an existing virtual machine
			IVirtualMachine virtualMachine = virtualHost.Open(vm.ImagePathName);
			// take a snapshot at the current state
			virtualMachine.Snapshots.CreateSnapshot(name, description);
		}

        public void ConsolidateSnapshot(VirtualMachine vm, string name, string description)
        {
            var virtualHost = RegisteredVirtualMachineService.GetVirtualHost();

            //// connect to a local VMWare Workstation virtual host
            //virtualHost.ConnectToVMWareWorkstation();
            // open an existing virtual machine
            IVirtualMachine virtualMachine = virtualHost.Open(vm.ImagePathName);
            foreach(VMWareSnapshot snapshot in virtualMachine.Snapshots)
                snapshot.RemoveSnapshot();
            // take a snapshot at the current state
            virtualMachine.Snapshots.CreateSnapshot(name, description);
        }

		private void PowerOn(VirtualMachine vm, RegisteredVirtualMachineService service)
		{
			vm.Status = VirtualMachine.POWERINGON;
			dataDB.SaveChanges();
			service.PowerOn();
			vm.Status = VirtualMachine.RUNNING;
			vm.LastStarted = DateTime.Now;
			dataDB.SaveChanges();
		}

		private void PowerOff(VirtualMachine vm, RegisteredVirtualMachineService service)
		{
			vm.Status = VirtualMachine.POWERINGOFF;
			dataDB.SaveChanges();
			service.PowerOff();
			vm.Status = VirtualMachine.STOPPED;
			vm.LastStopped = DateTime.Now;
			dataDB.SaveChanges();
		}
	}
}
