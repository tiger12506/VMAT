using System;

namespace VMAT.Models
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private DataEntities dataDB = new DataEntities();

        public ConfigurationRepository()
        {
            if (dataDB.HostConfiguration == null)
                dataDB.HostConfiguration = new HostConfiguration();
        }

        public DateTime GetVmCreationTime()
        {
            return dataDB.HostConfiguration.CreateVMTime;
        }

        public DateTime GetVmArchiveTime()
        {
            return dataDB.HostConfiguration.ArchiveVMTime;
        }

        public DateTime GetVmBackupTime()
        {
            return dataDB.HostConfiguration.BackupVMTime;
        }
    }
}
