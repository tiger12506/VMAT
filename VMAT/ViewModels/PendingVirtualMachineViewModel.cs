using VMAT.Models;

namespace VMAT.ViewModels
{
    public class PendingVirtualMachineViewModel
    {
        public string ImagePathName { get; set; }
        public string BaseImageName { get; set; }
        public string MachineName { get; set; }
        public string IP { get; set; }

        public PendingVirtualMachineViewModel() { }

        public PendingVirtualMachineViewModel(PendingVirtualMachine vm)
        {
            ImagePathName = vm.ImagePathName;
            BaseImageName = vm.BaseImageName;
            MachineName = vm.GetMachineName();
            IP = vm.IP;
        }
    }
}
