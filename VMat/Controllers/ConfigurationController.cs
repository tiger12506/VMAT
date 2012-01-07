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
            return View();
        }

        //
        // GET: /Configuration/Host

        public ActionResult Host()
        {
            var config = new HostConfiguration();

            return View(config);
        }
    }
}
