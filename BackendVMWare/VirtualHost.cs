using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;
namespace BackendVMWare
{
    class VirtualHost : IVirtualHost
    {

        private VMWareVirtualHost vh;

        public VirtualHost()
        {
            vh = new VMWareVirtualHost();
        }
        public VirtualHost(VMWareVirtualHost vh)
        {
            this.vh = vh;
        }

        public Vestris.VMWareLib.VMWareVirtualHost.ServiceProviderType ConnectionType
        {
            get { return vh.ConnectionType; }
        }

        public bool IsConnected
        {
            get { return vh.IsConnected; }
        }

        public IEnumerable<Vestris.VMWareLib.VMWareVirtualMachine> RegisteredVirtualMachines
        {
            get { return vh.RegisteredVirtualMachines; }
        }

        public IEnumerable<Vestris.VMWareLib.VMWareVirtualMachine> RunningVirtualMachines
        {
            get { return vh.RunningVirtualMachines; }
        }

        public void ConnectToVMWarePlayer()
        {
            vh.ConnectToVMWarePlayer();
        }

        public void ConnectToVMWarePlayer(int timeoutInSeconds)
        {
            vh.ConnectToVMWarePlayer(timeoutInSeconds);
        }

        public void ConnectToVMWareServer(string hostName, string username, string password)
        {
            vh.ConnectToVMWareServer(hostName, username, password);
        }

        public void ConnectToVMWareServer(string hostName, string username, string password, int timeoutInSeconds)
        {
            vh.ConnectToVMWareServer(hostName, username, password, timeoutInSeconds);
        }

        public void ConnectToVMWareVIServer(string hostName, string username, string password)
        {
            vh.ConnectToVMWareVIServer(hostName, username, password);
        }

        public void ConnectToVMWareVIServer(string hostName, string username, string password, int timeoutInSeconds)
        {
            vh.ConnectToVMWareVIServer(hostName, username, password, timeoutInSeconds);
        }

        public void ConnectToVMWareVIServer(Uri hostUri, string username, string password, int timeoutInSeconds)
        {
            vh.ConnectToVMWareVIServer(hostUri, username, password, timeoutInSeconds);
        }

        public void ConnectToVMWareWorkstation()
        {
            vh.ConnectToVMWareWorkstation();
        }

        public void ConnectToVMWareWorkstation(int timeoutInSeconds)
        {
            vh.ConnectToVMWareWorkstation(timeoutInSeconds);
        }

        public void Disconnect()
        {
            vh.Disconnect();
        }

        public void Dispose()
        {
            vh.Dispose();
        }

        public Vestris.VMWareLib.VMWareVirtualMachine Open(string fileName)
        {
            return vh.Open(fileName);
        }

        public Vestris.VMWareLib.VMWareVirtualMachine Open(string fileName, int timeoutInSeconds)
        {
            return vh.Open(fileName, timeoutInSeconds);
        }

        public void Register(string fileName)
        {
            vh.Register(fileName);
        }

        public void Register(string fileName, int timeoutInSeconds)
        {
            vh.Register(fileName, timeoutInSeconds);
        }

        public void Unregister(string fileName)
        {
            vh.Unregister(fileName);
        }

        public void Unregister(string fileName, int timeoutInSeconds)
        {
            vh.Unregister(fileName, timeoutInSeconds);
        }
    }
}
