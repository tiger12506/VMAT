using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VMAT.Controllers;

namespace VMAT.Tests.Controllers
{
    [TestClass]
    public class ErrorControllerTest
    {
        [TestMethod]
        public void ErrorIndex()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Forbidden()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var result = controller.Forbidden();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PageNotFound()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var result = controller.PageNotFound();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
