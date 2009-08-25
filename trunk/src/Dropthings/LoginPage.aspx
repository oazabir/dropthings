<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <style type="text/css">    
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
    </style>
</head>
<body bgcolor="#ffffff">
    <form id="loginform" runat="server">
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
                                <b><asp:Literal ID="ltlPersonalizeHomepage" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, PersonalizeHomepage%>" />b></td>
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
                <td valign="top">
                    <b><asp:Literal ID="ltlCareAboutNote" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, CareAboutNote%>" /></b>
                    <td valign="top">
            &nbsp;
        </table>
        <table width="94%" align="center" cellpadding="5" cellspacing="1">
            <tr>
                <td width="75%" valign="top">
                    <p>
                        <font size="-1"><asp:Literal ID="ltlWhatPersonalizeHomePageDo" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, WhatPersonalizeHomePageDo%>" />
                    </p>
                    <ul>
                        <li><asp:Literal ID="ltlPreviewLatestNote" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, PreviewLatestNote%>" /> </li>
                        <li style="padding-top: 5px;"><asp:Literal ID="ltlHeadlineNote" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, HeadlineNote%>" /> </li>
                        <li style="padding-top: 5px;"><asp:Literal ID="ltlFeedNote2" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, FeedNote2 %>" /> </li>
                        <li style="padding-top: 5px;"><asp:Literal ID="ltlFeedNote" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, FeedNote%>" /> </li>
                        <li style="padding-top: 5px;"><asp:Literal ID="ltlDNDNote" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, DNDNote%>" />
                        </li>
                    </ul>
                </td>
                <td valign="top">
                    <!-- LOGIN BOX -->
                    <table cellspacing="3" cellpadding="5" width="100%" class="login" bgcolor="#DDE8CC" style="border:1px solid #C3D9FF;">
                        <tr>
                            <td>
                                <table cellspacing="5" cellpadding="1" border="0" align="center" id="gaia_table">
                                    <tbody>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <font size="-1"><asp:Literal ID="ltlSignInNote" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, SignInNote%>" /></font>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" nowrap="nowrap">
                                                <asp:Label ID="InvalidLoginLabel" runat="server" EnableViewState="False" Font-Bold="True"
                                                    ForeColor="Red" Text="<%$Resources:SharedResources, InvalidCredentialMessage%>" Visible="False"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td nowrap="">
                                                <div align="right">
                                                    <span><asp:Literal ID="ltlEmail" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Email%>" /> </span>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Email" runat="Server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <span><asp:Literal ID="ltlPassword" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Password%>" /> </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Password" runat="server" TextMode="Password" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right">
                                                <input type="checkbox" checked="" value="yes" name="PersistentCookie" id="RememberMeCheckbox" runat="server" />
                                                <input type="hidden" value="1" name="rmShown" />
                                            </td>
                                            <td>
                                                <span class="gaia le rem"><asp:Literal ID="ltlRememberMe" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, RememberMe%>" /> </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td align="left">
                                                <asp:Button ID="LoginButton" runat="server" Text="<%$Resources:SharedResources, Login%>" OnClick="LoginButton_Click" /> <asp:Literal ID="ltlOr" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Or%>" />
                                                <asp:Button ID="RegisterButton" runat="Server" Text="<%$Resources:SharedResources, Register%>" OnClick="RegisterButton_Click"  />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <font size="-1"><asp:Literal ID="ltlForgotPassword" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ForgotPassword%>" /></font>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="">
                                                <div align="right">
                                                    <span><asp:Literal ID="ltlEmail2" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Email%>" /> </span>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ForgotEmail" runat="Server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td align="left">
                                                <asp:Button ID="btnSendPassword" runat="server" Text="<%$Resources:SharedResources, SendPassword%>" OnClick="SendPasswordButton_Click" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <!-- END LOGIN BOX -->
            </tr>
        </table>
    </form>
</body>
</html>