using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using BackendVMWare;

namespace VMat
{
    public partial class Default : System.Web.UI.Page
    {
        protected List<ProjectInfo> projects;
        VMManager vmManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            vmManager = new VMManager();
            LoadData();
        }

        private void LoadData()
        {
            projects = vmManager.GetProjectInfo();

            ImageClickEventHandler handleToggleDetails = new ImageClickEventHandler(ToggleDetailsPanel);
            ImageClickEventHandler handleCloseProject = new ImageClickEventHandler(CloseProject);
            EventHandler handleShowMore = new EventHandler(ShowMore);
            EventHandler handleEditMachine = new EventHandler(editMachine);

            foreach (ProjectInfo project in projects)
            {
                Table table = new Table();
                table.CellSpacing = 20;

                ImageButton closeProjectButton = new ImageButton();
                closeProjectButton.Click += handleCloseProject;
                closeProjectButton.ImageUrl = "/Images/icon_project-complete.png";
                closeProjectButton.AlternateText = "Completed";
                closeProjectButton.Width = 30;
                closeProjectButton.Height = 30;

                Panel projectPanel = new Panel();

                projectPanel.GroupingText = project.ProjectName + "   " + project.HostName;
                projectPanel.BorderWidth = 10;
                projectPanel.BorderStyle = BorderStyle.Outset;
                //creates the table and dynamic controls
                foreach (VMInfo vm in project.VirtualMachines)
                {
                    TableRow tableDescRow = new TableRow();
                    TableRow tableValueRow = new TableRow();
                    TableRow detailDescRow = new TableRow();
                    TableRow detailValueRow = new TableRow();
                    //detailDescRow.Visible = false;
                    //detailValueRow.Visible = false;

                    ImageButton statusButton = new ImageButton();
                    statusButton.Width = 32;
                    statusButton.Height = 32;
                    statusButton.Attributes.Add("VM", vm.ImagePathName);
                    statusButton.ImageUrl = GetStatusImagePath(vm.Status);
                    statusButton.Click += handleToggleDetails;
                    TableCell statusButtonCell = new TableCell();
                    statusButtonCell.Controls.Add(statusButton);
                    tableDescRow.Cells.Add(statusButtonCell);
                    tableValueRow.Cells.Add(new TableCell());
                    
                    Label vmNameLabel = new Label();
                    vmNameLabel.Text = "Name";
                    Label vmName = new Label();
                    vmName.Text = VMInfo.GetMachineName(vm.ImagePathName);
                    TableCell nameLabelCell = new TableCell();
                    nameLabelCell.Controls.Add(vmNameLabel);
                    TableCell nameCell = new TableCell();
                    nameCell.Controls.Add(vmName);
                    tableDescRow.Cells.Add(nameLabelCell);
                    tableValueRow.Cells.Add(nameCell);

                    detailDescRow.ID = vmName.Text + "descRow";
                    detailValueRow.ID = vmName.Text + "valueRow";

                    Label vmIPLabel = new Label();
                    vmIPLabel.Text = "IP Address";
                    Label vmIP = new Label();
                    vmIP.Text = vm.IP;
                    TableCell vmIPLabelCell = new TableCell();
                    vmIPLabelCell.Controls.Add(vmIPLabel);
                    TableCell vmIPCell = new TableCell();
                    vmIPCell.Controls.Add(vmIP);
                    tableDescRow.Cells.Add(vmIPLabelCell);
                    tableValueRow.Cells.Add(vmIPCell);

                    Label vmCreatedLabel = new Label();
                    vmCreatedLabel.Text = "Created";
                    Label vmCreated = new Label();
                    vmCreated.Text = vm.Created.ToString();
                    TableCell vmCreatedLabelCell = new TableCell();
                    vmCreatedLabelCell.Controls.Add(vmCreatedLabel);
                    TableCell vmCreatedCell = new TableCell();
                    vmCreatedCell.Controls.Add(vmCreated);
                    tableDescRow.Cells.Add(vmCreatedLabelCell);
                    tableValueRow.Cells.Add(vmCreatedCell);

                    Label vmShutdownLabel = new Label();
                    vmShutdownLabel.Text = "Last Shutdown Time";
                    Label vmShutdown = new Label();
                    vmShutdown.Text = vm.LastStopped.ToString();
                    TableCell vmShutdownLabelCell = new TableCell();
                    vmShutdownLabelCell.Controls.Add(vmShutdownLabel);
                    TableCell vmShutdownCell = new TableCell();
                    vmShutdownCell.Controls.Add(vmShutdown);
                    tableDescRow.Cells.Add(vmShutdownLabelCell);
                    tableValueRow.Cells.Add(vmShutdownCell);

                    detailDescRow.Cells.Add(new TableCell());
                    detailValueRow.Cells.Add(new TableCell());

                    Label vmImageFileLabel = new Label();
                    vmImageFileLabel.Text = "Image File";
                    Label vmImageFile = new Label();
                    vmImageFile.Text = vm.ImagePathName;
                    TableCell vmImageFileLabelCell = new TableCell();
                    TableCell vmImageFileCell = new TableCell();
                    vmImageFileLabelCell.Controls.Add(vmImageFileLabel);
                    vmImageFileCell.Controls.Add(vmImageFile);
                    detailDescRow.Cells.Add(vmImageFileLabelCell);
                    detailValueRow.Cells.Add(vmImageFileCell);

                    Button showMoreButton = new Button();
                    showMoreButton.Attributes.Add("VM", vmName.Text);
                    showMoreButton.Click += handleShowMore;
                    showMoreButton.Text = "Hide Details";
                    TableCell smbCell = new TableCell();
                    smbCell.Controls.Add(showMoreButton);
                    tableDescRow.Cells.Add(smbCell);

                    Button editMachineButton = new Button();
                    editMachineButton.Text = "Edit Machine";
                    editMachineButton.Attributes["imageName"] = vm.ImagePathName;
                    editMachineButton.Click += handleEditMachine;
                    TableCell eMBCell = new TableCell();
                    eMBCell.Controls.Add(editMachineButton);
//                    tableValueRow.Cells.Add(eMBCell);

                    Label vmLSTLabel = new Label();
                    vmLSTLabel.Text = "Last Start Time";
                    Label vmLST = new Label();
                    vmLST.Text = vm.LastStarted.ToString();
                    TableCell vmLSTLabelCell = new TableCell();
                    TableCell vmLSTCell = new TableCell();
                    vmLSTLabelCell.Controls.Add(vmLSTLabel);
                    vmLSTCell.Controls.Add(vmLST);
                    detailDescRow.Cells.Add(vmLSTLabelCell);
                    detailValueRow.Cells.Add(vmLSTCell);

                    Label vmLBTLabel = new Label();
                    vmLBTLabel.Text = "Last Backup Time";
                    Label vmLBT = new Label();
                    vmLBT.Text = vm.LastBackuped.ToString();
                    TableCell vmLBTLabelCell = new TableCell();
                    TableCell vmLBTCell = new TableCell();
                    vmLBTLabelCell.Controls.Add(vmLBTLabel);
                    vmLBTCell.Controls.Add(vmLBT);
                    detailDescRow.Cells.Add(vmLBTLabelCell);
                    detailValueRow.Cells.Add(vmLBTCell);

                    Label vmLATLabel = new Label();
                    vmLATLabel.Text = "Last Archive Time";
                    Label vmLAT = new Label();
                    vmLAT.Text = vm.LastArchived.ToString();
                    TableCell vmLATLabelCell = new TableCell();
                    TableCell vmLATCell = new TableCell();
                    vmLATLabelCell.Controls.Add(vmLATLabel);
                    vmLATCell.Controls.Add(vmLAT);
                    detailDescRow.Cells.Add(vmLATLabelCell);
                    detailValueRow.Cells.Add(vmLATCell);

                    //Line break to make it look prettier
                    Label l = new Label();
                    l.Text = "<hr />";
                    TableRow row = new TableRow();
                    TableCell cell = new TableCell();
                    cell.Controls.Add(l);
                    cell.ColumnSpan = 6;
                    row.Cells.Add(cell);

                    table.Rows.Add(tableDescRow);
                    table.Rows.Add(tableValueRow);
                    table.Rows.Add(detailDescRow);
                    table.Rows.Add(detailValueRow);
                    table.Rows.Add(row);
                    
                }
                projectPanel.Controls.Add(table);

                ServerListPanel.Controls.Add(projectPanel);
            }
        }

