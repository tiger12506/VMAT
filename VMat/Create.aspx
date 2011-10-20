<%@ Page Title="Create Server - VMAT" Language="C#" MasterPageFile="~/Site.Master"
  AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="VMat.Create" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Create A New Virtual Server
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <form action="">
  Server Name:
  <input type="text" name="mname" maxlength="30" /><br />
  Operating System:
  <asp:DropDownList ID="ImageList" runat="server" 
      onselectedindexchanged="ImageList_SelectedIndexChanged">
  </asp:DropDownList>
&nbsp;<br />
  Project Number:
  <input type="text" name="projnum" maxlength="20" /><br />
  <br />
  <asp:button runat="server" Text="Create" OnClick="CreateClick" />
  <asp:button runat="server" onclick="CancelClick" Text="Cancel" />
  </form>
</asp:Content>
