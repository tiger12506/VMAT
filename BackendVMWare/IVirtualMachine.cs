using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;
namespace BackendVMWare
{
    interface IVirtualMachine
    {

        public int CPUCount { get; }
        public VMWareVirtualMachine.VariableIndexer GuestEnvironmentVariables { get; }
        public Dictionary<long, VMWareVirtualMachine.Process> GuestProcesses { get; }
        public VMWareVirtualMachine.VariableIndexer GuestVariables { get; }
        public bool IsPaused { get; }
        public bool IsRecording { get; }
        public bool IsReplaying { get; }
        public bool IsRunning { get; }
        public bool IsSuspended { get; }
        public int MemorySize { get; }
        public string PathName { get; }
        public int PowerState { get; }
        public VMWareVirtualMachine.VariableIndexer RuntimeConfigVariables { get; }
        public VMWareSharedFolderCollection SharedFolders { get; }
        public VMWareRootSnapshotCollection Snapshots { get; }

        public VMWareSnapshot BeginRecording(string name);
        public VMWareSnapshot BeginRecording(string name, string description);
        public VMWareSnapshot BeginRecording(string name, string description, int timeoutInSeconds);
        public System.Drawing.Image CaptureScreenImage();
        public void Clone(VMWareVirtualMachineCloneType cloneType, string destConfigPathName);
        public void Clone(VMWareVirtualMachineCloneType cloneType, string destConfigPathName, int timeoutInSeconds);
        public void CopyFileFromGuestToHost(string guestPathName, string hostPathName);
        public void CopyFileFromGuestToHost(string guestPathName, string hostPathName, int timeoutInSeconds);
        public void CopyFileFromHostToGuest(string hostPathName, string guestPathName);
        public void CopyFileFromHostToGuest(string hostPathName, string guestPathName, int timeoutInSeconds);
        public void CreateDirectoryInGuest(string guestPathName);
        public void CreateDirectoryInGuest(string guestPathName, int timeoutInSeconds);
        public string CreateTempFileInGuest();
        public string CreateTempFileInGuest(int timeoutInSeconds);
        public void Delete();
        public void Delete(int deleteOptions);
        public void Delete(int deleteOptions, int timeoutInSeconds);
        public void DeleteDirectoryFromGuest(string guestPathName);
        public void DeleteDirectoryFromGuest(string guestPathName, int timeoutInSeconds);
        public void DeleteFileFromGuest(string guestPathName);
        public void DeleteFileFromGuest(string guestPathName, int timeoutInSeconds);
        public VMWareVirtualMachine.Process DetachProgramInGuest(string guestProgramName);
        public VMWareVirtualMachine.Process DetachProgramInGuest(string guestProgramName, string commandLineArgs);
        public VMWareVirtualMachine.Process DetachProgramInGuest(string guestProgramName, string commandLineArgs, int timeoutInSeconds);
        public VMWareVirtualMachine.Process DetachScriptInGuest(string interpreter, string scriptText);
        public VMWareVirtualMachine.Process DetachScriptInGuest(string interpreter, string scriptText, int timeoutInSeconds);
        public bool DirectoryExistsInGuest(string guestPathName);
        public bool DirectoryExistsInGuest(string guestPathName, int timeoutInSeconds);
        public override void Dispose();
        public void EndRecording();
        public void EndRecording(int timeoutInSeconds);
        public bool FileExistsInGuest(string guestPathName);
        public bool FileExistsInGuest(string guestPathName, int timeoutInSeconds);
        public VMWareVirtualMachine.GuestFileInfo GetFileInfoInGuest(string guestPathName);
        public VMWareVirtualMachine.GuestFileInfo GetFileInfoInGuest(string guestPathName, int timeoutInSeconds);
        public void InstallTools();
        public void InstallTools(int timeoutInSeconds);
        public List<string> ListDirectoryInGuest(string pathName, bool recurse);
        public List<string> ListDirectoryInGuest(string pathName, bool recurse, int timeoutInSeconds);
        public void LoginInGuest(string username, string password);
        public void LoginInGuest(string username, string password, int timeoutInSeconds);
        public void LoginInGuest(string username, string password, int options, int timeoutInSeconds);
        public void LogoutFromGuest();
        public void LogoutFromGuest(int timeoutInSeconds);
        public void Pause();
        public void Pause(int timeoutInSeconds);
        public void PowerOff();
        public void PowerOff(int powerOffOptions, int timeoutInSeconds);
        public void PowerOn();
        public void PowerOn(int timeoutInSeconds);
        public void PowerOn(int powerOnOptions, int timeoutInSeconds);
        public void Reset();
        public void Reset(int resetOptions);
        public void Reset(int resetOptions, int timeoutInSeconds);
        public VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName);
        public VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName, string commandLineArgs);
        public VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName, string commandLineArgs, int timeoutInSeconds);
        public VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName, string commandLineArgs, int options, int timeoutInSeconds);
        public VMWareVirtualMachine.Process RunScriptInGuest(string interpreter, string scriptText);
        public VMWareVirtualMachine.Process RunScriptInGuest(string interpreter, string scriptText, int options, int timeoutInSeconds);
        public void ShutdownGuest();
        public void ShutdownGuest(int timeoutInSeconds);
        public void Suspend();
        public void Suspend(int timeoutInSeconds);
        public void Unpause();
        public void Unpause(int timeoutInSeconds);
        public void UpgradeVirtualHardware();
        public void UpgradeVirtualHardware(int timeoutInSeconds);
        public void WaitForToolsInGuest();
        public void WaitForToolsInGuest(int timeoutInSeconds);
    }
}
