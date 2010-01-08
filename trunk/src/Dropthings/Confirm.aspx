<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="Confirm.aspx.cs" Inherits="ManageAccountPage" %>

<%@ Register Src="~/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="ltlMyAccount" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, MyAccount%>" /></title>
</head>
<body bgcolor="#ffffff">
    <form id="loginform" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" ScriptMode="Release">
        </asp:ScriptManager>
        
        <script type="text/javascript" src="Scripts/MyFramework.js"></script>
        
        <uc1:Header ID="Header1" runat="server" />

        <div id="altpage">
            <div id="altpageWrapper">
                <div id="altpageContent">

                    <div id="altpageHeading1">
                        <asp:Literal ID="ltlAccountSetting" EnableViewState="false" runat="server" 
                        Text="<%$Resources:SharedResources, AccountActivation%>" />
                    </div>

                    <div id="altpageHeading2">
                        <asp:Literal ID="ltlCareAboutNote" EnableViewState="false" runat="server" 
                        Text="<%$Resources:SharedResources, ActivationMessage%>" />
                    </div>

                </div>
            </div>
        </div>

        <uc2:Footer ID="Footer1" runat="server"></uc2:Footer>

    </form>
</body>
