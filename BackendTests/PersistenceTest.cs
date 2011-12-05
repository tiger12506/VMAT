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
        [TestMethod]
        public void TestGetValue()
        {
            string value = BackendVMWare.Persistence.GetValue("MaxIP");
            Assert.AreEqual(value, "192.168.1.255");
        }

        [TestMethod]
        public void TestWriteData()
        {
            string option = "What";
            string value = "lol";

            BackendVMWare.Persistence.WriteData(option, value);

            string result = BackendVMWare.Persistence.GetValue("What");
            Assert.AreEqual(result, "lol");
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
