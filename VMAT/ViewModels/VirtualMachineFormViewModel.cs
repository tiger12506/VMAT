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
        /// String to identify project. 4 sections: "G"+Project Number (4-digit), 
        /// Company, Site, tiny description. Project Identifier is latter 3 items.
        /// <summary>
        /// 4-digit project identifier
        /// </summary>
        [Required(ErrorMessage = "Project Name is required")]
        [RegularExpression("G[0-9][0-9][0-9][0-9]", ErrorMessage = "Project Name must follow the format 'G1234'")]
        [DisplayName("Project Number")]
        public string ProjectName { get; set; }

        /// <summary>
        /// The readable name of the virtual machine, derived from the Image Path Name.
        /// </summary>
        [Required(ErrorMessage = "Machine Name Suffix must be 1-5 characters long")]
        [RegularExpression("[0-9A-Za-z]{1,5}", ErrorMessage = "Machine Name Suffix must be 1-5 characters long")]
        [DisplayName("Machine Name Suffix")]
        public string MachineNameSuffix { get; set; }

        public string BaseImageFile { get; set; }

        [RegularExpression("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", 
            ErrorMessage = "Invalid IP Address")]
        [DisplayName("IP Address")]
        public string IP { get; set; }

        [DefaultValue(false)]
        [DisplayName("Startup")]
        public bool IsAutoStarted { get; set; }

        public VirtualMachineFormViewModel() { }

        public VirtualMachineFormViewModel(Models.VirtualMachine vm)
        {
            ProjectName = vm.GetProjectName();
            MachineNameSuffix = vm.GetMachineName().Substring("gapdev1111".Length + 1);
            BaseImageFile = vm.BaseImageName;
            IP = ((RegisteredVirtualMachine)vm).IP;

            if (vm.Lifecycle == VMLifecycle.Active)
                IsAutoStarted = true;
            else if (vm.Lifecycle == VMLifecycle.Idle)
                IsAutoStarted = false;
        }
    }
}