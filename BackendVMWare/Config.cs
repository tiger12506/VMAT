using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using System.Web.UI.WebControls;

namespace BackendVMWare
{
    public class Config
    {
        private static AppSettingsSection appSettings = WebConfigurationManager.OpenWebConfiguration("~").AppSettings;

        /// <summary>
        /// Set the appSettings section to the given instance of an object. Used primarily for
        /// testing purposes.
        /// </summary>
        /// <param name="settings">The intended appSettings section object</param>
        public static void SetWebConfigurationFile(AppSettingsSection settings)
        {
            appSettings = settings;
        }

        // Location of all VM files on host
        public static string GetHostVmPath()
        {
            return appSettings.Settings["HostVMPath"].Value;
        }

        public static string GetDatastore()
        {
            return appSettings.Settings["VMDatastore"].Value;
        }

        // Virtual machine folder on host must be accessible by webserver, no opportunity to provide user/pass yet (unless map network drive)
        public static string GetWebserverVmPath()
        {
            return appSettings.Settings["WebserverVMPath"].Value;
        }

        // A script file will be placed here for copy from webserver to guest
        public static string GetWebserverTmpPath()
        {
            return appSettings.Settings["WebserverTempPath"].Value;
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
    }
}
