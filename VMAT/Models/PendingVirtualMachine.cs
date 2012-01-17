using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Vestris.VMWareLib.Tools.Windows;
using VMAT.Models.VMware;
using VMAT.ViewModels;

namespace VMAT.Models
{
    public class PendingVirtualMachine : VirtualMachine
    {
        [StringLength(15, ErrorMessage ="Invalid IP Address")]
        [DisplayName("IP Address")]
        public string IP { get; set; }

        public PendingVirtualMachine()
        {
            // TODO: Implement
        }

        public PendingVirtualMachine(VirtualMachineFormViewModel vmForm)
        {
            // TODO: Fix to properly represent data
            ImagePathName = "path.vmx";
            BaseImageName = vmForm.BaseImageFile;
            IP = vmForm.IP1;
        }

        /// <summary>
        /// Create VM using this object's info. Assume that IP is not already taken.
        /// </summary>
        /// <returns>New object representing VM</returns>
        public RegisteredVirtualMachine CreateVM()
        {
            var vmm = new VirtualMachineManager();

            if (vmm.GetRegisteredVMImagePaths().Contains(ImagePathName))
                throw new InvalidDataException("Specified VM path already exists");
            if (!ImagePathName.StartsWith(AppConfiguration.GetDatastore()) || !BaseImageName.StartsWith(AppConfiguration.GetDatastore()))
                throw new InvalidDataException("Invalid ImagePathName or BaseImageName: doesn't contain datastore name");
            if (ImagePathName.Length < 8 || BaseImageName.Length < 8 || IP.Length < 7 || Hostname.Length < 3)
                throw new InvalidDataException("CreateVM required field unspecified or too short");

            //this all really needs to be async, report status, and handle errors in individual steps better
            CopyVMFiles();

            // Allot VMware time to copy the file
            System.Threading.Thread.Sleep(8 * 1000);

            VirtualMachineManager.GetVirtualHost().Register(ImagePathName);

            var newVM = new RegisteredVirtualMachine(ImagePathName);

            return newVM;

            //http://vmwaretasks.codeplex.com/discussions/276715
            //"[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx"
            //http://communities.vmware.com/message/1688542#1688542
            //http://panoskrt.wordpress.com/2009/01/20/clone-virtual-machine-on-vmware-server-20/
            //we don't seem to have vmware-vdiskmanager 

            //failed try:
            //var baseVM = openVM(info.BaseImageName);
            //var baseVM = openVM("[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx");
            //fails, error code 6, operation not supported. (because not supported on VMware Server 2) 
            //baseVM.Clone(VMWareVirtualMachineCloneType.Full, "[ha-datacenter/standard] Windows2003A/Windows2003A.vmx");  
        }

        private void CopyVMFiles()
        {
            string sourceVMX = VirtualMachineManager.ConvertPathToPhysical(BaseImageName);
            string sourceName = Path.GetFileNameWithoutExtension(sourceVMX);
            string sourcePath = Path.GetDirectoryName(sourceVMX);
            string destVMX = VirtualMachineManager.ConvertPathToPhysical(ImagePathName);
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
