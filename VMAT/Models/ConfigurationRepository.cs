using System;
using System.Linq;
using VMAT.ViewModels;

namespace VMAT.Models
{
	public class ConfigurationRepository : IConfigurationRepository
	{
		private DataEntities dataDB = new DataEntities();

		public ConfigurationRepository()
		{
			if (dataDB.HostConfiguration == null || dataDB.HostConfiguration.Count() == 0)
			{
				dataDB.HostConfiguration.Add(new HostConfiguration());
				dataDB.SaveChanges();
			}
		}

		public HostConfiguration GetHostConfiguration()
		{
			return dataDB.HostConfiguration.First();
		}

		public void SetHostConfiguration(ConfigurationFormViewModel config)
		{
			var delete = dataDB.HostConfiguration.First();
			delete = new HostConfiguration(config);
			dataDB.SaveChanges();
		}

		public DateTime GetVmCreationTime()
		{
			return dataDB.HostConfiguration.First().CreateVMTime;
		}

		public DateTime GetVmArchiveTime()
		{
			return dataDB.HostConfiguration.First().ArchiveVMTime;
		}

		public DateTime GetVmBackupTime()
		{
			return dataDB.HostConfiguration.First().BackupVMTime;
		}
	}
}
