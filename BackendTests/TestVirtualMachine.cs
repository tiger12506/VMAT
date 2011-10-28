using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Vestris.VMWareLib;
using BackendVMWare;

namespace BackendTests
{
    [TestClass]
    public class TestVirtualMachine
    {
        [TestMethod]
        public void TestSetIP_Succ()
        {
            //arrange
            var newIP = "12.123.1.255";
            var mVM = new Mock<IVirtualMachine>();
            //setup functions that SetIP should call

            mVM.Setup(foo => foo.RunProgramInGuest("notepad.exe")); //todo mock returned Process
            var rVM = new VirtualMachine(mVM.Object);

            //act
            rVM.SetIP(newIP);

            //assert
            mVM.VerifyAll();

        }

        [TestMethod]
        public void TestSetIP_Fail()
        {
            //arrange
            var newIP = "12.123.1255";
            var mVM = new Mock<IVirtualMachine>();

            //setup functions that SetIP should call

            mVM.Setup(foo => foo.RunProgramInGuest("notepad.exe")); //todo mock returned Process
            var rVM = new VirtualMachine(mVM.Object);

            //act
            rVM.SetIP(newIP);

            //assert
            mVM.VerifyAll();

        }
    }
}
