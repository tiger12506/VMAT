using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMAT.Models;

namespace VMAT.ViewModels
{
    public class VirtualMachineFormViewModel
    {
        [Required]
        [RegularExpression("G[0-9]{4}", ErrorMessage = "Project Name must follow the format 'G1234'")]
        [DisplayName("Project Number")]
        public string ProjectName { get; set; }

        [Required]
        [RegularExpression("[0-9A-Za-z]{1,5}", ErrorMessage = "Machine Name Suffix must be 1-5 characters long")]

        [DisplayName("Machine Name Suffix")]
        public string MachineName { get; set; }

        public string BaseImageFile { get; set; }

        [RegularExpression("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", 
            ErrorMessage = "Invalid IP Address")]
        [DisplayName("IP Address")]
        public string IP { get; set; }

        [DefaultValue(false)]
        [DisplayName("Startup")]
        public bool IsAutoStarted { get; set; }

        public VirtualMachineFormViewModel() { }

        public VirtualMachineFormViewModel(VirtualMachine vm, string projectName)
        {
            ProjectName = projectName;
            MachineName = vm.MachineName;
            BaseImageFile = vm.BaseImageName;
            IP = ((RegisteredVirtualMachine)vm).IP;
            IsAutoStarted = vm.IsAutoStarted;
        }
    }
}
