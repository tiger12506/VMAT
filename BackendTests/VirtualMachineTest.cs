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
    public class VirtualMachineTest
    {
        [TestMethod]
        public void TestSetIP_Succ()
        {
            //arrange
            var newIP = "12.123.1.255";
            var mVM = new Mock<IVirtualMachine>();
            var mProc = new Mock<IProcess>();

            //setup functions that SetIP should call
            mProc.Setup(proc => proc.getExitCode()).Returns(0);
            mVM.Setup(foo => foo.RunProgramInGuest("notepad.exe")).Returns(mProc.Object); //todo mock returned Process
            var rVM = new VirtualMachine(mVM.Object);

            //act
            //rVM.SetIP(newIP);

            //assert
            mVM.VerifyAll();

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestSetIP_Fail()
        {
            //arrange
            var newIP = "12.123.1255";
            var mVM = new Mock<IVirtualMachine>();

            var mProc = new Mock<IProcess>();

            //setup functions that SetIP should call
            mProc.Setup(proc => proc.getExitCode()).Returns(-1);
            mVM.Setup(foo => foo.RunProgramInGuest("notepad.exe")).Returns(mProc.Object); //todo mock returned Process
            var rVM = new VirtualMachine(mVM.Object);

            //act, will throw exception
            //rVM.SetIP(newIP);

            //assert
            mVM.VerifyAll();

        }
    }
}
