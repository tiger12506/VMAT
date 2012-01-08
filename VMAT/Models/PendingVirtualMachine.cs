using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Vestris.VMWareLib.Tools.Windows;
using VMAT.Models.VMware;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VMAT.Models
{
    public class PendingVirtualMachine
    {
        /// <summary>
        /// The image file that the VM will be running from (will be created). Should probably follow ProjectName/gapdevppppnnnnn.vmx, 
        /// but existing ones may not. p is project number, n is engineer-selected name (1-5 char).
        /// Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"
        /// </summary>
        [Required(ErrorMessage = "Base Image required")]
        public string ImagePathName { get; set; }

        [Required(ErrorMessage = "Machine Name Suffix required")]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "Machine Name Suffix must be 1-5 characters long")]
        public string MachineNameSuffix { get; set; }

        /// <summary>
        /// ie 137.112.147.145
        /// </summary>
        [StringLength(15, ErrorMessage ="Invalid IP Address")]
        public string IP { get; set; }

        /// <summary>
        /// Fully Qualified Domain Name, not all machines will be on domain. Will likely follow gapdevppppnnnnn. p is project number, n is engineer-selected name (1-5 char)
        /// </summary>
        public string HostnameWithDomain { get; set; }

        /// <summary>
        /// The base image file that the VM was originally copied from when first created. Unknown naming conventions, likely contains OS version.
        /// Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"
        /// </summary>
        public string BaseImageName { get; set; }

        /// <summary>
        /// String to identify project. 4 sections: "G"+Project Number (4-digit), Company, Site, tiny description. Project Identifier is latter 3 items.
        /// </summary>
        [Required(ErrorMessage = "Project Name required")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Project Name must be 4 digits long")]
        public string ProjectName { get; set; }


        /// <summary>
        /// Create VM using this object's info. Assume that IP is not already taken.
        /// </summary>
        /// <returns>New object representing VM</returns>
        public VirtualMachine CreateVM()
        {
            var vmm = new VirtualMachineManager();
            if (vmm.GetRegisteredVMs().Contains(ImagePathName))
                throw new InvalidDataException("Specified VM path already exists");
            if (!ImagePathName.StartsWith(AppConfiguration.GetDatastore()) || !BaseImageName.StartsWith(AppConfiguration.GetDatastore()))
                throw new InvalidDataException("Invalid ImagePathName or BaseImageName: doesn't contain datastore name");
            if (ImagePathName.Length < 8 || BaseImageName.Length < 8 || IP.Length < 7 || HostnameWithDomain.Length < 3)
                throw new InvalidDataException("CreateVM required field unspecified or too short");

            //this all really needs to be async, report status, and handle errors in individual steps better
            CopyVMFiles();

            // Allot VMware time to copy the file
            System.Threading.Thread.Sleep(8 * 1000);

            VirtualMachineManager.GetVH().Register(ImagePathName);

            var newVM = new VirtualMachine(ImagePathName);

            newVM.Status = VMStatus.Running;

            // Make triple-double-dog sure that the VM is online and ready.
            // Allow VM time to power on
            System.Threading.Thread.Sleep(180 * 1000);
            newVM.Reboot();
            // Allow VM time to power on
            System.Threading.Thread.Sleep(180 * 1000);

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
            //fails, error code 6, operation not supported. (because not supported on VMware Server 2) 
            //baseVM.Clone(VMWareVirtualMachineCloneType.Full, "[ha-datacenter/standard] Windows2003A/Windows2003A.vmx");  
        }

        private void CopyVMFiles()
        {
            string sourceVMX = VirtualMachine.ConvertPathToPhysical(BaseImageName);
            string sourceName = Path.GetFileNameWithoutExtension(sourceVMX);
            string sourcePath = Path.GetDirectoryName(sourceVMX);
            string destVMX = VirtualMachine.ConvertPathToPhysical(ImagePathName);
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
