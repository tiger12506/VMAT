using System;
using System.IO;
using System.Linq;
using VMAT.Models;

namespace VMAT.Services
{
    public class CreateVirtualMachineService
    {
        VirtualMachine VM;

        public CreateVirtualMachineService() { }

        public CreateVirtualMachineService(VirtualMachine vm)
        {
            VM = vm;
        }

        public VirtualMachine CreateVM()
        {
            if (RegisteredVirtualMachineService.GetRegisteredVMImagePaths().Contains(VM.ImagePathName))
                throw new InvalidDataException("Specified VM path already exists");
            if (!VM.ImagePathName.StartsWith(AppConfiguration.GetDatastore()) || !VM.BaseImageName.StartsWith(AppConfiguration.GetDatastore()))
                throw new InvalidDataException("Invalid ImagePathName or BaseImageName: doesn't contain datastore name");
            if (VM.ImagePathName.Length < 8 || VM.BaseImageName.Length < 8 || VM.IP.Length < 7)
                throw new InvalidDataException("CreateVM required field unspecified or too short");
            
            //this all really needs to be async, report status, and handle errors in individual steps better
            try
            {
                CopyVMFiles(VM.BaseImageName, VM.ImagePathName);
            }
            catch (Exception ex)
            {
                throw new SchedulerInfo("Error copying files, VM creation aborted.", ex);
            }

            // Allot time to finish copying the file
            System.Threading.Thread.Sleep(16 * 1000);

            RegisteredVirtualMachineService service = null;
            try
            {
                RegisteredVirtualMachineService.GetVirtualHost().Register(VM.ImagePathName);
                service = new RegisteredVirtualMachineService(VM.ImagePathName);
                // Make triple-double-dog sure that the VM is online and ready.
                // Allow VM time to power on
                service.PowerOn();
                System.Threading.Thread.Sleep(180 * 1000);

                // Allow VM time to reboot
                service.Reboot();
                System.Threading.Thread.Sleep(250 * 1000);
            }
            catch (Exception ex)
            {
                new SchedulerInfo("Error registering or first-booting new VM, will attempt to continue", ex).LogElmah();
            }
            SetIPHostname(service);

            // Allow VM time to reboot
            service.Reboot();
            System.Threading.Thread.Sleep(250 * 1000);

            return VM;

            //http://vmwaretasks.codeplex.com/discussions/276715
            //"[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx"
            //http://communities.vmware.com/message/1688542#1688542
            //http://panoskrt.wordpress.com/2009/01/20/clone-virtual-machine-on-vmware-server-20/
            //we don't seem to have vmware-vdiskmanager 

        }
        private void SetIPHostname(RegisteredVirtualMachineService service, bool retry = true)
        {
            try
            {
                System.Threading.Thread.Sleep(8 * 1000);
                service.SetHostname(VM.Hostname);
                System.Threading.Thread.Sleep(8 * 1000);
                service.SetIP(VM.IP);
                System.Threading.Thread.Sleep(8 * 1000);

            }
            catch (Exception ex)
            {
                if (retry) SetIPHostname(service, false);
                else new SchedulerInfo("Error setting IP or hostname of new VM, will need to be manually set but will continue", ex).LogElmah();
            }

        }
        private void CopyVMFiles(string baseImageName, string imagePathName)
        {
            string sourceVMX = RegisteredVirtualMachineService.ConvertPathToPhysical(baseImageName);
            string sourceName = Path.GetFileNameWithoutExtension(sourceVMX);
            string sourcePath = Path.GetDirectoryName(sourceVMX);
            string destVMX = RegisteredVirtualMachineService.ConvertPathToPhysical(imagePathName);
            string destName = Path.GetFileNameWithoutExtension(destVMX);
            string destPath = Path.GetDirectoryName(destVMX);

            Directory.CreateDirectory(destPath);
            File.Copy(sourceVMX, destVMX);

            foreach (string iPath in Directory.GetFiles(sourcePath, "*.vmdk", SearchOption.TopDirectoryOnly))
                File.Copy(iPath, iPath.Replace(sourcePath, destPath).Replace(sourceName, destName)); //can take several minutes

            String strFile = File.ReadAllText(destVMX);
            strFile = strFile.Replace(sourceName, destName);
            if (!strFile.Contains("uuid.action = \"create\""))
            {
                strFile += "\r\nuuid.action = \"create\"\r\n";
                strFile += "msg.autoAnswer = \"TRUE\"\r\n";
            }

            File.WriteAllText(destVMX, strFile);
        }
    }
}
