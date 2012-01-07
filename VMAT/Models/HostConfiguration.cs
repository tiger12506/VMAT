using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMAT.Models
{
    public class HostConfiguration
    {
        public int MaxVMCount { get; set; }
        public string MaxIP { get; set; }
        public string MinIP { get; set; }
        public DateTime CreateVMTime { get; set; }
        public DateTime BackupVMTime { get; set; }
        public DateTime ArchiveVMTime { get; set; }
    }
}