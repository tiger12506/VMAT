using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMAT.Models.VMware;

namespace VMAT.Models
{
    public class RegisteredVirtualMachine : VirtualMachine
    {
        [StringLength(15, ErrorMessage = "Invalid IP Address")]
        [DisplayName("IP Address")]
        public string IP { get; set; }

        [DisplayName("Hostname")]
        new public string Hostname { get; set; }

        [DisplayName("Last Shutdown")]
        public DateTime LastStopped { get; set; }

        [DisplayName("Last Started")]
        public DateTime LastStarted { get; set; }

        [DisplayName("Last Backed Up")]
        public DateTime LastBackuped { get; set; }

        [DisplayName("Last Archived")]
        public DateTime LastArchived { get; set; }

        [DisplayName("Created")]
        public DateTime CreatedTime { get; set; }

        public RegisteredVirtualMachine()
        {
            CreatedTime = DateTime.Now;
            LastStopped = DateTime.Now;
            LastStarted = DateTime.Now;
            LastBackuped = DateTime.Now;
            LastArchived = DateTime.Now;
        }

        public RegisteredVirtualMachine(IVirtualMachine vm) : this()
        {
            ImagePathName = vm.PathName;
        }

        public RegisteredVirtualMachine(string imagePathName) : this()
        {
            ImagePathName = imagePathName;
        }

        public RegisteredVirtualMachine(PendingVirtualMachine vm)
            : this(vm.ImagePathName)
        {
            IP = vm.IP;
            Hostname = vm.Hostname;
            BaseImageName = vm.BaseImageName;
            CreatedTime = DateTime.Now;
        }
    }
}
