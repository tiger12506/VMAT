using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Vestris.VMWareLib;
using System.IO;

namespace BackendVMWare
{
    public class VMManager
    {
        private static IVirtualHost vh;
        public static IVirtualHost getVH() {
            if (vh==null) 
                vh=new VirtualHost();
            if (!vh.IsConnected)
                vh.ConnectToVMWareVIServer(Config.getVMwareHostAndPort(), Config.getVMwareUsername(), Config.getVMwarePassword());
            return vh;
        }
        public VMManager(IVirtualHost vh)
        {
            VMManager.vh = vh;
            getVH();
        }

        public VMManager()
        {
            getVH();
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

        
        // given a name, looks up all info about the VM
        [Obsolete()]
        public VMInfo GetInfo(string imagePathName)
        {
            return new VMInfo(imagePathName);
        }

        /* 
         * Pull all of the information for each virtual machine. Parse the machine
         * and project name and fill in any other derived information. Group the
         * machines into their respective projects.
         */
        public List<ProjectInfo> GetProjectInfo()
        {
            List<ProjectInfo> projects = new List<ProjectInfo>();

            projects.Add(new ProjectInfo("gapdev"));

            foreach (string imageName in GetRegisteredVMs())
            {
                VMInfo vmInfo = new VMInfo(imageName);
                projects[0].AddVirtualMachine(vmInfo);
            }

            return projects;
        }


        /*public bool ChangeHostnameAndIp(string imagePathName, string newHostname, string newIP)
        {
            try
            {
                var vm = OpenVM(imagePathName);
                vm.SetIP(newIP);
                vm.SetHostname(newHostname);
                vm.PowerOffSafely();
                vm.PowerOn();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        */
        /* also need setting config options, which may require reading XML (since backend will have no persistence)
            IP address allowable range
            Set maximum simultaneous running server count
            Set VM creation, backup, and archive batch process times
            Set up list of base images & locations (optional, can just use folder names)
         */
    






    }
}
