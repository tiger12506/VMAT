using System.Collections.Generic;

namespace VMAT.Models
{
    public interface IVirtualMachineRepository
    {
        public void CreateProject(Project proj);
        public IEnumerable<Project> GetProjects();
        public IEnumerable<VirtualMachine> GetVirtualMachines();
        public VirtualMachine GetVirtualMachine(string imagePath);
        public void CreateRegisteredVirtualMachine(RegisteredVirtualMachine vm);
        public RegisteredVirtualMachine GetRegisteredVirtualMachine(string imagePath);
        public void CreateArchivedVirtualMachine(ArchivedVirtualMachine vm);
        public ArchivedVirtualMachine GetArchivedVirtualMachine(string imagePath);
        public void CreatePendingVirtualMachine(PendingVirtualMachine vm);
        public PendingVirtualMachine GetPendingVirtualMachine(string imagePath);
        public int GetNextAvailbaleIP();
        public void ToggleVMStatus(string imagePath);
    }
}
