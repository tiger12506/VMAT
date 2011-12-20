using System;
using System.Collections.Generic;

namespace BackendVMWare
{
    /// <summary>
    /// Represents a development 'project'. Projects have a name, a host
    /// server that all of their virtual machines are stored on, and a
    /// list of all of their virtual machines' information.
    /// </summary>
    public class ProjectInfo
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the name of the VMware host for the
        /// virtual machines within this project.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the list of the virtual machines within this project.
        /// </summary>
        public List<VMInfo> VirtualMachines { get; set; }


        /// <summary>
        /// Initialize a new instance of the 'ProjectInfo' class with the
        /// given name.
        /// </summary>
        /// <param name="projName">The name of the project.</param>
        public ProjectInfo(string projName)
        {
            ProjectName = projName;
            HostName = Config.GetVMHostName();

            VirtualMachines = new List<VMInfo>();
        }

        /// <summary>
        /// Add a virtual machine to this project.
        /// </summary>
        /// <param name="machineInfo">The information pertaining to the new virtual machine.</param>
        public void AddVirtualMachine(VMInfo machineInfo)
        {
            VirtualMachines.Add(machineInfo);
        }
    }
}
