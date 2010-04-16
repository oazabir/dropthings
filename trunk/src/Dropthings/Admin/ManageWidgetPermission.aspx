<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="ManageWidgetPermission.aspx.cs" Inherits="ManageWidgetPermissionPage" %>
<%@ Register Src="~/ScriptManagerControl.ascx" TagName="ScriptManagerControl" TagPrefix="common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="ltlManageWidgetPermission" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ManageWidgetPermission%>" /></title>
    <style>    
body, td, div, p, a, font, span {
font-family:arial,sans-serif;
}
body {
margin-top:2px;
}
.c {
height:4px;
width:4px;
}
.bubble {
background-color:#C3D9FF;
}
.tl {
padding:0pt;
text-align:left;
vertical-align:top;
width:4px;
}
.tr {
padding:0pt;
text-align:right;
vertical-align:top;
width:4px;
}
.bl {
padding:0pt;
text-align:left;
vertical-align:bottom;
width:4px;
}
.br {
padding:0pt;
text-align:right;
vertical-align:bottom;
width:4px;
}
.caption {
background:#EEEEEE none repeat scroll 0%;
color:#000000;
font-weight:bold;
margin-bottom:5px;
text-align:center;
white-space:nowrap;
}
.login
{
    font-size: 10pt;
}
a:link {
color:#0000CC;
}
a:visited {
color:#551A8B;
}
a:active {
color:#FF0000;
}
.CommonListArea
    {
	    padding-bottom: 8px;
	    padding-top: 8px;
    }

    .CommonListTitle
    {
	    padding: 6px 6px 6px 0;
	    color: #000;
	    font-size: 180%;
	    font-weight: bold;
	    margin: 0;
    }

    .CommonListTitle A, .CommonListTitle A:VISITED, .CommonListTitle A:ACTIVE
    {
	    color: #000;
	    text-decoration: none;
    }

    .CommonListTitle A:HOVER
    {
        text-decoration: underline;
    }

    .CommonListHeaderLeftMost
    {
	    text-align: left;
	    padding: 6px;
	    background-color: #dfdfdf;
	    color: #666;
	    font-family: Arial, Helvetica;
	    font-weight: bold;
    }

    .CommonListHeader
    {
	    text-align: left;
	    padding: 6px;
	    background-color: #e3e3e3;
	    color: #666;
	    font-family: Arial, Helvetica;
	    font-weight: bold;
    }

    .PermissionHeaderCenter
    {
	    text-align: center;
	    font-size: .8em;
    }

    .CommonListCellLeftMost
    {
	    padding: 6px;
	    color: #333333;
	    font-family: Arial, Helvetica;
    }

    .CommonListCell
    {
	    padding: 6px;
	    color: #333333;
	    font-family: Arial, Helvetica;
	    text-align: left;
    }

    .CommonListCell.Status
    {
	    padding: 0px 15px 0px 15px;
	    text-align: center;
    }
    </style>
</head>
<body bgcolor="#ffffff">
    <form id="loginform" runat="server">
        <common:ScriptManagerControl ID="ScriptManagerControl1" runat="server" />

        <table cellpadding="2" width="100%" cellspacing="0" border="0">
            <tr>
                <td colspan="2">
                    <img width="1" alt="" height="2" /></td>
            </tr>
            <tr>
                <td width="1%" valign="top">
                </td>
                <td valign="top">
                    <table cellpadding="0" width="100%" cellspacing="0" border="0">
                        <tr>
                            <td colspan="2">
                                <img width="1" alt="" height="15"></td>
                        </tr>
                        <tr bgcolor="#3366cc">
                            <td>
                                <img width="1" alt="" height="1"></td>
                        </tr>
                        <tr bgcolor="#DDE8CC">
                            <td style="padding-left: 4px; padding-bottom: 3px; padding-top: 2px; font-family: arial,sans-serif;">
                                <b><asp:Literal ID="lblManagePermission" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ManageWidgetPermission%>" /></b></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <img width="1" alt="" height="5"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>        
        <table width="94%" align="center" cellpadding="5" cellspacing="1">            
            <tr>
              <td colspan="2" width="100%" nowrap="nowrap" valign="top">
                  <div>
                    <%if (Widgets.Count > 0) %>
                    <%{ %>
                        <table>
                            <tbody>
                                <tr>
                                    <th class="CommonListHeaderLeftMost">Widget</th>
                                    <% for (int i = 0; i < Roles.Length; i++ ) %>
                                    <% { %>
                                         <th class="CommonListHeader PermissionHeaderCenter"><%= Roles[i]%></th>      
                                    <% } %>
                                </tr>
                                <% foreach (Dropthings.DataAccess.Widget widget in Widgets) %>
                                <% { %>
                                <tr id="widget<%= widget.ID%>" class="widgetItem">
                                    <td class="CommonListCellLeftMost"><%= widget.Name %> <input type="hidden" id="hfWidget<%= widget.ID.ToString() %>" /></td>
                                    <% for (int i = 0; i < Roles.Length; i++ ) %>
                                    <% { %>
                                         <td class="CommonListCell Status">
                                            <%if (IsWidgetInRole(widget.ID, Roles[i])) %>
                                            <%{ %>
                                                <input type="checkbox" id="chc<%= Roles[i]%>" checked=checked class="role" />
                                            <% } %>
                                            <%else %>
                                            <%{ %>
                                                <input type="checkbox"  id="chu<%= Roles[i]%>" class="role" />
                                            <% } %>
                                         </td>         
                                    <% } %>
                                </tr>
                                
                                <% } %>
                            </tbody>
                        </table>
                   
                    <%} %>
                    
                </div>
                <div>
                    <span id="Message"></span>
                </div>
                <div>
                    <span id="progress" style="display:none">working...</span>
                </div>
                <div>
                    
                </div>
                <div>
                    <asp:Button id="SaveButton" runat="server" Text="<%$Resources:SharedResources, Save%>" OnClientClick="WidgetPermission.Save();return false;" />&nbsp;
                </div>
              </td>
            </tr>
        </table>
    </form>
</body>
</html>
