<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FastFlickrWidget.ascx.cs" Inherits="Widgets_FastFlickrWidget" EnableViewState="false" %>
<asp:Panel ID="settingsPanel" runat="server" Visible="False">
    <asp:RadioButton ID="mostInterestingRadioButton" runat="server" AutoPostBack="True"
        Checked="True" GroupName="FlickrPhoto" OnCheckedChanged="photoTypeRadio_CheckedChanged"
        Text="Most Interesting" />
        <br />
    <asp:RadioButton ID="mostRecentRadioButton" runat="server" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="Most Recent" />
        <br />
    <asp:RadioButton ID="customTagRadioButton" runat="server" AutoPostBack="True" GroupName="FlickrPhoto"
        OnCheckedChanged="photoTypeRadio_CheckedChanged" Text="Tag: " />
        <asp:TextBox ID="CustomTagTextBox" runat="server" Text="Pretty" />
        <asp:Button ID="ShowTagButton" runat="server" Text="Show" OnClick="ShowTagButton_Clicked" />
        <hr />
</asp:Panel>
    
<asp:Panel ID="FlickrPhotoPanel" runat="server">

</asp:Panel>

<div style="overflow: auto; width:100%; white-space:nowrap; font-weight: bold; margin-top: 10px">
<asp:LinkButton ID="ShowPrevious" runat="server" style="float:left">< Prev Photos</asp:LinkButton><asp:LinkButton ID="ShowNext" runat="server"  style="float:right">Next Photos ></asp:LinkButton>
</div>

