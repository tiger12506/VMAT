using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VMAT.Services;
using Vestris.VMWareLib;

namespace VMAT.Models
{
	public class VirtualMachineRepository : IVirtualMachineRepository
	{
		private DataEntities dataDB;

		public VirtualMachineRepository() : this(new DataEntities()) { }

		public VirtualMachineRepository(DataEntities db)
		{
			dataDB = db;
		}

        public void Test()
        {
            Console.Write("HI");
        }

        public void CreateSnapshot()
        {
            VMWareVirtualHost virtualHost = new VMWareVirtualHost();

            // connect to a local VMWare Workstation virtual host
            virtualHost.ConnectToVMWareWorkstation();
            // open an existing virtual machine
            VMWareVirtualMachine virtualMachine = virtualHost.Open(@"Z:\1234\gapdev1234b355\gapdev1234b355.vmx");
            //// power on this virtual machine
            //virtualMachine.PowerOn();
            //// wait for VMWare Tools
            //virtualMachine.WaitForToolsInGuest();
            //// login to the virtual machine
            //virtualMachine.LoginInGuest("Administrator", "password");
            //// run notepad
            //virtualMachine.RunProgramInGuest("notepad.exe", string.Empty);
            // create a new snapshot
            string name = "New Snapshot";
            // take a snapshot at the current state
            virtualMachine.Snapshots.CreateSnapshot(name, "test snapshot");
            // power off
            //virtualMachine.PowerOff();
            // find the newly created snapshot
            //VMWareSnapshot snapshot = virtualMachine.Snapshots.GetNamedSnapshot(name);
            // revert to the new snapshot
            //snapshot.RevertToSnapshot();
            // delete snapshot
            //snapshot.RemoveSnapshot();
        }

		public void CreateProject(Project proj)
		{
			throw new NotImplementedException();
		}

		public Project GetProject(string projectName)
		{
			var project = new Project(projectName);

			foreach (var vm in GetAllRegisteredVirtualMachines())
			{
				if (vm.GetProjectName() == projectName)
					project.AddVirtualMachine(vm);
			}

			foreach (var vm in dataDB.VirtualMachines)
			{
				if (vm.GetType() != typeof(RegisteredVirtualMachine) &&
					vm.GetType() != typeof(PendingArchiveVirtualMachine))
				{
					if (vm.GetProjectName() == projectName)
						project.AddVirtualMachine(vm);
				}
			}

			return project;
		}

		public IEnumerable<Project> GetAllProjects()
		{
			var projects = new List<Project>();

			foreach (var vm in GetAllRegisteredVirtualMachines())
			{
				string projectName = vm.GetProjectName();
				bool found = false;

				foreach (Project proj in projects)
				{
					if (proj.ProjectName == projectName)
					{
						proj.AddVirtualMachine(vm);
						found = true;
						break;
					}
				}

				if (!found)
				{
					var newProject = new Project(projectName, AppConfiguration.GetVMHostName(),
						new List<VirtualMachine> { vm });
					projects.Add(newProject);
				}
			}

			foreach (var vm in dataDB.VirtualMachines)
			{
				if (vm.GetType() != typeof(RegisteredVirtualMachine) && 
					vm.GetType() != typeof(PendingArchiveVirtualMachine))
				{
					string projectName = vm.GetProjectName();
					bool found = false;

					foreach (Project proj in projects)
					{
						if (proj.ProjectName == projectName)
						{
							proj.AddVirtualMachine(vm);
							found = true;
							break;
						}
					}

					if (!found)
					{
						var newProject = new Project(projectName, AppConfiguration.GetVMHostName(),
							new List<VirtualMachine> { vm });
						projects.Add(newProject);
					}
				}
			}

			return projects;
		}

		public IEnumerable<VirtualMachine> GetAllVirtualMachines()
		{
			return dataDB.VirtualMachines as IEnumerable<VirtualMachine>;
		}

        public IEnumerable<PendingVirtualMachine> GetAllPendingVirtualMachines()
        {
            return dataDB.VirtualMachines.OfType<PendingVirtualMachine>();
        }

