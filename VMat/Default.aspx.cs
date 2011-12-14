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
            if(!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            vmManager = new VMManager();

            projects = vmManager.GetProjectInfo();

            ProjectDisplay.DataSource = projects;
            ProjectDisplay.DataBind();
        }

        protected bool IsMachineRunning(string machineName)
        {
            VMStatus status = vmManager.GetInfo(machineName).Status;

            // TODO: Expand for all possible statuses
            return (status == VMStatus.Running);
        }

        protected string GetStatusImagePath(string machineName)
        {
            if (IsMachineRunning(machineName))
                return "/Images/icon_led-green.png";
            else
                return "/Images/icon_led-red.png";
        }

        protected void ToggleMachineStatus(string machineName)
        {
            VirtualMachine vm = vmManager.OpenVM(Config.GetDatastore() + "/" + machineName) as VirtualMachine;

            if (vm.IsRunning) 
                vm.PowerOff();
            else
                vm.PowerOn();

            UpdateStatusIcon(machineName);
        }

        protected void ToggleMachineStatus(object sender, EventArgs e)
        {
            /*VirtualMachine vm = vmManager.OpenVM(Config.GetDatastore() + "/" + machineName) as VirtualMachine;

            if (vm.IsRunning)
                vm.PowerOff();
            else
                vm.PowerOn();

            UpdateStatusIcon(machineName);*/
        }

        protected void UpdateStatusIcon(string machineName)
        {

        }

        protected void UpdateStatusIcon(Button sender)
        {
            sender.Attributes["src"] = null;
        }
    }
}
