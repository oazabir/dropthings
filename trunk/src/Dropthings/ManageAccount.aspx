<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="ManageAccount.aspx.cs" Inherits="ManageAccountPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Account</title>
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
                                <b><asp:Literal ID="ltlAccountSetting" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, AccountSetting%>" /></b></td>
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
                    <b><asp:Literal ID="ltlCareAboutNote" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, CareAboutNote%>" /></b></td>
                    <td valign="top">&nbsp;</td>
            </tr>
            
        </table>
        <table width="94%" align="center" cellpadding="5" cellspacing="1">
            <tr>
              <td colspan="2" style="white-space:nowrap;">
                <strong><asp:Label ID="Message" runat="server" EnableViewState="false" Font-Bold="true"></asp:Label></strong>
              </td>
            </tr>
            <tr>
              <td style="white-space:nowrap;">
                <strong><font color="#333333"><asp:Literal ID="ltlEditPersonalInformation" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, EditPersonalInformation%>" /></font></strong>
              </td>
              <td width="100%"><hr /></td>
            </tr>
            <tr>
              <td width="20%" nowrap="nowrap" valign="top">
                  <font size="-1"><asp:Literal ID="ltlEmail" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Email%>" />:</font>
              </td>
              <td>
                  <asp:TextBox ID="EmailTextbox" runat="server"></asp:TextBox>
              </td>
            </tr>
            <tr>
                <td width="20%"></td>
                <td style="text-align:left;">
                    <asp:Button ID="SaveButton" runat="server" Text="<%$Resources:SharedResources, Save%>" EnableViewState="false" OnClick="SaveButton_Click" />&nbsp;
                </td>                
            </tr>
            <tr>
              <td style="white-space:nowrap;">
                <strong><font color="#333333"><asp:Literal ID="ltlChangePassword" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ChangePassword%>" /></font></strong>
              </td>
              <td width="100%"><hr /></td>
            </tr>
            <tr>
              <td width="20%" nowrap="nowrap" valign="top">
                  <font size="-1"><asp:Literal ID="ltlOldPassword" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, OldPassword%>" /></font>
              </td>
              <td>
                  <asp:TextBox ID="OldPasswordTextbox" runat="server" TextMode="Password"></asp:TextBox>
              </td>
             </tr>
             <tr>
              <td width="20%" nowrap="nowrap" valign="top">
                  <font size="-1"><asp:Literal ID="ltlNewPassword" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, NewPassword%>" /></font>
              </td>
              <td>
                  <asp:TextBox ID="NewPasswordTextbox" runat="server" TextMode="Password"></asp:TextBox>
              </td>
            </tr>
            <tr>
                <td width="20%"></td>
                <td style="text-align:left;">
                   <asp:Button ID="ChangePasswordButton" runat="server" Text="<%$Resources:SharedResources, Change%>" EnableViewState="false" OnClick="ChangePasswordButton_Click" />&nbsp;
                </td>                
            </tr>
        </table>
    </form>
</body>
