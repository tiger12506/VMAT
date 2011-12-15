<%@ Page Title="Virtual Machine Management - VMAT" Language="C#" MasterPageFile="~/Site.Master"
  AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VMat.Default" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Scripts/popupDiv.js"></script>
  <script type="text/javascript" src="/Scripts/vmControls.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
    Virtual Machine Management
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="machine-controls">
    <a href="CreateMachine.aspx">
      <span class="button">
        <span class="icon">
          <img src="/Images/icon_server-create.png" alt="" />
        </span>
        Create 
      </span>
    </a>
    <a href="ConfigHost.aspx">
      <span class="button">
        <span class="icon">
          <img src="/Images/icon_server-admin.png" alt="" />
        </span>
        Admin
      </span>
    </a>
  </div>


  <div class="machine-management">
    <div id="display-projects-list" class="list">
    <form runat="server">
      <asp:Panel ID="ServerListPanel" runat="server" />
      </form>
    </div>
  </div>

</asp:Content>
