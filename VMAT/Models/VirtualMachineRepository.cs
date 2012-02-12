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

            /*svar projects = new List<Project>();

            foreach (var vm in GetAllRegisteredVirtualMachines())
            {
                string projectName = vm.ProjectName;
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
                    string projectName = vm.ProjectName;
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

            return projects;*/
        }

        public IEnumerable<Project> GetAllProjectsWithVirtualMachines()
        {
            return dataDB.Projects.Include("VirtualMachines").ToList();
        }

        public IEnumerable<VirtualMachine> GetAllVirtualMachines()
        {
            return dataDB.VirtualMachines as IEnumerable<VirtualMachine>;
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
