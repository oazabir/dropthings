<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TwitterWidget.ascx.cs" Inherits="TwitterWidget" %>
<link rel="Stylesheet" id="twitterCSS" href="/Widgets/TwitterWidget/TwitterWidget.css" />
<div id="W<%= WidgetHostID %>_SettingsPanel" class="twSettings" style="display:none">
    <h3 class="twitterHeading"><asp:Literal ID="ltlTwitterCredentials" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, TwitterCredentials%>" /></h3>
    <div id="W<%= WidgetHostID %>_TwLoginProgress" class="twitterProgress" style="display:none"><asp:Literal ID="ltlLoggingIn" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, LoggingIn%>" /></div>
    <table width="100%" border="0">
        <tr>
            <td colspan="2" class="twitterError" id="W<%= WidgetHostID %>_TwError"></td>
        </tr>
        <tr>
            <td><asp:Literal ID="Literal2" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, TwitterUsername%>" /></td>
            <td><input type="text" id="W<%= WidgetHostID %>_TwUsername" class="twCredentialInput" /></td>
        </tr>
        <tr>
            <td><asp:Literal ID="Literal3" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, TwitterPassword%>" /></td>
            <td><input type="password" id="W<%= WidgetHostID %>_TwPassword" class="twCredentialInput" /></td>
        </tr>
        <tr>
            <td></td>
            <td><input type="button" id="W<%= WidgetHostID %>_TwSave" value="Save" />&nbsp;<input type="button" id="W<%= WidgetHostID %>_TwCancel" value="Cancel" /></td>
        </tr>
    </table>
</div>
<div id="W<%= WidgetHostID %>_Content">
    <div id="W<%= WidgetHostID %>_MainView" style="display:none">
        <span id="W<%= WidgetHostID %>_TwUpdateError" class="twitterError" style="display:none"></span>
        <div class="twitterTabs">
            <ul>
                <li><a id="W<%= WidgetHostID %>_TwUpdate" href="javascript:void(0)"><asp:Literal ID="ltlWorking" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Update%>" /></a></li>
                <li><a id="W<%= WidgetHostID %>_TwFriends" href="javascript:void(0)"><asp:Literal ID="Literal1" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Friends%>" /></a></li>
                <li><a id="W<%= WidgetHostID %>_TwArchive" href="javascript:void(0)"><asp:Literal ID="Literal4" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Archive%>" /></a></li>
                <li><a id="W<%= WidgetHostID %>_TwPublic" href="javascript:void(0)"><asp:Literal ID="Literal5" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Public%>" /></a></li>
            </ul>
        </div>
        <div class="twitterView" id="W<%= WidgetHostID %>_TwFeatures" style="display:none">
            <div id="W<%= WidgetHostID %>_TwFeatClose" class="twitterClose">close</div>
            <div class="twFeature"><asp:Literal ID="Literal6" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ViewFriendMessage%>" /></div>
        </div>
        <div id="W<%= WidgetHostID %>_TwView" class="twitterView"></div>
        <div id="W<%= WidgetHostID %>_TwUpdatePanel" class="twitterUpdate" style="display:none">
            <textarea id="W<%= WidgetHostID %>_TwUpdateText" rows="4" cols=""></textarea>
            <input type="button" id="W<%= WidgetHostID %>_TwUpdateBtn" value="Update" />
            <span id="W<%= WidgetHostID %>_TwUpdateLeft" class="twitterCCount">140 characters left</span>
        </div>
    </div>
    <div id="W<%= WidgetHostID %>_Progress" class="twitterProgress">
        <asp:Literal ID="ltlLoadingFromTwitter" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, LoadingFromTwitter%>" />
    </div>
</div>
