using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VMAT.Controllers;
using VMAT.Models;
using VMAT.ViewModels;
using Moq;

namespace VMAT.Tests.Controllers
{
    [TestClass]
    public class VirtualMachineControllerTest
    {
        [TestMethod]
        public void VirtualMachineIndex()
        {
            // Arrange
            var mockVmRepo = new Mock<IVirtualMachineRepository>();
            var mockConfigRepo = new Mock<IConfigurationRepository>();

            var projects = new List<ProjectViewModel>();
            projects.Add(new ProjectViewModel());
            projects.Add(new ProjectViewModel());

            var controller = new VirtualMachineController(mockVmRepo.Object, mockConfigRepo.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            var model = result.ViewData.Model as IList<ProjectViewModel>;
            Assert.AreEqual(2, model.Count);
        }

        [TestMethod]
        public void VirtualMachineCreate()
        {
            // Arrange
            var mockVmRepo = new Mock<IVirtualMachineRepository>();
            var mockConfigRepo = new Mock<IConfigurationRepository>();

            var controller = new VirtualMachineController(mockVmRepo.Object, mockConfigRepo.Object);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(VirtualMachineFormViewModel));
        }

        [TestMethod]
        public void VirtualMachineCreatePostValid()
        {
            // Arrange
            var mockVmRepo = new Mock<IVirtualMachineRepository>();
            var mockConfigRepo = new Mock<IConfigurationRepository>();

            var vm = new PendingVirtualMachine();

            mockVmRepo.Setup(v => v.CreatePendingVirtualMachine(vm));

            var projects = new List<ProjectViewModel>();
            projects.Add(new ProjectViewModel());
            projects.Add(new ProjectViewModel());

            var controller = new VirtualMachineController(mockVmRepo.Object, mockConfigRepo.Object);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            var model = result.ViewData.Model as IList<ProjectViewModel>;
            Assert.AreEqual(3, model.Count);
        }

        [TestMethod]
        public void VirtualMachineCreatePostInvalid()
        {
            // Arrange
            var mockVmRepo = new Mock<IVirtualMachineRepository>();
            var mockConfigRepo = new Mock<IConfigurationRepository>();

            var controller = new VirtualMachineController(mockVmRepo.Object, mockConfigRepo.Object);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(VirtualMachineFormViewModel));
        }
    }
}
