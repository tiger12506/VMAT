using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using VMAT.Models;
using VMAT.Services;
using VMAT.ViewModels;

namespace VMAT.Controllers
{
    [HandleError]
    public class VirtualMachineController : Controller
    {
        IVirtualMachineRepository vmRepo;

        public VirtualMachineController() : this(new VirtualMachineRepository()) { }

        public VirtualMachineController(IVirtualMachineRepository repo)
        {
            vmRepo = repo;
        }

        //
        // GET: /VirtualMachine/

        public ActionResult Index()
        {
            IEnumerable<Project> projectList = vmRepo.GetProjects();
            var projectViewList = new List<ProjectViewModel>();

            foreach (var project in projectList)
            {
                ProjectViewModel projectView = new ProjectViewModel(project);
                
                foreach (var vm in project.VirtualMachines)
                {
                    if (vm.GetType() == typeof(RegisteredVirtualMachine))
                    {
                        var vmView = new RegisteredVirtualMachineViewModel(
                            vm as RegisteredVirtualMachine);

                        projectView.AddRegisteredVirtualMachineViewModel(vmView);
                    }
                    else if (vm.GetType() == typeof(PendingVirtualMachine))
                    {
                        var vmView = new PendingVirtualMachineViewModel(
                            vm as PendingVirtualMachine);

                        projectView.AddPendingVirtualMachineViewModel(vmView);
                    }
                    else if (vm.GetType() == typeof(PendingArchiveVirtualMachine))
                    {
                        var vmView = new PendingArchiveVirtualMachineViewModel(
                            vm as PendingArchiveVirtualMachine);

                        projectView.AddPendingArchiveVirtualMachineViewModel(vmView);
                    }
                    else if (vm.GetType() == typeof(ArchivedVirtualMachine))
                    {
                        var vmView = new ArchivedVirtualMachineViewModel(
                            vm as ArchivedVirtualMachine);

                        projectView.AddArchivedVirtualMachineViewModel(vmView);
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
            RegisteredVirtualMachine vm = vmRepo.GetRegisteredVirtualMachine(image);

            DateTime started = vm.LastStarted;
            DateTime stopped = vm.LastStopped;

            var service = new RegisteredVirtualMachineService(image);
            VMStatus status = service.GetStatus();

            if (status == VMStatus.Running)
                stopped = service.PowerOff();
            else if (status == VMStatus.Stopped)
                started = service.PowerOn();

            status = service.GetStatus();

            var results = new ToggleStatusViewModel {
                Status = status.ToString().ToLower(),
                LastStartTime = started,
                LastShutdownTime = stopped
            };

            return Json(results);
        }

        //
        // GET: /VirtualMachine/Create

        public ActionResult Create()
        {
            int nextIP = vmRepo.GetNextAvailableIP();
            ViewBag.ProjectName = new SelectList(vmRepo.GetProjects(),
                "ProjectName", "ProjectName");
            ViewBag.BaseImageFile = new SelectList(VirtualMachineManager.GetBaseImageFiles());
            ViewBag.IP = nextIP;

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
                vmRepo.CreatePendingVirtualMachine(vm);

                return RedirectToAction("Index");
            }

            ViewBag.ProjectName = new SelectList(vmRepo.GetProjects(),
                "ProjectName", "ProjectName");
            ViewBag.BaseImageFile = new SelectList(VirtualMachineManager.GetBaseImageFiles());

            return View(vmForm);
        }

        //
        // GET: /VirtualMachine/Edit

        public ActionResult Edit(string img)
        {
            string imageFile = HttpUtility.UrlDecode(img);
            int nextIP = vmRepo.GetNextAvailableIP();

            // TODO: Handle all VM types
            VirtualMachine vm = new RegisteredVirtualMachine(imageFile);
            var form = new VirtualMachineFormViewModel(vm);

            ViewBag.ProjectName = new SelectList(vmRepo.GetProjects(),
                "ProjectName", "ProjectName");
            ViewBag.IP = nextIP;

            return View(form);
        }

        //
        // POST: /VirtualMachine/Edit

        [HttpPost]
        public ActionResult Edit(VirtualMachineFormViewModel vm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            ViewBag.ProjectName = new SelectList(vmRepo.GetProjects(),
                "ProjectName", "ProjectName");

            return View(vm);
        }

        //
        // POST: /VirtualMachine/GetNextIP

        [HttpPost]
        public ActionResult GetNextIP()
        {
            int nextIP = vmRepo.GetNextAvailableIP();

            return Json(nextIP);
        }

        //
        // POST: /VirtualMachine/ArchiveMachine

        [HttpPost]
        public ActionResult ArchiveMachine(RegisteredVirtualMachine vm)
        {
            try
            {
                vmRepo.CreatePendingArchiveVirtualMachine(new PendingArchiveVirtualMachine(vm));
            }
            catch (Exception)
            {
                // TODO: Handle exception
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
