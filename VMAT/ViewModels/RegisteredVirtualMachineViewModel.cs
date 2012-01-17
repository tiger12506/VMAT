using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VMAT.Models;

namespace VMAT.ViewModels
{
    public class RegisteredVirtualMachineViewModel
    {
        public string ImagePathName { get; set; }
        public string BaseImageName { get; set; }
        public string Status { get; set; }
        public string MachineName { get; set; }
        public string IP { get; set; }
        public string CreatedTime { get; set; }
        public string LastStarted { get; set; }
        public string LastStopped { get; set; }
        public string LastBackuped { get; set; }
        public string LastArchived { get; set; }
    }
}