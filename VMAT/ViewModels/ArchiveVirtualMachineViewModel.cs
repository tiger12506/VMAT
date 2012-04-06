﻿using VMAT.Models;

namespace VMAT.ViewModels
{
    public class ArchiveVirtualMachineViewModel : VirtualMachineViewModel
    {
        public string MachineName { get; set; }
        public string ArchiveTime { get; set; }

        public ArchiveVirtualMachineViewModel() { }

        public ArchiveVirtualMachineViewModel(ArchivedVirtualMachine vm)
        {
            MachineName = vm.MachineName;
            ArchiveTime = vm.LastArchived.ToString();
        }
    }
}
