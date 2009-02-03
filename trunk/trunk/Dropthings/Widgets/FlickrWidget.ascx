<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FlickrWidget.ascx.cs" Inherits="FlickrWidget" EnableViewState="True" %>
<style type="text/css">
div.previews { position: relative; }
div.preview { margin: 0 8px 8px 0; width:75px; height:75px; }
img.preview { position: absolute; z-index: 0; width: 75px; height: 75px; }
</style>
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
        <asp:TextBox ID="CustomTagTextBox" runat="server" Text="Pretty" /><asp:Button ID="ShowTagButton" runat="server" Text="Show" OnClick="ShowTagButton_Clicked" />
        <hr />
</asp:Panel>
<asp:MultiView ID="FlickrWidgetMultiview" runat="server" ActiveViewIndex="0">
<asp:View runat="server" ID="FlickrWidgetProgressView">
    <asp:Label runat="Server" ID="label1" Text="Loading..." Font-Size="smaller" ForeColor="DimGray" />
</asp:View>
<asp:View runat="server" ID="FlickrWidgetPhotoView">
    <asp:Panel ID="photoPanel" runat="server" CssClass="previews">

    </asp:Panel>
    <div style="overflow: auto; width:100%; white-space:nowrap; font-weight: bold; margin-top: 10px">
        <asp:LinkButton ID="ShowPrevious" runat="server" OnClick="ShowPrevious_Click" style="float:left">< Prev Photos</asp:LinkButton>
        <asp:LinkButton ID="ShowNext" runat="server" OnClick="ShowNext_Click" style="float:right">Next Photos ></asp:LinkButton>
    </div>
</asp:View>
</asp:MultiView>

<asp:Timer ID="FlickrWidgetTimer" Interval="100" OnTick="LoadPhotoView" runat="server" /> 