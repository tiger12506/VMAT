using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using VMAT.Models;
using VMAT.ViewModels;
using System.Text;
using System.Web.Script.Serialization;

namespace VMAT.Controllers
{
    [HandleError]
    public class VirtualMachineController : Controller
    {
        private IVirtualMachineRepository vmRepo;
        private IConfigurationRepository configRepo;

        public VirtualMachineController() : 
            this(new VirtualMachineRepository(), new ConfigurationRepository()) { }

        public VirtualMachineController(IVirtualMachineRepository vms, IConfigurationRepository config)
        {
            vmRepo = vms;
            configRepo = config;
        }

        //
        // GET: /VirtualMachine/

        public ActionResult Index()
        {
            IEnumerable<Project> projectList = vmRepo.GetProjects();
            var projectViewList = new List<ProjectViewModel>();

            foreach (var project in projectList)
            {
                projectViewList.Add(new ProjectViewModel(project));
            }

            ViewBag.CreationTime = configRepo.GetVmCreationTime();
            ViewBag.Hostname = "vmat.rose-hulman.edu"; // TODO: Pull from somewhere

            return View(projectViewList);
        }

        //
        // POST: /VirtualMachine/ToggleStatus

        [HttpPost]
        public ActionResult ToggleStatus(string image)
        {
            VMStatus status = vmRepo.ToggleVMStatus(image);
            RegisteredVirtualMachine vm = vmRepo.GetRegisteredVirtualMachine(image);

            var results = new ToggleStatusViewModel {
                Status = status.ToString().ToLower(),
                LastStartTime = vm.LastStarted,
                LastShutdownTime = vm.LastStopped
            };

            return Json(results);
        }

        //
        // POST: /VirtualMachine/UndoPendingOperation

        [HttpPost]
        public ActionResult UndoPendingOperation(string image)
        {
            vmRepo.DeleteVirtualMachine(image);
            return Json(image);
        }

        //
        // GET: /VirtualMachine/Create

        public ActionResult Create()
        {
            var vmForm = new VirtualMachineFormViewModel();
            ViewBag.ProjectName = new SelectList(vmRepo.GetProjects(),
                "ProjectName", "ProjectName");
            ViewBag.BaseImageFile = new SelectList(VirtualMachineRepository.GetBaseImageFiles());
            ViewBag.Hostname = "vmat.rose-hulman.edu"; // TODO: Pull from somewhere
            vmForm.IP = vmRepo.GetNextAvailableIP();

            return View(vmForm);
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
            ViewBag.BaseImageFile = new SelectList(VirtualMachineRepository.GetBaseImageFiles());
            ViewBag.Hostname = "vmat.rose-hulman.edu"; // TODO: Pull from somewhere
            vmForm.IP = vmRepo.GetNextAvailableIP();

            return View(vmForm);
        }

        //
        // GET: /VirtualMachine/Edit

        public ActionResult Edit(string img)
        {
            string imageFile = HttpUtility.UrlDecode(img);

            // TODO: Handle all VM types
            VirtualMachine vm = new RegisteredVirtualMachine(imageFile);
            var form = new VirtualMachineFormViewModel(vm);

            ViewBag.ProjectName = new SelectList(vmRepo.GetProjects(),
                "ProjectName", "ProjectName");
            ViewBag.Hostname = "vmat.rose-hulman.edu"; // TODO: Pull from somewhere

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
            ViewBag.Hostname = "vmat.rose-hulman.edu"; // TODO: Pull from somewhere

            return View(vm);
        }

        //
        // POST: /VirtualMachine/GetNextIP

        [HttpPost]
        public ActionResult GetNextIP()
        {
            string nextIP = vmRepo.GetNextAvailableIP();

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
            string folderName = AppConfiguration.GetWebserverVmPath() + project;
            ArchivedVirtualMachine.ArchiveFile(folderName, folderName + ".7z");

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
