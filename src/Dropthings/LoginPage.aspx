<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>
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
                                <b>Personalize Your Homepage</b></td>
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
                    <b>See information you care about on your homepage.</b>
                    <td valign="top">
            &nbsp;
        </table>
        <table width="94%" align="center" cellpadding="5" cellspacing="1">
            <tr>
                <td width="75%" valign="top">
                    <p>
                        <font size="-1">Your personalized homepage brings together content from across the
                        web, on a single page:
                    </p>
                    <ul>
                        <li>Preview latest <b>messages</b> </li>
                        <li style="padding-top: 5px;">See headlines from top news sources </li>
                        <li style="padding-top: 5px;">Get <b>weather</b> forecasts, <b>stock</b> quotes, and
                            <b>movie</b> showtimes </li>
                        <li style="padding-top: 5px;">Select from a variety of popular <b>feeds</b> </li>
                        <li style="padding-top: 5px;"><b>Drag and drop</b> the sections to rearrange the page
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
                                                <font size="-1">Sign in to Personalized Homepage with your account</font>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" nowrap="nowrap">
                                                <asp:Label ID="InvalidLoginLabel" runat="server" EnableViewState="False" Font-Bold="True"
                                                    ForeColor="Red" Text="Invalid Email or Password. Did you register before?" Visible="False"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td nowrap="">
                                                <div align="right">
                                                    <span>Email: </span>
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
                                                <span>Password: </span>
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
                                                <span class="gaia le rem">Remember me on this computer. </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td align="left">
                                                <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" /> or
                                                <asp:Button ID="RegisterButton" runat="Server" Text="Register" OnClick="RegisterButton_Click"  />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <font size="-1">Forgot Password?</font>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="">
                                                <div align="right">
                                                    <span>Email: </span>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ForgotEmail" runat="Server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td align="left">
                                                <asp:Button ID="btnSendPassword" runat="server" Text="Send Password" OnClick="SendPasswordButton_Click" />
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