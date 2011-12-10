using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Vestris.VMWareLib;
using BackendVMWare;

namespace BackendTests
{
    [TestClass]
    public class VMManagerTest
    {
        //TODO doesn't test right method
        [TestMethod]
        public void TestCreateVM()
        {
            //arrange
            var imageLocation = @"c:/vm.vmx";
            var vmm = new BackendVMWare.VMManager();
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

            VMManager man = new VMManager();
            var imageLocation = @"c:/vm.vmx";
            var mHost = new Mock<IVirtualHost>();
            var mVM = new Mock<IVirtualMachine>();
            mHost.Setup(host => host.ConnectToVMWareServer("vmat.reshall.rose-hulman.edu", "Nathan", "Vmat1234"));
            mHost.Setup(host => host.Open(imageLocation)).Returns(mVM.Object);
            mVM.Setup(vm => vm.WaitForToolsInGuest());
            mVM.Setup(vm => vm.LoginInGuest("Administrator", "password"));

            int nextIP = man.GetNextAvailableIP();
            Assert.AreEqual(3, nextIP);
        }
    }
}
