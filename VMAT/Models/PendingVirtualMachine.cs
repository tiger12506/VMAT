using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMAT.ViewModels;

namespace VMAT.Models
{
    public class PendingVirtualMachine : VirtualMachine
    {
        [StringLength(15, ErrorMessage ="Invalid IP Address")]
        [DisplayName("IP Address")]
        public string IP { get; set; }

        public PendingVirtualMachine() { }

        public PendingVirtualMachine(VirtualMachineFormViewModel vmForm)
        {
            string machineName = "gapdev" + vmForm.ProjectName + vmForm.MachineNameSuffix;
            ImagePathName = AppConfiguration.GetDatastore() + vmForm.ProjectName + "/" + machineName + "/" + machineName + ".vmx";
            Hostname = machineName;
            BaseImageName = vmForm.BaseImageFile;
            IP = vmForm.IP;

            if (vmForm.IsAutoStarted)
                Lifecycle = VMLifecycle.Active;
            else
                Lifecycle = VMLifecycle.Idle;
        }
    }
}
