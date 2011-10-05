<%@ Page Title="View Servers - VMAT" Language="C#" MasterPageFile="~/Site.Master"
  AutoEventWireup="true" CodeBehind="ViewList.aspx.cs" Inherits="VMat.ViewList" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Server Management
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="server-controls">
    <a href="Create.aspx"><span class="button"><span class="icon">
      <img src="Images/icon_server-create.png" alt="" />
    </span>Create </span></a><a href="Configure.aspx"><span class="button"><span class="icon">
      <img src="Images/icon_server-admin.png" alt="" />
    </span>Admin </span></a>
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
                <a href="/#" runat="server">
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
              <a href="/#"><span class="button"><span class="icon">
                <img src="Images/icon_server-complete.png" alt="" />
              </span>Complete </span></a>
            </td>
          </tr>
          <tr class="server-item">
            <td class="server-status">
              <div class="status-icon">
                <a href="/#" runat="server">
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
              <a href="/#"><span class="button"><span class="icon">
                <img src="Images/icon_server-complete.png" alt="" />
              </span>Complete </span></a>
            </td>
          </tr>
          <tr class="server-item">
            <td class="server-status">
              <div class="status-icon">
                <a href="/#" runat="server">
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
              <a href="/#"><span class="button"><span class="icon">
                <img src="Images/icon_server-complete.png" alt="" />
              </span>Complete </span></a>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</asp:Content>
