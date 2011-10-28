using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;

namespace BackendVMWare
{
    public interface IVirtualHost
    {
        

        VMWareVirtualHost.ServiceProviderType ConnectionType { get; }
        bool IsConnected { get; }
        IEnumerable<VMWareVirtualMachine> RegisteredVirtualMachines { get; }
        IEnumerable<VMWareVirtualMachine> RunningVirtualMachines { get; }

        void ConnectToVMWarePlayer();
        void ConnectToVMWarePlayer(int timeoutInSeconds);
        void ConnectToVMWareServer(string hostName, string username, string password);
        void ConnectToVMWareServer(string hostName, string username, string password, int timeoutInSeconds);
        void ConnectToVMWareVIServer(string hostName, string username, string password);
        void ConnectToVMWareVIServer(string hostName, string username, string password, int timeoutInSeconds);
        void ConnectToVMWareVIServer(Uri hostUri, string username, string password, int timeoutInSeconds);
        void ConnectToVMWareWorkstation();
        void ConnectToVMWareWorkstation(int timeoutInSeconds);
        void Disconnect();
        IVirtualMachine Open(string fileName);
        IVirtualMachine Open(string fileName, int timeoutInSeconds);
        void Register(string fileName);
        void Register(string fileName, int timeoutInSeconds);
        void Unregister(string fileName);
        void Unregister(string fileName, int timeoutInSeconds);

    }
}
