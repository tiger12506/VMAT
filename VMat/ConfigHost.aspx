<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfigHost.aspx.cs" Inherits="VMat.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHeader" runat="server">
  Configure Host
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <form class="config-form" action="" runat="server">
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
      <asp:Button ID="CancelButton" Text="Cancel" runat="server" OnClick="Default.aspx" />
    </div>
  </form>
</asp:Content>
