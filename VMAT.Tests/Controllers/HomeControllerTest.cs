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
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["controller"], "VirtualMachine");
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Help()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            ViewResult result = controller.Help() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
