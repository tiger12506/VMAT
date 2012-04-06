using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VMAT.Models
{
    public class HostConfiguration
    {
        [Key]
        [ScaffoldColumn(false)]
        [DisplayName("Hostname")]
        public string Hostname { get; set; }

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
        public DateTime CreateVMTime { get; set; }

        [DisplayName("Backup VM Time")]
        public DateTime BackupVMTime { get; set; }

        [DisplayName("Archive VM Time")]
        public DateTime ArchiveVMTime { get; set; }

        public HostConfiguration()
        {
            Hostname = "vmat.rose-hulman.edu";
            MaxVMCount = 0;
            MinIP = "192.168.1.1";
            MaxIP = "192.168.1.255";
            CreateVMTime = DateTime.Now.AddHours(12);
            BackupVMTime = DateTime.Now.AddHours(12);
            ArchiveVMTime = DateTime.Now.AddHours(12);
        }
    }
}
