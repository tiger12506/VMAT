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
        Idle // Do not auto-start
    }

    public abstract class VirtualMachine
    {
        [Key]
        [Required(ErrorMessage = "Image Path Name is required")]
        [DisplayName("Image Filepath")]
        public string ImagePathName { get; protected set; }

        [DisplayName("Base Image File")]
        public string BaseImageName { get; set; }

        [DisplayName("Hostname")]
        public string Hostname { get; set; }

        [DisplayName("Lifecycle")]
        public VMLifecycle Lifecycle { get; set; }

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
            int start = ImagePathName.LastIndexOf("gapdev") + "gapdev".Length;
            int length = 4;

            return ImagePathName.Substring(start, length);
        }

        public static string GetProjectName(string imagePathName)
        {
            int start = imagePathName.LastIndexOf("gapdev") + "gapdev".Length;
            int length = 4;

            return imagePathName.Substring(start, length);
        }
    }
}
