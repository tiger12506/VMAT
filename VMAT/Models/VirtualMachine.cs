using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Vestris.VMWareLib.Tools.Windows;
using VMAT.Models.VMware;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VMAT.Models
{
    public enum VMStatus
    {
        Stopped,
        Paused, // Still in memory, like sleep
        Suspended, // Still in disk, like hibernate. May not be supported
        Running,
        PoweringOn,
        PoweringOff
    }

    public enum VMLifecycle
    {
        Active,
        Idle, // Do not auto-start
        Archived // VM files compressed & stored elsewhere
    }

    public abstract class VirtualMachine
    {      
        /// <summary>
        /// The current image file that the VM is running on. Will not be modifiable. Should probably follow ProjectName/gapdevppppnnnnn.vmx, 
        /// but existing ones may not. p is project number, n is engineer-selected name (1-5 char).
        /// Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"
        /// </summary>
        [Required(ErrorMessage = "Image Path Name is required")]
        [DisplayName("Image Filepath")]
        public string ImagePathName { get; protected set; }

        /// <summary>
        /// The base image file that the VM was originally copied from when first created. 
        /// Unknown naming conventions, likely contains OS version.
        /// Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"
        /// </summary>
        [DisplayName("Base Image File")]
        public string BaseImageName { get; set; }

        /// <summary>
        /// Fully Qualified Domain Name, not all machines will be on domain. Will likely follow gapdevppppnnnnn. p is project number, n is engineer-selected name (1-5 char)
        /// Note: caller must reboot after setting. 
        /// </summary>
        [DisplayName("Hostname")]
        public string Hostname { get; set; }

        public string GetMachineName()
        {
            string ipnClean = ImagePathName.Replace('\\', '/');
            int index1 = ipnClean.LastIndexOf("/") + 1;
            int index2 = ipnClean.LastIndexOf(".") - index1;
            return ipnClean.Substring(index1, index2);
        }

        /// <summary>
        /// Takes either a physical path or datasource path and provides just the base path name
        /// </summary>
        /// <param name="PathName">Physical or datasource path, ie "//VMServer/VirtualMachines/Windows 7/Windows 7.VMx</param>
        /// <returns>Base path name, ie "Windows 7"</returns>
        public static string GetMachineName(string imagePathName)
        {
            string ipnClean = imagePathName.Replace('\\', '/');
            int index1 = ipnClean.LastIndexOf("/") + 1;
            int index2 = ipnClean.LastIndexOf(".") - index1;
            return ipnClean.Substring(index1, index2);
        }

        public string GetProjectName()
        {
            int start = ImagePathName.LastIndexOf("/gapdev") + "gapdev".Length + 1;
            int length = 4;

            return ImagePathName.Substring(start, length);
        }

        public static string GetProjectName(string imagePathName)
        {
            int start = imagePathName.LastIndexOf("/gapdev") + "gapdev".Length + 1;
            int length = 4;

            return imagePathName.Substring(start, length);
        }

        public static IEnumerable<string> GetBaseImageFiles()
        {
            List<string> filePaths = new List<string>(Directory.GetFiles(AppConfiguration.GetWebserverVmPath(), "*.vmx", SearchOption.AllDirectories));
            return filePaths.Select(foo => ConvertPathToDatasource(foo));
        }

        /// <summary>
        /// Converts datasource-style path to physical network path
        /// </summary>
        /// <param name="PathName">Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"</param>
        /// <returns>Physical absolute path (from webserver to VM server), ie "//VMServer/VirtualMachines/Windows 7/Windows 7.VMx</returns>
        public static string ConvertPathToPhysical(string PathName)
        {
            return PathName.Replace(AppConfiguration.GetDatastore(), AppConfiguration.GetWebserverVmPath()).Replace('/', '\\');
        }

        /// <summary>
        /// Converts physical network path to datasource-style path
        /// </summary>
        /// <param name="PathName">Physical absolute path (from webserver to VM server), ie "//VMServer/VirtualMachines/Windows 7/Windows 7.VMx</param>
        /// <returns>Datasource format, ie "[ha-datacenter/standard] Windows 7/Windows 7.VMx"</returns>
        public static string ConvertPathToDatasource(string PathName)
        {
            return PathName.Replace(AppConfiguration.GetWebserverVmPath(), AppConfiguration.GetDatastore()).Replace('\\', '/');
        }
    }
}
