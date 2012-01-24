using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace VMAT.Services
{
    public class CreateVirtualMachineService
    {
        /*
        /// <summary>
        /// Create VM using this object's info. Assume that IP is not already taken.
        /// </summary>
        /// <returns>New object representing VM</returns>
        public VMInfo CreateVM()
        {

            var vmm = new VMManager();
            if (vmm.GetRegisteredVMs().Contains(ImagePathName))
                throw new InvalidDataException("Specified VM path already exists");
            if (!ImagePathName.StartsWith(Config.GetDatastore()) || !BaseImageName.StartsWith(Config.GetDatastore()))
                throw new InvalidDataException("Invalid ImagePathName or BaseImageName: doesn't contain datastore name");
            if (ImagePathName.Length < 8 || BaseImageName.Length < 8 || IP.Length < 7 || HostnameWithDomain.Length < 3)
                throw new InvalidDataException("CreateVM required field unspecified or too short");

            //this all really needs to be async, report status, and handle errors in individual steps better
            CopyVMFiles();

            System.Threading.Thread.Sleep(8 * 1000); //ensure file copied 

            VMManager.GetVH().Register(ImagePathName);

            var newVM = new VMInfo(ImagePathName);

            newVM.Status = VMStatus.Running;
        
            //make triple-double-dog sure that the VM is online and ready
            System.Threading.Thread.Sleep(180 * 1000); //allow VM time to power on
            newVM.Reboot();
            System.Threading.Thread.Sleep(180 * 1000); //allow VM time to power on
            try
            {

            }
            catch (TimeoutException)
            {
            }
            newVM.IP = IP;
            newVM.HostnameWithDomain = HostnameWithDomain;
            newVM.BaseImageName = BaseImageName;
            newVM.ProjectName = ProjectName;
            newVM.Created = System.DateTime.Now;

            newVM.Reboot();

            return newVM;

            //http://vmwaretasks.codeplex.com/discussions/276715
            //"[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx"
            //http://communities.vmware.com/message/1688542#1688542
            //http://panoskrt.wordpress.com/2009/01/20/clone-virtual-machine-on-vmware-server-20/
            //we don't seem to have vmware-vdiskmanager 



            //failed try:
            //var baseVM = openVM(info.BaseImageName);
            //var baseVM = openVM("[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx");
            //baseVM.Clone(VMWareVirtualMachineCloneType.Full, "[ha-datacenter/standard] Windows2003A/Windows2003A.vmx");  fails, error code 6, operation not supported. (because not supported on VMware Server 2) 

        }

        private void CopyVMFiles()
        {
            string sourceVMX = VMInfo.ConvertPathToPhysical(BaseImageName);
            string sourceName = Path.GetFileNameWithoutExtension(sourceVMX);
            string sourcePath = Path.GetDirectoryName(sourceVMX);
            string destVMX = VMInfo.ConvertPathToPhysical(ImagePathName);
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
         * */
    }
}