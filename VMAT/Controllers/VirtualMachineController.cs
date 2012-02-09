﻿using System;
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

            ViewBag.CreationTime = configRepo.GetVmCreationTime().ToLongTimeString();
            ViewBag.ArchiveTime = configRepo.GetVmArchiveTime().ToLongTimeString();
            ViewBag.BackupTime = configRepo.GetVmBackupTime().ToLongTimeString();

            return View(projectViewList);
        }

        //
        // GET: /VirtualMachine/Create

        public ActionResult Create()
        {
            var vmForm = new VirtualMachineFormViewModel();
            var projectName = new SelectList(vmRepo.GetProjects(),
                "ProjectName", "ProjectName");
            
            foreach (var item in projectName)
            {
                item.Value = item.Value.Substring(item.Value.LastIndexOf('G') + 1);
                item.Text = item.Value;
            }

            ViewBag.ProjectName = projectName;
            ViewBag.BaseImageFile = new SelectList(VirtualMachineRepository.GetBaseImageFiles());
            ViewBag.Hostname = AppConfiguration.GetVMHostName();
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

            var projectName = new SelectList(vmRepo.GetProjects(),
                "ProjectName", "ProjectName");
            bool projectNameExists = false;

            foreach (var item in projectName)
            {
                if (item.Value == vmForm.ProjectName)
                {
                    projectNameExists = true;
                    break;
                }
            }

            if (!projectNameExists)
            {
                // TODO: Add in new project numbers
            }

            ViewBag.ProjectName = projectName;
            ViewBag.BaseImageFile = new SelectList(VirtualMachineRepository.GetBaseImageFiles());
            ViewBag.Hostname = AppConfiguration.GetVMHostName();
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

            var projectName = new SelectList(vmRepo.GetProjects(),
                "ProjectName", "ProjectName");

            foreach (var item in projectName)
            {
                item.Value = item.Value.Substring(item.Value.LastIndexOf('G') + 1);
                item.Text = item.Value;
            }

            ViewBag.ProjectName = projectName;
            ViewBag.Hostname = AppConfiguration.GetVMHostName();

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
            ViewBag.Hostname = AppConfiguration.GetVMHostName();

            return View(vm);
        }

        //
        // POST: /VirtualMachine/ToggleStatus

        [HttpPost]
        public ActionResult ToggleStatus(string image)
        {
            VMStatus status = vmRepo.ToggleVMStatus(image);
            RegisteredVirtualMachine vm = vmRepo.GetRegisteredVirtualMachine(image);

            var results = new ToggleStatusViewModel
            {
                Status = status.ToString().ToLower(),
                LastStartTime = vm.LastStarted,
                LastShutdownTime = vm.LastStopped
            };

            return Json(results);
        }

        //
        // POST: /VirtualMachine/UndoPendingCreateOperation

        [HttpPost]
        public ActionResult UndoPendingCreateOperation(string image)
        {
            try
            {
                vmRepo.DeleteVirtualMachine(image);
            }
            catch (InvalidOperationException)
            {
                // If this fails, the VM is already removed from the database.
                // Therefore, ignore it and send success response.
            }

            return Json(image);
        }

        //
        // POST: /VirtualMachine/UndoPendingArchiveOperation

        [HttpPost]
        public ActionResult UndoPendingArchiveOperation(string image)
        {
            vmRepo.UndoScheduleArchiveVirtualMachine(image);

            //return Json(image);
            return RedirectToAction("Index");
        }

        //
        // GET: /VirtualMachine/GetNextIP

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
            vmRepo.CreatePendingArchiveVirtualMachine(new PendingArchiveVirtualMachine(vm));
            vmRepo.DeleteVirtualMachine(vm.ImagePathName);
            
            return RedirectToAction("Index");
        }

        //
        // POST: /VirtualMachine/ArchiveProject

        [HttpPost]
        public ActionResult ArchiveProject(string project)
        {
            vmRepo.ScheduleArchiveProject(project);
            //string folderName = AppConfiguration.GetWebserverVmPath() + project;
            //ArchivedVirtualMachine.ArchiveFile(folderName, folderName + ".7z");
            /*
            var results = new ClosingProjectViewModel {
                Action = "archive",
                Time = DateTime.Now
            };

            return Json(results);*/
            return RedirectToAction("Index");
        }

        //
        // POST: /VirtualMachine/DeleteProject

        [HttpPost]
        public ActionResult DeleteProject(string project)
        {
            /*var results = new ClosingProjectViewModel {
                Action = "delete",
                Time = DateTime.Now
            };

            return Json(results);*/
            return RedirectToAction("Index");
        }
    }
}
