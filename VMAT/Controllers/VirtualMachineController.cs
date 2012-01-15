using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VMAT.Models;
using VMAT.ViewModels;

namespace VMAT.Controllers
{
    [HandleError]
    public class VirtualMachineController : Controller
    {
        VirtualMachineManager manager = new VirtualMachineManager();
        DataEntities dataDB = new DataEntities();

        //
        // GET: /VirtualMachine/

        public ActionResult Index()
        {
            IEnumerable<Project> projects = manager.GetProjects();

            return View(projects);
        }

        //
        // POST: /VirtualMachine/ToggleStatus

        [HttpPost]
        public ActionResult ToggleStatus(string image)
        {
            //var vm = new RunningVirtualMachine(image);
            RunningVirtualMachine vm = dataDB.RunningVirtualMachines.Include("ImagePathName").
                Single(d => d.ImagePathName == image);

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

        [HandleError]
        public ActionResult Edit(string img)
        {
            string imageFile = HttpUtility.UrlDecode(img);

            // TODO: Handle all VM types
            VirtualMachine vm = new RunningVirtualMachine(imageFile);
            var form = new VirtualMachineFormViewModel(vm);

            ViewBag.ProjectName = new SelectList(manager.GetProjectInfo(),
                "ProjectName", "ProjectName");

            return View(form);
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

            ViewBag.ProjectName = new SelectList(manager.GetProjectInfo(),
                "ProjectName", "ProjectName");

            return View(vm);
        }

        //
        // POST: /VirtualMachine/ArchiveMachine

        [HttpPost]
        public ActionResult ArchiveMachine(RunningVirtualMachine vm)
        {
            try
            {
                dataDB.PendingArchiveVirtualMachines.Add(
                    new PendingArchiveVirtualMachine(vm));
                dataDB.RunningVirtualMachines.Remove(vm);
            }
            catch (Exception)
            {

            }
            
            return RedirectToAction("Index");
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
