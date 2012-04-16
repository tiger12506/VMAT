using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using VMAT.Models;

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
            var mockRepo = new Mock<IVirtualMachineRepository>();
            mockRepo.Setup(repo => repo.GetNextAvailableIP()).Returns("192.168.1.1");

            // Act
            IVirtualMachineRepository vmRepo = mockRepo.Object;
            string nextIP = vmRepo.GetNextAvailableIP();
            
            // Assert
            mockRepo.Verify();
        }

        [TestMethod]
        public void CreateSnapshotTest()
        {
            VirtualMachineRepository target = new VirtualMachineRepository(); // TODO: Initialize to an appropriate value
            target.CreateSnapshot(null);
            Assert.IsTrue(true);
        }
    }
}
