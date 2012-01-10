﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VMAT.Models;
using VMAT.ViewModels;

namespace VMAT.Controllers
{
    public class VirtualMachineController : Controller
    {
        VirtualMachineManager manager = new VirtualMachineManager();
        DataEntities dataDB = new DataEntities();

        //
        // GET: /VirtualMachine/

        public ActionResult Index()
        {
            List<Project> projects = manager.GetProjectInfo();

            return View(projects);
        }

        //
        // POST: /VirtualMachine/ToggleStatus

        [HttpPost]
        public ActionResult ToggleStatus(string image)
        {
            var vm = new RunningVirtualMachine(image);

            if (vm.Status == VMStatus.Running)
                vm.PowerOff();
            else if (vm.Status == VMStatus.Stopped)
                vm.PowerOn();

            var results = new ToggleStatusViewModel {
                Status = vm.Status.ToString().ToLower(),
                LastStartTime = vm.LastStarted,
                LastShutdownTime = vm.LastStopped
            };

            return Json(results);
        }

        //
        // GET: /VirtualMachine/Create

        public ActionResult Create()
        {
            ViewBag.ProjectName = new SelectList(manager.GetProjectInfo(),
                "ProjectName", "ProjectName");
            ViewBag.BaseImageFile = new SelectList(Models.VirtualMachine.GetBaseImageFiles());

            return View();
        }

        //
        // POST: /VirtualMachine/Create

        [HttpPost]
        public ActionResult Create(Models.PendingVirtualMachine vm)
        {
            if (ModelState.IsValid)
            {
                dataDB.PendingVirtualMachines.Add(vm);
                dataDB.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ProjectName = new SelectList(manager.GetProjectInfo(),
                "ProjectName", "ProjectName");
            ViewBag.BaseImageFile = new SelectList(Models.VirtualMachine.GetBaseImageFiles());

            return View(vm);
        }

        //
        // GET: /VirtualMachine/Edit

        public ActionResult Edit(string img)
        {
            string imageFile = HttpUtility.UrlDecode(img);

            try
            {
                Models.VirtualMachine vm = new Models.RunningVirtualMachine(imageFile);
                ViewBag.ProjectName = new SelectList(manager.GetProjectInfo(),
                    "ProjectName", "ProjectName");

                return View(vm);
            }
            catch (Exception e)
            {
                /*return RedirectToAction("Error", "Home", 
                    new { ex = e, controller = this.ToString(), action = "Edit" });*/
                return RedirectToAction("Error", "Home");
            }
        }

        //
        // POST: /VirtualMachine/Edit

        [HttpPost]
        public ActionResult Edit(Models.VirtualMachine vm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            try
            {
                ViewBag.ProjectName = new SelectList(manager.GetProjectInfo(),
                    "ProjectName", "ProjectName");

                return View(vm);
            }
            catch (Exception e)
            {
                /*return RedirectToAction("Error", "Home", 
                    new { ex = e, controller = this.ToString(), action = "Edit" });*/
                return RedirectToAction("Error", "Home");
            }
        }

        //
        // POST: /VirtualMachine/ArchiveProject

        [HttpPost]
        public ActionResult ArchiveProject(string project)
        {
            /*var proj = GetProject(;

            foreach (var vm in proj.VirtualMachines)
            {
                //dataDB.ArchivedVirtualMachines.Add(vm);
            }*/

            var results = new ClosingProjectViewModel {
                Action = "archive",
                Time = DateTime.Now
            };

            return Json(results);
        }

        //
        // POST: /VirtualMachine/DeleteProject

        [HttpPost]
        public ActionResult DeleteProject(string project)
        {
            //var proj = proj.ProjectName;
            var results = new ClosingProjectViewModel {
                Action = "delete",
                Time = DateTime.Now
            };

            return Json(results);
        }
    }
}
