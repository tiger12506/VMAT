using System;
using Vestris.VMWareLib;

namespace VMAT.Models.VMware
{
    public class Process : IProcess
    {
        public void KillProcessInGuest() {}
        public void KillProcessInGuest(int timeoutInSeconds) {}
        private VMWareVirtualMachine.Process p;

        public Process()
        {
        }

        public Process(VMWareVirtualMachine.Process p)
        {
            this.p = p;
        }

        public string getCommand()
        {
            return p.Command;
        }

        public int getExitCode()
        {
            return p.ExitCode;
        }

        public long getId()
        {
            return p.Id;
        }

        public bool getIsBeingDebugged()
        {
            return p.IsBeingDebugged;
        }

        public string getName()
        {
            return p.Name;
        }

        public string getOwner()
        {
            return p.Owner;
        }

        public DateTime getStartDateTime()
        {
            return p.StartDateTime;
        }
    }
}
