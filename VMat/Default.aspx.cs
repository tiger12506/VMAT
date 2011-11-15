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
    public partial class Default : System.Web.UI.Page
    {
        private DataSet _projectData;

        protected void Page_Load(object sender, EventArgs e)
        {
            _projectData = new DataSet();

            DataTable projectTable = new DataTable();
            DataTable machineTable = new DataTable();

            _projectData.Tables.Add(projectTable);
            _projectData.Tables.Add(machineTable);

            DataColumn projectName = new DataColumn("ProjectName");
            DataColumn machineName = new DataColumn("MachineName");
            DataColumn machineImagePath = new DataColumn("ImagePath");
            DataColumn machineStatus = new DataColumn("Status");
            DataColumn machineHostname = new DataColumn("Hostname");
            DataColumn machineIPAddress = new DataColumn("IP");
            DataColumn machineCreated = new DataColumn("Created");
            DataColumn machineStopped = new DataColumn("Stopped");

            projectTable.Columns.Add("ProjectName", typeof(string));

            //Add a default project name to the 'Projects' table
            DataRow projectRow = projectTable.NewRow();
            projectRow[projectName.ColumnName] = "gapdev";
            projectTable.Rows.Add(projectRow);

            machineTable.Columns.Add(projectName);
            machineTable.Columns.Add(machineName);
            machineTable.Columns.Add(machineImagePath);
            machineTable.Columns.Add(machineStatus);
            machineTable.Columns.Add(machineHostname);
            machineTable.Columns.Add(machineIPAddress);
            machineTable.Columns.Add(machineCreated);
            machineTable.Columns.Add(machineStopped);

            //The virtual machines are associated to the projects in the 'Project' table by their project name
            machineTable.PrimaryKey = new DataColumn[] { projectTable.Columns["MachineName"] };
            _projectData.Relations.Add("project_machine", projectTable.Columns["ProjectName"], machineTable.Columns["ProjectName"]);

            VMManager vm_manager = new VMManager();

            //Add each machine to the 'Machine' table by pulling from the VMware server
            foreach (string imageName in vm_manager.GetRegisteredVMs())
            {
                DataRow machineData = machineTable.NewRow();
                var vmi = vm_manager.GetInfo(imageName);
                string name = vmi.ImagePathName.Substring((vmi.ImagePathName.LastIndexOf('/')));

                machineData[projectName.ColumnName] = "gapdev"; //TODO: Placeholder
                machineData[machineName.ColumnName] = name;
                machineData[machineImagePath.ColumnName] = vmi.ImagePathName;
                machineData[machineStatus.ColumnName] = vmi.Status;
                machineData[machineHostname.ColumnName] = vmi.HostnameWithDomain;
                machineData[machineIPAddress.ColumnName] = vmi.IP;
                machineData[machineCreated.ColumnName] = vmi.Created;
                machineData[machineStopped.ColumnName] = vmi.LastStopped;

                machineTable.Rows.Add(machineData);
            }

            

            //projectData.ReadXml(Server.MapPath("Projects.xml")); //TODO: Update this in the future to access from external project
            //ProjectDisplay.DataSource = projectData.Tables["project"];
            ProjectDisplay.DataSource = projectTable;
            ProjectDisplay.DataBind();

            /*// Create connection string variable. Modify the "Data Source"
            // parameter as appropriate for your environment.
            String path = @"/VirtualList.xlsx";
            String sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + path + ";" +
                "Extended Properties=Excel 12.0;";

            // Create connection object by using the preceding connection string.
            OleDbConnection objConn = new OleDbConnection(sConnectionString);

            // Open connection with the database.
            objConn.Open();

            // The code to follow uses a SQL SELECT command to display the data from the worksheet.

            // Create new OleDbCommand to return data from worksheet.
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM TestName", objConn);


            // Create new OleDbDataAdapter that is used to build a DataSet
            // based on the preceding SQL SELECT statement.
            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();

            // Pass the Select command to the adapter.
            objAdapter1.SelectCommand = objCmdSelect;

            // Create new DataSet to hold information from the worksheet.
            DataSet objDataset1 = new DataSet();

            // Fill the DataSet with the information from the worksheet.
            objAdapter1.Fill(objDataset1, "XLData");

            // Bind data to DataGrid control.
            //GridView1.DataSource = objDataset1.Tables[0].DefaultView;
            //GridView1.DataBind();


            objCmdSelect.CommandText = "Update TestName set FirstName = 'Jacob' where FirstName = 'Scott'";
            objCmdSelect.ExecuteNonQuery();

            // Clean up objects.
            objConn.Close();*/
        }

        public DataSet GetProjectData()
        {
            return _projectData;
        }
    }
}