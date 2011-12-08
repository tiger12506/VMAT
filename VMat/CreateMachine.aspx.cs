﻿using System;
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
            //if (!IsPostBack)
            //{
            //    ImageList_Load(sender, e);
            //    ProjectList_Load(sender, e);
            //}

            Update_Description(sender, e);
        }

        protected void ImageList_Load(object sender, EventArgs e)
        {
            DataSet imagelist = new DataSet();
            //TODO: Update this in the future to access from external project
            //TODO: Make this information accessible project-wide
            String conStr = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath("ImageFiles.xlsx") +
                ";Extended Properties=Excel 12.0;";
            OleDbConnection con = new OleDbConnection(conStr);
            con.Open();
            OleDbCommand cmd = new OleDbCommand("Select * From ImageFiles", con);
            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(imagelist, "iso");
            con.Close();
            //imagelist.ReadXml(Server.MapPath("ImageFiles.xml"));
            ImageList.DataSource = imagelist.Tables["iso"];
            ImageList.DataTextField = "name";
            ImageList.DataBind();
        }

        protected void ProjectList_Load(object sender, EventArgs e)
        {
            VMManager vmManager = new VMManager();
            List<ProjectInfo> projects = vmManager.GetProjectInfo();
            DropDownList ProjectList = (DropDownList) ConfigurationPanel.FindControl("ProjectList");
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
            string hostname = "gapdev" + project + machine;

            //Nathan changed, also see beTest for example
            var info = new PendingVM();
            info.ImagePathName = image;
            info.ProjectName = project;
            info.BaseImageName = hostname;
            info.HostnameWithDomain = hostname;

            VMInfo status = info.CreateVM();
        }

        protected void Update_Description(object sender, EventArgs e)
        {
            ProjectNumber.Text = ProjectList.SelectedValue;
            Hostname.Text = "gapdev" + ProjectList.SelectedValue + MachineNameSuffix.Text + "example.com";
        }

    }
}
