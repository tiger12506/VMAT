using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VMAT.Models
{
    public class Project
    {
        [ScaffoldColumn(false)]
        public int ProjectId { get; set; }

        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        public virtual ICollection<VirtualMachine> VirtualMachines { get; set; }

        public Project() { }

        public Project(string name)
        {
            ProjectName = name;
            VirtualMachines = new List<VirtualMachine>();
        }
    }
}
