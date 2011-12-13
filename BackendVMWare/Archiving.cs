using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BackendVMWare
{
    public class Archiving
    {
        public Archiving()
        {

        }

        public void ArchiveFile(string sourceName, string outName)
        {
            sourceName = "*.*";
	        outName = "Example.7z";

            //// 1
            //// Initialize process information.
            ////
            //ProcessStartInfo p = new ProcessStartInfo();
            //p.FileName = "7za";

            //// 2
            //// Use 7-zip
            //// specify a=archive and -tgzip=gzip
            //// and then target file in quotes followed by source file in quotes
            ////
            //p.Arguments = "a -t7z " + outName + " " + sourceName;
            //p.WindowStyle = ProcessWindowStyle.Hidden;
            
            //// 3.
            //// Start process and wait for it to exit
            ////
            //System.Diagnostics.Process x = System.Diagnostics.Process.Start(p);
            //x.WaitForExit();
            var bob = System.IO.Directory.GetCurrentDirectory();  
            System.Diagnostics.Process l = new System.Diagnostics.Process();
            l.StartInfo.FileName = "7za.exe";
            l.StartInfo.Arguments = "a -t7z C:\\Users\\sylvaiam\\VMAT\\VMat" + outName + " " + sourceName;
            l.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            l.EnableRaisingEvents = true;
            l.StartInfo.UseShellExecute = false;
            l.StartInfo.RedirectStandardOutput = true;
            l.Start(); // This is were it throuws the exception because it can't find the file.
            // Do stuff to verify zip archive is not corrupt
            l.WaitForExit();
        }
    }
}