		public IEnumerable<RegisteredVirtualMachine> GetAllRegisteredVirtualMachines()
		{
			var imagePathNames = RegisteredVirtualMachineService.GetRegisteredVMImagePaths();
			var vmList = new List<RegisteredVirtualMachine>();

			foreach (var path in imagePathNames)
			{
				RegisteredVirtualMachine vm;

				try
				{
					vm = dataDB.VirtualMachines.OfType<PendingArchiveVirtualMachine>().
						Single(d => d.ImagePathName == path);
				}
				catch (InvalidOperationException)
				{
					try
					{
						vm = dataDB.VirtualMachines.OfType<RegisteredVirtualMachine>().
							Single(d => d.ImagePathName == path);
					}
					catch (ArgumentNullException)
					{
						vm = new RegisteredVirtualMachine(path);
						dataDB.VirtualMachines.Add(vm);
					}
					catch (InvalidOperationException)
					{
						vm = new RegisteredVirtualMachine(path);
						dataDB.VirtualMachines.Add(vm);
					}
				}

				var service = new RegisteredVirtualMachineService(path);

				if (service.GetStatus() == VMStatus.Running)
				{
					vm.Hostname = AppConfiguration.GetVMHostName();
					vm.IP = service.GetIP();
				}

				vmList.Add(vm);
			}

			dataDB.SaveChanges();
			return vmList;
		}

		public VirtualMachine GetVirtualMachine(string imagePath)
		{
			return dataDB.VirtualMachines.Single(v => v.ImagePathName == imagePath);
		}

		public void DeleteVirtualMachine(string imagePath)
		{
			VirtualMachine vm = dataDB.VirtualMachines.Single(v => v.ImagePathName == imagePath);
			dataDB.VirtualMachines.Remove(vm);
			dataDB.SaveChanges();
		}

		public void CreateRegisteredVirtualMachine(RegisteredVirtualMachine vm)
		{
			dataDB.VirtualMachines.Add(vm);
			dataDB.SaveChanges();
		}

		public RegisteredVirtualMachine GetRegisteredVirtualMachine(string imagePath)
		{
			return dataDB.VirtualMachines.Single(v => v.ImagePathName == imagePath)
				as RegisteredVirtualMachine;
		}

		public void CreatePendingArchiveVirtualMachine(PendingArchiveVirtualMachine vm)
		{
			dataDB.VirtualMachines.Add(vm);
			dataDB.SaveChanges();
		}

		public void ScheduleArchiveVirtualMachine(string imagePath)
		{
			var vm = dataDB.VirtualMachines.Single(v => v.ImagePathName == imagePath) 
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

		public void ScheduleArchiveProject(string projectName)
		{
			IEnumerable<RegisteredVirtualMachine> vms = GetAllRegisteredVirtualMachines();

			foreach (var vm in vms)
			{
				if (vm.GetProjectName() == projectName)
					ScheduleArchiveVirtualMachine(vm.ImagePathName);
			}
		}

		public void UndoScheduleArchiveVirtualMachine(string imagePath)
		{
			var archiveVm = dataDB.VirtualMachines.Single(v => v.ImagePathName == imagePath)
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

		public PendingArchiveVirtualMachine GetPendingArchiveVirtualMachine(string imagePath)
		{
			return dataDB.VirtualMachines.Single(v => v.ImagePathName == imagePath)
				as PendingArchiveVirtualMachine;
		}

		public void CreateArchivedVirtualMachine(ArchivedVirtualMachine vm)
		{
			dataDB.VirtualMachines.Add(vm);
			dataDB.SaveChanges();
		}

		public ArchivedVirtualMachine GetArchivedVirtualMachine(string imagePath)
		{
			return dataDB.VirtualMachines.Single(v => v.ImagePathName == imagePath)
				as ArchivedVirtualMachine;
		}

		public void CreatePendingVirtualMachine(PendingVirtualMachine vm)
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
                foreach (PendingVirtualMachine pvm in GetAllPendingVirtualMachines())
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

		public PendingVirtualMachine GetPendingVirtualMachine(string imagePath)
		{
			return dataDB.VirtualMachines.Single(v => v.ImagePathName == imagePath)
				as PendingVirtualMachine;
		}

		public VMStatus ToggleVMStatus(string image)
		{
			RegisteredVirtualMachine vm = dataDB.VirtualMachines.
				OfType<RegisteredVirtualMachine>().Single(d => d.ImagePathName == image);
			var service = new RegisteredVirtualMachineService(image);

			if (service.IsRunning())
				PowerOff(vm, service);
			else
				PowerOn(vm, service);

			return service.GetStatus();
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

		public void ReserveIP(string imagePathName, string ip)
		{
			GlobalReservedIP.ReserveIP(imagePathName, ip);
		}

		public void UnreserveIP(string ip)
		{
			GlobalReservedIP.UnreserveIP(ip);
		}

		public void PowerOn(RegisteredVirtualMachine vm, RegisteredVirtualMachineService service)
		{
			service.PowerOn();
			vm.LastStarted = DateTime.Now;
			dataDB.SaveChanges();
		}

		public void PowerOff(RegisteredVirtualMachine vm, RegisteredVirtualMachineService service)
		{
			service.PowerOff();
			vm.LastStopped = DateTime.Now;
			dataDB.SaveChanges();
		}
	}
}
