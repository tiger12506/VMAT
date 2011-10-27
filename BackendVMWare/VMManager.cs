using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;

namespace BackendVMWare
{
    public class VMManager
    {
        public void createServer()
        {
            IVirtualHost virtualHost = new VirtualHost(new VMWareVirtualHost());
            createServer(virtualHost,@"C:/img.vmx");
        }

        public void Jacob(string baseImageFilename, string newImageName, string[] otherStuffz)
        {

        }
        //from vmwaretools example
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
