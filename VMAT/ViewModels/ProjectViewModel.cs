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
        public IEnumerable<ArchiveVirtualMachineViewModel> ArchivedVMs { get; set; }

        public ProjectViewModel()
        {
            RegisteredVMs = new List<RegisteredVirtualMachineViewModel>();
            PendingVMs = new List<PendingVirtualMachineViewModel>();
            PendingArchiveVMs = new List<PendingArchiveVirtualMachineViewModel>();
            ArchivedVMs = new List<ArchiveVirtualMachineViewModel>();
        }

        public ProjectViewModel(Project project) : this()
        {
            ProjectName = project.ProjectName;
            Hostname = project.Hostname;

            foreach (var vm in project.VirtualMachines)
            {
                if (vm.GetType() == typeof(RegisteredVirtualMachine))
                {
                    var vmView = new RegisteredVirtualMachineViewModel(
                        vm as RegisteredVirtualMachine);

                    AddRegisteredVirtualMachineViewModel(vmView);
                }
                else if (vm.GetType() == typeof(PendingVirtualMachine))
                {
                    var vmView = new PendingVirtualMachineViewModel(
                        vm as PendingVirtualMachine);

                    AddPendingVirtualMachineViewModel(vmView);
                }
                else if (vm.GetType() == typeof(PendingArchiveVirtualMachine))
                {
                    var vmView = new PendingArchiveVirtualMachineViewModel(
                        vm as PendingArchiveVirtualMachine);

                    AddPendingArchiveVirtualMachineViewModel(vmView);
                }
                else if (vm.GetType() == typeof(ArchivedVirtualMachine))
                {
                    var vmView = new ArchiveVirtualMachineViewModel(
                        vm as ArchivedVirtualMachine);

                    AddArchivedVirtualMachineViewModel(vmView);
                }
            }
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

        public void AddArchivedVirtualMachineViewModel(ArchiveVirtualMachineViewModel vm)
        {
            (ArchivedVMs as List<ArchiveVirtualMachineViewModel>).Add(vm);
        }
    }
}
