using System;
using VMAT.Models;

namespace VMAT.ViewModels
{
	public class ToggleStatusViewModel
	{
		public string Status { get; set; }
		public DateTime LastStartTime { get; set; }
		public DateTime LastShutdownTime { get; set; }

		public ToggleStatusViewModel() : this(VirtualMachine.RUNNING, DateTime.Now, DateTime.Now) { }

		public ToggleStatusViewModel(int st, DateTime lastStart, DateTime lastShutdown)
		{
			switch (st)
			{
				case VirtualMachine.STOPPED:
					Status = "stopped";
					break;
				case VirtualMachine.PAUSED:
					Status = "paused";
					break;
				case VirtualMachine.SUSPENDED:
					Status = "suspended";
					break;
				case VirtualMachine.RUNNING:
					Status = "running";
					break;
				case VirtualMachine.POWERINGON:
					Status = "powering-on";
					break;
				case VirtualMachine.POWERINGOFF:
					Status = "powering-off";
					break;
				case VirtualMachine.PENDING:
					Status = "pending";
					break;
				case VirtualMachine.ARCHIVED:
					Status = "archived";
					break;
			}

			LastStartTime = lastStart;
			LastShutdownTime = lastShutdown;
		}
	}
}
