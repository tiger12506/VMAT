<%@ Page Title="Virtual Machine Management - VMAT" Language="C#" MasterPageFile="~/Site.Master"
  AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VMat.Default" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript" src="/Scripts/popupDiv.js"></script>
  <script type="text/javascript" src="/Scripts/vmControls.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainHeader" runat="server">
  Virtual Machine Management
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="machine-controls">
    <a href="CreateMachine.aspx">
      <span class="button">
        <span class="icon">
          <img src="/Images/icon_server-create.png" alt="" />
        </span>
        Create 
      </span>
    </a>
    <a href="ConfigHost.aspx">
      <span class="button">
        <span class="icon">
          <img src="/Images/icon_server-admin.png" alt="" />
        </span>
        Admin
      </span>
    </a>
  </div>

  <div class="machine-management">
    <div id="display-projects-list" class="list">
      <asp:Repeater ID="ProjectDisplay" runat="server">
        <ItemTemplate>
          <div class="project-header">
            <h2><%# DataBinder.Eval(Container.DataItem, "ProjectName") %></h2>
            <h3>GAPDEV.COM</h3>
            <span class="project-complete">
              <a href="javascript:void(0)" onclick="">
                <span class="button">
                  <span class="icon">
                    <img src="/Images/icon_project-complete.png" alt="" />
                  </span>
                  Completed
                </span>
              </a>
            </span>
          </div>
          <div class="project-machines">
            <asp:Repeater ID="MachineDisplay" datasource='<%# DataBinder.Eval(Container.DataItem, "VirtualMachines") %>' runat="server">
              <ItemTemplate>
              <span class="machine-item-info">
                <span class="status-icon">
                  <button onclick="toggleMachineStatus('statusicon-<%# DataBinder.Eval(Container.DataItem, "MachineName") %>'); return false;">
                    <script type="text/javascript">document.write(getStatusIcon('status-<%# DataBinder.Eval(Container.DataItem, "MachineName") %>'));</script>
                  </button>
                </span>
                <span class="machine-name">
                  <span class="label">Machine Name</span>
                  <span class="tag">
                    <%# DataBinder.Eval(Container.DataItem, "MachineName") %>
                  </span>
                </span>
                
                <span class="ip-address">
                  <span class="label">IP Address</span>
                  <span class="tag">
                    <%# DataBinder.Eval(Container.DataItem, "IP") %>
                  </span>
                </span>
                <span class="creation-date">
                  <span class="label">Date Created</span>
                  <span class="tag">
                    <%# DataBinder.Eval(Container.DataItem, "Created") %>
                  </span>
                </span>
                <span class="last-shutdown-time">
                  <span class="label">Last Shutdown Time</span>
                  <span class="tag">
                    <%# DataBinder.Eval(Container.DataItem, "LastStopped") %>
                  </span>
                </span>
                <span class="machine-details-toggle">
                  <a href="javascript:void(0)" onclick="toggleMachineDetails('details-<%# DataBinder.Eval(Container.DataItem, "MachineName") %>'); return false;">Details</a>
                </span>
              </span>
              <span id='details-<%# DataBinder.Eval(Container.DataItem, "MachineName") %>' class="machine-details-info">
                <span class="iso-name">
                  <span class="label">Image File</span>
                  <span class="tag">
                    <%# DataBinder.Eval(Container.DataItem, "ImagePathName") %>
                  </span>
                </span>
                <span class="last-start-time">
                  <span class="label">Last Start Time</span>
                  <span class="tag">
                    <%# DataBinder.Eval(Container.DataItem, "LastStarted")%>
                  </span>
                </span>
                <span class="last-backup-time">
                  <span class="label">Last Backup Time</span>
                  <span class="tag">
                    <%# DataBinder.Eval(Container.DataItem, "LastBackuped")%>
                  </span>
                </span>
                <span class="last-archive-time">
                  <span class="label">Last Archive Time</span>
                  <span class="tag">
                    <%# DataBinder.Eval(Container.DataItem, "LastArchived")%>
                  </span>
                </span>
              </span>
              </ItemTemplate>
              <SeparatorTemplate>
                <hr />
              </SeparatorTemplate>
            </asp:Repeater>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>

</asp:Content>
