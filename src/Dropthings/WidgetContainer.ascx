<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WidgetContainer.ascx.cs" Inherits="WidgetContainer" %>
<asp:Panel ID="Widget" CssClass="widget" runat="server" onmouseover="this.className='widget widget_hover'" onmouseout="this.className='widget'">
    <asp:Panel id="WidgetHeader" CssClass="widget_header" runat="server">
        <asp:UpdatePanel ID="WidgetHeaderUpdatePanel" runat="server" UpdateMode="Conditional" >
            <ContentTemplate><table class="widget_header_table" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td class="widget_title"><img id="LockedWidget" alt="" runat="server" src="~/App_Themes/GreenBlue/WidgetIcons/locked.png" visible="false" /><asp:LinkButton ID="WidgetTitle" OnClick="WidgetTitle_Click" runat="Server" Text="Widget Title" CssClass="widget_title_label" /><asp:TextBox ID="WidgetTitleTextBox" runat="Server" CssClass="widget_title_input" /><asp:Button ID="SaveWidgetTitle" OnClick="SaveWidgetTitle_Click" runat="Server" CssClass="widget_title_submit" Text="OK" /></td>
                    <td class="widget_edit"><asp:LinkButton ID="EditWidget" runat="Server" Text="edit" OnClick="EditWidget_Click" CssClass="widget_edit" /><asp:LinkButton ID="CancelEditWidget" runat="Server" Visible="false" Text="close edit" OnClick="EditWidget_Click" CssClass="widget_edit_close" /></td>
                    <td class="widget_button"><asp:LinkButton ID="MaximizeWidget" runat="Server" Text="" CssClass="widget_max widget_box" /><asp:LinkButton ID="RestoreWidget" runat="Server" Text="" CssClass="widget_restore widget_box" /></td>
                    <td class="widget_button"><asp:LinkButton ID="CollapseWidget" OnClick="CollapseWidget_Click" runat="Server" Text="" CssClass="widget_min widget_box" /><asp:LinkButton ID="ExpandWidget" OnClick="ExpandWidget_Click" runat="Server" Text="" CssClass="widget_expand widget_box" /></td>
                    <td class="widget_button"><asp:LinkButton ID="CloseWidget" runat="Server" Text="" CssClass="widget_close widget_box" OnClick="CloseWidget_Click" /></td>
                </tr>
            </tbody>
            </table>
            </ContentTemplate>        
        </asp:UpdatePanel>    
    </asp:Panel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10" AssociatedUpdatePanelID="WidgetBodyUpdatePanel" >
    <ProgressTemplate><center>Working...</center></ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="WidgetBodyUpdatePanel" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
        <asp:Panel ID="WidgetResizeFrame" CssClass="widget_resize_frame" runat="server">
            <asp:Panel ID="WidgetBodyPanel" runat="Server" CssClass="widget_body"></asp:Panel>
        </asp:Panel>    
        </ContentTemplate>        
    </asp:UpdatePanel>  
</asp:Panel>