<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <ul id="display-projects-list" class="project-list">
      <xsl:for-each select="records/project">
        <li class="project-item">
          <div class="project-header">
            <h2>
              <xsl:value-of select="name" />
            </h2>
            <span class="project-complete">
              <a href="#">
                <span class="button">
                  <span class="icon">
                    <img src="/Images/icon_server-complete.png" alt="" />
                  </span>
                  Completed
                </span>
              </a>
            </span>
          </div>
          <div class="project-servers">
            <ul class="servers-list">
              <xsl:for-each select="server">
                <li class="server-item">
                  <span class="server-item-info">
                    <span class="status-icon">
                      <a onclick="toggleServerStatus();">
                        <xsl:choose>
                          <xsl:when test="status = 0">
                            <img src="/Images/icon_led-red.png" />
                          </xsl:when>
                          <xsl:when test="status = 1">
                            <img src="/Images/icon_led-green.png" />
                          </xsl:when>
                        </xsl:choose>
                      </a>
                    </span>
                    <span class="server-name">
                      <span class="server-item-label">Server Name</span>
                      <span class="server-item-tag">
                        <xsl:value-of select="name" />
                      </span>
                    </span>
                    <span class="os-icon">
                      <img src="/Images/logo_windows-server-2008.png" />
                    </span>
                    <span class="iso-name">
                      <span class="server-item-label">Image File</span>
                      <span class="server-item-tag">
                        <xsl:value-of select="isofile" />
                      </span>
                    </span>
                    <span class="ip-address">
                      <span class="server-item-label">IP Address</span>
                      <span class="server-item-tag">
                        <xsl:value-of select="ipaddress" />
                      </span>
                    </span>
                    <span class="creation-date">
                      <span class="server-item-label">Date Created</span>
                      <span class="server-item-tag">
                        <xsl:value-of select="creation-date" />
                      </span>
                    </span>
                  </span>
                </li>
              </xsl:for-each>
            </ul>
          </div>
        </li>
      </xsl:for-each>
    </ul>
  </xsl:template>
</xsl:stylesheet>
