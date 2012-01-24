using VMAT.Models;

namespace VMAT.ViewModels
{
    public class PendingArchiveVirtualMachineViewModel
    {
        public string MachineName { get; set; }
        public string ArchiveTime { get; set; }

        public PendingArchiveVirtualMachineViewModel() { }

        public PendingArchiveVirtualMachineViewModel(PendingArchiveVirtualMachine vm)
        {
            MachineName = vm.GetMachineName();
            //ArchiveTime = GetArchiveTime().ToString();
        }
    }
}
