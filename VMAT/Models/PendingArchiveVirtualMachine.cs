using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMAT.Models
{
    public class PendingArchiveVirtualMachine : RegisteredVirtualMachine
    {
        // Currently no different than Running VM

        public PendingArchiveVirtualMachine()
            : base()
        {

        }

        public PendingArchiveVirtualMachine(RegisteredVirtualMachine vm) : base(vm.ImagePathName)
        {
            
        }
    }
}
