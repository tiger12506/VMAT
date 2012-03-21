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
        [ScaffoldColumn(false)]
        public int VirtualMachineId { get; set; }

        [DisplayName("Machine Name")]
        public string MachineName { get; set; }

        [DisplayName("Image Filepath")]
        public string ImagePathName { get; set; }

        [DisplayName("Base Image File")]
        public string BaseImageName { get; set; }

        [DisplayName("Operating System")]
        [RegularExpression("[a-zA-Z0-9 ]")]
        public string OS { get; set; }

		[DisplayName("Hostname")]
		public string Hostname { get; set; }

        [DisplayName("Startup")]
        public bool IsAutoStarted { get; set; }

        public virtual Project Project { get; set; }
    }
}
