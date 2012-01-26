using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMAT.Models;

namespace VMAT.Tests.Services
{
    class MockCreateVirtualMachineService
    {
        public RegisteredVirtualMachine CreateVM(VMAT.Models.VMware.IVirtualHost iVirtualHost, 
            string imageLocation)
        {
            throw new NotImplementedException();
        }
    }
}
