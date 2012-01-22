using VMAT.Models;

namespace VMAT.ViewModels
{
    public class ArchiveVirtualMachineViewModel
    {
        public string MachineName { get; set; }
        public string ArchiveTime { get; set; }

        public ArchiveVirtualMachineViewModel() { }

        public ArchiveVirtualMachineViewModel(ArchivedVirtualMachine vm)
        {
            MachineName = vm.GetMachineName();
            ArchiveTime = vm.LastArchived.ToString();
        }
    }
}
