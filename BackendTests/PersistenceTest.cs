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
        public void TestWriteValue()
        {
            string option = "What";
            string value = "lol";

            BackendVMWare.Persistence.WriteData(option, value);
        }
    }
}
