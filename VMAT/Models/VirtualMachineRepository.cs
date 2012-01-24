using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VMAT.Models.VMware;
using VMAT.Services;
using VMAT.ViewModels;

namespace VMAT.Models
{
    public class VirtualMachineRepository// : IVirtualMachineRepository
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
            throw new NotImplementedException();
        }

        public VirtualMachine GetVirtualMachine(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void CreateRegisteredVirtualMachine(RegisteredVirtualMachine vm)
        {
            throw new NotImplementedException();
        }

        public RegisteredVirtualMachine GetRegisteredVirtualMachine(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void CreateArchivedVirtualMachine(ArchivedVirtualMachine vm)
        {
            throw new NotImplementedException();
        }

        public ArchivedVirtualMachine GetArchivedVirtualMachine(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void CreatePendingVirtualMachine(PendingVirtualMachine vm)
        {
            throw new NotImplementedException();
        }

        public PendingVirtualMachine GetPendingVirtualMachine(string imagePath)
        {
            throw new NotImplementedException();
        }

        public int GetNextAvailbaleIP()
        {
            List<string> ipList = dataDB.VirtualMachines.OfType<RegisteredVirtualMachine>().
                Select(v => v.IP) as List<string>;
            ipList.AddRange(dataDB.VirtualMachines.OfType<PendingVirtualMachine>().
                Select(v => v.IP) as List<string>);
            ipList.AddRange(dataDB.VirtualMachines.OfType<PendingArchiveVirtualMachine>().
                Select(v => v.IP) as List<string>);
            bool[] usedIP = new bool[256];

            foreach (var ip in ipList)
            {
                string longIP = ip;
                int ipTail = int.Parse(longIP.Substring(longIP.LastIndexOf('.')));
                usedIP[ipTail] = true;
            }

            for (int index = 0; index < usedIP.Length; index++)
            {
                if (!usedIP[index])
                    return index;
            }

            return -1;
        }

        public void ToggleStatus(string image)
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

            var results = new ToggleStatusViewModel
            {
                Status = status.ToString().ToLower(),
                LastStartTime = started,
                LastShutdownTime = stopped
            };
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
