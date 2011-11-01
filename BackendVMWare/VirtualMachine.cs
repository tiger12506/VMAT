using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;
namespace BackendVMWare
{
    public class VirtualMachine : IVirtualMachine
    {
        
            /*Function Set-WinVMIP ($VM, $HC, $GC, $IP, $SNM, $GW){
 $netsh = "c:\windows\system32\netsh.exe interface ip set address ""Local Area Connection"" static $IP $SNM $GW 1"
 Write-Host "Setting IP address for $VM..."
 Invoke-VMScript -VM $VM -HostCredential $HC -GuestCredential $GC -ScriptType bat -ScriptText $netsh
 Write-Host "Setting IP address completed."
}*/

        //vm used for wrapped methods, is real library instance
        private VMWareVirtualMachine vm;
        //ivm used for custom, added methods; is either this or an injected mock
        private IVirtualMachine ivm;

        public VirtualMachine(VMWareVirtualMachine vm)
        {

            this.vm = vm;
            ivm = this;
        }

        public VirtualMachine(IVirtualMachine ivm)
        {

            this.vm = null;
            this.ivm = ivm;
        }


        // * CUSTOM METHODS * (use ivm)
        public void SetIP(string newIP)
        {
            var p = ivm.RunProgramInGuest("notepad.exe");
            if (p!=null && p.getExitCode() != 0)
            {
                throw new InvalidOperationException("Failed to set IP address, exit code " + p.getExitCode());
            }
        }

        public void SetHostname(string newName)
        {
            ivm.RunProgramInGuest("notepad.exe");
        }

        public void RebootSafely()
        {
            ivm.RunProgramInGuest("notepad.exe");
        }




