using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMAT.ViewModels;
using System.Diagnostics;
using System.Collections.Generic;

namespace VMAT.Models
{
	public class VirtualMachine
	{
		public const int STOPPED = 0;
		public const int PAUSED = 1; // Still in memory, like sleep
		public const int SUSPENDED = 2; // Still in disk, like hibernate. May not be supported
		public const int RUNNING = 3;
		public const int POWERINGON = 4;
		public const int POWERINGOFF = 5;
		public const int PENDING = 6;
		public const int ARCHIVED = 7;

		[ScaffoldColumn(false)]
		public int VirtualMachineId { get; set; }

		[DisplayName("Machine Name")]
		public string MachineName { get; set; }

		[DisplayName("Image Filepath")]
		public string ImagePathName { get; set; }

		[DisplayName("Base Image File")]
		public string BaseImageName { get; set; }

		[DisplayName("Operating System")]
		public string OS { get; set; }

		[DisplayName("Hostname")]
		public string Hostname { get; set; }

		[DisplayName("Status")]
		public int Status { get; set; }

		[DisplayName("IP Address")]
		public string IP { get; set; }

		[DefaultValue(false)]
		[DisplayName("Startup")]
		public bool IsAutoStarted { get; set; }

		[DefaultValue(false)]
		[DisplayName("Pending Archive?")]
		public bool IsPendingArchive { get; set; }

		[DisplayName("Last Shutdown")]
		public DateTime LastStopped { get; set; }

		[DisplayName("Last Started")]
		public DateTime LastStarted { get; set; }

		[DisplayName("Last Backed Up")]
		public DateTime LastBackuped { get; set; }

		[DisplayName("Last Archived")]
		public DateTime LastArchived { get; set; }

		[DisplayName("Created")]
		public DateTime CreatedTime { get; set; }

		public virtual Project Project { get; set; }


		public VirtualMachine() 
		{
			LastArchived = DateTime.Now;
			LastBackuped = DateTime.Now;
			LastStarted = DateTime.Now;
			LastStopped = DateTime.Now;
			CreatedTime = DateTime.Now;
		}

		public VirtualMachine(VirtualMachineFormViewModel vmForm, DateTime creationTime)
		{
			MachineName = "gapdev" + vmForm.ProjectName.Trim('G') + vmForm.MachineName;
			ImagePathName = AppConfiguration.GetDatastore() + vmForm.ProjectName + "/" + 
				MachineName + "/" + MachineName + ".vmx";
			BaseImageName = vmForm.BaseImageFile;
		    Hostname = MachineName; //nathan added
			IP = vmForm.IP;
			IsAutoStarted = vmForm.IsAutoStarted;
			Status = PENDING;
			IsPendingArchive = false;
			LastArchived = creationTime;
			LastBackuped = creationTime;
			LastStarted = creationTime;
			LastStopped = creationTime;
			CreatedTime = creationTime;
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
