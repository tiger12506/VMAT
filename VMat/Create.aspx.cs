// This file has been deprecated. Please refer to Default.aspx in the popup section.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VMat
{
    public partial class Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet imagelist = new DataSet();
            imagelist.ReadXml(@"/Users/Jacob/Desktop/VMAT/VMat/Projects.xml");
            ImageList.DataSource = imagelist.Tables["iso"];
            ImageList.DataTextField = "name";
            ImageList.DataBind();
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void CreateClick(object sender, EventArgs e)
        {
            //Create code here
        }

        protected void ImageList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}