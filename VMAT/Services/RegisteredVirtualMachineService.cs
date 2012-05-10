using System;
using System.IO;
using System.Linq;
using Vestris.VMWareLib.Tools.Windows;
using VMAT.Models.VMware;
using VMAT.Models;
using System.Collections.Generic;

namespace VMAT.Services
{
	public class RegisteredVirtualMachineService
	{
		// Consider changing all methods to static methods, so long as they are thread-safe
		private IVirtualMachine VM;
		private static IVirtualHost virtualHost;

		public RegisteredVirtualMachineService(string imagePathName)
		{
			GetVirtualHost();
			VM = virtualHost.Open(imagePathName);
		}

		public RegisteredVirtualMachineService(VMAT.Models.VirtualMachine vm) : this(vm.ImagePathName) { }

		public static IVirtualHost GetVirtualHost()
		{
			if (virtualHost == null)
				virtualHost = new VirtualHost();
			if (!virtualHost.IsConnected)
				virtualHost.ConnectToVMWareVIServer(AppConfiguration.GetVMwareHostAndPort(),
					AppConfiguration.GetVMwareUsername(), AppConfiguration.GetVMwarePassword());

		   return virtualHost;
		}

		public int GetStatus()
		{
			if (VM.IsPaused) return Models.VirtualMachine.PAUSED;
			else if (VM.IsRunning) return Models.VirtualMachine.RUNNING;
			else if (VM.IsSuspended) return Models.VirtualMachine.SUSPENDED;
			else if (VM.IsRecording || VM.IsReplaying) return Models.VirtualMachine.RUNNING;
			else if (VM.PowerState == 0x0001) return Models.VirtualMachine.POWERINGOFF;
			else if (VM.PowerState == 0x0004) return Models.VirtualMachine.POWERINGON;
			else return Models.VirtualMachine.STOPPED;
		}

		public bool IsRunning()
		{
			return VM.IsRunning;
		}

		public string GetIP()
		{
			try
			{
				LoginTools();
			}
			catch (Exception)
			{
				return null;
			}

			return VM.GuestVariables["ip"].Replace("\n", "").Replace("\r", "");
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
			try
			{
				LoginTools();
				Shell guestShell = new Shell(VM.VM); //TODO: mock?
				Shell.ShellOutput output = guestShell.RunCommandInGuest("hostname");
				return output.StdOut.Replace("\n", "").Replace("\r", "");
			}
			catch (Exception)
			{
				return null;
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
									WScript.Echo ""rename-fail, error = "" & Err.Number
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
		public void PowerOn()
		{
			VM.PowerOn();
		}

		/// <summary>
		/// If the machine is powered on or sleeping, power it off.
		/// Otherwise, do nothing.
		/// </summary>
		/// <returns>The time of shutdown</returns>
		public void PowerOff()
		{
			try
			{
                //tries to shut the machine down from inside the OS
				VM.PowerOff(0x0004, 120); //VIX_VMPOWEROP_FROM_GUEST from vix.h
			}
			catch (Exception)
			{
                //otherwise, forcefully cuts the power to it
				VM.PowerOff();
			}
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
			// Allow VM time to power off (may not be needed)
			System.Threading.Thread.Sleep(20 * 1000); 
			PowerOn();
		}

		private void LoginTools(bool waitLong = false)
		{
			if (!VM.IsRunning) 
				throw new InvalidOperationException("VM is not running");
			VM.WaitForToolsInGuest(waitLong ? 120 : 30); //TODO: refactor this out somewhere
			VM.LoginInGuest(AppConfiguration.GetVMsUsername(), AppConfiguration.GetVMsPassword());
		}

		/// <summary>
		/// Converts datasource-style path to physical network path
		/// </summary>
		/// <param name="PathName">Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"</param>
		/// <returns>Physical absolute path (from webserver to VM server), ie "//VMServer/VirtualMachines/Windows 7/Windows 7.VMx</returns>
		public static string ConvertPathToPhysical(string PathName)
		{
			return PathName.Replace(AppConfiguration.GetDatastore(), AppConfiguration.GetWebserverVmPath()).Replace('/', '\\');
		}

		/// <summary>
		/// Converts physical network path to datasource-style path
		/// </summary>
		/// <param name="PathName">Physical absolute path (from webserver to VM server), ie "//VMServer/VirtualMachines/Windows 7/Windows 7.VMx</param>
		/// <returns>Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"</returns>
		public static string ConvertPathToDatasource(string PathName)
		{
			return PathName.Replace(AppConfiguration.GetWebserverVmPath(), AppConfiguration.GetDatastore()).Replace('\\', '/');
		}

		public static ICollection<string> GetRegisteredVMImagePaths()
		{
			GetVirtualHost();
			var ret = virtualHost.RegisteredVirtualMachines.Select(v => v.PathName).ToList();

			return ret;
		}

		public static ICollection<string> GetBaseImageFiles()
		{
			List<string> filePaths = new List<string>(Directory.GetFiles(
				AppConfiguration.GetWebserverBaseImagePath(), "*.vmx", SearchOption.AllDirectories));

			return filePaths.Where(foo => foo.Substring(foo.Length - 4) == ".vmx").
				Select(foo => RegisteredVirtualMachineService.ConvertPathToDatasource(foo)).ToList();
		}
	}
}
