using System;
using System.ComponentModel;

namespace VMAT.Models
{
    public class HostConfiguration
    {
        [DefaultValue(0)]
        [DisplayName("Maximum VM Count")]
        public int MaxVMCount { get; set; }

        [DefaultValue(0)]
        [DisplayName("Minimum IP Address")]
        public string MinIP { get; set; }

        [DefaultValue(0)]
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
            CreateVMTime = DateTime.Now;
            BackupVMTime = DateTime.Now;
            ArchiveVMTime = DateTime.Now;
        }
    }
}
