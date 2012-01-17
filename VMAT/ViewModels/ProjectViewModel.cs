using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMAT.ViewModels
{
    public class ProjectViewModel
    {
        public string ProjectName { get; set; }
        public string Hostname { get; set; }
        public List<RegisteredVirtualMachineViewModel> RegisteredVMs { get; set; }
        public List<PendingVirtualMachineViewModel> PendingVMs { get; set; }
        public List<PendingArchiveVirtualMachineViewModel> PendingArchiveVMs { get; set; }
        public List<ArchiveVirtualMachineViewModel> ArchivedVMs { get; set; }

        public ProjectViewModel()
        {
            RegisteredVMs = new List<RegisteredVirtualMachineViewModel>();
            PendingVMs = new List<PendingVirtualMachineViewModel>();
            PendingArchiveVMs = new List<PendingArchiveVirtualMachineViewModel>();
            ArchivedVMs = new List<ArchiveVirtualMachineViewModel>();
        }
    }
}