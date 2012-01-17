using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMAT.ViewModels
{
    public class PendingVirtualMachineViewModel
    {
        public string MachineName { get; set; }
        public string IP { get; set; }
        public string CreationTime { get; set; }
    }
}