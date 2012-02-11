using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VMAT.Models
{
    public class Project
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        [Key]
        [Required(ErrorMessage = "Project Name required")]
        [RegularExpression("G[0-9]{4}", ErrorMessage = "Project Name must follow the format 'G1234'")]
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the name of the VMware host for the
        /// virtual machines within this project.
        /// </summary>
        [DisplayName("Hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the list of the virtual machines within this project.
        /// </summary>
        public List<VirtualMachine> VirtualMachines { get; set; }

        public Project() { }

        public Project(string name) : this(name, AppConfiguration.GetVMHostName()) { }

        public Project(string name, string hostname)
        {
            ProjectName = name;
            Hostname = hostname;
            VirtualMachines = new List<VirtualMachine>();
        }

        public Project(string name, string hostname, List<VirtualMachine> vms)
        {
            ProjectName = name;
            Hostname = hostname;
            VirtualMachines = vms;
        }

        public void AddVirtualMachine(VirtualMachine vm)
        {
            VirtualMachines.Add(vm);
        }
    }
}
