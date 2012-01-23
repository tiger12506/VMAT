﻿using System;
using System.IO;
using System.Linq;
using VMAT.Models;

namespace VMAT.Services
{
    public class CreateVirtualMachineService
    {
        PendingVirtualMachine VM;

        public CreateVirtualMachineService() { }

        public CreateVirtualMachineService(PendingVirtualMachine vm)
        {
            VM = vm;
        }

        /// <summary>
        /// Create VM using this object's info. Assume that IP is not already taken.
        /// </summary>
        /// <returns>New object representing VM</returns>
        public RegisteredVirtualMachine CreateVM()
        {
            if (RegisteredVirtualMachineService.GetRegisteredVMImagePaths().Contains(VM.ImagePathName))
                throw new InvalidDataException("Specified VM path already exists");
            if (!VM.ImagePathName.StartsWith(AppConfiguration.GetDatastore()) || !VM.BaseImageName.StartsWith(AppConfiguration.GetDatastore()))
                throw new InvalidDataException("Invalid ImagePathName or BaseImageName: doesn't contain datastore name");
            if (VM.ImagePathName.Length < 8 || VM.BaseImageName.Length < 8 || VM.IP.Length < 7 || VM.Hostname.Length < 3)
                throw new InvalidDataException("CreateVM required field unspecified or too short");

            //this all really needs to be async, report status, and handle errors in individual steps better
            CopyVMFiles(VM.BaseImageName, VM.ImagePathName);

            // Allot VMware time to copy the file
            System.Threading.Thread.Sleep(8 * 1000);

            VirtualMachineManager.GetVirtualHost().Register(VM.ImagePathName);

            var newVM = new RegisteredVirtualMachine(VM.ImagePathName);

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
