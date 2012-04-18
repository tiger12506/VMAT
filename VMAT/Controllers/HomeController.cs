using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VMAT.Models;
using System.Management;
using System.IO;


namespace VMAT.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        //
        // GET: /

        public ActionResult Index()
        {
            return RedirectToAction("Index", "VirtualMachine");
        }

        //
        // GET: /Help

        public ActionResult Help()
        {
            return View();
        }

        //
        // GET: /About

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
                },
                LogoFile = "logo_rhit.png"
            });

            companies.Add(new Organization {
                Name = "Global Automation Partners",
                Authors = new List<string> {
                    "Brian Klimaszewski"
                },
                LogoFile = "logo_gap.png"
            });

            return View(companies);
        }
    }
}
