using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VMAT.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Error/Forbidden

        public ActionResult Forbidden()
        {
            return View();
        }

        //
        // GET: /Error/PageNotFound

        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}
