using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMAT.ViewModels;

namespace VMAT.Models
{
    public class PendingVirtualMachine : VirtualMachine
    {
        [RegularExpression("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "Invalid IP Address")]
        [DisplayName("IP Address")]
        public string IP { get; set; }

        public PendingVirtualMachine() { }

        public PendingVirtualMachine(VirtualMachineFormViewModel vmForm)
        {
            MachineName = "gapdev" + vmForm.ProjectName.Trim('G') + vmForm.MachineName;
            ImagePathName = AppConfiguration.GetDatastore() + vmForm.ProjectName + "/" + 
                MachineName + "/" + MachineName + ".vmx";
            BaseImageName = vmForm.BaseImageFile;
            IP = vmForm.IP;
            IsAutoStarted = vmForm.IsAutoStarted;
        }
    }
}
