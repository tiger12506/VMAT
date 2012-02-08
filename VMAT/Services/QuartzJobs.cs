using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;

namespace VMAT.Services
{
    public class MyJob : IJob
    {
        #region IJob Members
        private static Models.DataEntities dataDB = new Models.DataEntities();
        public void Execute(JobExecutionContext context)
        {
            JobDataMap data = context.MergedJobDataMap;
            
            string msg = data.GetString("MessageToLog") ?? string.Empty;

        }

        #endregion

        public static void CreateEm()
        {
            var ls=dataDB.VirtualMachines.OfType<Models.PendingVirtualMachine>();
            foreach (Models.PendingVirtualMachine pendingVM in ls)
            {
                var service = new CreateVirtualMachineService(pendingVM);
                Models.RegisteredVirtualMachine regVM=null;
                try
                {
                    regVM = service.CreateVM();
                }
                catch (Exception) { }

                //if that excepts, this should still continue
                try
                {
                    dataDB.VirtualMachines.Remove(pendingVM);
                    dataDB.VirtualMachines.Add(regVM);
                }
                finally
                {
                    dataDB.SaveChanges();
                }

            }
        }
    }
}