using System;
using System.Configuration;
using System.Web.Configuration;
using VMAT.Services;

namespace VMAT.Models
{
    /// <summary>
    /// Interface with the Web.config file.
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// The interface object for the 'appSettings' section of Web.config.
        /// </summary>
        private static AppSettingsSection appSettings = WebConfigurationManager.OpenWebConfiguration("~").AppSettings;

        public static string CheckConfigSettings()
        {
            string ret = "";
            string c = GetHostVmPath();
            if (!(c.EndsWith("/") || c.EndsWith("\\")))
                ret += "HostVMPath should end with / or \\<br />";

            c = GetDatastore();
            if (!(c.Contains("] ") && (c.Contains("["))))
                ret += "Datastore should contain [ and ] (with trailing space)<br />";

            ret+=CheckPath(GetWebserverVmPath(),"WebserverVmPath");
            ret += CheckPath(GetWebserverTmpPath(), "WebserverTmpPath");

            c = GetNetworkInterfaceName();
            if (c.Length < 6)
                ret += "Network Interface Name too short<br />";

            c = GetVMwareHostAndPort();
            if (c.Contains(":"))
            {
                var p = new System.Net.NetworkInformation.Ping();
                try
                {
                    p.Send(GetVMHostName(), 10000);
                    
                }
                catch (Exception e)
                {
                    ret += "couldn't ping VMwareHost, exception: " + e.Message+"<br />";
                }
            }
            else
                ret += "VMwareHostAndPort must contain hostname and port, seperated by colon<br />";

            try
            {
                RegisteredVirtualMachineService.GetVirtualHost();
            }
            catch (Exception e)
            {
                ret += "can't connect to VMware Server. Check hostname, username, password. Exception: "+e.Message+"<br />";
            }

            //usernames, passwords, GetDataFilesDirectory skipped

            ret += "<br />Check complete.<br />";

            return ret;
        }

        private static string CheckPath(string path, string name)
        {
            string ret="";
            if (!System.IO.Directory.Exists(path))
                ret += name + " doesn't exist<br />";
            if (!(path.EndsWith("/") || path.EndsWith("\\")))
                ret += name + " should end with / or \\<br />";
            return ret;
        }

        // Location of all VM files on host
        /// <summary>
        /// Return the path to the directory containing the virtual machines
        /// on the VMware server.
        /// </summary>
        /// <returns>The full path of the directory containing the virtual machines.</returns>
        public static string GetHostVmPath()
        {
            return appSettings.Settings["HostVMPath"].Value;
        }

        /// <summary>
        /// Return the datastore value for the VMware server.
        /// </summary>
        /// <returns>The datastore value.</returns>
        public static string GetDatastore()
        {
            return appSettings.Settings["VMDatastore"].Value;
        }

        // Virtual machine folder on host must be accessible by webserver,
        // no opportunity to provide user/pass yet (unless map network drive)
        /// <summary>
        /// Return the local directory containing the virtual machines
        /// on the VMware server.
        /// </summary>
        /// <returns>
        /// The full path of the directory containing the virtual machines
        /// relative the local file system of the VMware server.
        /// </returns>
        public static string GetWebserverVmPath()
        {
            return appSettings.Settings["WebserverVMPath"].Value;
        }

        // A script file will be placed here for copy from webserver to guest
        /// <summary>
        /// Return the local 'temp' directory.
        /// </summary>
        /// <returns>The full path of the temp directory.</returns>
        public static string GetWebserverTmpPath()
        {
            return appSettings.Settings["WebserverTempPath"].Value;
        }

        /// <summary>
        /// Return the interface name for the network (ex. "Local Area Connection").
        /// </summary>
        /// <returns>The network interface name.</returns>
        public static string GetNetworkInterfaceName()
        {
            return appSettings.Settings["NetworkInterfaceName"].Value;
        }

        /// <summary>
        /// Return the URL and port number of the VMware server.
        /// </summary>
        /// <returns>The complete URL, including port number, to the VMware server.</returns>
        public static string GetVMwareHostAndPort()
        {
            return appSettings.Settings["VMwareHostAndPort"].Value;
        }

        /// <summary>
        /// Remove the port number from the 'VMWareHostAndPort' option in Web.config.
        /// </summary>
        /// <returns>The hostname of the VMware server.</returns>
        public static string GetVMHostName()
        {
            string hostName = GetVMwareHostAndPort();
            hostName = hostName.Remove(hostName.IndexOf(':'));

            return hostName;
        }

        /// <summary>
        /// Return the username for the VMware server.
        /// </summary>
        /// <returns>The VMware username.</returns>
        public static string GetVMwareUsername()
        {
            return appSettings.Settings["VMwareUsername"].Value;
        }

        /// <summary>
        /// Return the password for the stored username for the VMware server.
        /// </summary>
        /// <returns>The VMware password.</returns>
        public static string GetVMwarePassword()
        {
            return appSettings.Settings["VMwarePassword"].Value;
        }

        /// <summary>
        /// Return the username to access each virtual machine.
        /// </summary>
        /// <returns>The username for the virtual machines.</returns>
        public static string GetVMsUsername()
        {
            return appSettings.Settings["VMUsername"].Value;
        }

        /// <summary>
        /// Return the password to access each virtual machine.
        /// </summary>
        /// <returns>The password for the virtual machines.</returns>
        public static string GetVMsPassword()
        {
            return appSettings.Settings["VMPassword"].Value;
        }
    }
}
