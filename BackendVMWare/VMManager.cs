using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;
using System.IO;

namespace BackendVMWare
{
    public class VMManager
    {
        IVirtualHost vh;
        public VMManager(IVirtualHost vh)
        {
            this.vh = vh;
        }

        public VMManager()
        {
            this.vh = new VirtualHost();
        }
        private void connectVH()
        {
            if (vh.IsConnected) return;
            vh.ConnectToVMWareVIServer("vmat.csse.rose-hulman.edu:8333", "csse department", "Vmat1234");
            
        }
        private IVirtualMachine openVM(string imagePathName)
        {
            return vh.Open(imagePathName);
        }
        public IEnumerable<string> getRegisteredVMs()
        {
            connectVH();
            foreach(VirtualMachine v in vh.RegisteredVirtualMachines) {
                try
                {
                    string p = v.PathName;
                    if (v.IsRunning)
                    {
                        string ip = v.GuestVariables["ip"];
                        var rcv = v.RuntimeConfigVariables;
                    }
                }
                catch (Exception e) { }

            }


            var ret = vh.RegisteredVirtualMachines.Select(v => v.PathName);

            return ret;
        }
        // given a name, looks up all info about the VM
        public VMInfo getInfo(string imagePathName)
        {
            var vmi = new VMInfo();
            var vm=vh.Open(imagePathName);
            vmi.setFields(vh, vm);
            

            return vmi;
        }
        //Create VM using provided info (Created, LastRunning fields ignored)
        //Assume that IP is not already taken (tracking is done by frontend; can switch)
        public VMInfo createVM(VMInfo newInfo)
        {

            //var baseVM = openVM(info.BaseImageName);
            //var baseVM = openVM("[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx");
            string sourcePath = Path.GetDirectoryName(VMInfo.convertPathToPhysical(newInfo.BaseImageName));
            
            string destPath = VMInfo.convertPathToPhysical(newInfo.ImagePathName);
            //Now Create all of the directories
            /*foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath));

            */
            //baseVM.Clone(VMWareVirtualMachineCloneType.Full, "[ha-datacenter/standard] Windows2003A/Windows2003A.vmx");  fails, error code 6, operation not supported

            return new VMInfo();
        }
        //Mark server as active, idle (pause & don't autostart), or archived (stops & archives)
        public void updateLifecycle(string imagePathName, VMLifecycle newLifecycle)
        {

        }
        //Start up, shut down, or pause server
        public void updateStatus(string imagePathName, VMStatus newStatus)
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
        public int vmTest() {
            VMWareVirtualHost vh1=new VMWareVirtualHost();
            VMWareVirtualHost vh2=new VMWareVirtualHost();
            try
            {
                vh1.ConnectToVMWareVIServer("vmat.reshall.rose-hulman.edu:8333", "Nathan", "Vmat1234", 15);//reshall
                vh2.ConnectToVMWareVIServer("vmat.csse.rose-hulman.edu:8333", "csse department", "Vmat1234");
            
            }
            catch (VMWareException vme)
            {
                ulong c=vme.ErrorCode;
            }

            int c1 = vh1.RunningVirtualMachines.Count();
            int c2 = vh2.RunningVirtualMachines.Count();
            try
            {
                VMWareVirtualMachine vm = vh2.RunningVirtualMachines.FirstOrDefault();
                if (vm != null)
                {
                    vm.LoginInGuest("John", "Vmat1234");
                    vm.CreateDirectoryInGuest("C:/ThisIsTestYeah");
                }
                
            }
            catch (Exception e) { }
            return c2;
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
