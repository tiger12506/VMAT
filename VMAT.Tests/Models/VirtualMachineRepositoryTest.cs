using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VMAT.Models;

namespace VMAT.Tests.Models
{
    [TestClass]
    public class VirtualMachineRepositoryTest
    {
        [TestMethod]
        public void TestGetNextAvailableIP()
        {
            // Arrange
            var mockRepo = new Mock<IVirtualMachineRepository>();
            mockRepo.Setup(repo => repo.GetNextAvailableIP()).Returns("192.168.1.1");

            // Act
            IVirtualMachineRepository vmRepo = mockRepo.Object;
            string nextIP = vmRepo.GetNextAvailableIP();
            
            // Assert
            mockRepo.Verify();
        }
    }
}
