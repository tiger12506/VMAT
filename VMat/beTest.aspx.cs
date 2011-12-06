using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackendVMWare;
namespace VMat
{
    public partial class beTest : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            mainText.Text = "<table><tr><td>ImagePathName</td><td>Status</td><td>HostnameWithDomain</td><td>IP</td><td>Created</td><td>LastRunning</td></tr>";
            var vmm = new VMManager();
            foreach (string imageName in vmm.GetRegisteredVMs())
            {
                var vmi = vmm.GetInfo(imageName);
                mainText.Text += "<tr><td><strong>" + vmi.ImagePathName + "</strong></td><td>" + vmi.Status + "</td><td>" + vmi.HostnameWithDomain + "</td><td>" + vmi.IP
                    + "</td><td>" + vmi.Created + "</td><td>" + vmi.LastStopped + "</td></tr>";
            }
            mainText.Text += "</table>";


        }


        protected void MakeServer_Click(object sender, EventArgs e)
        {
            var pvm = new PendingVM()
            {
                ImagePathName = "[ha-datacenter/standard] Server 2003 E/Server 2003 E.vmx",
                MachineName = "new",
                IP = "192.168.23.204",
                HostnameWithDomain = "hostname-e",
                BaseImageName = "[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx",
                ProjectName = "gapinfo"
            };
            pvm.CreateVM();
        }
        protected void rename_Click(object sender, EventArgs e)
        {
            var vmm = new VMManager();
            var name=vmm.GetRunningVMs().FirstOrDefault();

            var vm = new VMInfo(name);
            vm.Status = VMStatus.Running;
            vm.IP = "192.168.23.221";
            vm.HostnameWithDomain = "newhn-1";
            vm.Reboot();

            //mainT
        }
        protected void start_Click(object sender, EventArgs e)
        {
            var vmm = new VMManager();
            var vm = new VMInfo(vmm.GetRegisteredVMs().FirstOrDefault());
            vm.Status = VMStatus.Running;

            //mainT
        }
        protected void stop_Click(object sender, EventArgs e)
        {
            var vmm = new VMManager();
            var vm = new VMInfo(vmm.GetRunningVMs().FirstOrDefault());
            vm.Status = VMStatus.Stopped;

            //mainT
        }
    }
}
