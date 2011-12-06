using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using BackendVMWare;

namespace VMat
{
    public partial class Default : System.Web.UI.Page
    {
        protected List<ProjectInfo> projects;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            VMManager vmManager = new VMManager();

            projects = vmManager.GetProjectInfo();

            ProjectDisplay.DataSource = projects;
            ProjectDisplay.DataBind();
        }
    }
}
