using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vestris.VMWareLib;
using Vestris.VMWareLib.Tools.Windows;
using System.IO;

namespace BackendVMWare
{
    public enum VMStatus
    {
        Stopped,
        Paused, //still in memory, like sleep
        Suspended, //to disk, like hibernate. may not be supported
        Running
    }

    public enum VMLifecycle
    {
        Active,
        Idle, //not auto-start
        Archived //files compressed & moved elsewhere
    }

    /// <summary>
    /// Used when creating new VM.
    /// Only contains fields required when creating new VM, and fields not linked to actual VM.
    /// </summary>
    public class PendingVM
    {
        //all paths are "datastore-style," ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"
        //query from VM
        /// <summary>
        /// The image file that the VM will be running from (will be created). Should probably follow ProjectName/gapdevppppnnnnn.vmx, but existing ones may not. p is project number, n is engineer-selected name (1-5 char)
        /// Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"
        /// </summary>
        public string ImagePathName { get; set; } //ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx" actual HDD location harder to find

        /// <summary>
        /// ie 137.112.147.145
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// Fully Qualified Domain Name, not all machines will be on domain. Will likely follow gapdevppppnnnnn. p is project number, n is engineer-selected name (1-5 char)
        /// </summary>
        public string HostnameWithDomain { get; set; }

        //can't really query so must store elsewhere or somehow derive (ie from naming conventions)
        /// <summary>
        /// The base image file that the VM was originally copied from when first created. Unknown naming conventions, likely contains OS version.
        /// Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"
        /// </summary>
        public string BaseImageName { get; set; } //the pathname of the image file this VM was copied from
        /// <summary>
        /// String to identify project. 4 sections: "G"+Project Number (4-digit), Company, Site, tiny description. Project Identifier is latter 3 items.
        /// </summary>
        public string ProjectName { get; set; } //the name of the project, could be changed, format in reqs doc





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
    }
    /*
    public class CachedVMInfo : VMInfo
    {
        public CachedVMInfo(IVirtualMachine vm) : base(vm) { }
        public CachedVMInfo(IVirtualMachine vm) : base(vm) { }


        public string IP
        {
            get
            {
                try
                {
                    Persistence.GetIP(GetMachineName(this.ImagePathName));
                }
                catch (Exception) { }
                return ((VMInfo)this).IP;
            }
        }
    }*/
    /// <summary>
    /// Stores info about a particular VM.
    /// </summary>
    public class VMInfo
    {


        public static IEnumerable<string> GetBaseImageFiles()
        {
            List<string> filePaths = new List<string>(Directory.GetFiles(Config.GetWebserverVmPath(), "*.vmx", SearchOption.AllDirectories));
            return filePaths.Select(foo => VMInfo.ConvertPathToDatasource(foo));
        }


        /* I think those fields cover everything we'll need to show, but that should be verified. 
         * 
         * Note about querying these fields: 
         * We may want to cache everything we want to show about VMs, even if it can be queried:
         * queries may be slow and some data can't be accessed if VM's off.
         */

        public VMInfo(IVirtualMachine vm)
        {
            this.VM = vm;
            this.ImagePathName = VM.PathName;
            //this.MachineName = ImagePathName.Substring((ImagePathName.LastIndexOf('/') + 1));
        }

        // TODO error handle, check if starts with getDatasource
        public VMInfo(string imagePathName)
            : this(VMManager.GetVH().Open(imagePathName))
        { }


