using System;
using System.ComponentModel;
using VMAT.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace VMAT.Models
{
	public class HostConfiguration
	{
		[ScaffoldColumn(false)]
		public int HostConfigurationId { get; set; }

		[DisplayName("Maximum VM Count")]
		public int MaxVMCount { get; set; }

		[DisplayName("Minimum IP Address")]
		public string MinIP { get; set; }

		[DisplayName("Maximum IP Address")]
		public string MaxIP { get; set; }

		[DisplayName("Create VM Time")]
		public DateTime CreateVMTime { get; set; }

		[DisplayName("Backup VM Time")]
		public DateTime BackupVMTime { get; set; }

		[DisplayName("Archive VM Time")]
		public DateTime ArchiveVMTime { get; set; }

		public HostConfiguration()
		{
			MaxVMCount = 0;
			MinIP = "192.168.1.1";
			MaxIP = "192.168.1.255";
			CreateVMTime = DateTime.Now.AddHours(12);
			BackupVMTime = DateTime.Now.AddHours(12);
			ArchiveVMTime = DateTime.Now.AddHours(12);
		}
		
		public HostConfiguration(ConfigurationFormViewModel cfg)
		{
			MaxVMCount = cfg.MaxVMCount;
			MinIP = cfg.MinIP;
			MaxIP = cfg.MaxIP;
			CreateVMTime = DateTime.Parse(cfg.CreateVMTime.ToString());
			BackupVMTime = DateTime.Parse(cfg.BackupVMTime.ToString());
			ArchiveVMTime = DateTime.Parse(cfg.ArchiveVMTime.ToString());
		}
	}
}
