using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMAT.Models
{
    public class VirtualMachineRepository : IVirtualMachineRepository
    {
        DataEntities dataDB = new DataEntities();
    }
}