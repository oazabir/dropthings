<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HtmlWidget.ascx.cs" Inherits="Widgets_HtmlWidget" EnableViewState="false" %>

<asp:Panel ID="SettingsPanel" runat="server" Visible="false">
HTML: <br />
<asp:TextBox ID="HtmltextBox" runat="server" Width="100%" Height="200" MaxLength="5000" TextMode="MultiLine" EnableViewState="false" />
<asp:Button ID="SaveSettings" runat="server" OnClick="SaveSettings_Clicked" Text="<%$Resources:SharedResources, Save%>" style="visibility: hidden" />
<asp:TextBox ID="HtmlContent" runat="server" style="display:none" />
<input id="SaveButton" runat="server" type="button" value="<%$Resources:SharedResources, Save%>" onclick='var html = jQuery("#{0}").htmlarea("toHtmlString"); jQuery("#{0}").htmlarea("dispose"); jQuery("#{0}").remove(); jQuery("#{2}").val(escape(html)); {1}.click()' />
<hr />
</asp:Panel>

<asp:Literal ID="Output" runat="server" EnableViewState="false" />