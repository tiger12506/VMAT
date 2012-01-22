using VMAT.Models;

namespace VMAT.ViewModels
{
    public class PendingVirtualMachineViewModel
    {
        public string MachineName { get; set; }
        public string IP { get; set; }
        public string CreationTime { get; set; }

        public PendingVirtualMachineViewModel() { }

        public PendingVirtualMachineViewModel(PendingVirtualMachine vm)
        {
            MachineName = vm.GetMachineName();
            IP = vm.IP;
            //CreationTime = GetCreationTime().ToString();
        }
    }
}
