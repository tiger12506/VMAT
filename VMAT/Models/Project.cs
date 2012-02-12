using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VMAT.Models
{
    public class Project
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Project Name required")]
        [RegularExpression("G[0-9]{4}", ErrorMessage = "Project Name must follow the format 'G1234'")]
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        [DisplayName("Hostname")]
        public string Hostname { get; set; }

        public List<VirtualMachine> VirtualMachines { get; set; }

        public Project() { }

        public Project(string name) : this(name, AppConfiguration.GetVMHostName()) { }

        public Project(string name, string hostname)
        {
            ProjectName = name;
            Hostname = hostname;
            VirtualMachines = new List<VirtualMachine>();
        }
    }
}
