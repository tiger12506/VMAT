using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackendVMWare
{
    public enum VMStatus
    {
        
        Stopped,
        Paused,
        Running

    }
    public enum VMLifecycle
    {
        Active,
        Idle,
        Archived
    }
    public class VMInfo
    {

        public string ImageFileName { get; set; }
        public string BaseImageName { get; set; }
        public string ProjectName { get; set; }
        public VMStatus Status { get; set; }
        public VMLifecycle Lifecycle { get; set; }
        public DateTime LastRunning { get; set; }
        public DateTime Created { get; set; }
        public string IP { get; set; }
        public string HostnameWithDomain { get; set; }

    }
}
