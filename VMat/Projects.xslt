<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
          <xsl:for-each select="records/project">
          <!-- Single server project -->
          <li class="project-item">
            <div class="project-header">
              <h2>gapdev</h2>
              <span class="project-complete">
                <a href="#">
                  <span class="button">
                    <span class="icon">
                      <img src="Images/icon_server-complete.png" alt="" />
                    </span>
                    Completed
                  </span>
                </a>
              </span>
            </div>
            <div class="project-servers">
              <ul class="servers-list">
                <li class="server-item">
                  <span class="server-item-info">
                    <span class="status-icon">
                      <a href="#" runat="server">
                        <img src="/Images/icon_led-green.png" />
                      </a>
                    </span>
                    <span class="server-name">
                      <span class="server-item-label">Server Name</span>
                      <span class="server-item-tag">gapdev1234</span>
                    </span>
                    <span class="os-icon">
                      <img src="/Images/logo_windows-server-2008.png" />
                    </span>
                    <span class="iso-name">
                      <span class="server-item-label">Image File</span>
                      <span class="server-item-tag">fakewindowsserver</span>
                    </span>
                    <span class="ip-address">
                      <span class="server-item-label">IP Address</span>
                      <span class="server-item-tag">192.168.1.9</span>
                    </span>
                    <span class="creation-date">
                      <span class="server-item-label">Date Created</span>
                      <span class="server-item-tag">Jan. 11, 2012</span>
                    </span>
                  </span>
                </li>
              </ul>
            </div>
          </li>
          </xsl:for-each>
        </xsl:copy>
    </xsl:template>
</xsl:stylesheet>
