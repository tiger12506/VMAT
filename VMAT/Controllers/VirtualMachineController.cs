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
            ViewBag.BaseImageFile = new SelectList(VirtualMachine.GetBaseImageFiles());

            return View();
        }

        //
        // POST: /VirtualMachine/Create

        [HttpPost]
        public ActionResult Create(VirtualMachineFormViewModel vmForm)
        {
            if (ModelState.IsValid)
            {
                var vm = new PendingVirtualMachine(vmForm);
                dataDB.PendingVirtualMachines.Add(vm);
                dataDB.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ProjectName = new SelectList(manager.GetProjectInfo(),
                "ProjectName", "ProjectName");
            ViewBag.BaseImageFile = new SelectList(VirtualMachine.GetBaseImageFiles());

            return View(vmForm);
        }

        //
        // GET: /VirtualMachine/Edit

        public ActionResult Edit(string img)
        {
            string imageFile = HttpUtility.UrlDecode(img);

            try
            {
                // TODO: Handle all VM types
                VirtualMachine vm = new RunningVirtualMachine(imageFile);
                var form = new VirtualMachineFormViewModel(vm);

                ViewBag.ProjectName = new SelectList(manager.GetProjectInfo(),
                    "ProjectName", "ProjectName");

                return View(form);
            }
            catch (Exception)
            {
                // TODO: Consider sending error information to Error page
                /*return RedirectToAction("Error", "Home", 
                    new { ex = e, controller = this.ToString(), action = "Edit" });*/
                return RedirectToAction("Error", "Home");
            }
        }

        //
        // POST: /VirtualMachine/Edit

        [HttpPost]
        public ActionResult Edit(VirtualMachine vm)
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
            catch (Exception)
            {
                // TODO: Consider sending error information to Error page
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
