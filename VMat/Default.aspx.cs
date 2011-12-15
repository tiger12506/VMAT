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

            foreach (ProjectInfo project in projects)
            {
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

                foreach (VMInfo vm in project.VirtualMachines)
                {
                    Panel machinePanel = new Panel();

                    ImageButton statusButton = new ImageButton();
                    statusButton.Width = 30;
                    statusButton.Height = 30;
                    statusButton.Attributes.Add("VM", vm.ImagePathName);
                    statusButton.ImageUrl = GetStatusImagePath(vm.Status);
                    statusButton.Click += handleToggleDetails;
                    
                    Panel vmNamePanel = new Panel();
                    Label vmNameLabel = new Label();
                    vmNameLabel.Text = "Name";
                    Label vmName = new Label();
                    vmName.Text = VMInfo.GetMachineName(vm.ImagePathName);
//                    vmName.Text = vm.ImagePathName;
                    vmNamePanel.Controls.Add(vmNameLabel);
                    vmNamePanel.Controls.Add(vmName);

                    Panel vmIPPanel = new Panel();
                    Label vmIPLabel = new Label();
                    vmIPLabel.Text = "IP Address";
                    Label vmIP = new Label();
                    vmIP.Text = vm.IP;
                    vmIPPanel.Controls.Add(vmIPLabel);
                    vmIPPanel.Controls.Add(vmIP);

                    Panel vmCreatedPanel = new Panel();
                    Label vmCreatedLabel = new Label();
                    vmCreatedLabel.Text = "Created";
                    Label vmCreated = new Label();
                    vmCreated.Text = vm.Created.ToString();
                    vmCreatedPanel.Controls.Add(vmCreatedLabel);
                    vmCreatedPanel.Controls.Add(vmCreated);

                    Panel vmShutdownPanel = new Panel();
                    Label vmShutdownLabel = new Label();
                    vmShutdownLabel.Text = "Last Shutdown Time";
                    Label vmShutdown = new Label();
                    vmShutdown.Text = vm.LastStopped.ToString();
                    vmShutdownPanel.Controls.Add(vmShutdownLabel);
                    vmShutdownPanel.Controls.Add(vmShutdown);

                    // ******* Details Panel *******
                    Panel detailsPanel = new Panel();
                    detailsPanel.GroupingText = "Details go here!"; // Get rid of when details are added
                    detailsPanel.ID = vm.ImagePathName+"detail";
                    detailsPanel.Visible = false;
                    
                    Button showMoreButton = new Button();
                    showMoreButton.Attributes.Add("detailPanelID", detailsPanel.ID);
                    showMoreButton.Click += handleShowMore;
                    showMoreButton.Text = "Show More";

                    machinePanel.Controls.Add(statusButton);
                    machinePanel.Controls.Add(vmNamePanel);
                    machinePanel.Controls.Add(vmIPPanel);
                    machinePanel.Controls.Add(vmCreatedPanel);
                    machinePanel.Controls.Add(vmShutdownPanel);
                    machinePanel.Controls.Add(showMoreButton);
                    machinePanel.Controls.Add(detailsPanel);

                    projectPanel.Controls.Add(machinePanel);
                }

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
                    c.Visible = false;
                }

                // Do other stuff for closing out a project (call backend stuff)
            }
        }

        protected void ShowMore(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button statusButton = (Button)sender;
                string detailPanelID = statusButton.Attributes["detailPanelID"];
                Panel detailPanel = (Panel) statusButton.Parent.FindControl(detailPanelID);
                if (detailPanel.Visible)
                {
                    detailPanel.Visible = false;
                    statusButton.Text = "Show Details";
                }
                else
                {
                    detailPanel.Visible = true;
                    statusButton.Text = "Hide Details";
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
