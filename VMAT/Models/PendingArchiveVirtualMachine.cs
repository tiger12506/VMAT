using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMAT.Models
{
    public class PendingArchiveVirtualMachine : RunningVirtualMachine
    {
        // Currently no different than Running VM

        public PendingArchiveVirtualMachine(RunningVirtualMachine vm) : base(vm.ImagePathName)
        {
            
        }
    }
}