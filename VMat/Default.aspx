<%@ Page Title="Server Management - VMAT" Language="C#" MasterPageFile="~/Site.Master"
  AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VMat.ViewList" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript" src="/Scripts/popupDiv.js"></script>
  <script type="text/javascript" src="/Scripts/displayProjects.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Server Management
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="server-controls">
    <a href="#" onclick="setVisible('create-window');" target="_self">
      <span class="button">
        <span class="icon">
          <img src="Images/icon_server-create.png" alt="" />
        </span>
        Create 
      </span>
    </a>
    <a href="#" onclick="setVisible('config-window');" target="_self">
      <span class="button">
        <span class="icon">
          <img src="Images/icon_server-admin.png" alt="" />
        </span>
        Admin
      </span>
    </a>
  </div>

  <div class="server-management">
    <div id="display-projects-list" class="list">
      <asp:Repeater ID="ProjectDisplay" runat="server">
        <ItemTemplate>
          <div class="project-header">
            <h2><%# DataBinder.Eval(Container.DataItem, "projectname") %></h2>
            <span class="project-complete">
              <a href="#">
                <span class="button">
                  <span class="icon">
                    <img src="/Images/icon_server-complete.png" alt="" />
                  </span>
                  Completed
                </span>
              </a>
            </span>
          </div>
          <div class="project-servers">
            <asp:Repeater ID="ServerDisplay" datasource='<%# ((DataRowView)Container.DataItem)
              .Row.GetChildRows("project_server") %>' runat="server">
              <ItemTemplate>
              <span class="server-item-info">
                <span class="status-icon">
                  <a onclick="toggleServerStatus();">
                    <img src="/Images/icon_led-green.png" />
                  </a>
                </span>
                <span class="server-name">
                  <span class="server-item-label">Server Name</span>
                  <span class="server-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"servername\"]") %>
                  </span>
                </span>
                <span class="os-icon">
                  <img src="/Images/logo_windows-server-2008.png" />
                </span>
                <span class="iso-name">
                  <span class="server-item-label">Image File</span>
                  <span class="server-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"isofile\"]") %>
                  </span>
                </span>
                <span class="ip-address">
                  <span class="server-item-label">IP Address</span>
                  <span class="server-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"ipaddress\"]") %>
                  </span>
                </span>
                <span class="creation-date">
                  <span class="server-item-label">Date Created</span>
                  <span class="server-item-tag">
                    <%# DataBinder.Eval(Container.DataItem, "[\"creation-date\"]") %>
                  </span>
                </span>
                <span class="server-details">
                  <a href="#" onclick="">Details</a>
                </span>
              </span>
              </ItemTemplate>
            </asp:Repeater>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>

  <!-- Popup Windows for server controls -->

  <div class="popup-window" id="create-window">
    <div class="header">
      <h2>Create Server</h2>
      <a href="#" onclick="closeWindow('create-window');" target="_self">
        <img src="/Images/icon_close-window.png" alt="close" />
      </a>
    </div>
    <div class="create-form">
      <form action="" runat="server">
        Server Name Suffix:
        <asp:TextBox ID="ServerNameSuffix" MaxLength="5" Width="100" runat="server"></asp:TextBox><br />
        Image File:
        <asp:DropDownList ID="ImageList" runat="server" onload="ImageList_Load"></asp:DropDownList>
        <br />
        Project Number:
        <asp:DropDownList ID="ProjectList" runat="server" OnLoad="ProjectList_Load"></asp:DropDownList>
        <br />
        <div class="create-form-buttons">
          <input type="submit" value="Create" />
        </div>
      </form>
    </div>
  </div>

  <div class="popup-window" id="config-window">
    <div class="header">
      <h2>Configure Host</h2>
      <a href="#" onclick="closeWindow('config-window');" target="_self">
        <img src="/Images/icon_close-window.png" alt="close" />
      </a>
    </div>
    <div class="config-form">
      <form action="">
      </form>
    </div>
  </div>
</asp:Content>
