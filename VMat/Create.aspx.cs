using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VMat
{
    public partial class Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CancelClick(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void CreateClick(object sender, EventArgs e)
        {
            var vmm = new BackendVMWare.VMManager();
            vmm.createServer();
        }

        
    }
}