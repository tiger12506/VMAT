using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VMAT.Controllers;

namespace VMAT.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void HomeIndexRedirectsToVirtualMachine()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("VirtualMachine", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.About();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Help()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Help();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
