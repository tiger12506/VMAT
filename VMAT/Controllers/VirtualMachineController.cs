using System;
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
            var vm = new VirtualMachine(image);

            if (vm.Status == VMStatus.Running)
                vm.Status = VMStatus.Stopped;
            else if (vm.Status == VMStatus.Stopped)
                vm.Status = VMStatus.Running;

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
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        //
        // GET: /VirtualMachine/Edit

        public ActionResult Edit(string img)
        {
            string imageFile = HttpUtility.HtmlDecode(img);

            try
            {
                Models.VirtualMachine vm = new Models.VirtualMachine(imageFile);
                ViewBag.Projects = new SelectList(manager.GetProjectInfo(),
                    "Project", "ProjectName", vm.ProjectName);

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

            return View(vm);
        }

        //
        // POST: /VirtualMachine/ArchiveProject

        [HttpPost]
        public ActionResult ArchiveProject(Models.Project proj)
        {
            var projName = proj.ProjectName;

            return Json(projName);
        }

        //
        // POST: /VirtualMachine/DeleteProject

        [HttpPost]
        public ActionResult DeleteProject(Models.Project proj)
        {
            var projName = proj.ProjectName;

            return Json(projName);
        }
    }
}
