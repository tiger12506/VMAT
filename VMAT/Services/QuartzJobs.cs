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

        public void Execute(JobExecutionContext context)
        {
            JobDataMap data = context.MergedJobDataMap;
            
            string msg = data.GetString("MessageToLog") ?? string.Empty;

        }

        #endregion

        public static void CreateEm()
        {

        }
    }
}