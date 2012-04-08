using System.Collections.Generic;
using VMAT.Services;

namespace VMAT.Models
{
	public interface IVirtualMachineRepository
	{
		void CreateProject(Project proj);
		Project GetProject(int id);
		Project GetProjectWithVirtualMachines(int id);
		IEnumerable<Project> GetAllProjects();
		IEnumerable<Project> GetAllProjectsWithVirtualMachines();
		IEnumerable<VirtualMachine> GetAllVirtualMachines();
		IEnumerable<VirtualMachine> GetAllPendingVirtualMachines();
		void CreateVirtualMachine(VirtualMachine vm);
		VirtualMachine GetVirtualMachine(int id);
		void DeleteVirtualMachine(int id);
		void ScheduleArchiveVirtualMachine(int id);
		void UndoScheduleArchiveVirtualMachine(int id);
		void ScheduleArchiveProject(int id);
		string GetNextAvailableIP();
		VMStatus ToggleVMStatus(int id);
		void PowerOn(VirtualMachine vm, RegisteredVirtualMachineService service);
		void PowerOff(VirtualMachine vm, RegisteredVirtualMachineService service);
	}
}
