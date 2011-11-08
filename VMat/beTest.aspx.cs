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
            foreach (string imageName in vmm.getRegisteredVMs())
            {
                var vmi=vmm.getInfo(imageName);
                mainText.Text += "<tr><td><strong>" + vmi.ImagePathName + "</strong></td><td>" + vmi.Status + "</td><td>" + vmi.HostnameWithDomain + "</td><td>" + vmi.IP 
                    + "</td><td>" + vmi.Created + "</td><td>" + vmi.LastRunning + "</td></tr>";
            }
            mainText.Text += "</table>";

            vmm.createVM(new VMInfo() { ImagePathName = "[ha-datacenter/standard] Server 2003 B/Server 2003 B.vmx", 
                BaseImageName = "[ha-datacenter/standard] Windows Server 2003/Windows Server 2003.vmx" });
        }
    }
}