using System.Collections.Generic;
using VMAT.Services;

namespace VMAT.Models
{
    public interface IVirtualMachineRepository
    {
        void CreateProject(Project proj);
        IEnumerable<Project> GetProjects();
        IEnumerable<VirtualMachine> GetAllVirtualMachines();
        IEnumerable<RegisteredVirtualMachine> GetAllRegisteredVirtualMachines();
        VirtualMachine GetVirtualMachine(string imagePath);
        void DeleteVirtualMachine(string imagePath);
        void CreateRegisteredVirtualMachine(RegisteredVirtualMachine vm);
        RegisteredVirtualMachine GetRegisteredVirtualMachine(string imagePath);
        void CreatePendingArchiveVirtualMachine(PendingArchiveVirtualMachine vm);
        void ScheduleArchiveVirtualMachine(string imagePath);
        void ScheduleArchiveProject(string projectName);
        void UndoScheduleArchiveVirtualMachine(string imagePath);
        PendingArchiveVirtualMachine GetPendingArchiveVirtualMachine(string imagePath);
        void CreateArchivedVirtualMachine(ArchivedVirtualMachine vm);
        ArchivedVirtualMachine GetArchivedVirtualMachine(string imagePath);
        void CreatePendingVirtualMachine(PendingVirtualMachine vm);
        PendingVirtualMachine GetPendingVirtualMachine(string imagePath);
        string GetNextAvailableIP();
        VMStatus ToggleVMStatus(string imagePath);
        void PowerOn(RegisteredVirtualMachine vm, RegisteredVirtualMachineService service);
        void PowerOff(RegisteredVirtualMachine vm, RegisteredVirtualMachineService service);
    }
}
