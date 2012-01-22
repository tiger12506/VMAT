using System.Collections.Generic;

namespace VMAT.Models
{
    public interface IVirtualMachineRepository
    {
        void CreateProject(Project proj);
        Project GetProjects();
        IEnumerable<VirtualMachine> GetVirtualMachines();
        VirtualMachine GetVirtualMachine(string imagePath);
        void CreateRegisteredVirtualMachine(RegisteredVirtualMachine vm);
        RegisteredVirtualMachine GetRegisteredVirtualMachine(string imagePath);
        void CreateArchivedVirtualMachine(ArchivedVirtualMachine vm);
        ArchivedVirtualMachine GetArchivedVirtualMachine(string imagePath);
        void CreatePendingVirtualMachine(PendingVirtualMachine vm);
        PendingVirtualMachine GetPendingVirtualMachine(string imagePath);
    }
}
