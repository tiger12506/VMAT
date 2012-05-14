using System;
using System.Web.Mvc;
using VMAT.Models;
using VMAT.ViewModels;

namespace VMAT.Controllers
{
    public class ConfigurationController : Controller
    {
        //
        // GET: /Configuration/Test

        public ActionResult ScheduleRunAll()
        {
            VMAT.Services.QuartzJobs.CreatePendingVMs();
            VMAT.Services.QuartzJobs.ArchivePendingVMs();
            VMAT.Services.QuartzJobs.CreateSnapshots();

            return RedirectToAction("Success");
        }

        public ActionResult ThrowException()
        {
            Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Test thrown by /Configuration/ThrowException"));
            
            return RedirectToAction("Host");

        }

        private IConfigurationRepository configRepo;

        public ConfigurationController() : this(new ConfigurationRepository()) { }

        public ConfigurationController(IConfigurationRepository config)
        {
            configRepo = config;
        }

        //
        // GET: /Configuration/

        public ActionResult Index()
        {
            return RedirectToAction("Host");
        }

        public ActionResult Success()
        {
            return View();
        }

        //
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

        //
        // GET: /Configuration/Check

        public ActionResult Check()
        {
            ViewData["ConfigCheckOutput"] = AppConfiguration.CheckConfigSettings();
            return View();
        }
    }
}
