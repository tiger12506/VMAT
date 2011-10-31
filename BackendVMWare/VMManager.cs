using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;

namespace BackendVMWare
{
    public class VMManager
    {
        // given a name, looks up all info about the VM
        public VMInfo getInfo(string imageFileName)
        {
            return new VMInfo();
        }
        //Create VM using provided info (Created, LastRunning fields ignored)
        //Assume that IP is not already taken (tracking is done by frontend; can switch)
        public VMInfo createVM(VMInfo info)
        {
            return new VMInfo();
        }
        //Mark server as active, idle (pause & don't autostart), or archived (stops & archives)
        public void updateLifecycle(string imageFileName, VMLifecycle newLifecycle)
        {

        }
        //Start up, shut down, or pause server
        public void updateStatus(string imageFileName, VMStatus newStatus)
        {

        }

        /* also need setting config options, which may require reading XML (since backend will have no persistence)
            IP address allowable range
            Set maximum simultaneous running server count
            Set VM creation, backup, and archive batch process times
            Set up list of base images & locations (optional, can just use folder names)
         */
    



        private void createServer()
        {
            IVirtualHost virtualHost = new VirtualHost(new VMWareVirtualHost());
            createServer(virtualHost, @"C:/img.vmx");
        }

        //shouldn't really be called
        public IVirtualMachine createServer(IVirtualHost virtualHost, string imageLocation)
        {
            // TODO: add to autostart & backup lists
            // http://kb.vmware.com/selfservice/microsites/search.do?language=en_US&cmd=displayKC&externalId=1370

            // declare a virtual host
            // connect to a local VMWare Workstation virtual host
            virtualHost.ConnectToVMWareServer("vmat.reshall.rose-hulman.edu", "Nathan", "Vmat1234");

            // open an existing virtual machine
            IVirtualMachine vm = virtualHost.Open(imageLocation);
            // power on this virtual machine
            
            // wait for VMWare Tools
            vm.WaitForToolsInGuest();
            // login to the virtual machine
            vm.LoginInGuest("Administrator", "password");

            return vm;
        }
    }
}
