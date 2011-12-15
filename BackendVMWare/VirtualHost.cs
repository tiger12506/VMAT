using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.VMWareLib;
using System.IO;

namespace BackendVMWare
{
    public class VirtualHost : IVirtualHost
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

        public List<string> GetBaseImageFiles()
        {
            List<string> filePaths = new List<string>(Directory.GetFiles(Config.GetWebserverVmPath(), "*.vmx", SearchOption.AllDirectories));

            return filePaths;
        }

        public Vestris.VMWareLib.VMWareVirtualHost.ServiceProviderType ConnectionType
        {
            get { return vh.ConnectionType; }
        }

        public bool IsConnected
        {
            get { return vh.IsConnected; }
        }

        public IEnumerable<VirtualMachine> RegisteredVirtualMachines
        {
            get
            {
                List<VirtualMachine> ls = new List<VirtualMachine>();

                foreach (VMWareVirtualMachine v in vh.RegisteredVirtualMachines)
                {
                    ls.Add(new VirtualMachine(v));
                }
                return ls.AsEnumerable();
            }
        }

        public IEnumerable<VirtualMachine> RunningVirtualMachines
        {
            get
            {
                List<VirtualMachine> ls = new List<VirtualMachine>();

                foreach (VMWareVirtualMachine v in vh.RunningVirtualMachines)
                {
                    ls.Add(new VirtualMachine(v));
                }
                return ls.AsEnumerable();
            }
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
            try
            {
                vh.ConnectToVMWareVIServer(hostName, username, password);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message + ": Connection to VMware server timed out.");
            }
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

        public IVirtualMachine Open(string fileName)
        {
            return new VirtualMachine(vh.Open(fileName));
        }

        public IVirtualMachine Open(string fileName, int timeoutInSeconds)
        {
            return new VirtualMachine(vh.Open(fileName, timeoutInSeconds));
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
