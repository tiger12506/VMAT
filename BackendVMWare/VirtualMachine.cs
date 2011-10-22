using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;
namespace BackendVMWare
{
    class VirtualMachine : IVirtualMachine
    {
        private VMWareVirtualMachine vm;

        public VirtualMachine(VMWareVirtualMachine vm)
        {
            this.vm = vm;
        }
        public int CPUCount
        {
            get { return vm.CPUCount; }
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.VariableIndexer GuestEnvironmentVariables
        {
            get { return vm.GuestEnvironmentVariables; }
        }

        public Dictionary<long, Vestris.VMWareLib.VMWareVirtualMachine.Process> GuestProcesses
        {
            get { throw new NotImplementedException(); }
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.VariableIndexer GuestVariables
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsPaused
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsRecording
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReplaying
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsRunning
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSuspended
        {
            get { throw new NotImplementedException(); }
        }

        public int MemorySize
        {
            get { throw new NotImplementedException(); }
        }

        public string PathName
        {
            get { throw new NotImplementedException(); }
        }

        public int PowerState
        {
            get { throw new NotImplementedException(); }
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.VariableIndexer RuntimeConfigVariables
        {
            get { throw new NotImplementedException(); }
        }

        public Vestris.VMWareLib.VMWareSharedFolderCollection SharedFolders
        {
            get { throw new NotImplementedException(); }
        }

        public Vestris.VMWareLib.VMWareRootSnapshotCollection Snapshots
        {
            get { throw new NotImplementedException(); }
        }

        public Vestris.VMWareLib.VMWareSnapshot BeginRecording(string name)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareSnapshot BeginRecording(string name, string description)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareSnapshot BeginRecording(string name, string description, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Image CaptureScreenImage()
        {
            throw new NotImplementedException();
        }

        public void Clone(Vestris.VMWareLib.VMWareVirtualMachineCloneType cloneType, string destConfigPathName)
        {
            throw new NotImplementedException();
        }

        public void Clone(Vestris.VMWareLib.VMWareVirtualMachineCloneType cloneType, string destConfigPathName, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void CopyFileFromGuestToHost(string guestPathName, string hostPathName)
        {
            throw new NotImplementedException();
        }

        public void CopyFileFromGuestToHost(string guestPathName, string hostPathName, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void CopyFileFromHostToGuest(string hostPathName, string guestPathName)
        {
            throw new NotImplementedException();
        }

        public void CopyFileFromHostToGuest(string hostPathName, string guestPathName, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void CreateDirectoryInGuest(string guestPathName)
        {
            throw new NotImplementedException();
        }

        public void CreateDirectoryInGuest(string guestPathName, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public string CreateTempFileInGuest()
        {
            throw new NotImplementedException();
        }

        public string CreateTempFileInGuest(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Delete(int deleteOptions)
        {
            throw new NotImplementedException();
        }

        public void Delete(int deleteOptions, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectoryFromGuest(string guestPathName)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectoryFromGuest(string guestPathName, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void DeleteFileFromGuest(string guestPathName)
        {
            throw new NotImplementedException();
        }

        public void DeleteFileFromGuest(string guestPathName, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process DetachProgramInGuest(string guestProgramName)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process DetachProgramInGuest(string guestProgramName, string commandLineArgs)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process DetachProgramInGuest(string guestProgramName, string commandLineArgs, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process DetachScriptInGuest(string interpreter, string scriptText)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process DetachScriptInGuest(string interpreter, string scriptText, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExistsInGuest(string guestPathName)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExistsInGuest(string guestPathName, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public void EndRecording()
        {
            throw new NotImplementedException();
        }

        public void EndRecording(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public bool FileExistsInGuest(string guestPathName)
        {
            throw new NotImplementedException();
        }

        public bool FileExistsInGuest(string guestPathName, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.GuestFileInfo GetFileInfoInGuest(string guestPathName)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.GuestFileInfo GetFileInfoInGuest(string guestPathName, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void InstallTools()
        {
            throw new NotImplementedException();
        }

        public void InstallTools(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public List<string> ListDirectoryInGuest(string pathName, bool recurse)
        {
            throw new NotImplementedException();
        }

        public List<string> ListDirectoryInGuest(string pathName, bool recurse, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void LoginInGuest(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void LoginInGuest(string username, string password, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void LoginInGuest(string username, string password, int options, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void LogoutFromGuest()
        {
            throw new NotImplementedException();
        }

        public void LogoutFromGuest(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Pause(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void PowerOff()
        {
            throw new NotImplementedException();
        }

        public void PowerOff(int powerOffOptions, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void PowerOn()
        {
            throw new NotImplementedException();
        }

        public void PowerOn(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void PowerOn(int powerOnOptions, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Reset(int resetOptions)
        {
            throw new NotImplementedException();
        }

        public void Reset(int resetOptions, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName, string commandLineArgs)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName, string commandLineArgs, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process RunProgramInGuest(string guestProgramName, string commandLineArgs, int options, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process RunScriptInGuest(string interpreter, string scriptText)
        {
            throw new NotImplementedException();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine.Process RunScriptInGuest(string interpreter, string scriptText, int options, int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void ShutdownGuest()
        {
            throw new NotImplementedException();
        }

        public void ShutdownGuest(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }

        public void Suspend(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void Unpause()
        {
            throw new NotImplementedException();
        }

        public void Unpause(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void UpgradeVirtualHardware()
        {
            throw new NotImplementedException();
        }

        public void UpgradeVirtualHardware(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }

        public void WaitForToolsInGuest()
        {
            throw new NotImplementedException();
        }

        public void WaitForToolsInGuest(int timeoutInSeconds)
        {
            throw new NotImplementedException();
        }
    }
}
