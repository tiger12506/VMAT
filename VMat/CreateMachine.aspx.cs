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
            if (!IsPostBack)
            {
                ImageList_Load();
                ProjectList_Load();
            }
        }

        private void ImageList_Load()
        {
            DataSet imagelist = new DataSet();
            //TODO: Update this in the future to access from external project
            imagelist.ReadXml(Server.MapPath("ImageFiles.xml"));
            ImageList.DataSource = imagelist.Tables["iso"];
            ImageList.DataTextField = "name";
            ImageList.DataBind();
        }

        private void ProjectList_Load()
        {
            VMManager vmManager = new VMManager();
            List<ProjectInfo> projects = vmManager.GetProjectInfo();
            ProjectList.DataSource = projects;
            ProjectList.DataTextField = "ProjectName";
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