        //query from VM
        /// <summary>
        /// The current image file that the VM is running on. Will not be modifiable. Should probably follow ProjectName/gapdevppppnnnnn.vmx, but existing ones may not. p is project number, n is engineer-selected name (1-5 char)
        /// Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"
        /// </summary>
        public string ImagePathName { get; private set; } //ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx" actual HDD location harder to find
        /// <summary>
        /// Stopped, Paused (in memory), Suspended (to disk), Running
        /// </summary>
        public VMStatus Status
        {
            get
            {
                if (VM.IsPaused) return VMStatus.Paused;
                else if (VM.IsRunning) return VMStatus.Running;
                else if (VM.IsSuspended) return VMStatus.Suspended;
                else if (VM.IsRecording || VM.IsReplaying) return VMStatus.Running;
                else return VMStatus.Stopped;
            }
            set
            {
                if (value == Status) return;
                switch (value)
                {
                    case VMStatus.Running:
                        if (Status == VMStatus.Paused) VM.Unpause();
                        else if (Status == VMStatus.Stopped) VM.PowerOn();
                        else throw new InvalidOperationException("Cannot set VM to run, invalid state " + Status);
                        break;
                    case VMStatus.Stopped:
                        try
                        {
                            VM.PowerOff(0x0004, 120); //VIX_VMPOWEROP_FROM_GUEST from vix.h
                        }
                        catch (Exception)
                        {
                            VM.PowerOff();
                        }
                        break;
                    case VMStatus.Paused:
                        VM.Pause();
                        break;
                }
            }
        }
        public void Reboot()
        {
            Status = VMStatus.Stopped;
            System.Threading.Thread.Sleep(20 * 1000); //allow VM time to power off (may not be needed)
            Status = VMStatus.Running;
        }
        //probably query, uncertain
        public DateTime LastStopped { get; set; }
        public DateTime LastStarted { get; set; }
        public DateTime LastBackuped { get; set; }
        public DateTime LastArchived { get; set; }
        public DateTime Created { get; set; }

        private void LoginTools(bool waitLong=false)
        {
            if (!VM.IsRunning) throw new InvalidOperationException("VM is not running");
            VM.WaitForToolsInGuest(waitLong?120:30); //todo refactor this out somewhere
            VM.LoginInGuest(Config.GetVMsUsername(), Config.GetVMsPassword());
        }
        //query from running vm

