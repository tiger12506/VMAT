using VMAT.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using VMAT.Models;

namespace VMAT.Tests
{
    
    
    /// <summary>
    ///This is a test class for RegisteredVirtualMachineServiceTest and is intended
    ///to contain all RegisteredVirtualMachineServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RegisteredVirtualMachineServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod]
        public void PowerOffTest()
        {
            VirtualMachineRepository vmr = new VirtualMachineRepository();
            VirtualMachine vm = vmr.GetAllRegisteredVirtualMachines().GetEnumerator().Current; // TODO: Initialize to an appropriate value
            RegisteredVirtualMachineService target = new RegisteredVirtualMachineService(vm); // TODO: Initialize to an appropriate value
            if (target.GetStatus() != VirtualMachine.PAUSED && target.GetStatus() != VirtualMachine.POWERINGOFF && target.GetStatus() != VirtualMachine.STOPPED)
            {
                target.PowerOff();
                Assert.IsTrue(target.GetStatus() == VirtualMachine.STOPPED || target.GetStatus() == VirtualMachine.POWERINGOFF);
            }
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        public void PauseTest()
        {
            VirtualMachineRepository vmr = new VirtualMachineRepository();
            VirtualMachine vm = vmr.GetAllRegisteredVirtualMachines().GetEnumerator().Current; // TODO: Initialize to an appropriate value
            RegisteredVirtualMachineService target = new RegisteredVirtualMachineService(vm); // TODO: Initialize to an appropriate value
            if (target.GetStatus() == VirtualMachine.RUNNING)
            {
                target.Pause();
                Assert.Equals(target.GetStatus(), VirtualMachine.PAUSED);
            }
            else
                Assert.Inconclusive();
        }

        [TestMethod]
        public void PowerOnTest()
        {
            VirtualMachineRepository vmr = new VirtualMachineRepository();
            VirtualMachine vm = vmr.GetAllRegisteredVirtualMachines().GetEnumerator().Current; // TODO: Initialize to an appropriate value
            RegisteredVirtualMachineService target = new RegisteredVirtualMachineService(vm); // TODO: Initialize to an appropriate value
            if (target.GetStatus() == VirtualMachine.STOPPED)
            {
                target.PowerOn();
                Assert.IsTrue(target.GetStatus() == VirtualMachine.POWERINGON || target.GetStatus() == VirtualMachine.RUNNING);
            }
            else
                Assert.Inconclusive();
        }
        
        [TestMethod]
        public void SetIPTest()
        {
            VirtualMachineRepository vmr = new VirtualMachineRepository();
            VirtualMachine vm = vmr.GetAllRegisteredVirtualMachines().GetEnumerator().Current; // TODO: Initialize to an appropriate value
            RegisteredVirtualMachineService target = new RegisteredVirtualMachineService(vm); // TODO: Initialize to an appropriate value
            string oldIP = target.GetIP();
            //set the new IP to the same first several sets of characters, but change the last few
            string value = oldIP.Substring(0, oldIP.LastIndexOf('.')) + ".200";
            target.SetIP(value);
            string newIP = target.GetIP();
            target.SetIP(oldIP);
            Assert.Equals(newIP, value);
        }

        [TestMethod]
        public void SetHostnameTest()
        {
            VirtualMachineRepository vmr = new VirtualMachineRepository();
            VirtualMachine vm = vmr.GetAllRegisteredVirtualMachines().GetEnumerator().Current; // TODO: Initialize to an appropriate value
            RegisteredVirtualMachineService target = new RegisteredVirtualMachineService(vm); // TODO: Initialize to an appropriate value
            string hostname = string.Empty; // TODO: Initialize to an appropriate value
            target.SetHostname(hostname);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod]
        public void UnpauseTest()
        {
            VirtualMachineRepository vmr = new VirtualMachineRepository();
            VirtualMachine vm = vmr.GetAllRegisteredVirtualMachines().GetEnumerator().Current; // TODO: Initialize to an appropriate value
            RegisteredVirtualMachineService target = new RegisteredVirtualMachineService(vm); // TODO: Initialize to an appropriate value
            if (target.GetStatus() == VirtualMachine.PAUSED)
            {
                target.Unpause();
                Assert.Equals(target.GetStatus(), VirtualMachine.RUNNING);
            }
            else
                Assert.Inconclusive();
        }
    }
}
