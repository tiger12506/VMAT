<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateMachine.aspx.cs" Inherits="VMat.CreateMachine" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Create Machine
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <form id="create-form" action="" runat="server">
    Machine Name Suffix:
    <asp:TextBox ID="MachineNameSuffix" MaxLength="5" Columns="8" runat="server" /><br />
    Image File:
    <asp:DropDownList ID="ImageList" runat="server" onload="ImageList_Load" /><br />
    Project Number:
    <asp:DropDownList ID="ProjectList" runat="server" OnLoad="ProjectList_Load" /><br />
    <div class="create-form-buttons">
      <asp:Button ID="CreateMachineSubmitButton" Text="Create" runat="server" OnClick="CreateMachine" />
      <asp:Button ID="CancelButton" Text="Cancel" runat="server" OnClick="Default.aspx" />
    </div>
  </form>
</asp:Content>
