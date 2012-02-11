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
            MachineName = "gapdev" + vm.ProjectName.Trim('G') + vm.MachineName;
            ArchiveTime = vm.LastArchived.ToString();
        }
    }
}
