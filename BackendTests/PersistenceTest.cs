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
        [TestInitialize]
        public void Setup()
        {
            System.IO.File.Copy(@"C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/Host.xls",
                @"C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/HostTest.xls", true);
            System.IO.File.Copy(@"C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/VirtualMachines.xls",
                @"C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/VirtualMachinesTest.xls", true);
            Persistence.ChangeFileLocations(@"C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/HostTest.xls",
                @"C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/VirtualMachinesTest.xls");
        }

        [TestCleanup]
        public void Cleanup()
        {
            System.IO.File.Delete("C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/HostTest.xls");
            System.IO.File.Delete("C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/VirtualMachinesTest.xls");
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
    }
}
