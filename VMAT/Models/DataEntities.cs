using System.Data.Entity;

namespace VMAT.Models
{
    public class DataEntities : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<VirtualMachine> VirtualMachines { get; set; }
        public DbSet<HostConfiguration> HostConfiguration { get; set; }
    }
}
