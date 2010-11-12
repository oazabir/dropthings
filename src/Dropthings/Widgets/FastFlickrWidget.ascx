<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FastFlickrWidget.ascx.cs" Inherits="Widgets_FastFlickrWidget" EnableViewState="false" %>
<asp:Panel ID="settingsPanel" runat="server" Visible="False">
    <asp:RadioButton ID="mostInterestingRadioButton" runat="server" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="<%$Resources:SharedResources, MostInteresting%>" />
        <br />
    <asp:RadioButton ID="mostRecentRadioButton" runat="server" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="<%$Resources:SharedResources, MostRecent%>" />
        <br />
    <asp:RadioButton ID="customTagRadioButton" runat="server" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="<%$Resources:SharedResources, Tag%>" />
        <asp:TextBox ID="CustomTagTextBox" runat="server" Text="<%$Resources:SharedResources, Pretty%>" />
        <asp:Button ID="ShowTagButton" runat="server" Text="<%$Resources:SharedResources, Show%>" OnClick="ShowTagButton_Clicked" />
        <hr />
</asp:Panel>
    
<asp:Panel ID="FlickrPhotoPanel" runat="server">

</asp:Panel>

<div style="overflow: auto; width:100%; white-space:nowrap; font-weight: bold; margin-top: 10px">
<asp:LinkButton ID="ShowPrevious" runat="server" style="float:left" Text="<%$Resources:SharedResources, PrevPhotos%>"></asp:LinkButton><asp:LinkButton ID="ShowNext" runat="server"  style="float:right" Text="<%$Resources:SharedResources, NextPhotos%>"></asp:LinkButton>
</div>

