<%@ Page Title="Create Server - VMAT" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="VMat.Create" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Create A New Virtual Server</h2>
    Server Name: <input type="text" name="mname" maxlength="30" /><br />
    Operating System: <input type="button" name="os" /><br />
    Project Number: <input type= "text" name="projnum" maxlength="40"/><br />
    <br />
    <input type="submit" value="Create" />
</asp:Content>
