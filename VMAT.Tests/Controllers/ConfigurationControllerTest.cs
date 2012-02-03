using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VMAT.Controllers;

namespace VMAT.Tests.Controllers
{
    [TestClass]
    public class ConfigurationControllerTest
    {
        [TestMethod]
        public void ConfigurationIndex()
        {
            // Arrange
            var controller = new ConfigurationController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