        // * WRAPPED METHODS * (use vm, all one-line wraps)
        public int CPUCount
        {
            get { return vm.CPUCount; }
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.VariableIndexer GuestEnvironmentVariables
        {
            get { return vm.GuestEnvironmentVariables; }
        }

        public Dictionary<long, VMWareVirtualMachine.Process> GuestProcesses
        {
            get { return vm.GuestProcesses; }
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.VariableIndexer GuestVariables
        {
            get { return vm.GuestVariables; }
        }

        public bool IsPaused
        {
            get { return vm.IsPaused; }
        }

        public bool IsRecording
        {
            get { return vm.IsRecording; }
        }

        public bool IsReplaying
        {
            get { return vm.IsReplaying; }
        }

        public bool IsRunning
        {
            get { return vm.IsRunning; }
        }

        public bool IsSuspended
        {
            get { return vm.IsSuspended; }
        }

        public int MemorySize
        {
            get { return vm.MemorySize; }
        }

        public string PathName
        {
            get { return vm.PathName; }
        }

        public int PowerState
        {
            get { return vm.PowerState; }
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.VariableIndexer RuntimeConfigVariables
        {
            get { return vm.RuntimeConfigVariables; }
        }

        public Vestris.VMWareLib.VMWareSharedFolderCollection SharedFolders
        {
            get { return vm.SharedFolders; }
        }

        public Vestris.VMWareLib.VMWareRootSnapshotCollection Snapshots
        {
            get { return vm.Snapshots; }
        }

        public Vestris.VMWareLib.VMWareSnapshot BeginRecording(string name)
        {
            return vm.BeginRecording(name);
        }

        public Vestris.VMWareLib.VMWareSnapshot BeginRecording(string name, string description)
        {
            return vm.BeginRecording(name, description);
        }

        public Vestris.VMWareLib.VMWareSnapshot BeginRecording(string name, string description, int timeoutInSeconds)
        {
            return vm.BeginRecording(name, description, timeoutInSeconds);
        }

        public void Clone(Vestris.VMWareLib.VMWareVirtualMachineCloneType cloneType, string destConfigPathName)
        {
            vm.Clone(cloneType, destConfigPathName);
        }

        public void Clone(Vestris.VMWareLib.VMWareVirtualMachineCloneType cloneType, string destConfigPathName, int timeoutInSeconds)
        {
            vm.Clone(cloneType, destConfigPathName, timeoutInSeconds);
        }

        public void CopyFileFromGuestToHost(string guestPathName, string hostPathName)
        {
            vm.CopyFileFromGuestToHost(guestPathName, hostPathName);
        }

        public void CopyFileFromGuestToHost(string guestPathName, string hostPathName, int timeoutInSeconds)
        {
            vm.CopyFileFromGuestToHost(guestPathName, hostPathName, timeoutInSeconds);
        }

        public void CopyFileFromHostToGuest(string hostPathName, string guestPathName)
        {
            vm.CopyFileFromHostToGuest(hostPathName, guestPathName);
        }

        public void CopyFileFromHostToGuest(string hostPathName, string guestPathName, int timeoutInSeconds)
        {
            vm.CopyFileFromHostToGuest(hostPathName, guestPathName, timeoutInSeconds);
        }

        public void CreateDirectoryInGuest(string guestPathName)
        {
            vm.CreateDirectoryInGuest(guestPathName);
        }

        public void CreateDirectoryInGuest(string guestPathName, int timeoutInSeconds)
        {
            vm.CreateDirectoryInGuest(guestPathName, timeoutInSeconds);
        }

        public string CreateTempFileInGuest()
        {
            return vm.CreateTempFileInGuest();
        }

        public string CreateTempFileInGuest(int timeoutInSeconds)
        {
            return vm.CreateTempFileInGuest(timeoutInSeconds);
        }

        public void Delete()
        {
            vm.Delete();
        }

        public void Delete(int deleteOptions)
        {
            vm.Delete(deleteOptions);
        }

        public void Delete(int deleteOptions, int timeoutInSeconds)
        {
            vm.Delete(deleteOptions, timeoutInSeconds);
        }

        public void DeleteDirectoryFromGuest(string guestPathName)
        {
            vm.DeleteDirectoryFromGuest(guestPathName);
        }

        public void DeleteDirectoryFromGuest(string guestPathName, int timeoutInSeconds)
        {
            vm.DeleteDirectoryFromGuest(guestPathName, timeoutInSeconds);
        }

        public void DeleteFileFromGuest(string guestPathName)
        {
            vm.DeleteFileFromGuest(guestPathName);
        }

        public void DeleteFileFromGuest(string guestPathName, int timeoutInSeconds)
        {
            vm.DeleteFileFromGuest(guestPathName, timeoutInSeconds);
        }

        public IProcess DetachProgramInGuest(string guestProgramName)
        {
            return new Process(vm.DetachProgramInGuest(guestProgramName));
        }

        public IProcess DetachProgramInGuest(string guestProgramName, string commandLineArgs)
        {
            return new Process(vm.DetachProgramInGuest(guestProgramName, commandLineArgs));
        }

        public IProcess DetachProgramInGuest(string guestProgramName, string commandLineArgs, int timeoutInSeconds)
        {
            return new Process(vm.DetachProgramInGuest(guestProgramName, commandLineArgs, timeoutInSeconds));
        }

        public IProcess DetachScriptInGuest(string interpreter, string scriptText)
        {
            return new Process(vm.DetachScriptInGuest(interpreter, scriptText));
        }

        public IProcess DetachScriptInGuest(string interpreter, string scriptText, int timeoutInSeconds)
        {
            return new Process(vm.DetachScriptInGuest(interpreter, scriptText, timeoutInSeconds));
        }

        public bool DirectoryExistsInGuest(string guestPathName)
        {
            return vm.DirectoryExistsInGuest(guestPathName);
        }

        public bool DirectoryExistsInGuest(string guestPathName, int timeoutInSeconds)
        {
            return vm.DirectoryExistsInGuest(guestPathName, timeoutInSeconds);
        }

        public void EndRecording()
        {
            vm.EndRecording();
        }

        public void EndRecording(int timeoutInSeconds)
        {
            vm.EndRecording(timeoutInSeconds);
        }

        public bool FileExistsInGuest(string guestPathName)
        {
            return vm.FileExistsInGuest(guestPathName);
        }

        public bool FileExistsInGuest(string guestPathName, int timeoutInSeconds)
        {
            return vm.FileExistsInGuest(guestPathName, timeoutInSeconds);
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.GuestFileInfo GetFileInfoInGuest(string guestPathName)
        {
            return vm.GetFileInfoInGuest(guestPathName);
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.GuestFileInfo GetFileInfoInGuest(string guestPathName, int timeoutInSeconds)
        {
            return vm.GetFileInfoInGuest(guestPathName, timeoutInSeconds);
        }

        public void InstallTools()
        {
            vm.InstallTools();
        }

        public void InstallTools(int timeoutInSeconds)
        {
            vm.InstallTools(timeoutInSeconds);
        }

        public List<string> ListDirectoryInGuest(string pathName, bool recurse)
        {
            return vm.ListDirectoryInGuest(pathName, recurse);
        }

        public List<string> ListDirectoryInGuest(string pathName, bool recurse, int timeoutInSeconds)
        {
            return vm.ListDirectoryInGuest(pathName, recurse, timeoutInSeconds);
        }

        public void LoginInGuest(string username, string password)
        {
            vm.LoginInGuest(username, password);
        }

        public void LoginInGuest(string username, string password, int timeoutInSeconds)
        {
            vm.LoginInGuest(username, password, timeoutInSeconds);
        }

        public void LoginInGuest(string username, string password, int options, int timeoutInSeconds)
        {
            vm.LoginInGuest(username, password, options, timeoutInSeconds);
        }

        public void LogoutFromGuest()
        {
            vm.LogoutFromGuest();
        }

        public void LogoutFromGuest(int timeoutInSeconds)
        {
            vm.LogoutFromGuest(timeoutInSeconds);
        }

        public void Pause()
        {
            vm.Pause();
        }

        public void Pause(int timeoutInSeconds)
        {
            vm.Pause(timeoutInSeconds);
        }

        public void PowerOff()
        {
            vm.PowerOff();
        }

        public void PowerOff(int powerOffOptions, int timeoutInSeconds)
        {
            vm.PowerOff(powerOffOptions, timeoutInSeconds);
        }

        public void PowerOn()
        {
            vm.PowerOn();
        }

        public void PowerOn(int timeoutInSeconds)
        {
            vm.PowerOn(timeoutInSeconds);
        }

        public void PowerOn(int powerOnOptions, int timeoutInSeconds)
        {
            vm.PowerOn(powerOnOptions, timeoutInSeconds);
        }

        public void Reset()
        {
            vm.Reset();
        }

        public void Reset(int resetOptions)
        {
            vm.Reset(resetOptions);
        }

        public void Reset(int resetOptions, int timeoutInSeconds)
        {
            vm.Reset(resetOptions, timeoutInSeconds);
        }

        public IProcess RunProgramInGuest(string guestProgramName)
        {
            return new Process(vm.RunProgramInGuest(guestProgramName));
        }

        public IProcess RunProgramInGuest(string guestProgramName, string commandLineArgs)
        {
            return new Process(vm.RunProgramInGuest(guestProgramName, commandLineArgs));
        }

        public IProcess RunProgramInGuest(string guestProgramName, string commandLineArgs, int timeoutInSeconds)
        {
            return new Process(vm.RunProgramInGuest(guestProgramName, commandLineArgs, timeoutInSeconds));
        }

        public IProcess RunProgramInGuest(string guestProgramName, string commandLineArgs, int options, int timeoutInSeconds)
        {
            return new Process(vm.RunProgramInGuest(guestProgramName, commandLineArgs, options, timeoutInSeconds));
        }

        public IProcess RunScriptInGuest(string interpreter, string scriptText)
        {
            return new Process(vm.RunScriptInGuest(interpreter, scriptText));
        }

        public IProcess RunScriptInGuest(string interpreter, string scriptText, int options, int timeoutInSeconds)
        {
            return new Process(vm.RunScriptInGuest(interpreter, scriptText, options, timeoutInSeconds));
        }

        public void ShutdownGuest()
        {
            vm.ShutdownGuest();
        }

        public void ShutdownGuest(int timeoutInSeconds)
        {
            vm.ShutdownGuest(timeoutInSeconds);
        }

        public void Suspend()
        {
            vm.Suspend();
        }

        public void Suspend(int timeoutInSeconds)
        {
            vm.Suspend(timeoutInSeconds);
        }

        public void Unpause()
        {
            vm.Unpause();
        }

        public void Unpause(int timeoutInSeconds)
        {
            vm.Unpause(timeoutInSeconds);
        }

        public void UpgradeVirtualHardware()
        {
            vm.UpgradeVirtualHardware();
        }

        public void UpgradeVirtualHardware(int timeoutInSeconds)
        {
            vm.UpgradeVirtualHardware(timeoutInSeconds);
        }

        public void WaitForToolsInGuest()
        {
            vm.WaitForToolsInGuest();
        }

        public void WaitForToolsInGuest(int timeoutInSeconds)
        {
            vm.WaitForToolsInGuest(timeoutInSeconds);
        }


    }
}
