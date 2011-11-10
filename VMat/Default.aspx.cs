using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Xml;

namespace VMat
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet projectData = new DataSet();
            projectData.ReadXml(Server.MapPath("Projects.xml")); //TODO: Update this in the future to access from external project
            ProjectDisplay.DataSource = projectData.Tables["project"];
            ProjectDisplay.DataBind();
        }
    }
}