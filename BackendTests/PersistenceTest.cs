using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendVMWare;

namespace BackendTests
{
    [TestClass]
    public class PersistenceTest
    {
        private string hostPath = Config.GetDataFilesDirectory() + "/Host.xls";
        private string vmPath = Config.GetDataFilesDirectory() + "/VirtualMachines.xls";
        private string testHostPath = Config.GetDataFilesDirectory() + "/HostTest.xls";
        private string testVMPath = Config.GetDataFilesDirectory() + "/VirtualMachinesTest.xls";

        [TestInitialize]
        public void Setup()
        {
            System.IO.File.Copy(hostPath, testHostPath, true);
            System.IO.File.Copy(vmPath, testVMPath, true);
            Persistence.ChangeFileLocations(testHostPath, testVMPath);
        }

        [TestCleanup]
        public void Cleanup()
        {
            System.IO.File.Delete(testHostPath);
            System.IO.File.Delete(testVMPath);
        }

        [TestMethod]
        public void TestGetValue()
        {
            string value = Persistence.GetValue("maxIP");
            Assert.AreEqual(value, "255");
        }

        [TestMethod]
        public void TestGetIP()
        {
            string value = Persistence.GetIP("gapdev1234");
            Assert.AreEqual(value, "192.168.1.1");
        }

        [TestMethod]
        public void TestWriteData()
        {
            string option = "maxIP";
            string value = "1000";

            Persistence.WriteData(option, value);

            string result = Persistence.GetValue("maxIP");
            Assert.AreEqual("1000", result);
        }

        [TestMethod]
        public void TestWriteVMIP()
        {
            string name = "gapdev1234";
            string ip = "192.168.1.16";

            Persistence.WriteVMIP(name, ip);

            string result = BackendVMWare.Persistence.GetIP(name);
            Assert.AreEqual("192.168.1.16", result);
        }

        [TestMethod]
        public void TestGetVirtualMachineData()
        {
            Assert.Inconclusive("Unimplemented Test");
        }

        [TestMethod]
        public void TestGetNextAvailableIP()
        {
            int nextIP = Persistence.GetNextAvailableIP();
            Assert.AreEqual(3, nextIP);
        }
    }
}
