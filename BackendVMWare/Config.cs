using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackendVMWare
{
    class Config
    {

        //all folder names need trailing \
        //location of all VM files on host
        public static string getHostVmPath()
        {
            return @"C:\Virtual Machines\";
        }
        public static string getDatastore()
        {
            return @"[ha-datacenter/standard] ";
        }
        //virtual machine folder on host must be accessible by webserver, no opportunity to provide user/pass yet (unless map network drive)
        public static string getWebserverVmPath()
        {
            return @"\\vmat.csse.rose-hulman.edu\VirtualMachines\";
        }
        //a script file will be placed here for copy from webserver to guest
        public static string getWebserverTmpPath()
        {
            return @"C:\temp\";
        }
        public static string getNetworkInterfaceName()
        {
            return "\"Local Area Connection\"";
        }

        //credentials for VMware Server 2.0
        public static string getVMwareHostAndPort()
        {
            return @"vmat.csse.rose-hulman.edu:8333";
        }
        public static string getVMwareUsername()
        {
            return "csse department";
        }
        public static string getVMwarePassword()
        {
            return "Vmat1234";
        }

      

        //credentials for guest VMs
        public static string getVMsUsername()
        {
            return "Administrator";
        }
        public static string getVMsPassword()
        {
            return "Vmat1234";
        }

    }
}
