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
    }
}
