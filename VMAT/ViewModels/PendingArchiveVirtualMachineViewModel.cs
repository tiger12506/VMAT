using VMAT.Models;

namespace VMAT.ViewModels
{
    public class PendingArchiveVirtualMachineViewModel : RegisteredVirtualMachineViewModel
    {
        public PendingArchiveVirtualMachineViewModel() : base() { }

        public PendingArchiveVirtualMachineViewModel(PendingArchiveVirtualMachine vm) : base(vm) { }
    }
}
