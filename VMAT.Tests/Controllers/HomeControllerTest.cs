using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VMAT.Controllers;

namespace VMAT.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestIndexRedirects()
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
        public void TestAbout()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("About", result.ViewName);
        }

        [TestMethod]
        public void TestHelp()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            ViewResult result = controller.Help() as ViewResult;

            // Assert
            Assert.IsNotNull("Help", result.ViewName);
        }
    }
}
