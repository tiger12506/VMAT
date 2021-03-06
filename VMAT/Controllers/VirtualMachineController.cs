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
			ICollection<Project> projectList = vmRepo.GetAllProjects();
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
			var projectName = new SelectList(vmRepo.GetAllProjects(),
				"ProjectName", "ProjectName");

			ViewBag.ProjectName = projectName;
			ViewBag.BaseImageFile = new SelectList(
				VMAT.Services.RegisteredVirtualMachineService.GetBaseImageFiles());
			vmForm.IP = vmRepo.GetNextAvailableIP().ToString();

			return View(vmForm);
		}

		//
		// POST: /VirtualMachine/Create

		[HttpPost]
		public ActionResult Create(VirtualMachineFormViewModel vmForm)
		{
			if (vmRepo.GetAllRegisteredVirtualMachines().Count < configRepo.GetMaxVmCount() ||
				configRepo.GetMaxVmCount() <= 0)
			{
				if (ModelState.IsValid)
				{
					var vm = new VirtualMachine(vmForm, configRepo.GetVmCreationTime());
					vmRepo.CreateVirtualMachine(vm, vmForm.ProjectName);

					return RedirectToAction("Index");
				}
			}
			//TODO: Add comments to what this does
			var projectName = new SelectList(vmRepo.GetAllProjects(),
				"ProjectName", "ProjectName");

			ViewBag.ProjectName = projectName;
			ViewBag.BaseImageFile = new SelectList(
				VMAT.Services.RegisteredVirtualMachineService.GetBaseImageFiles());
			ViewBag.Hostname = AppConfiguration.GetVMHostName();
			vmForm.IP = vmRepo.GetNextAvailableIP().ToString();

			return View(vmForm);
		}

		//
		// GET: /VirtualMachine/Edit

		public ActionResult Edit(int id)
		{
			VirtualMachine vm = vmRepo.GetVirtualMachine(id);
			var form = new VirtualMachineFormViewModel(vm);

			return View(form);
		}

		//
		// POST: /VirtualMachine/Edit

		[HttpPost]
		public ActionResult Edit(VirtualMachineFormViewModel vm)
		{
			if (ModelState.IsValid)
			{
				// save changes (removed)
				return RedirectToAction("Index");
			}

			return View(vm);
		}

		//
		// POST: /VirtualMachine/ToggleStatus

		[HttpPost]
		public ActionResult ToggleStatus(int id)
		{
			int status = vmRepo.ToggleVMStatus(id);
			VirtualMachine vm = vmRepo.GetVirtualMachine(id);

			var results = new ToggleStatusViewModel(status, vm.LastStarted, vm.LastStopped);

			return Json(results);
		}

		//
		// POST: /VirtualMachine/UndoPendingCreateOperation

		[HttpPost]
		public ActionResult UndoPendingCreateOperation(int id)
		{
			try
			{
				vmRepo.DeleteVirtualMachine(id);
			}
			catch (InvalidOperationException)
			{
				// If this fails, the VM is already removed from the database.
				// Therefore, ignore it and send success response.
			}

			return Json(id);
		}

		//
		// POST: /VirtualMachine/UndoPendingArchiveOperation

		[HttpPost]
		public ActionResult UndoPendingArchiveOperation(int id)
		{
			vmRepo.UndoScheduleArchiveVirtualMachine(id);
			var vm = vmRepo.GetVirtualMachine(id);
			var viewModel = new VirtualMachineViewModel(vm);

			return PartialView("_RegisteredVirtualMachine", viewModel);
		}

		//
		// GET: /VirtualMachine/GetNextIP

		public ActionResult GetNextIP()
		{
			string nextIP = vmRepo.GetNextAvailableIP().ToString();

			return Json(nextIP);
		}

		//
		// POST: /VirtualMachine/ArchiveMachine

		[HttpPost]
		public ActionResult ArchiveMachine(int id)
		{
			vmRepo.ScheduleArchiveVirtualMachine(id);
			var vm = vmRepo.GetVirtualMachine(id);
			
			var viewModel = new VirtualMachineViewModel(vm);
			ViewBag.ArchiveTime = configRepo.GetVmArchiveTime();

			return PartialView("_PendingArchiveVirtualMachine", viewModel);
		}

		//
		// POST: /VirtualMachine/ArchiveProject

		[HttpPost]
		public ActionResult ArchiveProject(int id)
		{
			vmRepo.ScheduleArchiveProject(id);
			var proj = vmRepo.GetProject(id);
			var viewModel = new ProjectViewModel(proj);
			ViewBag.ArchiveTime = configRepo.GetVmArchiveTime();

			return PartialView("_Project", viewModel);
		}
	}
}
