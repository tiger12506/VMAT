using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMAT.Models;
using VMAT.Services;
using VMAT.Tests.Services;

namespace VMAT.Tests.Models
{
    class MockVirtualMachineRepository : IVirtualMachineRepository
    {
        private List<VirtualMachine> vmList = new List<VirtualMachine>();
        
        public void CreateProject(Project proj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project> GetProjects()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VirtualMachine> GetVirtualMachines()
        {
            return vmList;
        }

        public VirtualMachine GetVirtualMachine(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void CreateRegisteredVirtualMachine(RegisteredVirtualMachine vm)
        {
            vmList.Add(vm);
        }

        public RegisteredVirtualMachine GetRegisteredVirtualMachine(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void CreatePendingArchiveVirtualMachine(PendingArchiveVirtualMachine vm)
        {
            vmList.Add(vm);
        }

        public PendingArchiveVirtualMachine GetPendingArchiveVirtualMachine(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void CreateArchivedVirtualMachine(ArchivedVirtualMachine vm)
        {
            vmList.Add(vm);
        }

        public ArchivedVirtualMachine GetArchivedVirtualMachine(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void CreatePendingVirtualMachine(PendingVirtualMachine vm)
        {
            vmList.Add(vm);
        }

        public PendingVirtualMachine GetPendingVirtualMachine(string imagePath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VirtualMachine> GetRegisteredVMs()
        {
            var imagePathNames = MockRegisteredVirtualMachineService.GetRegisteredVMImagePaths();
            var vmList = new List<VirtualMachine>();

            foreach (var path in imagePathNames)
            {
                RegisteredVirtualMachine vm;

                try
                {
                    vm = GetVirtualMachineByImagePath(path) as RegisteredVirtualMachine;
                }
                catch (ArgumentNullException)
                {
                    vm = new RegisteredVirtualMachine(path);
                    vmList.Add(vm);
                }
                catch (InvalidOperationException)
                {
                    vm = new RegisteredVirtualMachine(path);
                    vmList.Add(vm);
                }

                var service = new MockRegisteredVirtualMachineService(path);

                if (service.GetStatus() == VMStatus.Running)
                {
                    vm.Hostname = service.GetHostname();
                    vm.IP = service.GetIP();
                }

                vmList.Add(vm);
            }

            return vmList;
        }

        public string GetNextAvailableIP()
        {
            throw new NotImplementedException();
        }

        public VMStatus ToggleVMStatus(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void PowerOn(RegisteredVirtualMachine vm, RegisteredVirtualMachineService service)
        {
            throw new NotImplementedException();
        }

        public void PowerOff(RegisteredVirtualMachine vm, RegisteredVirtualMachineService service)
        {
            throw new NotImplementedException();
        }

        private VirtualMachine GetVirtualMachineByImagePath(string imagePathName)
        {
            foreach (var vm in vmList)
            {
                if (vm.ImagePathName == imagePathName)
                    return vm;
            }

            throw new NullReferenceException();
        }
    }
}
