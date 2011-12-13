<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateMachine.aspx.cs" Inherits="VMat.CreateMachine" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Create Machine
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="form-content">
    <form class="create-form" runat="server">
      <asp:Panel ID="ConfigurationPanel" runat="server" GroupingText="Configuration">
          Project Number:
          <asp:DropDownList ID="ProjectList" runat="server" AutoPostBack="True" 
              onload="ProjectList_Load" onselectedindexchanged="Update_Description" />
          <br />
          Machine Name Suffix:
          <asp:TextBox ID="MachineNameSuffix" MaxLength="5" Columns="8" 
    runat="server" AutoPostBack="True" ontextchanged="Update_Description" />
          <br />
          Image File:
          <asp:DropDownList ID="ImageList" runat="server" onload="ImageList_Load" />
          <br />
      </asp:Panel>
      <asp:Panel ID="DescriptionPanel" runat="server" BorderStyle="None" 
          GroupingText="Description" Height="240px" style="width: 960px" Width="531px">
          <asp:Table ID="DescriptionTable" runat="server" 
    Height="136px" Width="531px" CellPadding="0" onload="DescriptionTable_Load">
              <asp:TableRow ID="ProjectNumberRow" runat="server">
                  <asp:TableCell runat="server">Project Number:</asp:TableCell>
                  <asp:TableCell ID="ProjectNumber" runat="server">Gxxxx</asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="HostnameRow" runat="server">
                  <asp:TableCell runat="server">Hostname: </asp:TableCell>
                  <asp:TableCell ID="Hostname" runat="server">gapdevxxxxyyyyy.example.com</asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="IPAddressRow" runat="server">
                  <asp:TableCell runat="server">IP Address:</asp:TableCell>
                  <asp:TableCell ID="IPAddress" runat="server">None Available</asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="ImageFileRow" runat="server">
                  <asp:TableCell runat="server">Image File:</asp:TableCell>
                  <asp:TableCell ID="ImageFile" runat="server"></asp:TableCell>
              </asp:TableRow>
          </asp:Table>
      </asp:Panel>
      <br />
      <div class="create-form-buttons">
        <asp:Button ID="CreateMachineSubmitButton" Text="Create" runat="server" OnClick="CreateNewMachine" />
        <asp:Button ID="CancelButton" Text="Cancel" runat="server" OnClick="Cancel_Click" />
      </div>
    </form>
  </div>
</asp:Content>
