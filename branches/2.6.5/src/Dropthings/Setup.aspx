<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="Setup.aspx.cs" Inherits="Setup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Literal ID="ltlSetupCompleteMessage" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, SetupCompleteMessage%>" /> <a href="Default.aspx">Default.aspx</a>
    </div>
    </form>
</body>
</html>
