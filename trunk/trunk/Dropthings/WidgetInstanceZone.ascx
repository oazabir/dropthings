<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WidgetInstanceZone.ascx.cs" Inherits="WidgetInstanceZone" %>
<div class="widget_zone_container">
    <asp:Panel ID="WidgetPanel" runat="server" >
        <asp:UpdatePanel ID="WidgetZoneUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton ID="WidgetHolderPanelTrigger" runat="server" OnClick="WidgetHolderPanelTrigger_Click" />
                <asp:Panel ID="WidgetHolderPanel" runat="server" >
        
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>