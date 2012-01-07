using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMAT.Models
{
    public class Project
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
        public List<VirtualMachine> VirtualMachines { get; set; }

        public Project(string name) : this(name, "vmat.csse.rose-hulman.edu") { }

        public Project(string name, string hostname)
        {
            ProjectName = name;
            HostName = hostname;
            VirtualMachines = new List<VirtualMachine>();
        }

        public void AddVirtualMachine(VirtualMachine vm)
        {
            VirtualMachines.Add(vm);
        }
    }
}