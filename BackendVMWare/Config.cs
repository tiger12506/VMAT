using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace BackendVMWare
{
    /// <summary>
    /// Interface with the Web.config file.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// The interface object for the 'appSettings' section of Web.config.
        /// </summary>
        private static AppSettingsSection appSettings = WebConfigurationManager.OpenWebConfiguration("~").AppSettings;

        /// <summary>
        /// Set the appSettings section to the given instance of an object. Used primarily for
        /// testing purposes.
        /// </summary>
        /// <param name="settings">The intended appSettings section object.</param>
        public static void SetWebConfigurationFile(AppSettingsSection settings)
        {
            appSettings = settings;
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

        // Virtual machine folder on host must be accessible by webserver, no opportunity to provide user/pass yet (unless map network drive)
        /// <summary>
        /// Return the 
        /// </summary>
        /// <returns></returns>
        public static string GetWebserverVmPath()
        {
            return appSettings.Settings["WebserverVMPath"].Value;
        }

        // A script file will be placed here for copy from webserver to guest
        public static string GetWebserverTmpPath()
        {
            return appSettings.Settings["WebserverTmpPath"].Value;
        }

        public static string GetNetworkInterfaceName()
        {
            return appSettings.Settings["NetworkInterfaceName"].Value;
        }

        // Credentials for VMware Server 2.0
        public static string GetVMwareHostAndPort()
        {
            return appSettings.Settings["VMwareHostAndPort"].Value;
        }

        public static string GetVMwareUsername()
        {
            return appSettings.Settings["VMwareUsername"].Value;
        }

        public static string GetVMwarePassword()
        {
            return appSettings.Settings["VMwarePassword"].Value;
        }


        // Credentials for guest VMs
        public static string GetVMsUsername()
        {
            return appSettings.Settings["VMUsername"].Value;
        }

        public static string GetVMsPassword()
        {
            return appSettings.Settings["VMPassword"].Value;
        }


        // Filepaths for data sources and configuration files
        public static string GetAppConfigFilepath()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            return config.FilePath;
        }

        public static string GetDataFilesDirectory()
        {
            return appSettings.Settings["DataFilesDirectory"].Value;
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
    }
}
