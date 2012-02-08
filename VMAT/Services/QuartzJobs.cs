using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Core;
using Quartz.Impl;
namespace VMAT.Services
{
    public class QuartzJobs
    {
        private static Models.DataEntities dataDB = new Models.DataEntities();
        private static ISchedulerFactory schedFact;
        private static IScheduler sched;
        public static void RegisterJobs()
        {
            // construct a scheduler factory
            schedFact = new StdSchedulerFactory();

            // get a scheduler
            sched = schedFact.GetScheduler();
            sched.Start();


            // Create VMs
            JobDetail createJD = new JobDetail("CreateVMs", null, typeof(CreateVMsJob));

            var createStartTime = dataDB.HostConfiguration.Single().CreateVMTime.ToUniversalTime().TimeOfDay;//Quartz uses UTC time for Trigger
            Trigger createTrigger = TriggerUtils.MakeHourlyTrigger(24);
            createTrigger.StartTimeUtc = DateTime.Now.Date.Add(createStartTime); //only want to use time part from DB
            createTrigger.Name = "CreateVMsTrigger";

            sched.ScheduleJob(createJD, createTrigger);



            // Archive VMs
            JobDetail archiveJD = new JobDetail("ArchiveVMs", null, typeof(ArchiveVMsJob));

            var archiveStartTime = dataDB.HostConfiguration.Single().ArchiveVMTime.ToUniversalTime().TimeOfDay;//Quartz uses UTC time for Trigger
            Trigger archiveTrigger = TriggerUtils.MakeHourlyTrigger(24);
            archiveTrigger.StartTimeUtc = DateTime.Now.Date.Add(archiveStartTime); //only want to use time part from DB
            archiveTrigger.Name = "ArchiveVMsTrigger";

            sched.ScheduleJob(archiveJD, archiveTrigger);

        }

        public static void ArchivePendingVMs()
        {
            var ls = dataDB.VirtualMachines.OfType<Models.PendingArchiveVirtualMachine>();
            foreach (var pendingVM in ls)
            {
                var service = new ArchiveVirtualMachineService();
                try
                {
                    //TODO insert archiving code
                }
                catch (Exception) { }

                //important: if that excepts, this should probably still continue?
                try
                {
                    dataDB.VirtualMachines.Remove(pendingVM);
                    //dataDB.VirtualMachines.Add(regVM);
                }
                finally
                {
                    dataDB.SaveChanges();
                }

            }
        }

        public static void CreatePendingVMs()
        {
            var ls = dataDB.VirtualMachines.OfType<Models.PendingVirtualMachine>();
            foreach (Models.PendingVirtualMachine pendingVM in ls)
            {
                var service = new CreateVirtualMachineService(pendingVM);
                Models.RegisteredVirtualMachine regVM = null;
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

    public class CreateVMsJob : IJob
    {
        #region IJob Members
        public void Execute(JobExecutionContext context)
        {
            JobDataMap data = context.MergedJobDataMap;

            string msg = data.GetString("MessageToLog") ?? string.Empty;

            QuartzJobs.CreatePendingVMs();
        }

        #endregion

    }

    public class ArchiveVMsJob : IJob
    {
        #region IJob Members
        public void Execute(JobExecutionContext context)
        {
            JobDataMap data = context.MergedJobDataMap;

            string msg = data.GetString("MessageToLog") ?? string.Empty;

            QuartzJobs.ArchivePendingVMs();
        }

        #endregion

    }
}