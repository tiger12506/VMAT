using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using VMAT.Models.VMware;
using VMAT.Services;

namespace VMAT.Models
{
    public class VirtualMachineManager
    {
        DataEntities dataDB = new DataEntities();

        private static IVirtualHost vh;

        public static IVirtualHost GetVirtualHost()
        {
            if (vh == null)
                vh = new VirtualHost();
            if (!vh.IsConnected)
                vh.ConnectToVMWareVIServer(AppConfiguration.GetVMwareHostAndPort(), AppConfiguration.GetVMwareUsername(), AppConfiguration.GetVMwarePassword());
            return vh;
        }

        public VirtualMachineManager(IVirtualHost vh)
        {
            VirtualMachineManager.vh = vh;
            GetVirtualHost();
        }

        public VirtualMachineManager()
        {
            GetVirtualHost();
        }

        public IVirtualMachine OpenVM(string imagePathName)
        {
            return vh.Open(imagePathName);
        }

        public IEnumerable<string> GetRegisteredVMImagePaths()
        {
            var ret = vh.RegisteredVirtualMachines.Select(v => v.PathName);

            return ret;
        }

        public IEnumerable<VirtualMachine> GetRegisteredVMs()
        {
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

        /// <summary>
        /// Pull all of the information for each virtual machine. Parse the machine
        /// and project name and fill in any other derived information. Group the
        /// machines into their respective projects.
        /// </summary>
        /// <returns>A list of project items and information</returns>
        public IEnumerable<Project> GetProjectInfo()
        {
            var projects = new List<Project>();

            foreach (string imageName in GetRegisteredVMImagePaths())
            {
                VirtualMachine vm = new RegisteredVirtualMachine(imageName);
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

        /// <summary>
        ///  Find the lowest available IP address.
        /// </summary>
        /// <returns>The last octet of the lowest available IP address.</returns>
        public int GetNextAvailableIP()
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

        public static IEnumerable<string> GetBaseImageFiles()
        {
            List<string> filePaths = new List<string>(Directory.GetFiles(AppConfiguration.GetWebserverVmPath(), "*.vmx", SearchOption.AllDirectories));
            return filePaths.Select(foo => ConvertPathToDatasource(foo));
        }

        /// <summary>
        /// Converts datasource-style path to physical network path
        /// </summary>
        /// <param name="PathName">Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"</param>
        /// <returns>Physical absolute path (from webserver to VM server), ie "//VMServer/VirtualMachines/Windows 7/Windows 7.VMx</returns>
        public static string ConvertPathToPhysical(string PathName)
        {
            return PathName.Replace(AppConfiguration.GetDatastore(), AppConfiguration.GetWebserverVmPath()).Replace('/', '\\');
        }

        /// <summary>
        /// Converts physical network path to datasource-style path
        /// </summary>
        /// <param name="PathName">Physical absolute path (from webserver to VM server), ie "//VMServer/VirtualMachines/Windows 7/Windows 7.VMx</param>
        /// <returns>Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"</returns>
        public static string ConvertPathToDatasource(string PathName)
        {
            return PathName.Replace(AppConfiguration.GetWebserverVmPath(), AppConfiguration.GetDatastore()).Replace('\\', '/');
        }
    }
}
