using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace VMAT.Models
{
    public class DataEntities : DbContext
    {
        public DbSet<VirtualMachine> VirtualMachines { get; set; }
        public DbSet<HostConfiguration> HostConfiguration { get; set; }
    }
}
