using System.Collections.Generic;
using VMAT.Services;

namespace VMAT.Models
{
	public interface IVirtualMachineRepository
	{
		void CreateProject(Project proj);
		Project GetProject(int id);
		IEnumerable<Project> GetAllProjects();
		IEnumerable<VirtualMachine> GetAllVirtualMachines();
		IEnumerable<VirtualMachine> GetAllPendingVirtualMachines();
		void CreateVirtualMachine(VirtualMachine vm, string projectName);
		VirtualMachine GetVirtualMachine(int id);
		void DeleteVirtualMachine(int id);
		void ScheduleArchiveVirtualMachine(int id);
		void UndoScheduleArchiveVirtualMachine(int id);
		void ScheduleArchiveProject(int id);
		string GetNextAvailableIP();
		int ToggleVMStatus(int id);
		void PowerOn(VirtualMachine vm, RegisteredVirtualMachineService service);
		void PowerOff(VirtualMachine vm, RegisteredVirtualMachineService service);
	}
}
