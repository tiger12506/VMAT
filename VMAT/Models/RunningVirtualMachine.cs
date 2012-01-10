using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Vestris.VMWareLib.Tools.Windows;
using VMAT.Models.VMware;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace VMAT.Models
{
    public class RunningVirtualMachine : VirtualMachine
    {
        /// <summary>
        /// Stopped, Paused (in memory), Suspended (to disk), Running
        /// </summary>
        [ScaffoldColumn(false)]
        [DisplayName("Status")]
        public VMStatus Status
        {
            get
            {
                if (VM.IsPaused) return VMStatus.Paused;
                else if (VM.IsRunning) return VMStatus.Running;
                else if (VM.IsSuspended) return VMStatus.Suspended;
                else if (VM.IsRecording || VM.IsReplaying) return VMStatus.Running;
                else if (VM.PowerState == 0x0001) return VMStatus.PoweringOff;
                else if (VM.PowerState == 0x0004) return VMStatus.PoweringOn;
                else return VMStatus.Stopped;
            }

            private set
            {
                return;
            }
        }

        /// <summary>
        /// ie 137.112.147.145
        /// Note: caller must reboot after setting.
        /// </summary>
        [StringLength(15, ErrorMessage = "Invalid IP Address")]
        [DisplayName("IP Address")]
        public string IP
        {
            get
            {
                try
                {
                    if (!this.VM.IsRunning) return "offline";
                    LoginTools();
                    var ret = this.VM.GuestVariables["ip"].Replace("\n", "").Replace("\r", "");
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
                Shell guestShell = new Shell(VM.VM); //TODO: mock?
                string cmd = "netsh interface ip set address " + AppConfiguration.GetNetworkInterfaceName() + " static " + value + " 255.255.255.0";
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

        [DisplayName("Hostname")]
        new public string Hostname
        {
            get
            {
                if (!VM.IsRunning) return "offline";
                try
                {
                    LoginTools();
                    Shell guestShell = new Shell(this.VM.VM); //TODO: mock?
                    Shell.ShellOutput output = guestShell.RunCommandInGuest("hostname");
                    return output.StdOut.Replace("\n", "").Replace("\r", "");
                }
                catch (TimeoutException)
                {
                    return "name_timeout";
                }
                catch (Exception)
                {
                    return "name_error";
                }
            }

            set
            {
                if (value.Length < 3)
                    throw new ArgumentException("Hostname too short");
                Shell.ShellOutput output = new Shell.ShellOutput();

                LoginTools(true);
                var renameScriptHost = AppConfiguration.GetWebserverTmpPath() + "renamecomp.vbs";
                if (!File.Exists(renameScriptHost))
                {
                    if (!Directory.Exists(AppConfiguration.GetWebserverTmpPath()))
                        Directory.CreateDirectory(AppConfiguration.GetWebserverTmpPath());
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

                Shell guestShell = new Shell(this.VM.VM); //TODO: mock?
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
        /// Active, Idle, Archived (won't be able to query)
        /// </summary>
        [DisplayName("Lifecycle")]
        public VMLifecycle Lifecycle { get; set; }

        /// <summary>
        /// The VMwareTasks API object behind this VM instance.
        /// </summary>
        private IVirtualMachine VM { get; set; }

        [DisplayName("Last Shutdown")]
        public DateTime LastStopped { get; set; }

        [DisplayName("Last Started")]
        public DateTime LastStarted { get; set; }

        [DisplayName("Last Backed Up")]
        public DateTime LastBackuped { get; set; }

        [DisplayName("Last Archived")]
        public DateTime LastArchived { get; set; }

        [DisplayName("Created")]
        public DateTime Created { get; set; }

        public RunningVirtualMachine(IVirtualMachine vm) : base()
        {
            VM = vm;
            ImagePathName = vm.PathName;
        }

        // TODO: error handle, check if starts with getDatasource
        public RunningVirtualMachine(string imagePathName)
            : this(VirtualMachineManager.GetVirtualHost().Open(imagePathName))
        { }

        /// <summary>
        /// If the machine is powered off, power it on. If the machine is sleeping, unsleep it.
        /// Otherwise, do nothing.
        /// </summary>
        public void PowerOn()
        {
            if (!(Status == VMStatus.Running || Status == VMStatus.PoweringOn))
            {
                if (Status == VMStatus.Paused)
                {
                    Unpause();
                }
                else if (Status == VMStatus.Stopped)
                {
                    Status = VMStatus.PoweringOn;
                    VM.PowerOn();
                    Status = VMStatus.Running;
                }
                else throw new InvalidOperationException("Cannot set VM to run, invalid state " + Status);
            }
        }

        /// <summary>
        /// If the machine is powered on or sleeping, power it off.
        /// Otherwise, do nothing.
        /// </summary>
        public void PowerOff()
        {
            if (!(Status == VMStatus.Stopped || Status == VMStatus.PoweringOff))
            {
                try
                {
                    Status = VMStatus.PoweringOff;
                    VM.PowerOff(0x0004, 120); //VIX_VMPOWEROP_FROM_GUEST from vix.h
                    Status = VMStatus.Stopped;
                }
                catch (Exception)
                {
                    Status = VMStatus.PoweringOff;
                    VM.PowerOff();
                    Status = VMStatus.Stopped;
                }
            }
        }

        /// <summary>
        /// Put the machine in a sleeping state.
        /// </summary>
        public void Pause()
        {
            if (Status == VMStatus.Running)
            {
                VM.Pause();
                Status = VMStatus.Paused;
            }
        }

        /// <summary>
        /// Unsleep the machine.
        /// </summary>
        public void Unpause()
        {
            if (Status == VMStatus.Paused)
            {
                VM.Unpause();
                Status = VMStatus.Running;
            }
        }

        public void Reboot()
        {
            Status = VMStatus.Stopped;
            System.Threading.Thread.Sleep(20 * 1000); //allow VM time to power off (may not be needed)
            Status = VMStatus.Running;
        }

        private void LoginTools(bool waitLong=false)
        {
            if (!VM.IsRunning) throw new InvalidOperationException("VM is not running");
            VM.WaitForToolsInGuest(waitLong?120:30); //TODO: refactor this out somewhere
            VM.LoginInGuest(AppConfiguration.GetVMsUsername(), AppConfiguration.GetVMsPassword());
        }

        //should probably move, ie to CachedVM
        private string GetCacheIP()
        {
            string ipAddress = Persistence.GetIP(GetMachineName());

            return ipAddress;
        }
    }
}