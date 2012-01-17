using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VMAT.Models
{
    public class ArchivedVirtualMachine : VirtualMachine
    {
        [DisplayName("Last Shutdown")]
        public DateTime LastStopped { get; set; }

        [DisplayName("Last Started")]
        public DateTime LastStarted { get; set; }

        [DisplayName("Last Backed Up")]
        public DateTime LastBackuped { get; set; }

        [DisplayName("Last Archived")]
        public DateTime LastArchived { get; set; }

        [DisplayName("Created")]
        public DateTime Created { get; set; }

        public ArchivedVirtualMachine()
        {
            LastArchived = DateTime.Now;
            LastBackuped = DateTime.Now;
            LastStarted = DateTime.Now;
            LastStopped = DateTime.Now;
            Created = DateTime.Now;
        }

        public void ArchiveFile(string sourceName, string outName)
        {
            // 1
            // Initialize process information.
            //
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = "7za";

            // 2
            // Use 7-zip
            // specify a=archive and -tgzip=gzip
            // and then target file in quotes followed by source file in quotes
            //
            //the a stands for archive, e for extract
            p.Arguments = "a -t7z " + outName + " " + sourceName + " -mx=7";
            p.WindowStyle = ProcessWindowStyle.Hidden;

            // 3.
            // Start process and wait for it to exit
            //
            //System.Diagnostics.Process x = System.Diagnostics.Process.Start(p);
            //x.WaitForExit();


            //this way, a command window pops up momentarily
            //var bob = System.IO.Directory.GetCurrentDirectory();  
            //System.Diagnostics.Process l = new System.Diagnostics.Process();
            //l.StartInfo.FileName = "7za.exe";
            //l.StartInfo.Arguments = "a -t7z C:\\Users\\sylvaiam\\VMAT\\VMat\\" + outName + "2 " + sourceName;
            //l.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //l.EnableRaisingEvents = true;
            //l.StartInfo.UseShellExecute = false;
            //l.StartInfo.RedirectStandardOutput = true;
            //l.Start(); // This is were it throuws the exception because it can't find the file.
            //// Do stuff to verify zip archive is not corrupt
            //l.WaitForExit();
        }
    }
}
