using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VMAT.Controllers;

namespace VMAT.Tests.Controllers
{
    [TestClass]
    public class ErrorControllerTest
    {
        [TestMethod]
        public void TestIndex()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestForbidden()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var result = controller.Forbidden() as ViewResult;

            // Assert
            Assert.AreEqual("Forbidden", result.ViewName);
        }

        [TestMethod]
        public void TestPageNotFound()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var result = controller.PageNotFound() as ViewResult;

            // Assert
            Assert.AreEqual("PageNotFound", result.ViewName);
        }
    }
}
