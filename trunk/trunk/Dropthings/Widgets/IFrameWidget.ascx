<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IFrameWidget.ascx.cs" Inherits="Widgets_IFrameWidget" %>
<asp:Panel ID="SettingsPanel" runat="server" Visible="false">
URL: <asp:TextBox ID="UrlTextBox" runat="server" /><br />
Width: <asp:TextBox ID="WidthTextBox" runat="server" /><br />
Height: <asp:TextBox ID="HeightTextBox" runat="server" /><br />
Scrollbar: <asp:CheckBox ID="ScrollCheckBox" runat="server" Checked="false" />
<asp:Button ID="SaveSettings" runat="server" OnClick="SaveSettings_Clicked" Text="Save" />
</asp:Panel>

<iframe src="<%= Url %>" width="<%= Width %>" height="<%= Height %>" frameborder="0" scrolling="<%=Scrollbar ? "yes":"no"%>" allowtransparency="true" >
Sorry your browser does not support IFRAME
</iframe>