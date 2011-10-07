﻿<%@ Page Title="View Servers - VMAT" Language="C#" MasterPageFile="~/Site.Master"
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

  <div class="server-management">
    <div class="list">
      <ol class="projects-list">
        <li class="project-item">
          <div class="project-header">
            <h3>BH90210</h3>
            <div class="project-complete">
              <a href="#">
                <span class="button">
                  <span class="icon">
                    <img src="Images/icon_server-complete.png" alt="" />
                  </span>
                  Completed
                </span>
              </a>
            </div>
          </div>
          <div class="project-servers">
            <ol class="servers-list">
              <li class="server-item">
                <div class="status-icon">
                  <a href="#" runat="server">
                    <img src="/Images/icon_led-green.png" />
                  </a>
                </div>
                <div class="server-name">
                  <h4>Pikachu</h4>
                </div>
                <div class="os-icon">
                  <img src="/Images/logo_windows-server-2008.png" />
                </div>
                <div class="iso-name">
                  fakewindowsserver
                </div>
                <div class="ip-address">
                  192.168.1.9
                </div>
                <div class="server-date">
                  Jan. 11, 2012
                </div>
              </li>
            </ol>
          </div>
        </li>
        <li class="project-item">
          <div class="project-header">
            <h3>BH90210</h3>
            <div class="project-complete">
              <a href="#">
                <span class="button">
                  <span class="icon">
                    <img src="Images/icon_server-complete.png" alt="" />
                  </span>
                  Completed
                </span>
              </a>
            </div>
          </div>
          <div class="project-servers">
            <ol class="servers-list">
              <li class="server-item">
                <div class="status-icon">
                  <a href="#" runat="server">
                    <img src="/Images/icon_led-red.png" />
                  </a>
                </div>
                <div class="server-name">
                  <h4>Pikachu</h4>
                </div>
                <div class="os-icon">
                  <img src="/Images/logo_windows-server-2008.png" />
                </div>
                <div class="iso-name">
                  fakewindowsserver
                </div>
                <div class="ip-address">
                  192.168.1.9
                </div>
                <div class="server-date">
                  Jan. 11, 2012
                </div>
              </li>
              <li class="server-item">
                <div class="status-icon">
                  <a href="#" runat="server">
                    <img src="/Images/icon_led-green.png" />
                  </a>
                </div>
                <div class="server-name">
                  <h4>Pikachu</h4>
                </div>
                <div class="os-icon">
                  <img src="/Images/logo_windows-server-2008.png" />
                </div>
                <div class="iso-name">
                  fakewindowsserver
                </div>
                <div class="ip-address">
                  192.168.1.9
                </div>
                <div class="server-date">
                  Jan. 11, 2012
                </div>
              </li>
            </ol>
          </div>
        </li>
      </ol>
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