        /// <summary>
        /// ie 137.112.147.145
        /// Note: caller must reboot after setting. 
        /// </summary>
        public string IP
        {
            get
            {
                try
                {
                    if (!this.VM.IsRunning) return "offline";
                    LoginTools();
                    var ret=this.VM.GuestVariables["ip"];
                    Persistence.WriteVMIP(ImagePathName, ret);
                    return ret;
                }
                catch (Exception e)
                {
                    return "IP error";
                }
            }
            set
            {
                if (value.Length < 7)
                    throw new InvalidDataException("IP too short");
                Shell.ShellOutput output = new Shell.ShellOutput();

                LoginTools(true);
                Shell guestShell = new Shell(VM.VM); //todo mock?
                string cmd = "netsh interface ip set address " + Config.GetNetworkInterfaceName() + " static " + value + " 255.255.255.0";
                output = guestShell.RunCommandInGuest(cmd);

                if (output.StdOut.Length < 12) //depending on OS, should print "Ok.\n\n" or not print any output if success
                {
                    return;
                }
                else if (output.StdOut.Contains("failed"))
                {
                    throw new InvalidOperationException(cmd + "\n" + output.StdOut);
                }
                else
                {
                    throw new InvalidProgramException(cmd + "\n" + output.StdOut);
                }
            }
        }
        /// <summary>
        /// Fully Qualified Domain Name, not all machines will be on domain. Will likely follow gapdevppppnnnnn. p is project number, n is engineer-selected name (1-5 char)
        /// Note: caller must reboot after setting. 
        /// </summary>
        public string HostnameWithDomain
        {
            get
            {
                if (!VM.IsRunning) return "offline";
                try
                {
                    LoginTools();
                    Shell guestShell = new Shell(this.VM.VM); //todo mock?
                    Shell.ShellOutput output = guestShell.RunCommandInGuest("hostname");
                    return output.StdOut;
                }
                catch (TimeoutException)
                {
                    return "name_timeout";
                }
                catch (Exception) {
                    return "name_error";
                }
            }
            set
            {
                if (value.Length < 3)
                    throw new ArgumentException("Hostname too short");
                Shell.ShellOutput output = new Shell.ShellOutput();

                LoginTools(true);
                var renameScriptHost = Config.GetWebserverTmpPath() + "renamecomp.vbs";
                if (!File.Exists(renameScriptHost))
                {
                    if (!Directory.Exists(Config.GetWebserverTmpPath()))
                        Directory.CreateDirectory(Config.GetWebserverTmpPath());
                    //if (!File.Exists(renameScriptHost))
                    //File.Create(renameScriptHost);
                    File.WriteAllText(renameScriptHost, @"Set objWMIService = GetObject(""Winmgmts:root\cimv2"")

For Each objComputer in _
    objWMIService.InstancesOf(""Win32_ComputerSystem"")
    Name = WScript.Arguments.Item(0)
    Return = objComputer.rename(Name,NULL,NULL)
        If Return <> 0 Then
           WScript.Echo ""rename-fail""
        Else
           WScript.Echo ""rename-succ""
        End If
Next
");
                }
                
                //note: Host means Webserver, NOT VMware server
                if (!VM.DirectoryExistsInGuest(@"C:\temp"))
                    VM.CreateDirectoryInGuest(@"C:\temp");
                if (!VM.FileExistsInGuest(@"C:\temp\renamecomp.vbs"))
                    VM.CopyFileFromHostToGuest(renameScriptHost, @"C:\temp\renamecomp.vbs");

                Shell guestShell = new Shell(this.VM.VM); //todo mock?
                output = guestShell.RunCommandInGuest(@"cscript c:\temp\renamecomp.vbs " + value);
                //output = guestShell.RunCommandInGuest(@"cscript "+Config.getWebserverVMPath()+@"\renamecomp.vbs " + newName);

                if (output.StdOut.Contains("rename-succ"))
                    return;
                else if (output.StdOut.Contains("rename-fail"))
                    throw new InvalidOperationException(output.StdOut);
                else
                    throw new InvalidOperationException(output.StdOut);
            }
        }
        /// <summary>
        /// Not stored separately, generated from Image Path Name. 1-5 char engineer-selected name
        /// </summary>
        public string MachineName
        {
            get {
                return ImagePathName.Substring(ImagePathName.LastIndexOf("\\") + 1, ImagePathName.LastIndexOf("."));
            }
        }
        //can't really query so must store elsewhere or somehow derive (ie from naming conventions)
        /// <summary>
        /// The base image file that the VM was originally copied from when first created. Unknown naming conventions, likely contains OS version.
        /// Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"
        /// </summary>
        public string BaseImageName { get; set; }
        /// <summary>
        /// String to identify project. 4 sections: "G"+Project Number (4-digit), Company, Site, tiny description. Project Identifier is latter 3 items.
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Active, Idle, Archived
        /// </summary>
        public VMLifecycle Lifecycle { get; set; } //if archived, won't be able to query a thing obviously
        
        /// <summary>
        /// The VMwareTasks API object behind this VM instance.
        /// </summary>
        private IVirtualMachine VM { get; set; }

        //should probably move, ie to CachedVM
        private string GetCacheIP()
        {
            string ipAddress = Persistence.GetIP(MachineName);

            return ipAddress;
        }

        /// <summary>
        /// Converts datasource-style path to physical network path
        /// </summary>
        /// <param name="PathName">Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"</param>
        /// <returns>Physical absolute path (from webserver to VM server), ie "//VMServer/VirtualMachines/Windows 7/Windows 7.VMx</returns>
        public static string ConvertPathToPhysical(string PathName)
        {
            return PathName.Replace(Config.GetDatastore(), Config.GetWebserverVmPath()).Replace('/', '\\');
        }

        /// <summary>
        /// Converts physical network path to datasource-style path
        /// </summary>
        /// <param name="PathName">Physical absolute path (from webserver to VM server), ie "//VMServer/VirtualMachines/Windows 7/Windows 7.VMx</param>
        /// <returns>Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"</returns>
        public static string ConvertPathToDatasource(string PathName)
        {
            return PathName.Replace(Config.GetWebserverVmPath(), Config.GetDatastore()).Replace('\\', '/');
        }

        public static string GetMachineName(string imagePathName)
        {
//            string imagePathNameTail = imagePathName.Substring(imagePathName.LastIndexOf("\\") + 1);
//            string machineName = imagePathNameTail.Substring(0, imagePathNameTail.LastIndexOf("."));
//            return machineName;
            return imagePathName.Substring(imagePathName.LastIndexOf("\\") + 1, imagePathName.LastIndexOf("."));
        }

    }
}
