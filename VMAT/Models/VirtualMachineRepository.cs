using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VMAT.Models.VMware;
using VMAT.Services;

namespace VMAT.Models
{
    public class VirtualMachineRepository : IVirtualMachineRepository
    {
        private DataEntities dataDB = new DataEntities();

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
                    var newProject = new Project(projectName, vm.Hostname,
                        new List<VirtualMachine> { vm });
                    projects.Add(newProject);
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

        public int GetNextAvailbaleIP()
        {
            List<string> ipList = new List<string>();
            ipList = dataDB.VirtualMachines.OfType<RegisteredVirtualMachine>().Select(v => v.IP).ToList<string>();
            // TODO: Actually check these errors
            try
            {
                ipList.AddRange(dataDB.VirtualMachines.OfType<PendingVirtualMachine>().Select(v => v.IP) as List<string>);
            }
            catch (Exception) { }

            try
            {
                ipList.AddRange(dataDB.VirtualMachines.OfType<PendingArchiveVirtualMachine>().Select(v => v.IP) as List<string>);
            }
            catch (Exception) { }

            bool[] usedIP = new bool[256];

            foreach (var ip in ipList)
            {
                string longIP = ip;
                int ipTail = int.Parse(longIP.Substring(longIP.LastIndexOf('.') + 1));
                usedIP[ipTail] = true;
            }

            for (int index = 0; index < usedIP.Length; index++)
            {
                if (!usedIP[index])
                    return index;
            }

            return -1;
        }

        public void ToggleVMStatus(string image)
        {
            RegisteredVirtualMachine vm = dataDB.VirtualMachines.
                OfType<RegisteredVirtualMachine>().Single(d => d.ImagePathName == image);

            DateTime started = vm.LastStarted;
            DateTime stopped = vm.LastStopped;

            RegisteredVirtualMachineService.SetRegisteredVirtualMachine(image);
            VMStatus status = RegisteredVirtualMachineService.GetStatus();

            if (status == VMStatus.Running)
                stopped = RegisteredVirtualMachineService.PowerOff();
            else if (status == VMStatus.Stopped)
                started = RegisteredVirtualMachineService.PowerOn();

            status = RegisteredVirtualMachineService.GetStatus();
        }

        public IEnumerable<VirtualMachine> GetRegisteredVMs()
        {
            var vh = new VirtualHost();
            var imagePathNames = vh.RegisteredVirtualMachines.Select(v => v.PathName);
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

                RegisteredVirtualMachineService.SetRegisteredVirtualMachine(path);

                if (RegisteredVirtualMachineService.GetStatus() == VMStatus.Running)
                {
                    vm.Hostname = RegisteredVirtualMachineService.GetHostname();
                    vm.IP = RegisteredVirtualMachineService.GetIP();
                }

                dataDB.SaveChanges();
                vmList.Add(vm);
            }

            return vmList;
        }
    }
}
