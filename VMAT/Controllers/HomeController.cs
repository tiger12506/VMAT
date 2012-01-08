﻿using System;
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
        // POST: /Error

        /*[HttpPost]
        public ActionResult Error(Exception ex, string controller, string action)
        {
            var error = new HandleErrorInfo(ex, controller, action);

            return View(error);
        }*/

        public ActionResult Error()
        {
            return View();
        }
    }
}
