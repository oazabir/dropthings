<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RSSWidget.ascx.cs" Inherits="Widgets_RSSWidget" EnableViewState="false" %>
<asp:Panel ID="SettingsPanel" runat="Server" Visible="False" >
<asp:Literal ID="ltlURL" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, URL%>" />: <asp:TextBox ID="FeedUrl" Text="" runat="server" MaxLength="2000" Columns="40" /><br />
<asp:Literal ID="Literal1" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Show%>" />
<asp:DropDownList ID="FeedCountDropDownList" runat="Server">
<asp:ListItem>1</asp:ListItem>
<asp:ListItem>2</asp:ListItem>
<asp:ListItem>3</asp:ListItem>
<asp:ListItem>4</asp:ListItem>
<asp:ListItem>5</asp:ListItem>
<asp:ListItem>6</asp:ListItem>
<asp:ListItem>7</asp:ListItem>
<asp:ListItem>8</asp:ListItem>
<asp:ListItem>9</asp:ListItem>
<asp:ListItem>10</asp:ListItem>
<asp:ListItem>11</asp:ListItem>
<asp:ListItem>12</asp:ListItem>
<asp:ListItem>13</asp:ListItem>
<asp:ListItem>14</asp:ListItem>
<asp:ListItem>15</asp:ListItem>
<asp:ListItem>16</asp:ListItem>
<asp:ListItem>17</asp:ListItem>
<asp:ListItem>18</asp:ListItem>
<asp:ListItem>19</asp:ListItem>
<asp:ListItem>20</asp:ListItem>
</asp:DropDownList>
<asp:Literal ID="ltlWorking" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Items%>" />
<asp:Button ID="SaveSettings" runat="Server" OnClick="SaveSettings_Click" Text="<%$Resources:SharedResources, Save%>" />
</asp:Panel>

<asp:MultiView ID="RSSMultiview" runat="server" ActiveViewIndex="0">

<asp:View runat="server" ID="RSSProgressView">
    
    <asp:Label runat="Server" ID="label1" Text="<%$Resources:SharedResources, Loading%>" Font-Size="smaller" ForeColor="DimGray" />
</asp:View>

<asp:View runat="server" ID="RSSFeedView">

    <asp:DataList ID="FeedList" runat="Server" EnableViewState="False">
    <ItemTemplate>
    <asp:HyperLink ID="FeedLink" runat="server" Target="_blank" CssClass="feed_item_link" NavigateUrl='<%# Eval("link") %>' ToolTip='<%# Eval("description") %>'>
    <%# Eval("title") %>
    </asp:HyperLink>
    </ItemTemplate>
    </asp:DataList>

</asp:View>

</asp:MultiView>

<asp:Timer ID="RSSWidgetTimer" Interval="100" OnTick="LoadRSSView" runat="server" /> 