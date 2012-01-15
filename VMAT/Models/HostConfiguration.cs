using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace VMAT.Models
{
    public class HostConfiguration
    {
        [DefaultValue(0)]
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
    }
}