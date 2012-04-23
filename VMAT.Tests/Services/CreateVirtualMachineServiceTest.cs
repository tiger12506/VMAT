using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VMAT.Models.VMware;
using VMAT.Services;

namespace VMAT.Tests.Services
{
    [TestClass]
    public class CreateVirtualMachineServiceTest
    {
        //TODO: doesn't test right method


        [TestMethod]
        public void CreateVirtualMachine()
        {
            // Arrange
            var imageLocation = @"c:/vm.vmx";
            var mHost = new Mock<IVirtualHost>();
            var mVM = new Mock<IVirtualMachine>();
            var service = new CreateVirtualMachineService(mVM.Object);
            mHost.Setup(host => host.ConnectToVMWareServer("vmat.reshall.rose-hulman.edu", "Nathan", "Vmat1234"));
            mHost.Setup(host => host.Open(imageLocation)).Returns(mVM.Object);
            mVM.Setup(vm => vm.WaitForToolsInGuest());
            mVM.Setup(vm => vm.LoginInGuest("Administrator", "password"));

            // Act
            service.CreateVM();

            // Assert
            mHost.VerifyAll();
            mVM.VerifyAll();
        }

    }
}
