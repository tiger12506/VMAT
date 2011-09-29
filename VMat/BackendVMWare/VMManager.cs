using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;

namespace BackendVMWare
{
    class VMManager
    {
        //from vmwaretools example
        public void createServer()
        {
            // declare a virtual host
            VMWareVirtualHost virtualHost = new VMWareVirtualHost();
            // connect to a local VMWare Workstation virtual host
            virtualHost.ConnectToVMWareWorkstation();
            // open an existing virtual machine
            VMWareVirtualMachine virtualMachine = virtualHost.Open(@"C:\Virtual Machines\xp\xp.vmx");
            // power on this virtual machine
            virtualMachine.PowerOn();
            // wait for VMWare Tools
            virtualMachine.WaitForToolsInGuest();
            // login to the virtual machine
            virtualMachine.LoginInGuest("Administrator", "password");
            // run notepad
            virtualMachine.RunProgramInGuest("notepad.exe", string.Empty);
            // create a new snapshot
            string name = "New Snapshot";
            // take a snapshot at the current state
            virtualMachine.Snapshots.CreateSnapshot(name, "test snapshot");
            // power off
            virtualMachine.PowerOff();
            // find the newly created snapshot
            VMWareSnapshot snapshot = virtualMachine.Snapshots.GetNamedSnapshot(name);
            // revert to the new snapshot
            snapshot.RevertToSnapshot();
            // delete snapshot
            snapshot.RemoveSnapshot();
        }
    }
}
