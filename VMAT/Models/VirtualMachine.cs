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

    public abstract class VirtualMachine
    {
        [Key]
        [Required(ErrorMessage = "Image Path Name required")]
        [DisplayName("Image Filepath")]
        public string ImagePathName { get; protected set; }

        [Required(ErrorMessage = "Machine Name Suffix required")]
        [DisplayName("Machine Name Suffix")]
        public string MachineName { get; set; }

        [Required]
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        [DisplayName("Base Image File")]
        public string BaseImageName { get; set; }

        [DisplayName("Operating System")]
        public string OS { get; set; }

        [DisplayName("Hostname")]
        public string Hostname { get; set; }

        [DisplayName("Startup")]
        public bool IsAutoStarted { get; set; }
    }
}
