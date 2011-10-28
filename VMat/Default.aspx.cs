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
    public partial class ViewList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet projectData = new DataSet();
            projectData.ReadXml(Server.MapPath("Projects.xml"));
            ProjectDisplay.DataSource = projectData.Tables["project"];
            ProjectDisplay.DataBind();
        }

        protected void ImageList_Load(object sender, EventArgs e)
        {
            DataSet imagelist = new DataSet();
            imagelist.ReadXml(Server.MapPath("ImageFiles.xml"));
            ImageList.DataSource = imagelist.Tables["iso"];
            ImageList.DataTextField = "name";
            ImageList.DataBind();
        }

        protected void ProjectList_Load(object sender, EventArgs e)
        {
            DataSet projectList = new DataSet();
            projectList.ReadXml(Server.MapPath("Projects.xml"));
            ProjectList.DataSource = projectList.Tables["project"];
            ProjectList.DataTextField = "projectname";
            ProjectList.DataBind();
        }

        protected void CreateMachine(object sender, EventArgs e)
        {
            string project = ProjectList.SelectedValue;
            string image = ImageList.SelectedValue;
            string machine= MachineNameSuffix.Text;
        }
    }
}