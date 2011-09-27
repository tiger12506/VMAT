<%@ Page Title="Create Server - VMAT" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="VMat.Create" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
    Create A New Virtual Server
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form action="">
        Server Name: <input type="text" name="mname" maxlength="30" /><br />
        Operating System:
        <select name="operatingsystems">
            <!--- Either make this dynamic or update list based on client's OS's --->
            <option value="win7">Windows 7</option>
            <option value="winserv2003">Windows Server 2003</option>
            <option value="winserv2008">Windows Server 2008</option>
        </select> <br />
        Project Number: <input type= "text" name="projnum" maxlength="20"/><br />
        <br />
        <input type="submit" value="Create" />
    </form>
</asp:Content>
