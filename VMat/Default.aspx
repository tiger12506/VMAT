<%@ Page Title="Server Management - VMAT" Language="C#" MasterPageFile="~/Site.Master"
  AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VMat.ViewList" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript" src="/Scripts/popupDiv.js"></script>
  <script type="text/javascript" src="/Scripts/displayProjects.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Server Management
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent"  runat="server">
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
    <div class="list">
      <ul id="display-projects-list" class="projects-list">
        <!-- Insert XSLT document here -->
        <script>displayResult();</script>
      </ul>
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
      <form action="">
        Server Name:
        <input type="text" name="mname" maxlength="30" /><br />
        Operating System:
        <select name="operatingsystems">
          <!-- Either make this dynamic or update list based on client's OS's -->
          <option value="win7">Windows 7</option>
          <option value="winserv2003">Windows Server 2003</option>
          <option value="winserv2008">Windows Server 2008</option>
        </select>
        <br />
        Project Number:
        <input type="text" name="projnum" maxlength="20" /><br />
        <br />
        <div class="create-form-buttons">
          <input type="submit" value="Create" />
          <button onclick="closeWindow('create-window');">
            Reset
          </button>
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
  </div>
</asp:Content>
