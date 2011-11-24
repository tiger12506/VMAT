using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackendVMWare
{
    public class ProjectInfo
    {
        public string ProjectName { get; set; }
        public string HostName { get; set; }

        public List<VMInfo> VirtualMachines { get; set; }

        public ProjectInfo(string projName)
        {
            ProjectName = projName;
            HostName = "gapdev.com";

            VirtualMachines = new List<VMInfo>();
        }

        public void AddVirtualMachine(VMInfo machineInfo)
        {
            VirtualMachines.Add(machineInfo);
        }
    }
}
