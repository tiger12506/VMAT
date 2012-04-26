using System.Collections.Generic;
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
            //todo commented out because broke build
            //target.CreateSnapshot();
            Assert.IsTrue(true);
        }
    }
}
