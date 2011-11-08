using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackendVMWare
{
    public enum VMStatus
    {

        Stopped,
        Paused, //still in memory, like sleep
        Suspended, //to disk, like hibernate
        Running

    }
    public enum VMLifecycle
    {
        Active,
        Idle, //not auto-start
        Archived //files compressed & moved elsewhere
    }
    
    /// <summary>
    /// Stores info about a particular VM.
    /// </summary>
    public class VMInfo
    {
        /* I think those fields cover everything we'll need to show, but that should be verified. 
         * 
         * Note about querying these fields: 
         * We may want to cache everything we want to show about VMs, even if it can be queried:
         * queries may be slow and some data can't be accessed if VM's off.
         */

        //query from vm
        public string ImagePathName { get; set; } //ie "[ha-datacenter/standard] Windows 7/Windows 7.vmx" actual HDD location harder to find
        public VMStatus Status { get; set; }

        //probably query, uncertain
        public DateTime LastRunning { get; set; }
        public DateTime Created { get; set; }

        //query from _running_ vm
        public string IP { get; set; }
        public string HostnameWithDomain { get; set; }

        //can't really query so must store elsewhere or somehow derive (ie from naming conventions)
        public string BaseImageName { get; set; }
        public string ProjectName { get; set; }
        public VMLifecycle Lifecycle { get; set; } //if archived, won't be able to query a thing obviously


        
        //queries & populates fields from a real vm (called by VMManager.getInfo)
        public void setFields(IVirtualHost vh, IVirtualMachine vm)
        {
            this.ImagePathName = vm.PathName;

            if (vm.IsPaused) Status = VMStatus.Paused;
            else if (vm.IsRunning) Status = VMStatus.Running;
            else if (vm.IsSuspended) Status = VMStatus.Suspended;
            else if (vm.IsRecording || vm.IsReplaying) Status = VMStatus.Running;
            else Status = VMStatus.Stopped;

            LastRunning = DateTime.Now; //todo
            Created = DateTime.Now;

            this.IP = vm.IpAddress;
            this.HostnameWithDomain = vm.GetHostname();

        }

        public static string convertPathToPhysical(string PathName)
        {
            return PathName.Replace("[ha-datacenter/standard] ", @"C:\Virtual Machines\").Replace('/', '\\'); //todo get path better way
        }

    }
}
