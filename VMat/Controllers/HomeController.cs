using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VMAT.Models;

namespace VMAT.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /
        public ActionResult Index()
        {
            return RedirectToAction("Index", "VirtualMachine");
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult About()
        {
            var companies = new List<Organization>();

            companies.Add(new Organization { 
                Name = "Rose-Hulman Institute of Technology",
                Authors = new List<string> { 
                    "Nathan Mendel",
                    "Anthony Sylvain",
                    "Calvin Mlynarczyk",
                    "Jacob Schmidt"
                }
            });

            companies.Add(new Organization {
                Name = "Global Automation Partners",
                Authors = new List<string> {
                    "Brian Klimaszewski"
                }
            });

            return View(companies);
        }

        //
        // GET: /Error?data=lol&error=sucks

        public ActionResult Error(object data, Exception error)
        {
            ViewBag.Data = data;
            ViewBag.Error = error;

            return View();
        }
    }
}
