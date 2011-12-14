using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;

namespace BackendVMWare
{
    public class Config
    {
        private static AppSettingsSection appSettings = WebConfigurationManager.OpenWebConfiguration("~").AppSettings;
            //ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings;

        //all folder names need trailing \
        //location of all VM files on host
        public static string GetHostVmPath()
        {
            return appSettings.Settings["HostVMPath"].Value;
        }

        public static string GetDatastore()
        {
            return appSettings.Settings["VMDatastore"].Value;
        }

        //virtual machine folder on host must be accessible by webserver, no opportunity to provide user/pass yet (unless map network drive)
        public static string GetWebserverVmPath()
        {
            return appSettings.Settings["WebserverVMPath"].Value;
        }

        //a script file will be placed here for copy from webserver to guest
        public static string GetWebserverTmpPath()
        {
            return appSettings.Settings["WebserverTmpPath"].Value;
        }

        public static string GetNetworkInterfaceName()
        {
            return appSettings.Settings["NetworkInterfaceName"].Value;
        }

        //credentials for VMware Server 2.0
        public static string GetVMwareHostAndPort()
        {
            return appSettings.Settings["VmwareHostAndPort"].Value;
        }

        public static string GetVMwareUsername()
        {
            return appSettings.Settings["VMwareUsername"].Value;
        }

        public static string GetVMwarePassword()
        {
            return appSettings.Settings["VMwarePassword"].Value;
        }


        //credentials for guest VMs
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
