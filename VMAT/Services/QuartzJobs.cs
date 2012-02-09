using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Core;
using Quartz.Impl;

using Elmah;
namespace VMAT.Services
{
    public class SchedulerInfo : Exception
    {
        public SchedulerInfo()
        {
        }
        public SchedulerInfo(string message)
            : base(message)
        {
        }
        public SchedulerInfo(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        /// <summary>
        /// Logs this exception in Elmah
        /// </summary>
        public void LogElmah()
        {
            //can't use this cuz no http context: Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            ErrorLog.GetDefault(null).Log(new Error(this));
        }
    }
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

            new SchedulerInfo("Jobs scheduled (Application_Start fired)").LogElmah();
        }

        public static void ArchivePendingVMs()
        {
            var ls = dataDB.VirtualMachines.OfType<Models.PendingArchiveVirtualMachine>();
            foreach (var pendingVM in ls)
            {
                new SchedulerInfo("Beginning archive of hostname " + pendingVM.Hostname).LogElmah();
                var service = new ArchiveVirtualMachineService();
                try
                {
                    //TODO insert archiving code
                    if (Models.ArchivedVirtualMachine.ArchiveFile(pendingVM.ImagePathName, pendingVM.ImagePathName + ".7z"))
                    {
                        //It was a success
                        //IMPORTANT: if it fails, should it not continue?
                    }
                }
                catch (Exception ex)
                {
                    new SchedulerInfo("Uncaught VM archive error", ex).LogElmah();
                }

                //important: if that excepts, this should probably still continue?
                try
                {
                    dataDB.VirtualMachines.Remove(pendingVM);
                    //dataDB.VirtualMachines.Add(regVM);
                }
                catch (Exception ex)
                {
                    new SchedulerInfo("Error updating database post-VM archive, may need manual modification", ex).LogElmah();
                }
                try
                {
                    dataDB.SaveChanges();
                }

                catch (Exception ex)
                {
                    new SchedulerInfo("Error updating database post-VM archive, may need manual modification", ex).LogElmah();
                }

            }
            new SchedulerInfo("All archiving completed").LogElmah();
        }

        public static void CreatePendingVMs()
        {
            var ls = dataDB.VirtualMachines.OfType<Models.PendingVirtualMachine>();
            foreach (Models.PendingVirtualMachine pendingVM in ls)
            {
                new SchedulerInfo("Beginning creation of hostname " + pendingVM.Hostname).LogElmah();
                var service = new CreateVirtualMachineService(pendingVM);
                Models.RegisteredVirtualMachine regVM = null;
                try
                {
                    regVM = service.CreateVM();
                }
                catch (Exception ex)
                {
                    new SchedulerInfo("Uncaught VM creation error",ex).LogElmah();
                }

                //if that excepts, this should still continue
                try
                {
                    dataDB.VirtualMachines.Remove(pendingVM); //TODO: do we just want to remove this before starting to create the VM?
                    dataDB.VirtualMachines.Add(regVM);
                }
                catch (Exception ex)
                {
                    new SchedulerInfo("Error updating database post-VM creation, may need manual modification", ex).LogElmah();
                }
                try
                {
                    dataDB.SaveChanges();
                }

                catch (Exception ex)
                {
                    new SchedulerInfo("Error updating database post-VM creation, may need manual modification", ex).LogElmah();
                }

            }
            new SchedulerInfo("All creation completed").LogElmah();

        }
    }

    public class CreateVMsJob : IJob
    {
        #region IJob Members
        public void Execute(JobExecutionContext context)
        {
            //JobDataMap data = context.MergedJobDataMap;

            //string msg = data.GetString("MessageToLog") ?? string.Empty;

            QuartzJobs.CreatePendingVMs();
        }

        #endregion

    }

    public class ArchiveVMsJob : IJob
    {
        #region IJob Members
        public void Execute(JobExecutionContext context)
        {
            //JobDataMap data = context.MergedJobDataMap;

            //string msg = data.GetString("MessageToLog") ?? string.Empty;

            QuartzJobs.ArchivePendingVMs();
        }

        #endregion

    }
}