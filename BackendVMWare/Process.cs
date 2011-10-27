using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;

namespace BackendVMWare
{
    public class Process : IProcess
    {
        public string Command;
        public int ExitCode;
        public long Id;
        public bool IsBeingDebugged;
        public string Name;
        public string Owner;
        public DateTime StartDateTime;

        public void KillProcessInGuest() {}
        public void KillProcessInGuest(int timeoutInSeconds) {}
        private VMWareVirtualMachine.Process p;
        Process(VMWareVirtualMachine.Process p)
        {
            this.p = p;
        }
    }
}
