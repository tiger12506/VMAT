using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VMAT.Models.VMware;

namespace VMAT.Tests.Services
{
    [TestClass]
    public class CreateVirtualMachineServiceTest
    {
        //TODO: doesn't test right method
        [TestMethod]
        public void TestCreateVM()
        {
            // Arrange
            var imageLocation = @"c:/vm.vmx";
            var mHost = new Mock<IVirtualHost>();
            var mVM = new Mock<IVirtualMachine>();
            mHost.Setup(host => host.ConnectToVMWareServer("vmat.reshall.rose-hulman.edu", "Nathan", "Vmat1234"));
            mHost.Setup(host => host.Open(imageLocation)).Returns(mVM.Object);
            mVM.Setup(vm => vm.WaitForToolsInGuest());
            mVM.Setup(vm => vm.LoginInGuest("Administrator", "password"));

            // Act
            

            // Assert
            mHost.VerifyAll();
            mVM.VerifyAll();
            Assert.Inconclusive("Not Implemented");
        }
    }
}
