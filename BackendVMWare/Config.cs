using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackendVMWare
{
    class Config
    {

        public static string getHostVmPath()
        {
            return @"C:\Virtual Machines\";
        }
        public static string getWebserverVmPath()
        {
            return @"\\vmat.csse.rose-hulman.edu\VirtualMachines\";
        }

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
