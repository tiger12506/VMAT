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
                var vmi=vmm.GetInfo(imageName);
                mainText.Text += "<tr><td><strong>" + vmi.ImagePathName + "</strong></td><td>" + vmi.Status + "</td><td>" + vmi.HostnameWithDomain + "</td><td>" + vmi.IP 
                    + "</td><td>" + vmi.Created + "</td><td>" + vmi.LastStopped + "</td></tr>";
            }
            mainText.Text += "</table>";

            
        }

        protected void MakeServer_Click(object sender, EventArgs e)
        {
            var vmm = new VMManager();
            vmm.CreateVM(new VMInfo()
            {
                ImagePathName = "[ha-datacenter/standard] Server 2003 C/Server 2003 C.vmx",
                BaseImageName = "[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx"
            });
        }

        protected void rename_Click(object sender, EventArgs e)
        {
            var vmm = new VMManager();
            var succ1 = vmm.ChangeHostnameAndIp(vmm.GetRunningVMs().FirstOrDefault(), "hostname4", "192.168.23.184");
            mainText.Text += "<h1>" + succ1 + "</h1>";
        }
    }
}
