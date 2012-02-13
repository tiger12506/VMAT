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
        [ScaffoldColumn(false)]
        public int VirtualMachineId { get; set; }

        [ScaffoldColumn(false)]
        public int ProjectId { get; set; }

        [RegularExpression("^gapdev[0-9]{4}[a-zA-Z0-9]{1,5}$")]
        [DisplayName("Machine Name")]
        public string MachineName { get; set; }

        [DisplayName("Image Filepath")]
        public string ImagePathName { get; set; }

        [DisplayName("Base Image File")]
        public string BaseImageName { get; set; }

        [DisplayName("Operating System")]
        [RegularExpression("[a-zA-Z0-9 ]")]
        public string OS { get; set; }

        [DisplayName("Startup")]
        public bool IsAutoStarted { get; set; }

        public Project Project { get; set; }
    }
}
