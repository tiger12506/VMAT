using System;
using VMAT.ViewModels;

namespace VMAT.Models
{
    public interface IConfigurationRepository
    {
        HostConfiguration GetHostConfiguration();
        void SetHostConfiguration(ConfigurationFormViewModel config);
        DateTime GetVmCreationTime();
        DateTime GetVmArchiveTime();
        DateTime GetVmBackupTime();
    }
}
