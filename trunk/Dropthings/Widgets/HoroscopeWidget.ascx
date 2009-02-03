<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HoroscopeWidget.ascx.cs" Inherits="Widgets_HoroscopeWidget" %>
<asp:MultiView ID="Multiview" runat="server" ActiveViewIndex="0">

<asp:View runat="server" ID="ProgressView">
    <asp:image runat="server" ID="image1" ImageAlign="middle" ImageUrl="~/indicator.gif" />
    <asp:Label runat="Server" ID="label1" Text="Loading..." Font-Size="smaller" ForeColor="DimGray" />
</asp:View>

<asp:View runat="server" ID="ContentView">

<table>
<tr><td>Select Horoscope: 
    <asp:DropDownList ID="ddlHoroscope" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlHoroscope_SelectedIndexChanged">
        <asp:ListItem Value="0">Aries</asp:ListItem>
        <asp:ListItem Value="1">Gemini</asp:ListItem>
        <asp:ListItem Value="2">Taurus</asp:ListItem>
        <asp:ListItem Value="3">Cancer</asp:ListItem>
        <asp:ListItem Value="4">Leo</asp:ListItem>
        <asp:ListItem Value="5">Virgo</asp:ListItem>
        <asp:ListItem Value="6">Libra</asp:ListItem>
        <asp:ListItem Value="7">Scorpio</asp:ListItem>
        <asp:ListItem Value="8">Sagittarius</asp:ListItem>
        <asp:ListItem Value="9">Capricorn</asp:ListItem>
        <asp:ListItem Value="10">Aquarius</asp:ListItem>
        <asp:ListItem Value="11">Pisces</asp:ListItem>
    </asp:DropDownList></td></tr>
<tr><td valign="middle">
    <asp:Label ID="lblHoroscope" runat="server" Text="" EnableViewState="false"></asp:Label></td></tr>
</table>

</asp:View>

</asp:MultiView>

<asp:Timer ID="MultiviewTimer" Interval="100" OnTick="LoadContentView" runat="server" /> 