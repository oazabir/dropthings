<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="Confirm.aspx.cs" Inherits="ManageAccountPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="ltlMyAccount" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, MyAccount%>" /></title>
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
                                <b><asp:Literal ID="ltlAccountActivation" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, AccountActivation%>" /></b></td>
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
                    <b><asp:Literal ID="ltlActivationMessage" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ActivationMessage%>" /></b></td>
                    <td valign="top">&nbsp;</td>
            </tr>            
        </table>        
    </form>
</body>
