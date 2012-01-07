using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VMAT;
using VMAT.Models;
using VMAT.Models.VMware;
using System.Data;
using System.Web.Configuration;
using System.Configuration;

namespace VMAT.Tests
{
    [TestClass]
    public class PersistenceTest
    {
        private string hostPath;
        private string vmPath;
        private string testHostPath;
        private string testVMPath;

        public PersistenceTest()
        {
            AppConfiguration.SetWebConfigurationFile(
                WebConfigurationManager.OpenWebConfiguration(Properties.Settings.Default["WebConfigFilePath"].ToString()).AppSettings);
            hostPath = AppConfiguration.GetDataFilesDirectory() + "Host.xls";
            vmPath = AppConfiguration.GetDataFilesDirectory() + "VirtualMachines.xls";
            testHostPath = AppConfiguration.GetDataFilesDirectory() + "HostTest.xls";
            testVMPath = AppConfiguration.GetDataFilesDirectory() + "VirtualMachinesTest.xls";
        }

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

            string result = Persistence.GetIP(name);
            Assert.AreEqual("192.168.1.16", result);
        }

        [TestMethod]
        public void TestGetVirtualMachineData()
        {
            DataTable actualData = Persistence.GetVirtualMachineData();

            DataTable expectedData = new DataTable("VirtualMachines");
            expectedData.MinimumCapacity = 99;
            expectedData.Columns.Add("Name");
            expectedData.Columns.Add("IP");
            expectedData.Rows.Add(new string[] { "gapdev1234", "192.168.1.1" });
            expectedData.Rows.Add(new string[] { "gapdev5678", "192.168.1.2" });
            expectedData.Rows.Add(new string[] { "gapdev9999", "192.168.1.255" });

            Assert.AreEqual(expectedData, actualData);
        }

        [TestMethod]
        public void TestGetNextAvailableIP()
        {
            int nextIP = Persistence.GetNextAvailableIP();
            Assert.AreEqual(3, nextIP);
        }
    }
}
