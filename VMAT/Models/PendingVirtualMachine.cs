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
            ImagePathName = vmForm.ProjectName + "\\" + machineName + "\\" + machineName + ".vmx";
            BaseImageName = vmForm.BaseImageFile;
            Lifecycle = vmForm.Lifecycle;
            IP = vmForm.IP;
        }
    }
}
