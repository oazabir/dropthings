<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HtmlWidget.ascx.cs" Inherits="Widgets_HtmlWidget" %>
<asp:Panel ID="SettingsPanel" runat="server" Visible="false">
HTML: <br />
<asp:TextBox ID="HtmltextBox" runat="server" Width="300" Height="200" MaxLength="2000" TextMode="MultiLine" />
<asp:Button ID="SaveSettings" runat="server" OnClick="SaveSettings_Clicked" Text="<%$Resources:SharedResources, Save%>" />
</asp:Panel>
<asp:Literal ID="Output" runat="server" />