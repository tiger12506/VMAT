using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Vestris.VMWareLib;

namespace BackendVMWare
{
    /// <summary>
    /// Interact with the VMware server.
    /// </summary>
    public class VMManager
    {
        /// <summary>
        /// 
        /// </summary>
        private static IVirtualHost vh;

        public static IVirtualHost GetVH()
        {
            if (vh == null)
                vh = new VirtualHost();
            if (!vh.IsConnected)
                vh.ConnectToVMWareVIServer(Config.GetVMwareHostAndPort(), Config.GetVMwareUsername(), Config.GetVMwarePassword());
            return vh;
        }

        public VMManager(IVirtualHost vh)
        {
            VMManager.vh = vh;
            GetVH();
        }

        public VMManager()
        {
            GetVH();
        }

        private IVirtualMachine OpenVM(string imagePathName)
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
        /// Return all the information associated to the given virtual machine.
        /// </summary>
        /// <param name="imagePathName">The name of the selected virtual machine.</param>
        /// <returns>The information for the virtual machine.</returns>
        [Obsolete()]
        public VMInfo GetInfo(string imagePathName)
        {
            return new VMInfo(imagePathName);
        }

        /// <summary>
        /// Pull all of the information for each virtual machine. Parse the machine
        /// and project name and fill in any other derived information. Group the
        /// machines into their respective projects.
        /// </summary>
        /// <returns>A list of project items and information</returns>
        public List<ProjectInfo> GetProjectInfo()
        {
            List<ProjectInfo> projects = new List<ProjectInfo>();

            projects.Add(new ProjectInfo("1234"));

            foreach (string imageName in GetRegisteredVMs())
            {
                VMInfo vmInfo = new VMInfo(imageName);
                projects[0].AddVirtualMachine(vmInfo);
            }

            return projects;
        }

        /// <summary>
        ///  Find the lowest available IP address.
        /// </summary>
        /// <returns>The last octet of the lowest available IP address.</returns>
        [Obsolete()]
        public int GetNextAvailableIP()
        {
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // +++++++++++ USE Persistence.cs VERSION OF THE METHOD INSTEAD +++++++++++++++
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

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

        /* also need setting config options, which may require reading XML (since backend will have no persistence)
            IP address allowable range
            Set maximum simultaneous running server count
            Set VM creation, backup, and archive batch process times
            Set up list of base images & locations (optional, can just use folder names)
         */
    }
}
