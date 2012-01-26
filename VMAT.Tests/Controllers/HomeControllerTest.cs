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
            var destController = new VirtualMachineController();

            // Act
            var result = controller.Index() as RedirectResult;
            var destView = destController.Index() as ViewResult;

            // Assert
            Assert.AreEqual(destView, result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
