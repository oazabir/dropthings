<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>

<%@ Register Src="~/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>
<body bgcolor="#ffffff">
    <form id="loginform" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" ScriptMode="Release">
        </asp:ScriptManager>
        
        <script type="text/javascript" src="Scripts/MyFramework.js"></script>
        
        <div id="altpage">
            <div id="altpageWrapper">
                <div id="altpageContent">
                
                <div id="altpageHeading1">
                    <asp:Literal ID="ltlAccountSetting" EnableViewState="false" runat="server" 
                    Text="<%$Resources:SharedResources, PersonalizeHomepage%>" />
                </div>

                <div id="altpageHeading2">
                    <asp:Literal ID="Literal1" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, CareAboutNote%>" />
                </div>
                
                <div id="altpageTable">

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
                    
                    </div>
                </div>
            </div>
        </div>

        <uc2:Footer ID="Footer1" runat="server"></uc2:Footer>

    </form>
</body>
</html>