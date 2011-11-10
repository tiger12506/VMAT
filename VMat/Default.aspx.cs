using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Xml;
using BackendVMWare;

namespace VMat
{
    public partial class ViewList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet projectData = new DataSet();
            DataTable projectTable = new DataTable();
            DataColumn machineImagePath = new DataColumn("ImagePath");
            DataColumn machineStatus = new DataColumn("Status");
            DataColumn machineHostname = new DataColumn("Hostname");
            DataColumn machineIPAddress = new DataColumn("IP");
            DataColumn machineCreated = new DataColumn("Created");
            DataColumn machineStopped = new DataColumn("Stopped");

            projectTable.Columns.Add(machineImagePath);
            projectTable.Columns.Add(machineStatus);
            projectTable.Columns.Add(machineHostname);
            projectTable.Columns.Add(machineIPAddress);
            projectTable.Columns.Add(machineCreated);
            projectTable.Columns.Add(machineStopped);

            VMManager vm_manager = new VMManager();

            foreach (string imageName in vm_manager.getRegisteredVMs())
            {
                DataRow machineData = projectTable.NewRow();
                var vmi = vm_manager.getInfo(imageName);

                machineData[machineImagePath.ColumnName] = vmi.ImagePathName;
                machineData[machineStatus.ColumnName] = vmi.Status;
                machineData[machineHostname.ColumnName] = vmi.HostnameWithDomain;
                machineData[machineIPAddress.ColumnName] = vmi.IP;
                machineData[machineCreated.ColumnName] = vmi.Created;
                machineData[machineStopped.ColumnName] = vmi.LastStopped;

                projectTable.Rows.Add(machineData);
            }

            //projectData.ReadXml(Server.MapPath("Projects.xml")); //TODO: Update this in the future to access from external project
            //ProjectDisplay.DataSource = projectData.Tables["project"];
            ProjectDisplay.DataSource = projectTable;
            ProjectDisplay.DataBind();
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

        protected void CreateMachine(object sender, EventArgs e)
        {
            string project = ProjectList.SelectedValue;
            string image = ImageList.SelectedValue;
            string machine= MachineNameSuffix.Text;
        }
    }
}