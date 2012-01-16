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
        public IEnumerable<RegisteredVirtualMachineViewModel> RegisteredVMs { get; set; }
        public IEnumerable<PendingVirtualMachineViewModel> PendingVMs { get; set; }
        public IEnumerable<PendingArchiveVirtualMachineViewModel> PendingArchiveVMs { get; set; }
        public IEnumerable<ArchiveVirtualMachineViewModel> ArchivedVMs { get; set; }
    }
}