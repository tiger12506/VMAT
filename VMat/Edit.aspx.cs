using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackendVMWare;

namespace VMat
{
    public partial class Edit : System.Web.UI.Page
    {
        VMManager vmm;
        VMInfo vm;
        private string imageName;
        private string ipAddress;
        private string hostname;

        protected void Page_Load(object sender, EventArgs e)
        {
            vmm = new VMManager();
            vm = new VMInfo(imageName);

//            imageName = Server.UrlDecode(Request["imageName"]);
            ipAddress = vm.IP;
            hostname = vm.HostnameWithDomain;
            IPAddress.Text = ipAddress;
            Hostname.Text = hostname;
        }

        protected void ApplyButton_Click(object sender, EventArgs e)
        {
            vm.IP = IPAddress.Text;
            vm.HostnameWithDomain = Hostname.Text;

            vm.Reboot();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}