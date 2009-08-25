<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HoroscopeWidget.ascx.cs" Inherits="Widgets_HoroscopeWidget" %>
<asp:MultiView ID="Multiview" runat="server" ActiveViewIndex="0">

<asp:View runat="server" ID="ProgressView">
    <asp:image runat="server" ID="image1" ImageAlign="middle" ImageUrl="~/indicator.gif" />
    <asp:Label runat="Server" ID="label1" Text="<%$Resources:SharedResources, Loading%>" Font-Size="smaller" ForeColor="DimGray" />
</asp:View>

<asp:View runat="server" ID="ContentView">

<table>
<tr><td><asp:Literal ID="ltlSelectHoroscope" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, SelectHoroscope%>" />
    <asp:DropDownList ID="ddlHoroscope" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlHoroscope_SelectedIndexChanged">
        <asp:ListItem Value="0" Text="<%$Resources:SharedResources, Aries%>"></asp:ListItem>
        <asp:ListItem Value="1" Text="<%$Resources:SharedResources, Gemini%>"></asp:ListItem>
        <asp:ListItem Value="2" Text="<%$Resources:SharedResources, Taurus%>"></asp:ListItem>
        <asp:ListItem Value="3" Text="<%$Resources:SharedResources, Cancer%>"></asp:ListItem>
        <asp:ListItem Value="4" Text="<%$Resources:SharedResources, Leo%>"></asp:ListItem>
        <asp:ListItem Value="5" Text="<%$Resources:SharedResources, Virgo%>"></asp:ListItem>
        <asp:ListItem Value="6" Text="<%$Resources:SharedResources, Libra%>"></asp:ListItem>
        <asp:ListItem Value="7" Text="<%$Resources:SharedResources, Scorpio%>"></asp:ListItem>
        <asp:ListItem Value="8" Text="<%$Resources:SharedResources, Sagittarius%>"></asp:ListItem>
        <asp:ListItem Value="9" Text="<%$Resources:SharedResources, Capricorn%>"></asp:ListItem>
        <asp:ListItem Value="10" Text="<%$Resources:SharedResources, Aquarius%>"></asp:ListItem>
        <asp:ListItem Value="11" Text="<%$Resources:SharedResources, Pisces%>"></asp:ListItem>
    </asp:DropDownList></td></tr>
<tr><td valign="middle">
    <asp:Label ID="lblHoroscope" runat="server" Text="" EnableViewState="false"></asp:Label></td></tr>
</table>

</asp:View>

</asp:MultiView>

<asp:Timer ID="MultiviewTimer" Interval="1000" OnTick="LoadContentView" runat="server" /> 