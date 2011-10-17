﻿<%@ Page Title="Welcome - VMAT" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
  CodeBehind="Default.aspx.cs" Inherits="VMat._Default" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Welcome to VMAT
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="startpage-buttons">
    <ul>
      <li class="create"><a href="Create.aspx">
        <div class="icon">
          <img src="Images/icon_create.png" alt="Create Server" />
        </div>
        <div class="label">
          <h3>
            Create Server
          </h3>
        </div>
      </a></li>
      <li class="viewlist"><a href="ViewList.aspx">
        <div class="icon">
          <img src="Images/icon_servers.png" alt="View Server List" />
        </div>
        <div class="label">
          <h3>
            Manage Servers</h3>
        </div>
      </a></li>
      <li class="configure"><a href="Configure.aspx">
        <div class="icon">
          <img src="Images/icon_configure.png" alt="Configure Host" />
        </div>
        <div class="label">
          <h3>
            Configure Host</h3>
        </div>
      </a></li>
    </ul>
  </div>
</asp:Content>