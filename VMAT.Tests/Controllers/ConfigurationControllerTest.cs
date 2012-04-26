using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VMAT.Controllers;
using VMAT.Models;

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
        /*
         *         //
        // GET: /Configuration/Host

        public ActionResult Host()
        {
            return View(new ConfigurationFormViewModel(configRepo.GetHostConfiguration()));
        }

        //
        // POST: /Configuration/Host

        [HttpPost]
        public ActionResult Host(ConfigurationFormViewModel config)
        {
            if (ModelState.IsValid)
            {
                configRepo.SetHostConfiguration(config);

                return RedirectToAction("Index", "VirtualMachine");
            }

            return View(config);
        }
*/
        [TestMethod]
        public void TestGetConfigurationEditForm()
        {
            //arrange
            var controller = new ConfigurationController();
            var hostConfig = new HostConfiguration();
            var mockConfigRepo = new Mock<IConfigurationRepository>();
            mockConfigRepo.Setup(r => r.GetHostConfiguration()).Returns(hostConfig);

            //act
            var result=controller.Host() as ViewResult;

            //assert
            mockConfigRepo.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(ViewModels.ConfigurationFormViewModel));
        }

        [TestMethod]
        public void TestPostConfigurationValid()
        {
            //arrange
            var controller = new ConfigurationController();
            //var hostConfig = new HostConfiguration();
            var configFormVM = new ViewModels.ConfigurationFormViewModel();
            var mockConfigRepo = new Mock<IConfigurationRepository>();
            mockConfigRepo.Setup(r => r.SetHostConfiguration(configFormVM));

            //act
            var result = controller.Host() as RedirectToRouteResult;

            //assert
            mockConfigRepo.Verify();
            //incomplete\

        }
    }
}
