using VMAT.Models;

namespace VMAT.ViewModels
{
    public class ArchivedVirtualMachineViewModel
    {
        public string MachineName { get; set; }
        public string ArchiveTime { get; set; }

        public ArchivedVirtualMachineViewModel() { }

        public ArchivedVirtualMachineViewModel(ArchivedVirtualMachine vm)
        {
            MachineName = vm.GetMachineName();
            ArchiveTime = vm.LastArchived.ToString();
        }
    }
}
