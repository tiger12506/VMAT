using System;

namespace VMAT.Models
{
    public interface IConfigurationRepository
    {
        DateTime GetVmCreationTime();
        DateTime GetVmArchiveTime();
        DateTime GetVmBackupTime();
    }
}
