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
    public class TestVMManager
    {
        [TestMethod]
        public void testCreateVM()
        {
            //arrange
            var imageLocation = @"c:/vm.vmx";
            var vmm = new BackendVMWare.VMManager();
            var mHost = new Mock<IVirtualHost>();
            var mVM = new Mock<IVirtualMachine>();
            mHost.Setup(foo => foo.ConnectToVMWareServer("vmat.reshall.rose-hulman.edu", "Nathan", "Vmat1234"));
            mHost.Setup(foo => foo.Open(imageLocation)).Returns(mVM.Object);
            mVM.Setup(foo => foo.WaitForToolsInGuest());
            mVM.Setup(foo => foo.LoginInGuest("Administrator", "password"));

            //act
            vmm.createServer(mHost.Object, imageLocation);

            //assert
            mHost.VerifyAll();
            mVM.VerifyAll();

        }
    }
}
