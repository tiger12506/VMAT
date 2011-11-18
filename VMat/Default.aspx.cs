﻿using System;
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
        protected List<ProjectInfo> _projects;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            VMManager vmManager = new VMManager();

            _projects = vmManager.GetProjectInfo();

            ProjectDisplay.DataSource = _projects;
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
    }
}