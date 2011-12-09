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
            System.IO.File.Copy("C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/Host.xls",
                "C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/HostTest.xls", true);
            System.IO.File.Copy("C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/VirtualMachines.xls",
                "C:/Users/Calvin/Documents/VMAT/VMAT/BackendVMWare/VirtualMachinesTest.xls", true);
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
            string value = BackendVMWare.Persistence.GetValue("MaxIP");
            Assert.AreEqual(value, "255");
        }

        [TestMethod]
        public void TestWriteData()
        {
            string option = "test";
            string value = "it worked";

            BackendVMWare.Persistence.WriteData(option, value);

            string result = BackendVMWare.Persistence.GetValue("test");
            Assert.AreEqual(result, "it worked");
        }

        [TestMethod]
        public void TestWriteVMIP()
        {
            string name = "gapdev1234";
            string ip = "192.168.1.16";

            BackendVMWare.Persistence.WriteVMIP(name, ip);

            string result = BackendVMWare.Persistence.GetIP(name);
            Assert.AreEqual(result, "192.168.1.16");
        }
    }
}
