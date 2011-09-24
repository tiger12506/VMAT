<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="VMat._Default" %>

<form id="form1" runat="server">
    <h2>Setup new VM</h2>
    <asp:label id="Label1" runat="server" text="Requested Virtual Machine Size (GB):"></asp:label>
    <asp:textbox id="TextBox1" runat="server"></asp:textbox>
    <br />
    <asp:label id="Label2" runat="server" text="Requested Machine Name:"></asp:label>
    <asp:textbox id="TextBox2" runat="server"></asp:textbox>
    <br />
    <asp:button id="SubmitButton" runat="server" text="Submit" />
</form>