﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
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
        private void ConnectVH()
        {
            if (vh.IsConnected) return;
            vh.ConnectToVMWareVIServer("vmat.csse.rose-hulman.edu:8333", "csse department", "Vmat1234");
            
        }
        private IVirtualMachine OpenVM(string imagePathName)
        {
            return vh.Open(imagePathName);
        }
        public IEnumerable<string> GetRegisteredVMs()
        {
            ConnectVH();
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
        public VMInfo GetInfo(string imagePathName)
        {
            var vmi = new VMInfo();
            var vm=vh.Open(imagePathName);
            vmi.setFields(vh, vm);
            

            return vmi;
        }
        //Create VM using provided info (Created, LastRunning fields ignored)
        //Assume that IP is not already taken (tracking is done by frontend; can switch)
        public VMInfo CreateVM(VMInfo newInfo)
        {
            //"[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx"
            //http://communities.vmware.com/message/1688542#1688542
            //http://panoskrt.wordpress.com/2009/01/20/clone-virtual-machine-on-vmware-server-20/
            //we don't seem to have vmware-vdiskmanager 

            string sourceVMX = VMInfo.ConvertPathToPhysical(newInfo.BaseImageName);
            string sourceName = Path.GetFileNameWithoutExtension(sourceVMX);
            string sourcePath = Path.GetDirectoryName(sourceVMX);
            string destVMX = VMInfo.ConvertPathToPhysical(newInfo.ImagePathName);
            string destName = Path.GetFileNameWithoutExtension(destVMX);
            string destPath = Path.GetDirectoryName(destVMX);
            
            Directory.CreateDirectory(destPath);
            File.Copy(sourceVMX, destVMX);
            foreach(string iPath in Directory.GetFiles(sourcePath,"*.vmdk",SearchOption.TopDirectoryOnly))
                File.Copy(iPath, iPath.Replace(sourcePath,destPath).Replace(sourceName,destName)); //can take several minutes

            String strFile = File.ReadAllText(destVMX);
            strFile = strFile.Replace(sourceName, destName);
            strFile += "\r\nuuid.action = \"create\"\r\n";
            strFile += "msg.autoAnswer = \"TRUE\"\r\n";
            File.WriteAllText(destVMX, strFile);
            
            ConnectVH();
            var ss = VMInfo.ConvertPathToDatasource(destVMX);
            vh.Register(ss);

            var newVM = vh.Open(ss);
            string s=newVM.PathName;
            bool b = newVM.IsRunning;
            newVM.PowerOn();
            //http://vmwaretasks.codeplex.com/discussions/276715

            return GetInfo(ss);

            //failed try:
            //var baseVM = openVM(info.BaseImageName);
            //var baseVM = openVM("[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx");
            //baseVM.Clone(VMWareVirtualMachineCloneType.Full, "[ha-datacenter/standard] Windows2003A/Windows2003A.vmx");  fails, error code 6, operation not supported. (because not supported on VMware Server 2) 
            
        }
        //Mark server as active, idle (pause & don't autostart), or archived (stops & archives)
        public void UpdateLifecycle(string imagePathName, VMLifecycle newLifecycle)
        {

        }
        //Start up, shut down, or pause server
        public void UpdateStatus(string imagePathName, VMStatus newStatus)
        {

        }

        /* also need setting config options, which may require reading XML (since backend will have no persistence)
            IP address allowable range
            Set maximum simultaneous running server count
            Set VM creation, backup, and archive batch process times
            Set up list of base images & locations (optional, can just use folder names)
         */
    



        private void CreateServer()
        {
            IVirtualHost virtualHost = new VirtualHost(new VMWareVirtualHost());
            CreateServer(virtualHost, @"C:/img.vmx");
        }
        public int VMTest() {
            VMWareVirtualHost vh1=new VMWareVirtualHost();
            VMWareVirtualHost vh2=new VMWareVirtualHost();

            try
            {
                vh1.ConnectToVMWareVIServer("vmat.reshall.rose-hulman.edu:8333", "Nathan", "Vmat1234", 15);//reshall
                vh2.ConnectToVMWareVIServer("vmat.csse.rose-hulman.edu:8333", "csse department", "Vmat1234");

            }
            catch (VMWareException vme)
            {
                ulong c = vme.ErrorCode;
            }
            catch (COMException cme)
            {
                return cme.ErrorCode;
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
        public IVirtualMachine CreateServer(IVirtualHost virtualHost, string imageLocation)
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
