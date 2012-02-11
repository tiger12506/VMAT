using VMAT.Models;

namespace VMAT.ViewModels
{
    public class PendingVirtualMachineViewModel : VirtualMachineViewModel
    {
        public string ImagePathName { get; set; }
        public string OS { get; set; }
        public string MachineName { get; set; }
        public string IP { get; set; }

        public PendingVirtualMachineViewModel() { }

        public PendingVirtualMachineViewModel(PendingVirtualMachine vm)
        {
            ImagePathName = vm.ImagePathName;
            OS = vm.OS;
            MachineName = vm.MachineName;
            IP = vm.IP;
        }
    }
}
