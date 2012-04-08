using System;
using System.ComponentModel;
using System.Diagnostics;

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

		public ArchivedVirtualMachine(RegisteredVirtualMachine vm)
		{
			VirtualMachineId = vm.VirtualMachineId;
			BaseImageName = vm.BaseImageName;
			LastArchived = vm.LastArchived;
			LastBackuped = vm.LastBackuped;
			LastStarted = vm.LastStarted;
			LastStopped = vm.LastStopped;
			Created = vm.CreatedTime;
		}

		public static bool ArchiveFile(string sourceName, string outName)
		{
			//SevenZip.SevenZipCompressor s = new SevenZip.SevenZipCompressor();
			//SevenZip.SevenZipCompressor.SetLibraryPath(@"C:\Program Files\7-Zip\7z.dll");
			//s.ArchiveFormat = SevenZip.OutArchiveFormat.SevenZip;
			//s.CompressionLevel = SevenZip.CompressionLevel.Fast;
			////s.CompressDirectory("C:\\Users\\sylvaiam\\VMAT\\VMat", @"C:\Users\sylvaiam\VMAT\VMat\bin\Vmat.7z");
			//if (System.IO.Directory.Exists(sourceName))
			//    s.CompressDirectory(sourceName, outName);
			//else
			//    s.CompressFiles(outName, new string[] { sourceName });

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
			System.Diagnostics.Process x = System.Diagnostics.Process.Start(p);
			x.WaitForExit();

			return x.ExitCode == 0;
			
			////this way, a command window pops up momentarily
			//var bob = System.IO.Directory.GetCurrentDirectory();  
			//System.Diagnostics.Process l = new System.Diagnostics.Process();
			//l.StartInfo.FileName = "7za.exe";
			//l.StartInfo.Arguments = "a -t7z " + AppConfiguration.GetHostVmPath() + outName + "2 " + sourceName;
			//l.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			//l.EnableRaisingEvents = true;
			//l.StartInfo.UseShellExecute = false;
			//l.StartInfo.RedirectStandardOutput = true;
			//l.Start(); // This is were it throuws the exception because it can't find the file.
			////// Do stuff to verify zip archive is not corrupt
			//l.WaitForExit();
		}
	}
}
