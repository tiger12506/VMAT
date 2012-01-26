using System;
using System.Collections.Generic;

namespace VMAT.Tests.Services
{
    class MockRegisteredVirtualMachineService
    {
        private List<string> imagePaths = new List<string>();
        private string path;

        public MockRegisteredVirtualMachineService(string path)
        {
            // TODO: Complete member initialization
            this.path = path;
        }

        public static IEnumerable<string> GetRegisteredVMImagePaths()
        {
            throw new NotImplementedException();
        }

        internal string GetIP()
        {
            throw new NotImplementedException();
        }

        internal string GetHostname()
        {
            throw new NotImplementedException();
        }

        internal VMAT.Models.VMStatus GetStatus()
        {
            throw new NotImplementedException();
        }
    }
}