        protected void CloseProject(object sender, EventArgs e)
        {
            if (sender is ImageButton)
            {
                ImageButton closeButton = (ImageButton)sender;
                foreach (Control c in closeButton.Parent.Controls)
                {
                    if(!c.Equals(sender))
                    c.Visible = false;
                }

                // Do other stuff for closing out a project (call backend stuff)
            }
        }

        protected void editMachine(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button editButton = (Button)sender;
                string imageName = editButton.Attributes["imageName"];
                string encoded = Server.UrlEncode(imageName);
                Response.Redirect("Edit.aspx?imageName=" + encoded);
            }
        }

        protected void ShowMore(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button showDetailsButton = (Button)sender;
                TableCell sdbCell = (TableCell) showDetailsButton.Parent;
                TableRow sdbcRow = (TableRow)sdbCell.Parent;
                Table table = (Table)sdbcRow.Parent;
                TableRow descRow = (TableRow) table.FindControl(showDetailsButton.Attributes["VM"] + "descRow");
                TableRow valueRow = (TableRow) table.FindControl(showDetailsButton.Attributes["VM"] + "valueRow");
                if (descRow.Visible)
                {
                    descRow.Visible = false;
                    valueRow.Visible = false;
                    showDetailsButton.Text = "Show Details";
                }
                else
                {
                    descRow.Visible = true;
                    valueRow.Visible = true;
                    showDetailsButton.Text = "Hide Details";
                }

            }
        }

        protected void ToggleDetailsPanel(object sender, EventArgs e)
        {
            if (sender is ImageButton)
            {
                ImageButton statusButton = (ImageButton)sender;
                string machineName = statusButton.Attributes["VM"];

                VMInfo machine = new VMInfo(machineName);
                statusButton.ImageUrl = GetStatusImagePath(machine.Status);

                ToggleMachineStatus(machine);
            }
        }

        protected string GetStatusImagePath(VMStatus status)
        {
            return ((status == VMStatus.Running) ? "/Images/icon_led-green.png" : "/Images/icon_led-red.png");
        }

        protected VMStatus ToggleMachineStatus(VMInfo vm)
        {
            if (vm.Status == VMStatus.Running) 
                vm.Status = VMStatus.Stopped;
            else
                vm.Status = VMStatus.Running;

            return vm.Status;
        }

    }
}
