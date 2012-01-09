using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Vestris.VMWareLib;
using VMAT;
using VMAT.Models;
using VMAT.Models.VMware;

namespace VMAT.Tests
{
    [TestClass]
    public class VMManagerTest
    {
        //TODO: doesn't test right method
        [TestMethod]
        public void TestCreateVM()
        {
            //arrange
            var imageLocation = @"c:/vm.vmx";
            var vmm = new VirtualMachineManager();
            var mHost = new Mock<IVirtualHost>();
            var mVM = new Mock<IVirtualMachine>();
            mHost.Setup(host => host.ConnectToVMWareServer("vmat.reshall.rose-hulman.edu", "Nathan", "Vmat1234"));
            mHost.Setup(host => host.Open(imageLocation)).Returns(mVM.Object);
            mVM.Setup(vm => vm.WaitForToolsInGuest());
            mVM.Setup(vm => vm.LoginInGuest("Administrator", "password"));

            //act
            //vmm.CreateServer(mHost.Object, imageLocation);
            

            //assert
            mHost.VerifyAll();
            mVM.VerifyAll();
        }

        [TestMethod]
        public void TestGetNextAvailableIP()
        {
            Persistence.ChangeFileLocations(@"C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/HostTest.xls",
                @"C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/VirtualMachinesTest.xls");

            VirtualMachineManager man = new VirtualMachineManager();
            var imageLocation = @"c:/vm.vmx";
            var mHost = new Mock<IVirtualHost>();
            var mVM = new Mock<IVirtualMachine>();
            mHost.Setup(host => host.ConnectToVMWareServer("vmat.reshall.rose-hulman.edu", "Nathan", "Vmat1234"));
            mHost.Setup(host => host.Open(imageLocation)).Returns(mVM.Object);
            mVM.Setup(vm => vm.WaitForToolsInGuest());
            mVM.Setup(vm => vm.LoginInGuest("Administrator", "password"));

            int nextIP = VirtualMachineManager.GetNextAvailableIP();
            //Assert.AreEqual(3, nextIP);
            Assert.Inconclusive("Method moved to Persistence.cs");
        }
    }
}
