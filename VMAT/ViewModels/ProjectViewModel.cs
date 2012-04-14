using System.Collections.Generic;
using VMAT.Models;

namespace VMAT.ViewModels
{
	public class ProjectViewModel
	{
		public int ProjectId { get; set; }
		public string ProjectName { get; set; }
		public List<VirtualMachineViewModel> RegisteredVMs { get; set; }
		public List<VirtualMachineViewModel> PendingVMs { get; set; }
		public List<VirtualMachineViewModel> PendingArchiveVMs { get; set; }
		public List<VirtualMachineViewModel> ArchivedVMs { get; set; }

		public ProjectViewModel()
		{
			RegisteredVMs = new List<VirtualMachineViewModel>();
			PendingVMs = new List<VirtualMachineViewModel>();
			PendingArchiveVMs = new List<VirtualMachineViewModel>();
			ArchivedVMs = new List<VirtualMachineViewModel>();
		}

		public ProjectViewModel(Project project) : this()
		{
			ProjectId = project.ProjectId;
			ProjectName = project.ProjectName;

			foreach (var vm in project.VirtualMachines)
			{
				var vmView = new VirtualMachineViewModel(vm);

				if (vm.Status == VirtualMachine.PENDING)
					PendingVMs.Add(vmView);
				else if (vm.Status == VirtualMachine.ARCHIVED)
					ArchivedVMs.Add(vmView);
				else if (vm.IsPendingArchive)
					PendingArchiveVMs.Add(vmView);
				else
					RegisteredVMs.Add(vmView);
			}
		}
	}
}
