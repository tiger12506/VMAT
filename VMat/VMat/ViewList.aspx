<%@ Page Title="View Servers - VMAT" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewList.aspx.cs" Inherits="VMat.ViewList" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
    Server Management
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="server-controls">
        <a href="Create.aspx">
            <span class="button">
                <span class="icon">
                    <img src="Images/icon_server-create.png" />
                </span>
                Create
            </span>
        </a>
        <a href="Configure.aspx">
            <span class="button">
                <span class="icon">
                    <img src="Images/icon_server-admin.png" />
                </span>
                Admin
            </span>
        </a>
    </div>

    <div class="servers">
        <div class="servers-list">
            <table class="servers-table">
                <tbody>
                    <tr class="servers-table-header">
                        <th class="servers-table-header-status">Status</th>
                        <th class="servers-table-header-servername">Server Name</th>
                        <th class="servers-table-header-os">Operating System</th>
                        <th class="servers-table-header-projectname">Project Name</th>
                    </tr>
                    <tr class="server-item">
                        <td class="server-status">
                            <div class="status-icon">
                                <center>
                                    <a href="/#" runat="server">
                                        <img src="Images/icon_led-red.png" alt="Inactive"/>
                                    </a>
                                </center>
                            </div>
                        </td>
                        <td class="server-name">Pikachu</td>
                        <td class="server-os">
                            <div class="os-icon"></div>
                            <div class="os-name">Windows ME</div>
                        </td>
                        <td class="server-project">MAKE ALL THE PROGRAMS!!!</td>
                        <td class="server-complete"></td>
                    </tr>
                    <tr class="server-item">
                        <td class="server-status">
                            <div class="status-icon">
                                <center>
                                    <a href="/#" runat="server">
                                        <img src="Images/icon_led-green.png" alt="Active"/>
                                    </a>
                                </center>
                            </div>
                        </td>
                        <td class="server-name">Charmander</td>
                        <td class="server-os">
                            <div class="os-icon"></div>
                            <div class="os-name">WebOS</div>
                        </td>
                        <td class="server-project">The New Coke</td>
                    </tr>
                    <tr class="server-item">
                        <td class="server-status">
                            <div class="status-icon">
                                <center>
                                    <a href="/#" runat="server">
                                        <img src="Images/icon_led-green.png" alt="Active"/>
                                    </a>
                                </center>
                            </div>
                        </td>
                        <td class="server-name">Mudkip</td>
                        <td class="server-os">
                            <div class="os-icon"></div>
                            <div class="os-name">FreeBSD</div>
                        </td>
                        <td class="server-project">Operation Spank-Noodle</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
