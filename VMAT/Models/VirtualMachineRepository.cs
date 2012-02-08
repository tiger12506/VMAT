using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VMAT.Services;

namespace VMAT.Models
{
    public class VirtualMachineRepository : IVirtualMachineRepository
    {
        private DataEntities dataDB = new DataEntities();

        int[] reservedIPs;

        public void CreateProject(Project proj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project> GetProjects()
        {
            var projects = new List<Project>();

            foreach (var vm in GetRegisteredVMs())
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
                if (vm.GetType() != typeof(RegisteredVirtualMachine))
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

        public IEnumerable<VirtualMachine> GetVirtualMachines()
        {
            return dataDB.VirtualMachines as IEnumerable<VirtualMachine>;
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
            dataDB.VirtualMachines.Add(vm);
            dataDB.SaveChanges();
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

        public IEnumerable<VirtualMachine> GetRegisteredVMs()
        {
            var imagePathNames = RegisteredVirtualMachineService.GetRegisteredVMImagePaths();
            var vmList = new List<VirtualMachine>();

            foreach (var path in imagePathNames)
            {
                RegisteredVirtualMachine vm;

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

                var service = new RegisteredVirtualMachineService(path);

                if (service.GetStatus() == VMStatus.Running)
                {
                    vm.Hostname = AppConfiguration.GetVMHostName();
                    vm.IP = service.GetIP();
                }

                dataDB.SaveChanges();
                vmList.Add(vm);
            }

            return vmList;
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

            ipList.AddRange(GlobalReservedIP.GetReservedIPs().Values);

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

        public static IEnumerable<string> GetBaseImageFiles()
        {
            List<string> filePaths = new List<string>(Directory.GetFiles(
                AppConfiguration.GetWebserverVmPath(), "*.vmx", SearchOption.AllDirectories));
            return filePaths.Select(foo => RegisteredVirtualMachineService.ConvertPathToDatasource(foo));
        }
    }
}
