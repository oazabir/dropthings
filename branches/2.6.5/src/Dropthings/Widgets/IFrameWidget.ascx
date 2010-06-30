<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IFrameWidget.ascx.cs" Inherits="Widgets_IFrameWidget" %>
<asp:Panel ID="SettingsPanel" runat="server" Visible="false">
<asp:Literal ID="ltlURL" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, URL%>" />: <asp:TextBox ID="UrlTextBox" runat="server" /><br />
<asp:Literal ID="ltlWidth" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Width%>" />: <asp:TextBox ID="WidthTextBox" runat="server" /><br />
<asp:Literal ID="ltlHeight" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Height%>" />: <asp:TextBox ID="HeightTextBox" runat="server" /><br />
<asp:Literal ID="ltlScrollbar" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Scrollbar%>" />: <asp:CheckBox ID="ScrollCheckBox" runat="server" Checked="false" />
<asp:Button ID="SaveSettings" runat="server" OnClick="SaveSettings_Clicked" Text="<%$Resources:SharedResources, Save%>" /> />
</asp:Panel>

<iframe src="<%= Url %>" width="<%= Width %>" height="<%= Height %>" frameborder="0" scrolling="<%=Scrollbar ? "yes":"no"%>" allowtransparency="true" >
<asp:Literal ID="ltlIFrameNotSupportedMessage" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, IFrameNotSupportedMessage%>" />
</iframe>