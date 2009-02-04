<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WidgetPanels.ascx.cs" Inherits="WidgetPanels" %>
<%@ Register Src="WidgetContainer.ascx" TagName="WidgetContainer" TagPrefix="widget" %>
<div id="widgetMaxBackground" class="widget_max_holder" style="display:none;">&nbsp;</div>
<table id="WidgetContainer" width="98%" cellspacing="5" align="left" class="table_fixed">
    <tbody>
        <tr>
            <td style="width:<%=LeftPanelSize%>;vertical-align:top" >
                <asp:UpdatePanel ID="LeftUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:Panel ID="LeftPanel" runat="server" class="widget_holder" columnNo="0">
                </asp:Panel>
                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="visibility:<%=MiddlePanelVisible%>" class="column_divider">&nbsp;</td>
            <td style="width:<%=MiddlePanelSize%>;vertical-align:top;visibility:<%=MiddlePanelVisible%>" >
                <asp:UpdatePanel ID="MiddleUpdatePanel" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                <asp:Panel ID="MiddlePanel" runat="server" class="widget_holder" columnNo="1">
                </asp:Panel>
                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="visibility:<%=RightPanelVisible%>">&nbsp;</td>
            <td style="width:<%=RightPanelSize%>;vertical-align:top;visibility:<%=RightPanelVisible%>" >

            <asp:UpdatePanel ID="RightUpdatePanel" runat="server" UpdateMode="Conditional" >
                <ContentTemplate>
                <asp:Panel ID="RightPanel" runat="server" class="widget_holder" columnNo="2" >
                </asp:Panel>
                     
                </ContentTemplate>
                </asp:UpdatePanel>
                </td>
        </tr>
    </tbody>
</table>   

