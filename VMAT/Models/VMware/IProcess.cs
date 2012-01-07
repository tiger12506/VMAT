using System;

namespace VMAT.Models.VMware
{
    public interface IProcess
    {
        string getCommand();
        int getExitCode();
        long getId();
        bool getIsBeingDebugged();
        string getName();
        string getOwner();
        DateTime getStartDateTime();

        void KillProcessInGuest();
        void KillProcessInGuest(int timeoutInSeconds);
    }
}
