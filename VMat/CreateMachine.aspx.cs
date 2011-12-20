using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BackendVMWare;
using System.Data.OleDb;

namespace VMat
{
    public partial class CreateMachine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var bob = System.IO.Directory.GetCurrentDirectory();           
            //Archiving arc = new Archiving();
            //arc.ArchiveFile("C:\\Users\\sylvaiam\\VMAT\\BackendVMWare\\Archiving.cs", "");
            if (!IsPostBack)
            {
                ImageList_Load(sender, e);
                ProjectList_Load(sender, e);
            }
            else
            {
                Update_Description(sender, e);
            }
        }

        protected void ImageList_Load(object sender, EventArgs e)
        {
            /*DataSet imagelist = new DataSet();
            //TODO: Update this in the future to access from external project
            //TODO: Make this information accessible project-wide

            String conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("ImageFiles.xls") +
                ";Extended Properties=Excel 8.0;";
//            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/MyWebSiteRoot");
//            conStr = rootWebConfig.ConnectionStrings.ConnectionStrings["ImageFiles"].ConnectionString;

            OleDbConnection con = new OleDbConnection(conStr);
            con.Open();
            OleDbCommand cmd = new OleDbCommand("Select * From ImageFiles", con);
            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(imagelist, "iso");
            con.Close();
            ImageList.DataSource = imagelist.Tables["iso"];
            ImageList.DataTextField = "name";*/

            ImageList.DataSource = VMInfo.GetBaseImageFiles();
            ImageList.DataBind();
        }

        protected void ProjectList_Load(object sender, EventArgs e)
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
            string project = ProjectList.SelectedValue; //4-digit
            string baseImagePathName = VMInfo.ConvertPathToDatasource(ImageList.SelectedValue);
            string machine = MachineNameSuffix.Text;
            string hostname = "gapdev" + project + machine;

            string imageName = Config.GetDatastore() + project + "/" + hostname +"/"+hostname+ ".vmx";

            var info = new PendingVM();
            info.ImagePathName = imageName;
            info.ProjectName = project;
            info.BaseImageName = baseImagePathName;
            info.HostnameWithDomain = hostname;
            info.IP = IPAddress.Text;

            VMInfo status = info.CreateVM();

            Response.Redirect("Default.aspx");
        }

        protected void Update_Description(object sender, EventArgs e)
        {
            ProjectNumber.Text = ProjectList.SelectedValue;
            Hostname.Text = "gapdev" + ProjectList.SelectedValue + MachineNameSuffix.Text + ".example.com";
            ImageFile.Text = VMInfo.ConvertPathToDatasource(ImageList.SelectedValue);
        }

        protected void DescriptionTable_Load(object sender, EventArgs e)
        {
            IPAddress.Text = "192.168.1."+Persistence.GetNextAvailableIP().ToString();
        }

    }
}
