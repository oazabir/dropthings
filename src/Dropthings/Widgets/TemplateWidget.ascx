<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TemplateWidget.ascx.cs" Inherits="Widgets_TemplateWidget" %>
<asp:Panel ID="SettingsPanel" runat="server" Visible="false">
<asp:Label ID="Nothing" Text="<%$Resources:SharedResources, NoSetting%>" runat="server" />
</asp:Panel>

<asp:Panel ID="Body" runat="server" Style="text-align:center">
<asp:HyperLink ID="BookLink" runat="server" NavigateUrl="http://www.oreilly.com/catalog/9780596510503/">
<asp:Image ID="BookImage" runat="server" ImageUrl="http://www.oreilly.com/catalog/covers/9780596510503_cat.gif" Width="120" Height="140" />
</asp:HyperLink>
<br />
<asp:Label ID="Caption" runat="server"><asp:Literal ID="ltlPortalBookInformation" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, PortalBookInformation%>" /></asp:Label>
<br />
<asp:HyperLink ID="AmazonLink" runat="server" NavigateUrl="http://www.amazon.com/Building-Web-2-0-Portal-ASP-NET/dp/0596510500" Text="Buy it on Amazon" />
</asp:Panel>