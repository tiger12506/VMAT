<%@ Page Title="Virtual Machine Management - VMAT" Language="C#" MasterPageFile="~/Site.Master"
  AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VMat.ViewList" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript" src="/Scripts/popupDiv.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Virtual Machine Management
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="machine-controls">
    <a href="CreateMachine.aspx">
      <span class="button">
        <span class="icon">
          <img src="Images/icon_server-create.png" alt="" />
        </span>
        Create 
      </span>
    </a>
    <a href="ConfigHost.aspx">
      <span class="button">
        <span class="icon">
          <img src="Images/icon_server-admin.png" alt="" />
        </span>
        Admin
      </span>
    </a>
  </div>

  <div class="machine-management">
    <div id="display-projects-list" class="list">
      <asp:Repeater ID="ProjectDisplay" runat="server">
        <ItemTemplate>
          <div class="project-header">
            <h2><%# DataBinder.Eval(Container.DataItem, "projectname") %></h2>
            <h3>(<%# DataBinder.Eval(Container.DataItem, "hostname") %>)</h3>
            <span class="project-complete">
              <a href="javascript:void(0)" onclick="">
                <span class="button">
                  <span class="icon">
                    <img src="/Images/icon_project-complete.png" alt="" />
                  </span>
                  Completed
                </span>
              </a>
            </span>
          </div>
          <div class="project-machines">
            <asp:Repeater ID="MachineDisplay" datasource='<%# ((DataRowView)Container.DataItem)
              .Row.GetChildRows("project_machine") %>' runat="server">
              <ItemTemplate>
              <span class="machine-item-info">
                <span class="status-icon">
                  <a href="javascript:void(0)" onclick="toggleMachineStatus();">
                    <img src="/Images/icon_led-green.png" />
                  </a>
                </span>
                <span class="machine-name">
                  <span class="machine-item-label">Machine Name</span>
                  <span class="machine-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"machinename\"]") %>
                  </span>
                </span>
                <span class="os-icon">
                  <img src="/Images/logo_windows-server-2008.png" />
                </span>
                <span class="iso-name">
                  <span class="machine-item-label">Image File</span>
                  <span class="machine-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"isofile\"]") %>
                  </span>
                </span>
                <span class="ip-address">
                  <span class="machine-item-label">IP Address</span>
                  <span class="machine-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"ipaddress\"]") %>
                  </span>
                </span>
                <span class="creation-date">
                  <span class="machine-item-label">Date Created</span>
                  <span class="machine-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"creation-date\"]") %>
                  </span>
                </span>
                <span class="machine-details-toggle">
                  <a href="javascript:void(0)" onclick="toggleMachineDetails();">Details</a>
                </span>
              </span>
              <span class="machine-details-info">
                <span class="last-start-time">
                  <span class="details-item-label">Last Start Time</span>
                  <span class="details-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"start-time\"]") %>
                  </span>
                </span>
                <span class="last-shutdown-time">
                  <span class="details-item-label">Last Shutdown Time</span>
                  <span class="details-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"shutdown-time\"]") %>
                  </span>
                </span>
                <span class="last-backup-time">
                  <span class="details-item-label">Last Backup Time</span>
                  <span class="details-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"backup-time\"]") %>
                  </span>
                </span>
                <span class="last-archive-time">
                  <span class="details-item-label">Last Archive Time</span>
                  <span class="details-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"archive-time\"]") %>
                  </span>
                </span>
              </span>
              </ItemTemplate>
            </asp:Repeater>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>

  <!-- UNUSED: Popup Windows for virtual machine controls

  <div class="popup-window" id="create-window">
    <div class="header">
      <h2>Create Machine</h2>
      <a href="javascript:void(0)" onclick="closeWindow('create-window');" target="_self">
        <img src="/Images/icon_close-window.png" alt="close" />
      </a>
    </div>
    <div class="create-form">
      <form action="" runat="server">
        Machine Name Suffix:
        <asp:TextBox ID="MachineNameSuffix" MaxLength="5" Columns="8" runat="server" /><br />
        Image File:
        <asp:DropDownList ID="ImageList" runat="server" onload="ImageList_Load" /><br />
        Project Number:
        <asp:DropDownList ID="ProjectList" runat="server" OnLoad="ProjectList_Load" /><br />
        <div class="create-form-buttons">
          <asp:Button ID="CreateMachineSubmitButton" Text="Create" runat="server" OnClick="CreateMachine" />
        </div>
      </form>
    </div>
  </div>

  <div class="popup-window" id="config-window">
    <div class="header">
      <h2>Configure Host</h2>
      <a href="javascript:void(0)" onclick="closeWindow('config-window');" target="_self">
        <img src="/Images/icon_close-window.png" alt="close" />
      </a>
    </div>
    <div class="config-form">
      <form action="">
        Max Virtual Machine Count:
        <asp:TextBox ID="MaxVMCount" MaxLength="2" Columns="3" runat="server" /><br />
        IP Address Range: <br />
        192.168.1.
        <asp:TextBox ID="IPLowerBound" MaxLength="3" Columns="4" runat="server" />
        to
        <asp:TextBox ID="IPUpperBound" MaxLength="3" Columns="4" runat="server" /><br />
        VM Creation Time:
        <asp:TextBox ID="CreationTime" MaxLength="10" Columns="12" runat="server" /><br />
        VM Backup Time:
        <asp:TextBox ID="BackupTime" MaxLength="10" Columns="12" runat="server" /><br />
        VM Archive Time:
        <asp:TextBox ID="ArchiveTime" MaxLength="10" Columns="12" runat="server" /><br />
        <div class="update-host-config-buttons">
          <asp:Button ID="UpdateHostConfigButton" Text="Save" runat="server" OnClick="UpdateHostConfig" />
        </div>
      </form>
    </div>
  </div>
  -->
</asp:Content>
