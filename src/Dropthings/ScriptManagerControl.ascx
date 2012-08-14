<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScriptManagerControl.ascx.cs" Inherits="ScriptManagerControl" %>
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" ScriptMode="Release">
    <Services>
        <asp:ServiceReference InlineScript="false" Path="~/API/Proxy.svc/ajax" />
        <asp:ServiceReference InlineScript="false" Path="~/API/Widget.svc/ajax" />
        <asp:ServiceReference InlineScript="false" Path="~/API/Page.svc/ajax" />
    </Services>
    <Scripts>
<%--<asp:ScriptReference Path="http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js" />
        <asp:ScriptReference Path="http://ajax.googleapis.com/ajax/libs/jqueryui/1/jquery-ui.min.js" />
--%>        
        <asp:ScriptReference Path="/Scripts/jquery-1.7.2.min.js" />
        <asp:ScriptReference Path="/Scripts/jquery-ui-1.8.22.custom.min.js" />
        <asp:ScriptReference Path="/Scripts/jquery.micro_template.js" />
        <asp:ScriptReference Path="/Scripts/tabscroll.js" />
        <asp:ScriptReference Path="/Scripts/Myframework.js" />
        <asp:ScriptReference Path="/Scripts/Ensure.js" />        
    </Scripts>    
</asp:ScriptManager>
        