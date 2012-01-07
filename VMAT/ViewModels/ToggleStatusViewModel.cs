using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMAT.ViewModels
{
    public class ToggleStatusViewModel
    {
        public string Status { get; set; }
        public DateTime LastStartTime { get; set; }
        public DateTime LastShutdownTime { get; set; }
    }
}