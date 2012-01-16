using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMAT.Models.VMware;

namespace VMAT.Models
{
    public class RegisteredVirtualMachine : VirtualMachine
    {
        [ScaffoldColumn(false)]
        [DisplayName("Status")]
        public VMStatus Status { get; set; }

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
        public DateTime Created { get; set; }

        public RegisteredVirtualMachine()
        {
            Created = DateTime.Now;
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
            Created = DateTime.Now;

            try
            {
                RegisteredVirtualMachineService.SetRegisteredVirtualMachine(ImagePathName);
                // Make triple-double-dog sure that the VM is online and ready.
                // Allow VM time to power on
                RegisteredVirtualMachineService.PowerOn();
                System.Threading.Thread.Sleep(180 * 1000);

                // Allow VM time to reboot
                RegisteredVirtualMachineService.Reboot();
                System.Threading.Thread.Sleep(250 * 1000);
            }
            catch (TimeoutException)
            {
                // TODO: Handle time-out
            }
        }
    }
}
