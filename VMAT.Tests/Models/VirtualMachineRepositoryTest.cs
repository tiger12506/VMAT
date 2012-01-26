using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VMAT.Tests.Models
{
    [TestClass]
    public class VirtualMachineRepositoryTest
    {
        [TestMethod]
        public void TestGetNextAvailableIP()
        {
            // Arrange
            var vmRepo = new MockVirtualMachineRepository();

            // Act
            string nextIP = vmRepo.GetNextAvailableIP();
            
            // Assert
            Assert.AreEqual(3, nextIP);
            Assert.Inconclusive("Not Implemented");
        }
    }
}
