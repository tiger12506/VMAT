using System;

namespace VMAT.Models
{
    public interface IConfigurationRepository
    {
        HostConfiguration GetHostConfiguration();
        void SetHostConfiguration(HostConfiguration config);
        DateTime GetVmCreationTime();
        DateTime GetVmArchiveTime();
        DateTime GetVmBackupTime();
    }
}
