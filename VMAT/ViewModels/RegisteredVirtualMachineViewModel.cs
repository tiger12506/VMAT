using VMAT.Models;
using System.Web.UI.WebControls;

namespace VMAT.ViewModels
{
	public class RegisteredVirtualMachineViewModel : VirtualMachineViewModel
	{
		public string ImagePathName { get; set; }
		public string BaseImageName { get; set; }
		public string OperatingSystemIcon { get; set; }
		public string Status { get; set; }
		public string MachineName { get; set; }
		public string IP { get; set; }
		public string CreatedTime { get; set; }
		public string LastStarted { get; set; }
		public string LastStopped { get; set; }
		public string LastBackuped { get; set; }
		public string LastArchived { get; set; }

		public RegisteredVirtualMachineViewModel() { }

		public RegisteredVirtualMachineViewModel(RegisteredVirtualMachine vm)
		{
			VirtualMachineId = vm.VirtualMachineId;
			ImagePathName = vm.ImagePathName;
			BaseImageName = vm.BaseImageName;
			Status = vm.Status.ToString().ToLower();
			MachineName = vm.MachineName;
			IP = vm.IP;
			CreatedTime = vm.CreatedTime.ToString();
			LastStopped = vm.LastStopped.ToString();
			LastStarted = vm.LastStarted.ToString();
			LastArchived = vm.LastArchived.ToString();
			LastBackuped = vm.LastBackuped.ToString();

			if (vm.BaseImageName == "Windows 7")
				OperatingSystemIcon = "~/Content/themes/images/logo_windows-7.png";
			else if (vm.BaseImageName == "Windows Server 2008")
				OperatingSystemIcon = "~/Content/themes/images/logo_windows-server-2008.png";
			else
				OperatingSystemIcon = "~/Content/themes/images/logo_windows-7.png";
		}
	}
}
