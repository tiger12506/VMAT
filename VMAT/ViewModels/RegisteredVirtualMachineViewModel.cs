﻿using VMAT.Models;
using VMAT.Services;

namespace VMAT.ViewModels
{
    public class RegisteredVirtualMachineViewModel
    {
        public string ImagePathName { get; set; }
        public string BaseImageName { get; set; }
        public string Status { get; set; }
        public string MachineName { get; set; }
        public string IP { get; set; }
        public string CreatedTime { get; set; }
        public string LastStarted { get; set; }
        public string LastStopped { get; set; }
        public string LastBackuped { get; set; }
        public string LastArchived { get; set; }

        public RegisteredVirtualMachineViewModel() { }

        public RegisteredVirtualMachineViewModel(RegisteredVirtualMachine vm)
        {
            var service = new RegisteredVirtualMachineService(vm.ImagePathName);

            ImagePathName = vm.ImagePathName;
            BaseImageName = vm.BaseImageName;
            Status = service.GetStatus().ToString().ToLower();
            MachineName = vm.GetMachineName();
            IP = vm.IP;
            CreatedTime = vm.CreatedTime.ToString();
            LastStopped = vm.LastStopped.ToString();
            LastStarted = vm.LastStarted.ToString();
            LastArchived = vm.LastArchived.ToString();
            LastBackuped = vm.LastBackuped.ToString();
        }
    }
}