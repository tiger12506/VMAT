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
            IEnumerable<Project> projectList = manager.GetProjects();
            var projectViewList = new List<ProjectViewModel>();

            foreach (var project in projectList)
            {
                ProjectViewModel projectView = new ProjectViewModel();

                projectView.ProjectName = project.ProjectName;
                projectView.Hostname = project.Hostname;

                foreach (var vm in project.VirtualMachines)
                {
                    if (vm.GetType() == typeof(RegisteredVirtualMachine))
                    {
                        var vmView = new RegisteredVirtualMachineViewModel();

                        RegisteredVirtualMachineService.SetRegisteredVirtualMachine(vm.ImagePathName);

                        vmView.ImagePathName = vm.ImagePathName;
                        vmView.Status = RegisteredVirtualMachineService.GetStatus().ToString().ToLower();
                        vmView.MachineName = vm.GetMachineName();
                        vmView.IP = ((RegisteredVirtualMachine)vm).IP;
                        vmView.CreatedTime = ((RegisteredVirtualMachine)vm).CreatedTime.ToString();
                        vmView.LastStopped = ((RegisteredVirtualMachine)vm).LastStopped.ToString();
                        vmView.LastStarted = ((RegisteredVirtualMachine)vm).LastStarted.ToString();
                        vmView.LastArchived = ((RegisteredVirtualMachine)vm).LastArchived.ToString();
                        vmView.LastBackuped = ((RegisteredVirtualMachine)vm).LastBackuped.ToString();
                        vmView.BaseImageName = vm.BaseImageName;

                        projectView.RegisteredVMs.Add(vmView);
                    }
                    else if (vm.GetType() == typeof(PendingVirtualMachine))
                    {

                    }
                    else if (vm.GetType() == typeof(PendingArchiveVirtualMachine))
                    {

                    }
                    else if (vm.GetType() == typeof(ArchivedVirtualMachine))
                    {

                    }
                }

                projectViewList.Add(projectView);
            }

            return View(projectViewList);
        }

        //
        // POST: /VirtualMachine/ToggleStatus

        [HttpPost]
        public ActionResult ToggleStatus(string image)
        {
            RegisteredVirtualMachine vm = dataDB.VirtualMachines.
                OfType<RegisteredVirtualMachine>().Single(d => d.ImagePathName == image);

            RegisteredVirtualMachineService.SetRegisteredVirtualMachine(image);
            VMStatus status = RegisteredVirtualMachineService.GetStatus();

            if (status == VMStatus.Running)
                RegisteredVirtualMachineService.PowerOff();
            else if (status == VMStatus.Stopped)
                RegisteredVirtualMachineService.PowerOn();

            dataDB.SaveChanges();
            status = RegisteredVirtualMachineService.GetStatus();

            var results = new ToggleStatusViewModel {
                Status = status.ToString().ToLower(),
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
            ViewBag.BaseImageFile = new SelectList(VirtualMachineManager.GetBaseImageFiles());

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
                dataDB.VirtualMachines.Add(vm);
                dataDB.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ProjectName = new SelectList(manager.GetProjectInfo(),
                "ProjectName", "ProjectName");
            ViewBag.BaseImageFile = new SelectList(VirtualMachineManager.GetBaseImageFiles());

            return View(vmForm);
        }

        //
        // GET: /VirtualMachine/Edit

        [HandleError]
        public ActionResult Edit(string img)
        {
            string imageFile = HttpUtility.UrlDecode(img);

            // TODO: Handle all VM types
            VirtualMachine vm = new RegisteredVirtualMachine(imageFile);
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
        public ActionResult ArchiveMachine(RegisteredVirtualMachine vm)
        {
            try
            {
                dataDB.VirtualMachines.Add(new PendingArchiveVirtualMachine(vm));
                dataDB.VirtualMachines.Remove(vm);
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
