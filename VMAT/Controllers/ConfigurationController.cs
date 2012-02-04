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
        // GET: /Configuration/

        public ActionResult Index()
        {
            return RedirectToAction("Host");
        }

        //
        // GET: /Configuration/Host

        public ActionResult Host()
        {
            var config = new HostConfiguration();

            return View(config);
        }

        //
        // POST: /Configuration/Host

        [HttpPost]
        public ActionResult Host(HostConfiguration config)
        {
            if (ModelState.IsValid)
            {
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
