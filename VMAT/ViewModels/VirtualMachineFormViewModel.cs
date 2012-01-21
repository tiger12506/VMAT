using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VMAT.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VMAT.ViewModels
{
    public class VirtualMachineFormViewModel
    {
        /// <summary>
        /// String to identify project. 4 sections: "G"+Project Number (4-digit), 
        /// Company, Site, tiny description. Project Identifier is latter 3 items.
        /// </summary>
        [Required(ErrorMessage = "Project Name must be 4 digits")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Project Name must be 4 digits")]
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        /// <summary>
        /// The readable name of the virtual machine, derived from the Image Path Name.
        /// </summary>
        [Required(ErrorMessage = "Machine Name Suffix must be 1-5 characters long")]
        [StringLength(5, ErrorMessage = "Machine Name Suffix must be 1-5 characters long")]
        [DisplayName("Machine Name Suffix")]
        public string MachineNameSuffix { get; set; }

        public string BaseImageFile { get; set; }
        public string IP1 { get; set; }
        public string IP2 { get; set; }
        public string IP3 { get; set; }
        public string IP4 { get; set; }
        public VMLifecycle Lifecycle { get; set; }

        public VirtualMachineFormViewModel(Models.VirtualMachine vm)
        {
            ProjectName = vm.GetProjectName();
            MachineNameSuffix = vm.GetMachineName().Substring("gapdev1111".Length + 1);
            BaseImageFile = vm.BaseImageName;
            string[] ip = ((RegisteredVirtualMachine)vm).IP.Split('.');
            Lifecycle = vm.Lifecycle;
        }
    }
}