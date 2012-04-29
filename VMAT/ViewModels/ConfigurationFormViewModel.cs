using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMAT.Models;

namespace VMAT.ViewModels
{
	public class ConfigurationFormViewModel
	{
		[DisplayName("Maximum VM Count")]
		public int MaxVMCount { get; set; }

		[RegularExpression("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
			ErrorMessage = "Invalid IP Address")]
		[DisplayName("Minimum IP Address")]
		public string MinIP { get; set; }

		[RegularExpression("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
			ErrorMessage = "Invalid IP Address")]
		[DisplayName("Maximum IP Address")]
		public string MaxIP { get; set; }

		[DisplayName("Create VM Time")]
		public TimeSpan CreateVMTime { get; set; }

		[DisplayName("Backup VM Time")]
		public TimeSpan BackupVMTime { get; set; }

		[DisplayName("Archive VM Time")]
		public TimeSpan ArchiveVMTime { get; set; }

		public ConfigurationFormViewModel() : this(new HostConfiguration()) { }

		public ConfigurationFormViewModel(HostConfiguration cfg)
		{
			MaxVMCount = cfg.MaxVMCount;
			MinIP = cfg.MinIP;
			MaxIP = cfg.MaxIP;
			CreateVMTime = cfg.CreateVMTime.TimeOfDay;
			BackupVMTime = cfg.BackupVMTime.TimeOfDay;
			ArchiveVMTime = cfg.ArchiveVMTime.TimeOfDay;
		}
	}
}
