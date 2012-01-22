using System.Collections.Generic;
using VMAT.Models;

namespace VMAT.ViewModels
{
    public class ProjectViewModel
    {
        public string ProjectName { get; set; }
        public string Hostname { get; set; }
        public IEnumerable<RegisteredVirtualMachineViewModel> RegisteredVMs { get; set; }
        public IEnumerable<PendingVirtualMachineViewModel> PendingVMs { get; set; }
        public IEnumerable<PendingArchiveVirtualMachineViewModel> PendingArchiveVMs { get; set; }
        public IEnumerable<ArchivedVirtualMachineViewModel> ArchivedVMs { get; set; }

        public ProjectViewModel()
        {
            RegisteredVMs = new List<RegisteredVirtualMachineViewModel>();
            PendingVMs = new List<PendingVirtualMachineViewModel>();
            PendingArchiveVMs = new List<PendingArchiveVirtualMachineViewModel>();
            ArchivedVMs = new List<ArchivedVirtualMachineViewModel>();
        }

        public ProjectViewModel(Project project) : this()
        {
            ProjectName = project.ProjectName;
            Hostname = project.Hostname;
        }

        public void AddRegisteredVirtualMachineViewModel(RegisteredVirtualMachineViewModel vm)
        {
            (RegisteredVMs as List<RegisteredVirtualMachineViewModel>).Add(vm);
        }

        public void AddPendingVirtualMachineViewModel(PendingVirtualMachineViewModel vm)
        {
            (PendingVMs as List<PendingVirtualMachineViewModel>).Add(vm);
        }

        public void AddPendingArchiveVirtualMachineViewModel(PendingArchiveVirtualMachineViewModel vm)
        {
            (PendingArchiveVMs as List<PendingArchiveVirtualMachineViewModel>).Add(vm);
        }

        public void AddArchivedVirtualMachineViewModel(ArchivedVirtualMachineViewModel vm)
        {
            (ArchivedVMs as List<ArchivedVirtualMachineViewModel>).Add(vm);
        }
    }
}
