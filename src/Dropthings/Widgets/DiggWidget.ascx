<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiggWidget.ascx.cs" Inherits="Widgets_DiggWidget" %>
    
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<asp:Panel ID="SettingsPanel" runat="server" Visible="false">

</asp:Panel>
<div style="height:100%"><div style="height:450px; position:relative;">
    <asp:Silverlight ID="diggXaml" runat="server" Source="~/ClientBin/Dropthing.Silverlight.xap?v=1" MinimumVersion="2.0.30923.0" Width="100%" Height="100%">
        <PluginNotInstalledTemplate>
            <asp:Literal ID="ltlWorking" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, SilverlightPluginError%>" />
        </PluginNotInstalledTemplate>
    </asp:Silverlight></div>
</div>
