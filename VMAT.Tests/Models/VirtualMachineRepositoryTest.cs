using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using VMAT.Models;
using Vestris.VMWareLib;
using VMAT.Services;

namespace VMAT.Tests.Models
{
    [TestClass]
    public class VirtualMachineRepositoryTest
    {
        private Mock<DbContext> mockDB = new Mock<DbContext>();
        
        [TestMethod]
        public void GetNextAvailableIP()
        {
            // Arrange
            var vmRepo = new VirtualMachineRepository(true);
            var ipList = new List<string>() { "192.168.1.1", "192.168.1.3", "192.168.1.5", "192.168.1.6" };

            // Act
            string nextIP = vmRepo.GetNextAvailableIP(ipList);
            
            // Assert
            Assert.AreEqual(nextIP,"192.168.1.2");
        }

        [TestMethod]
        public void CreateSnapshotTest()
        {
            VirtualMachineRepository target = new VirtualMachineRepository(); // TODO: Initialize to an appropriate value
            string name = System.Guid.NewGuid().ToString();
            VirtualMachine vm = target.GetAllRegisteredVirtualMachines().GetEnumerator().Current;
            target.CreateSnapshot(vm, name, "");
            VMWareVirtualHost virtualHost = new VMWareVirtualHost();
            VMWareVirtualMachine vmw = virtualHost.Open(vm.ImagePathName);
            VMWareSnapshot s = vmw.Snapshots.GetNamedSnapshot(name);
            //not sure if we need to create a boolean first to see if s is null
            s.RemoveSnapshot();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void ToggleVMStatusTest()
        {
            VirtualMachineRepository target = new VirtualMachineRepository(); // TODO: Initialize to an appropriate value
            VirtualMachine vm = target.GetAllRegisteredVirtualMachines().GetEnumerator().Current;
            var service = new RegisteredVirtualMachineService(vm.ImagePathName);
            if (service.IsRunning())
            {
                int status = target.ToggleVMStatus(vm.VirtualMachineId);
                Assert.IsTrue(status == VirtualMachine.STOPPED || status == VirtualMachine.POWERINGOFF);
            }
            else
            {
                int status = target.ToggleVMStatus(vm.VirtualMachineId);
                Assert.IsTrue(status == VirtualMachine.RUNNING || status == VirtualMachine.POWERINGON);
            }
        }
    }
}
