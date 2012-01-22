﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VMAT.Models.VMware;
using VMAT.Services;

namespace VMAT.Models
{
    public class VirtualMachineRepository : IVirtualMachineRepository
    {
        private DataEntities dataDB = new DataEntities();
        private IVirtualHost virtualHost;

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

        public IVirtualHost GetVirtualHost()
        {
            if (virtualHost == null)
                virtualHost = new VirtualHost();
            if (!virtualHost.IsConnected)
                virtualHost.ConnectToVMWareVIServer(AppConfiguration.GetVMwareHostAndPort(), AppConfiguration.GetVMwareUsername(), AppConfiguration.GetVMwarePassword());
            return virtualHost;
        }

        public VirtualMachineRepository(IVirtualHost vh)
        {
            virtualHost = vh;
            GetVirtualHost();
        }

        public VirtualMachineRepository()
        {
            GetVirtualHost();
        }

        public IVirtualMachine OpenVM(string imagePathName)
        {
            return virtualHost.Open(imagePathName);
        }
    }
}
