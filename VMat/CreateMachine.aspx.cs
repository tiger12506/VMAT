using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BackendVMWare;

namespace VMat
{
    public partial class CreateMachine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void ImageList_Load(object sender, EventArgs e)
        {
            DataSet imagelist = new DataSet();
            imagelist.ReadXml(Server.MapPath("ImageFiles.xml")); //TODO: Update this in the future to access from external project
            ImageList.DataSource = imagelist.Tables["iso"];
            ImageList.DataTextField = "name";
            ImageList.DataBind();
        }

        protected void ProjectList_Load(object sender, EventArgs e)
        {
            DataSet projectList = new DataSet();
            projectList.ReadXml(Server.MapPath("Projects.xml")); //TODO: Update this in the future to access from external project
            ProjectList.DataSource = projectList.Tables["project"];
            ProjectList.DataTextField = "projectname";
            ProjectList.DataBind();
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void CreateNewMachine(object sender, EventArgs e)
        {
            string project = ProjectList.SelectedValue;
            string image = ImageList.SelectedValue;
            string machine = MachineNameSuffix.Text;

            VMInfo info = new VMInfo();
            info.ImagePathName = image;
            info.ProjectName = project;
            info.BaseImageName = machine;

            VMManager manager = new VMManager();
            VMInfo status = manager.CreateVM(info);
        }
    }
}