using System;
using System.IO;
using System.Linq;
using Vestris.VMWareLib.Tools.Windows;
using VMAT.Models.VMware;
using VMAT.Models;

namespace VMAT.Services
{
    public class RegisteredVirtualMachineService
    {
        private DataEntities dataDB = new DataEntities();
        private IVirtualMachine VM;
        private RegisteredVirtualMachine virtualMachine;

        public RegisteredVirtualMachineService(string imagePathName)
        {
            VM = VirtualMachineManager.GetVirtualHost().Open(imagePathName);

            try
            {
                virtualMachine = dataDB.VirtualMachines.OfType<RegisteredVirtualMachine>().
                    Single(v => v.ImagePathName == imagePathName);
            }
            catch (InvalidOperationException)
            {
                virtualMachine = new RegisteredVirtualMachine(imagePathName);
            }
        }

        public VMStatus GetStatus()
        {
            if (VM.IsPaused) return VMStatus.Paused;
            else if (VM.IsRunning) return VMStatus.Running;
            else if (VM.IsSuspended) return VMStatus.Suspended;
            else if (VM.IsRecording || VM.IsReplaying) return VMStatus.Running;
            else if (VM.PowerState == 0x0001) return VMStatus.PoweringOff;
            else if (VM.PowerState == 0x0004) return VMStatus.PoweringOn;
            else return VMStatus.Stopped;
        }

        public string GetIP()
        {
            if (!VM.IsRunning)
                return virtualMachine.IP;
            else
            {
                LoginTools();
                return VM.GuestVariables["ip"].Replace("\n", "").Replace("\r", "");
            }
        }

        public void SetIP(string value)
        {
            if (value.Length < 7)
                throw new InvalidDataException("IP too short");
            Shell.ShellOutput output = new Shell.ShellOutput();

            LoginTools(true);
            Shell guestShell = new Shell(VM.VM); // TODO: mock?
            string cmd = "netsh interface ip set address " +
                AppConfiguration.GetNetworkInterfaceName() + " static " + value + " 255.255.255.0";
            output = guestShell.RunCommandInGuest(cmd);

            // Depending on OS, should print "Ok.\n\n" or not print any output if success
            if (output.StdOut.Length < 12)
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

        public string GetHostname()
        {
            if (!VM.IsRunning)
                return virtualMachine.Hostname;
            else
            {
                try
                {
                    LoginTools();
                    Shell guestShell = new Shell(VM.VM); //TODO: mock?
                    Shell.ShellOutput output = guestShell.RunCommandInGuest("hostname");
                    return output.StdOut.Replace("\n", "").Replace("\r", "");
                }
                catch (TimeoutException)
                {
                    return "hostname_timeout";
                }
                catch (Exception)
                {
                    return "hostname_error";
                }
            }
        }

        public void SetHostname(string hostname)
        {
            if (hostname.Length < 3)
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

            Shell guestShell = new Shell(VM.VM); //TODO: mock?
            output = guestShell.RunCommandInGuest(@"cscript c:\temp\renamecomp.vbs " + hostname);
            //output = guestShell.RunCommandInGuest(@"cscript "+Config.getWebserverVMPath()+@"\renamecomp.vbs " + newName);

            if (output.StdOut.Contains("rename-succ"))
                return;
            else if (output.StdOut.Contains("rename-fail"))
                throw new InvalidOperationException(output.StdOut);
            else
                throw new InvalidOperationException(output.StdOut);
        }

        /// <summary>
        /// If the machine is powered off, power it on. Otherwise, do nothing.
        /// </summary>
        /// <returns>The time of startup</returns>
        public DateTime PowerOn()
        {
            VM.PowerOn();
            virtualMachine.LastStarted = DateTime.Now;
            dataDB.SaveChanges();

            return virtualMachine.LastStarted;
        }

        /// <summary>
        /// If the machine is powered on or sleeping, power it off.
        /// Otherwise, do nothing.
        /// </summary>
        /// <returns>The time of shutdown</returns>
        public DateTime PowerOff()
        {
            try
            {
                VM.PowerOff(0x0004, 120); //VIX_VMPOWEROP_FROM_GUEST from vix.h
            }
            catch (Exception)
            {
                VM.PowerOff();
            }
            finally
            {
                virtualMachine.LastStopped = DateTime.Now;
                dataDB.SaveChanges();
            }

            return virtualMachine.LastStopped;
        }

        /// <summary>
        /// Put the machine in a sleeping state.
        /// </summary>
        public void Pause()
        {
            VM.Pause();
        }

        /// <summary>
        /// Unsleep the machine.
        /// </summary>
        public void Unpause()
        {
            VM.Unpause();
        }

        public void Reboot()
        {
            PowerOff();
            System.Threading.Thread.Sleep(20 * 1000); //allow VM time to power off (may not be needed)
            PowerOn();
        }

        private void LoginTools(bool waitLong = false)
        {
            // TODO: Handle in case powered off better
            if (!VM.IsRunning) throw new InvalidOperationException("VM is not running");
            VM.WaitForToolsInGuest(waitLong ? 120 : 30); //TODO: refactor this out somewhere
            VM.LoginInGuest(AppConfiguration.GetVMsUsername(), AppConfiguration.GetVMsPassword());
        }
    }
}
