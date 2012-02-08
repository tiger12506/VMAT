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
            string machineName = "gapdev" + vmForm.ProjectName + vmForm.MachineNameSuffix;
            ImagePathName = vmForm.ProjectName + "/" + machineName + "/" + machineName + ".vmx";
            BaseImageName = vmForm.BaseImageFile;
            Hostname = AppConfiguration.GetVMHostName();
            IP = vmForm.IP;

            if (vmForm.IsAutoStarted)
                Lifecycle = VMLifecycle.Active;
            else
                Lifecycle = VMLifecycle.Idle;
        }
    }
}
