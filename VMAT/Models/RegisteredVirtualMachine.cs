using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMAT.Models.VMware;

namespace VMAT.Models
{
    public class RegisteredVirtualMachine : VirtualMachine
    {
        [RegularExpression("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "Invalid IP Address")]
        [DisplayName("IP Address")]
        public string IP { get; set; }

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

        public RegisteredVirtualMachine(PendingVirtualMachine vm) : this()
        {
            ImagePathName = vm.ImagePathName;
            IP = vm.IP;
            BaseImageName = vm.BaseImageName;
            IsAutoStarted = vm.IsAutoStarted;
            OS = vm.OS;
        }

        public RegisteredVirtualMachine(PendingArchiveVirtualMachine vm)
        {
            ImagePathName = vm.ImagePathName;
            BaseImageName = vm.BaseImageName;
            OS = vm.OS;
            IsAutoStarted = vm.IsAutoStarted;
            IP = vm.IP;
            CreatedTime = vm.CreatedTime;
            LastStarted = vm.LastStarted;
            LastStopped = vm.LastStopped;
            LastArchived = vm.LastArchived;
            LastBackuped = vm.LastBackuped;
        }
    }
}
