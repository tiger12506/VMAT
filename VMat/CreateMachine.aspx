<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateMachine.aspx.cs" Inherits="VMat.CreateMachine" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Create Machine
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="form-content">
    <form class="create-form" runat="server">
      Machine Name Suffix:
      <asp:TextBox ID="MachineNameSuffix" MaxLength="5" Columns="8" runat="server" /><br />
      Image File:
      <asp:DropDownList ID="ImageList" runat="server" /><br />
      Project Number:
      <asp:DropDownList ID="ProjectList" runat="server" /><br />
      <div class="create-form-buttons">
        <asp:Button ID="CreateMachineSubmitButton" Text="Create" runat="server" OnClick="CreateNewMachine" />
        <asp:Button ID="CancelButton" Text="Cancel" runat="server" OnClick="Cancel_Click" />
      </div>
    </form>
  </div>
</asp:Content>
