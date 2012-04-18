using System;
using VMAT.ViewModels;

namespace VMAT.Models
{
	public interface IConfigurationRepository
	{
		HostConfiguration GetHostConfiguration();
		void SetHostConfiguration(ConfigurationFormViewModel config);
		int GetMaxVmCount();
		Tuple<string, string> GetIpRange();
		DateTime GetVmCreationTime();
		DateTime GetVmArchiveTime();
		DateTime GetVmBackupTime();
	}
}
