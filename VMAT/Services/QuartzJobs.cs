using System;
using System.Linq;
using Elmah;
using Quartz;
using Quartz.Impl;
using VMAT.Models;
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

			if (dataDB.HostConfiguration.Count() < 1)
			{
				new SchedulerInfo("No jobs scheduled due to lack of configuration").LogElmah();
				return;
			}
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

            // Create Snaphots
            //todo commented out because broke build
            /*JobDetail snapshotJD = new JobDetail("Snapshots", null, typeof(CreateSnapshotsJob));

            var snapshotStartTime = dataDB.HostConfiguration.Single().ArchiveVMTime.ToUniversalTime().TimeOfDay;//Quartz uses UTC time for Trigger
            Trigger snapshotTrigger = TriggerUtils.MakeHourlyTrigger(24);
            archiveTrigger.StartTimeUtc = DateTime.Now.Date.Add(snapshotStartTime);
            snapshotTrigger.Name = "SnapshotsTrigger";

			sched.ScheduleJob(archiveJD, archiveTrigger);

			new SchedulerInfo("Jobs scheduled (likely Application_Start fired). Create start time is " + createTrigger.StartTimeUtc).LogElmah();
		*/}

		public static void ArchivePendingVMs()
		{
			var ls = dataDB.VirtualMachines.Where(v => v.IsPendingArchive);
			foreach (var pendingVM in ls)
			{
				new SchedulerInfo("Beginning archive of project " + pendingVM.Hostname).LogElmah();
				var service = new ArchiveVirtualMachineService();
				try
				{
					//TODO insert archiving code
					string vmFilename = RegisteredVirtualMachineService.ConvertPathToPhysical(pendingVM.ImagePathName);
					vmFilename = vmFilename.Substring(0, vmFilename.LastIndexOf('\\'));
					if (!Models.VirtualMachine.ArchiveFile(vmFilename, vmFilename + ".7z"))
					{
						//It was a failure
						//IMPORTANT: if it fails, should it not continue?
						new SchedulerInfo("VM archiving failed").LogElmah();
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
					//TODO create & add an ArchivedVM
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
			var ls = dataDB.VirtualMachines.Where(v => v.Status == VirtualMachine.PENDING);
			foreach (Models.VirtualMachine pendingVM in ls)
			{
                new SchedulerInfo("Beginning creation of machine " + pendingVM.MachineName).LogElmah(); //todo Hostname field is empty
				var service = new CreateVirtualMachineService(pendingVM);
				Models.VirtualMachine regVM = null;
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
					//done: now that every VM is the same type, should we just change the flag and save?
				    pendingVM.Status = VirtualMachine.STOPPED;
					//dataDB.VirtualMachines.Remove(pendingVM); //TODO: do we just want to remove this before starting to create the VM?
					//dataDB.VirtualMachines.Add(regVM);
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
					new SchedulerInfo("Error saving database post-VM creation, may need manual modification", ex).LogElmah();
				}
			}
			new SchedulerInfo("All creation completed").LogElmah();
		}

        public static void CreateSnapshots()
        {
            var ls = dataDB.VirtualMachines.Where(v => v.Status != VirtualMachine.PENDING && v.Status != VirtualMachine.ARCHIVED);
            foreach (Models.VirtualMachine vm in ls)
            {
                new SchedulerInfo("Beginning creation of snapshot for " + vm.Hostname).LogElmah();
                try
                {
                    var vmr = new Models.VirtualMachineRepository();
                    vmr.CreateSnapshot(vm, DateTime.Today.DayOfWeek.ToString(), "Snapshot taken on " + DateTime.Now);
                }
                catch (Exception ex)
                {
                    new SchedulerInfo("Uncaught snapshot creation error", ex).LogElmah();
                }
            }
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

    public class CreateSnapshotsJob : IJob
    {
        #region IJob Members
        public void Execute(JobExecutionContext context)
        {
            QuartzJobs.CreateSnapshots();
        }
        #endregion
    }
}