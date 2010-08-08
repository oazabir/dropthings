<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Setup_Default" %>

<%@ OutputCache NoStore="true" Location="None" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dropthings Setup Page</title>
</head>
<style type="text/css">   
    
    .inprogress, .pass, .fail, .warning
    {
        padding-left: 35px;
        display: block;
        min-height: 35px;
        font-size: 120%;
        vertical-align: middle;
    }
    
    .inprogress
    {
        background: transparent url("ajax-loader.gif") no-repeat;        
    }
    
    .pass 
    {
        background: transparent url("tick.png") no-repeat;        
    }
    
    .fail
    {
        background: transparent url("cross.png") no-repeat;        
    }
    
    .warning 
    {
        background: transparent url("warning.png") no-repeat;        
    }
    
    .error
    {
        color: Red;
    }
    
    .suggestion
    {
        color: darkred;
        font-style: italic;
        font-size: 120%;
    }
    
    ul
    {        
        list-style: none;
    }
</style>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
        ScriptMode="Release">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Timer runat="server" ID="RefreshTimer" Interval="500" Enabled="true" OnTick="RefreshTimer_Tick" />
            <h1>
                Setup</h1>
            <h2>
                Database Configuration</h2>
            <ul>
                <li>
                    <asp:Label CssClass="inprogress" Text="Can read and write to database using the connection string."
                        runat="server" ID="ConnectionStringStatusLabel" /></li>
            </ul>
            <h2>
                ASP.NET Configuration</h2>
            <ul>
                <li>
                    <asp:Label CssClass="inprogress" ID="MembershipLabel" Text="Membership, Profile and Role Manager configuration is correct."
                        runat="server" /></li>
                <li>
                    <asp:Label CssClass="inprogress" ID="AppDataLabel" Text="Write permission on App_Data folder."
                        runat="server" /></li>
            </ul>
            <h2>
                Web.config appSettings</h2>
            <ul>
                <li>
                    <asp:Label CssClass="inprogress" Text="Developer Mode should be turned off in production."
                        ID="DeveloperModeLabel" runat="server" /></li>
                <li>
                    <asp:Label CssClass="inprogress" Text="WebRoot matches with the address of this site."
                        ID="WebRootLabel" runat="server" /></li>
                <li>
                    <asp:Label CssClass="inprogress" Text="CssPrefix is either empty or is reachable."
                        ID="CssPrefixLabel" runat="server" /></li>
                <li>
                    <asp:Label CssClass="inprogress" Text="JsPrefix is either empty or is reachable."
                        ID="JSPrefixLabel" runat="server" /></li>
                <li>
                    <asp:Label CssClass="inprogress" Text="ImgPrefix is either empty or is reachable."
                        ID="ImgPrefixLabel" runat="server" /></li>
                <li>
                    <asp:Label CssClass="inprogress" Text="DisableDOSCheck is false on production." ID="DisableDOSCheckLabel"
                        runat="server" /></li>
                <li>
                    <asp:Label CssClass="inprogress" Text="DisableCache is false on production." ID="DisableCacheLabel"
                        runat="server" /></li>
            </ul>
            <h2>
                User templates exist</h2>
            <ul>
                <li>
                    <asp:Label CssClass="inprogress" Text="Template user for anonymous visit exists and has pages."
                        ID="AnonUserLabel" runat="server" /></li>
                <li>
                    <asp:Label CssClass="inprogress" Text="Template user for registered users exists and has pages."
                        ID="RegisteredUserLabel" runat="server" /></li>
            </ul>
            <h2>
                Mail Delivery</h2>
            <ul>
                <li>
                    <asp:Label CssClass="inprogress" Text="SMTP server responded to EHLO" runat="server"
                        ID="SMTPLabel" /></li>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>
    <h1>
        Conclusion</h1>
    <h2>
        Is everything alright?right?</h2>
    <asp:Button Text="Yes, complete setup" ID="YesButton" runat="server" OnClick="YesButton_Clicked" />
    <asp:Button Text="No, let me fix" ID="NoButton" runat="server" OnClick="NoButton_Clicked" />
    </form>
</body>
</html>
