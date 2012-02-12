using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMAT.Models
{
    public class PendingArchiveVirtualMachine : RegisteredVirtualMachine
    {
        // Currently no different than Registered VM

        public PendingArchiveVirtualMachine() : base() { }

        public PendingArchiveVirtualMachine(RegisteredVirtualMachine vm)
        {
            ImagePathName = vm.ImagePathName;
            BaseImageName = vm.BaseImageName;
            OS = vm.OS;
            Hostname = vm.Hostname;
            Lifecycle = vm.Lifecycle;
            IP = vm.IP;
            CreatedTime = vm.CreatedTime;
            LastStarted = vm.LastStarted;
            LastStopped = vm.LastStopped;
            LastArchived = vm.LastArchived;
            LastBackuped = vm.LastBackuped;
        }
    }
}
