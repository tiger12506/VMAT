<%@ Page Title="View Servers - VMAT" Language="C#" MasterPageFile="~/Site.Master"
  AutoEventWireup="true" CodeBehind="ViewList.aspx.cs" Inherits="VMat.ViewList" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript" src="/Scripts/popupDiv.js"></script>
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
  <div class="servers">
    <div class="servers-list">
      <table class="servers-table">
        <tbody>
          <tr class="servers-table-header">
            <th id="server-status">
              Status
            </th>
            <th>
              Server Name
            </th>
            <th>
              Operating System
            </th>
            <th>
              Project Name
            </th>
            <th>
              Created On
            </th>
          </tr>
          <tr class="server-item">
            <td class="server-status">
              <div class="status-icon">
                <a href="#" runat="server">
                  <img src="/Images/icon_led-red.png" alt="Idle" />
                </a>
              </div>
            </td>
            <td class="server-name">
              Pikachu
            </td>
            <td class="server-os">
              <div class="os-icon">
              </div>
              <div class="os-name">
                Windows ME</div>
            </td>
            <td class="server-project">
              MAKE ALL THE PROGRAMS!!!
            </td>
            <td class="server-date">
              Jan. 11, 2012
            </td>
            <td class="project-complete">
              <a href="#"><span class="button"><span class="icon">
                <img src="Images/icon_server-complete.png" alt="" />
              </span>Complete </span></a>
            </td>
          </tr>
          <tr class="server-item">
            <td class="server-status">
              <div class="status-icon">
                <a href="#" runat="server">
                  <img src="/Images/icon_led-green.png" alt="Active" />
                </a>
              </div>
            </td>
            <td class="server-name">
              Charmander
            </td>
            <td class="server-os">
              <div class="os-icon">
              </div>
              <div class="os-name">
                WebOS</div>
            </td>
            <td class="server-project">
              The New Coke
            </td>
            <td class="server-date">
              Jan. 9, 2012
            </td>
            <td class="project-complete">
              <a href="#"><span class="button"><span class="icon">
                <img src="Images/icon_server-complete.png" alt="" />
              </span>Complete </span></a>
            </td>
          </tr>
          <tr class="server-item">
            <td class="server-status">
              <div class="status-icon">
                <a href="#" runat="server">
                  <img src="/Images/icon_led-green.png" alt="Active" />
                </a>
              </div>
            </td>
            <td class="server-name">
              Mudkip
            </td>
            <td class="server-os">
              <div class="os-icon">
              </div>
              <div class="os-name">
                FreeBSD</div>
            </td>
            <td class="server-project">
              Operation Spank-Noodle
            </td>
            <td class="server-date">
              Dec. 4, 2010
            </td>
            <td class="project-complete">
              <a href="#"><span class="button"><span class="icon">
                <img src="Images/icon_server-complete.png" alt="" />
              </span>Complete </span></a>
            </td>
          </tr>
        </tbody>
      </table>
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
      <h2>Configure Server</h2>
      <a href="#" onclick="closeWindow('config-window');" target="_self">
        <img src="/Images/icon_close-window.png" alt="close" />
      </a>
    </div>
  </div>
</asp:Content>
