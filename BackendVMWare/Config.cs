using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BackendVMWare
{
    public class Config
    {

        //all folder names need trailing \
        //location of all VM files on host
        public static string GetHostVmPath()
        {
            return @"C:\Virtual Machines\";
        }
        public static string GetDatastore()
        {
            return @"[ha-datacenter/standard] ";
        }
        //virtual machine folder on host must be accessible by webserver, no opportunity to provide user/pass yet (unless map network drive)
        public static string GetWebserverVmPath()
        {
            return @"\\vmat.csse.rose-hulman.edu\VirtualMachines\";
        }
        //a script file will be placed here for copy from webserver to guest
        public static string GetWebserverTmpPath()
        {
            return @"C:\temp\";
        }
        public static string GetNetworkInterfaceName()
        {
            return "\"Local Area Connection\"";
        }

        //credentials for VMware Server 2.0
        public static string GetVMwareHostAndPort()
        {
            return @"vmat.csse.rose-hulman.edu:8333";
        }
        public static string GetVMwareUsername()
        {
            return "csse department";
        }
        public static string GetVMwarePassword()
        {
            return "Vmat1234";
        }


        //credentials for guest VMs
        public static string GetVMsUsername()
        {
            return "Administrator";
        }
        public static string GetVMsPassword()
        {
            return "Vmat1234";
        }


        // Filepaths for data sources and configuration files
        public static string GetAppConfigFilepath()
        {
            return "App.config";
        }

        public static string GetDataFilesDirectory()
        {
            System.Configuration.Configuration config =
               ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            System.Configuration.ConnectionStringsSection appSettings =
                (System.Configuration.ConnectionStringsSection)config.ConnectionStrings;
            
            foreach (string key in appSettings.ConnectionStrings)
                Console.WriteLine(key);

            return appSettings.ConnectionStrings["DataFilesDirectory"].ConnectionString;
        }
    }
}
