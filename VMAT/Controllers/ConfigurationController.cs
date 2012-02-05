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
