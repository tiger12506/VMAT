using System.Collections.Generic;

namespace VMAT.Models
{
	public interface IVirtualMachineRepository
	{
		void CreateProject(Project proj);
		Project GetProject(int id);
		ICollection<Project> GetAllProjects();
		ICollection<VirtualMachine> GetAllVirtualMachines();
		ICollection<VirtualMachine> GetAllPendingVirtualMachines();
		ICollection<VirtualMachine> GetAllRegisteredVirtualMachines();
		void CreateVirtualMachine(VirtualMachine vm, string projectName);
		VirtualMachine GetVirtualMachine(int id);
		void DeleteVirtualMachine(int id);
		void ScheduleArchiveVirtualMachine(int id);
		void UndoScheduleArchiveVirtualMachine(int id);
		void ScheduleArchiveProject(int id);
		string GetNextAvailableIP();
		int ToggleVMStatus(int id);
	}
}
