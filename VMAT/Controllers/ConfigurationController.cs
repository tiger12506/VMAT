using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VMAT.Models;

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
            return RedirectToAction("Host");
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

        //
        // GET: /Configuration/Snapshot

        public ActionResult Snapshot()
        {

            VirtualMachineRepository target = new VirtualMachineRepository(); // TODO: Initialize to an appropriate value
            target.CreateSnapshot(target.GetAllRegisteredVirtualMachines().First(), DateTime.Today.DayOfWeek.ToString(), "Snapshot taken on " + DateTime.Now);
            return RedirectToAction("Host");
        }

        //
        // GET: /Configuration/Host

        public ActionResult Host()
        {
            return View(configRepo.GetHostConfiguration());
        }

        //
        // POST: /Configuration/Host

        [HttpPost]
        public ActionResult Host(HostConfiguration config)
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
