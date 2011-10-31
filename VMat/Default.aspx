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
    <a href="javascript:void(0)" onclick="setVisible('create-window');" target="_self">
      <span class="button">
        <span class="icon">
          <img src="Images/icon_server-create.png" alt="" />
        </span>
        Create 
      </span>
    </a>
    <a href="javascript:void(0)" onclick="setVisible('config-window');" target="_self">
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
                <span class="machine-details">
                  <a href="javascript:void(0)" onclick="displayMachineDetails();">Details</a>
                </span>
              </span>
              </ItemTemplate>
            </asp:Repeater>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>

  <!-- Popup Windows for virtual machine controls -->

  <div class="popup-window" id="create-window">
    <div class="header">
      <h2>Create Machine</h2>
      <a href="javascript:void(0)" onclick="closeWindow('create-window');" target="_self">
        <img src="/Images/icon_close-window.png" alt="close" />
      </a>
    </div>
    <div class="create-form">
      <form runat="server">
        Machine Name Suffix:
        <asp:TextBox ID="MachineNameSuffix" MaxLength="5" Width="100" runat="server" /><br />
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
      </form>
    </div>
  </div>
</asp:Content>
