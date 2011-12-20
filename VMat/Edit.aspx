<%@ Page Title="Edit Page" Language="C#" MasterPageFile="~/Site.Master"
 AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="VMat.Edit" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHeader" runat="server">
    Edit Machine Parameters
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<form id="form1" runat="server">

<asp:Panel ID="MainPanel" runat="server">
    <asp:Label ID="ImageLabel" runat="server" Text="Image File: "></asp:Label>
    <asp:Label ID="ImageFile" runat="server" Text="[image name not specified]"></asp:Label>
    <br />
    <br />
    <asp:Label ID="HostnameLabel" runat="server" Text="Hostname: "></asp:Label>
    <asp:TextBox ID="Hostname" runat="server"></asp:TextBox>
    <br />
    <br />
    <asp:Label ID="IPAddressLabel" runat="server" Text="IP Address: "></asp:Label>
    <asp:TextBox ID="IPAddress" runat="server"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="ApplyButton" runat="server" onclick="ApplyButton_Click" 
        Text="Apply" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="CancelButton" runat="server" onclick="CancelButton_Click" 
        Text="Cancel" />
</asp:Panel>

    </form>

</asp:Content>
