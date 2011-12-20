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
        private VMInfo vm;

        protected void Page_Load(object sender, EventArgs e)
        {
            string imageName;
            string ipAddress;
            string hostname;

            imageName = Server.UrlDecode(Request["imageName"]);
            if (imageName == null || imageName.Length < 6) Response.Redirect("Default.aspx");

            vm = new VMInfo(imageName);

            //can't edit if not running
            if (vm.Status != VMStatus.Running) Response.Redirect("Default.aspx"); //todo failure message

            if (!IsPostBack)
            {
                ImageFile.Text = imageName;
                ipAddress = vm.IP;
                hostname = vm.HostnameWithDomain;
                IPAddress.Text = ipAddress;
                Hostname.Text = hostname;
            }
        }

        protected void ApplyButton_Click(object sender, EventArgs e)
        {
            vm.IP = IPAddress.Text;
            vm.HostnameWithDomain = Hostname.Text;

            vm.Reboot();
            Response.Redirect("Default.aspx");

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}