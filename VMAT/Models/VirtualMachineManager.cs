using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Web;
using VMAT.Models.VMware;
using System.Runtime.InteropServices;
using Vestris.VMWareLib;

namespace VMAT.Models
{
    public class VirtualMachineManager
    {
        DataEntities dataDB = new DataEntities();

         /// <summary>
        /// 
        /// </summary>
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

        public IEnumerable<string> GetRunningVMs()
        {
            var ret = vh.RunningVirtualMachines.Select(v => v.PathName);

            return ret;
        }

        public IEnumerable<string> GetRegisteredVMs()
        {
            var ret = vh.RegisteredVirtualMachines.Select(v => v.PathName);

            return ret;
        }

        /// <summary>
        /// Pull all of the information for each virtual machine. Parse the machine
        /// and project name and fill in any other derived information. Group the
        /// machines into their respective projects.
        /// </summary>
        /// <returns>A list of project items and information</returns>
        public List<Project> GetProjectInfo()
        {
            List<Project> projects = new List<Project>();

            foreach (string imageName in GetRegisteredVMs())
            {
                VirtualMachine vm = new RunningVirtualMachine(imageName);
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
        public static int GetNextAvailableIP()
        {
            DataTable virtualMachineInfo = Persistence.GetVirtualMachineData();
            bool[] usedIP = new bool[256];

            foreach (DataRow currentRow in virtualMachineInfo.Rows)
            {
                string longIP = currentRow.Field<string>("IP");
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
    }
}
