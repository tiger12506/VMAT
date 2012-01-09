using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace VMAT.Models
{
    public class DataEntities : DbContext
    {
        public DbSet<RunningVirtualMachine> VirtualMachines { get; set; }
        public DbSet<PendingVirtualMachine> PendingVirtualMachines { get; set; }
        public DbSet<ArchivedVirtualMachine> ArchivedVirtualMachines { get; set; }
        public DbSet<PendingArchiveVirtualMachine> PendingArchiveVirtualMachines { get; set; }
    }
}
