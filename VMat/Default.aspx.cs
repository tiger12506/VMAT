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

//            ProjectDisplay.DataSource = projects;
//            ProjectDisplay.DataBind();

            foreach (ProjectInfo project in projects)
            {
                Panel projectPanel = new Panel();

                // ******* Project Header *******
                Panel projectHeader = new Panel();

                Label projectName = new Label();
                projectName.Text = project.ProjectName;

                Label projectDomain = new Label();
                projectDomain.Text = project.HostName;

                Button closeProjectButton = new Button();

                Image closeProjectImage = new Image();
                closeProjectImage.ImageUrl = "/Images/icon_project-complete.png";

                Label closeProjectLabel = new Label();
                closeProjectLabel.Text = "Completed";

                closeProjectButton.Controls.Add(closeProjectImage);
                closeProjectButton.Controls.Add(closeProjectLabel);

                projectHeader.Controls.Add(projectName);
                projectHeader.Controls.Add(projectDomain);
                projectHeader.Controls.Add(closeProjectButton);

                projectPanel.Controls.Add(projectHeader);

                foreach (VMInfo vm in project.VirtualMachines)
                {
                    Panel machinePanel = new Panel();

                    ImageButton statusButton = new ImageButton();
                    statusButton.Width = 30;
                    statusButton.Height = 30;
                    statusButton.Attributes.Add("VM", vm.ImagePathName);
                    statusButton.ImageUrl = GetStatusImagePath(vm.Status);
                    statusButton.Click += new ImageClickEventHandler(ToggleDetailsPanel);
                    
                    Panel vmNamePanel = new Panel();
                    Label vmNameLabel = new Label();
                    vmNameLabel.Text = "Name";
                    Label vmName = new Label();
                    vmName.Text = vm.ImagePathName; // Fix me
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

                    Button showMoreButton = new Button();
                    showMoreButton.Text = "Show More";

                    // ******* Details Panel *******
                    Panel detailsPanel = new Panel();
                    detailsPanel.Visible = false;


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
