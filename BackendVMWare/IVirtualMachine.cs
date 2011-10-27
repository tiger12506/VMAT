using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;
namespace BackendVMWare
{
    interface IVirtualMachine
    {

        int CPUCount { get; }
        VMWareVirtualMachine.VariableIndexer GuestEnvironmentVariables { get; }
        Dictionary<long, VMWareVirtualMachine.Process> GuestProcesses { get; }
        VMWareVirtualMachine.VariableIndexer GuestVariables { get; }
        bool IsPaused { get; }
        bool IsRecording { get; }
        bool IsReplaying { get; }
        bool IsRunning { get; }
        bool IsSuspended { get; }
        int MemorySize { get; }
        string PathName { get; }
        int PowerState { get; }
        VMWareVirtualMachine.VariableIndexer RuntimeConfigVariables { get; }
        VMWareSharedFolderCollection SharedFolders { get; }
        VMWareRootSnapshotCollection Snapshots { get; }

        VMWareSnapshot BeginRecording(string name);
        VMWareSnapshot BeginRecording(string name, string description);
        VMWareSnapshot BeginRecording(string name, string description, int timeoutInSeconds);
        void Clone(VMWareVirtualMachineCloneType cloneType, string destConfigPathName);
        void Clone(VMWareVirtualMachineCloneType cloneType, string destConfigPathName, int timeoutInSeconds);
        void CopyFileFromGuestToHost(string guestPathName, string hostPathName);
        void CopyFileFromGuestToHost(string guestPathName, string hostPathName, int timeoutInSeconds);
        void CopyFileFromHostToGuest(string hostPathName, string guestPathName);
        void CopyFileFromHostToGuest(string hostPathName, string guestPathName, int timeoutInSeconds);
        void CreateDirectoryInGuest(string guestPathName);
        void CreateDirectoryInGuest(string guestPathName, int timeoutInSeconds);
        string CreateTempFileInGuest();
        string CreateTempFileInGuest(int timeoutInSeconds);
        void Delete();
        void Delete(int deleteOptions);
        void Delete(int deleteOptions, int timeoutInSeconds);
        void DeleteDirectoryFromGuest(string guestPathName);
        void DeleteDirectoryFromGuest(string guestPathName, int timeoutInSeconds);
        void DeleteFileFromGuest(string guestPathName);
        void DeleteFileFromGuest(string guestPathName, int timeoutInSeconds);
        VMWareVirtualMachine.Process DetachProgramInGuest(string guestProgramName);
        VMWareVirtualMachine.Process DetachProgramInGuest(string guestProgramName, string commandLineArgs);
        VMWareVirtualMachine.Process DetachProgramInGuest(string guestProgramName, string commandLineArgs, int timeoutInSeconds);
        VMWareVirtualMachine.Process DetachScriptInGuest(string interpreter, string scriptText);
        VMWareVirtualMachine.Process DetachScriptInGuest(string interpreter, string scriptText, int timeoutInSeconds);
        bool DirectoryExistsInGuest(string guestPathName);
        bool DirectoryExistsInGuest(string guestPathName, int timeoutInSeconds);
        void EndRecording();
        void EndRecording(int timeoutInSeconds);
        bool FileExistsInGuest(string guestPathName);
        bool FileExistsInGuest(string guestPathName, int timeoutInSeconds);
        VMWareVirtualMachine.GuestFileInfo GetFileInfoInGuest(string guestPathName);
        VMWareVirtualMachine.GuestFileInfo GetFileInfoInGuest(string guestPathName, int timeoutInSeconds);
        void InstallTools();
        void InstallTools(int timeoutInSeconds);
        List<string> ListDirectoryInGuest(string pathName, bool recurse);
        List<string> ListDirectoryInGuest(string pathName, bool recurse, int timeoutInSeconds);
        void LoginInGuest(string username, string password);
        void LoginInGuest(string username, string password, int timeoutInSeconds);
        void LoginInGuest(string username, string password, int options, int timeoutInSeconds);
        void LogoutFromGuest();
        void LogoutFromGuest(int timeoutInSeconds);
        void Pause();
        void Pause(int timeoutInSeconds);
        void PowerOff();
        void PowerOff(int powerOffOptions, int timeoutInSeconds);
        void PowerOn();
        void PowerOn(int timeoutInSeconds);
        void PowerOn(int powerOnOptions, int timeoutInSeconds);
        void Reset();
        void Reset(int resetOptions);
        void Reset(int resetOptions, int timeoutInSeconds);
        VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName);
        VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName, string commandLineArgs);
        VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName, string commandLineArgs, int timeoutInSeconds);
        VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName, string commandLineArgs, int options, int timeoutInSeconds);
        VMWareVirtualMachine.Process RunScriptInGuest(string interpreter, string scriptText);
        VMWareVirtualMachine.Process RunScriptInGuest(string interpreter, string scriptText, int options, int timeoutInSeconds);
        void ShutdownGuest();
        void ShutdownGuest(int timeoutInSeconds);
        void Suspend();
        void Suspend(int timeoutInSeconds);
        void Unpause();
        void Unpause(int timeoutInSeconds);
        void UpgradeVirtualHardware();
        void UpgradeVirtualHardware(int timeoutInSeconds);
        void WaitForToolsInGuest();
        void WaitForToolsInGuest(int timeoutInSeconds);
    }
}
