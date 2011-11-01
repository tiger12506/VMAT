using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackendVMWare
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
