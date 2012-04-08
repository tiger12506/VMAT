using System.Collections.Generic;
using System.Data.Objects;
using VMAT.Models;
using System;

namespace VMAT.ViewModels
{
	public class ProjectViewModel
	{
		public int ProjectId { get; set; }
		public string ProjectName { get; set; }
		public IEnumerable<RegisteredVirtualMachineViewModel> RegisteredVMs { get; set; }
		public IEnumerable<PendingVirtualMachineViewModel> PendingVMs { get; set; }
		public IEnumerable<PendingArchiveVirtualMachineViewModel> PendingArchiveVMs { get; set; }
		public IEnumerable<ArchiveVirtualMachineViewModel> ArchivedVMs { get; set; }

		public ProjectViewModel()
		{
			RegisteredVMs = new List<RegisteredVirtualMachineViewModel>();
			PendingVMs = new List<PendingVirtualMachineViewModel>();
			PendingArchiveVMs = new List<PendingArchiveVirtualMachineViewModel>();
			ArchivedVMs = new List<ArchiveVirtualMachineViewModel>();
		}

		public ProjectViewModel(Project project) : this()
		{
			ProjectId = project.ProjectId;
			ProjectName = project.ProjectName;

			foreach (var vm in project.VirtualMachines)
			{
				Type type = ObjectContext.GetObjectType(vm.GetType());

				if (type == typeof(RegisteredVirtualMachine))
				{
					var vmView = new RegisteredVirtualMachineViewModel(
						vm as RegisteredVirtualMachine);

					AddRegisteredVirtualMachineViewModel(vmView);
				}
				else if (type == typeof(PendingVirtualMachine))
				{
					var vmView = new PendingVirtualMachineViewModel(
						vm as PendingVirtualMachine);

					AddPendingVirtualMachineViewModel(vmView);
				}
				else if (type == typeof(PendingArchiveVirtualMachine))
				{
					var vmView = new PendingArchiveVirtualMachineViewModel(
						vm as PendingArchiveVirtualMachine);

					AddPendingArchiveVirtualMachineViewModel(vmView);
				}
				else if (type == typeof(ArchivedVirtualMachine))
				{
					var vmView = new ArchiveVirtualMachineViewModel(
						vm as ArchivedVirtualMachine);

					AddArchivedVirtualMachineViewModel(vmView);
				}
			}
		}

		public void AddRegisteredVirtualMachineViewModel(RegisteredVirtualMachineViewModel vm)
		{
			(RegisteredVMs as List<RegisteredVirtualMachineViewModel>).Add(vm);
		}

		public void AddPendingVirtualMachineViewModel(PendingVirtualMachineViewModel vm)
		{
			(PendingVMs as List<PendingVirtualMachineViewModel>).Add(vm);
		}

		public void AddPendingArchiveVirtualMachineViewModel(PendingArchiveVirtualMachineViewModel vm)
		{
			(PendingArchiveVMs as List<PendingArchiveVirtualMachineViewModel>).Add(vm);
		}

		public void AddArchivedVirtualMachineViewModel(ArchiveVirtualMachineViewModel vm)
		{
			(ArchivedVMs as List<ArchiveVirtualMachineViewModel>).Add(vm);
		}
	}
}
