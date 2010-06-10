<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Header" EnableViewState="false" %>

<script type="text/javascript">
function doSearch()
{
    var searchQuery = encodeURI(document.getElementById('query').value);
    if( searchQuery.length > 0 ) document.location.href = "http://www.google.com/search?q=" + searchQuery;
    return false;
}
</script>
<div id="header">

    <h1><a href="/">
        <asp:Literal ID="Literal1" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, SiteTitle%>" /> <asp:Literal ID="Literal2" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Slogan%>" />
    </a></h1>

    <div id="login_panel">
        <div id="login_panel_top" class="login_panel_left">
            <div class="login_panel_right">
               <div class="login_panel_centre"></div>
            </div>
        </div>
        <div id="login_panel_middle" class="login_panel_left">
            <div class="login_panel_right">
                <div class="login_panel_centre">
                    <span class="login_panel_label">
                        <asp:Label ID="UserNameLabel" runat="server" EnableViewState="false" Text="" Visible="false"></asp:Label>
                    </span>
                    <span class="login_panel_label">
                        <asp:HyperLink ID="AccountLinkButton" Text="<%$Resources:SharedResources, MyAccount%>" runat="server" NavigateUrl="~/ManageAccount.aspx" />
                    </span>
                    <span class="login_panel_label">
                        <asp:HyperLink ID="LoginLinkButton" Text="<%$Resources:SharedResources, Login%>" runat="server" NavigateUrl="~/LoginPage.aspx" /> | 
                    </span>
                    <span class="login_panel_label">
                        <asp:HyperLink ID="LogoutLinkButton" Text="<%$Resources:SharedResources, Logout%>" runat="server" NavigateUrl="~/Logout.ashx" />
                    </span>
                    <span class="login_panel_label">
                        <asp:HyperLink ID="StartOverButton" Text="<%$Resources:SharedResources, StartOver%>" runat="server" NavigateUrl="~/Logout.ashx" /> | 
                    </span>
                    <span class="login_panel_label">
                        <a id="HelpLink" href="javascript:void(0)" onclick="DropthingsUI.Actions.showHelp()"><asp:Literal ID="ltlHelp" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Help%>" /></a>       
                    </span>
                </div>
            </div>
        </div>
        <div id="login_panel_bottom" class="login_panel_left">
            <div class="login_panel_right">
               <div class="login_panel_centre"></div>
            </div>
        </div>
    </div>    
    
    <div id="search_bar">
        <div id="search_bar_wrapper">
            <div id="google_search">
                <img class="google_logo" src="img/google.jpg" alt="Google" />
                <input id="query" name="ctl00$masterWebpartManager$gwpCustomLogin1$CustomLogin1$UserName" size="40" maxlength="2048" value="" type="text" onkeypress="if( event.keyCode == 13 ) return doSearch(); " />
                <asp:Button ID="SearchButton" runat="server" Text="<%$Resources:SharedResources, Search%>" OnClientClick="return doSearch();" />
            </div>
            
            <!--
            <div id="live_search">
                <img class="livesearch_logo" src="img/LiveSearch.jpg" alt="Live Search" />

                <meta name="Search.WLSearchBox" content="1.1, en-US" />
                <div id="WLSearchBoxDiv">
                    <table cellpadding="0" cellspacing="0" style="width: 322px"><tr id="WLSearchBoxPlaceholder"><td style="width: 100%; border:solid 2px #4B7B9F;border-right-style: none;"><input id="WLSearchBoxInput" type="text" value="&#x4c;&#x6f;&#x61;&#x64;&#x69;&#x6e;&#x67;&#x2e;&#x2e;&#x2e;" disabled="disabled" style="padding:0;background-image: url(http://search.live.com//siteowner/s/siteowner/searchbox_background.png);background-position: right;background-repeat: no-repeat;height: 16px; width: 100%; border:none 0 Transparent" /></td><td style="border:solid 2px #4B7B9F;"><input id="WLSearchBoxButton" type="image" src="http://search.live.com//siteowner/s/siteowner/searchbutton_normal.png" align="absBottom" style="padding:0;border-style: none" /></td></tr></table>
	                    <script type="text/javascript" charset="utf-8">
	                    var WLSearchBoxConfiguration=
	                    {
		                    "global":{
			                    "serverDNS":"search.live.com",
			                    "market":"en-US"
		                    },
		                    "appearance":{
			                    "autoHideTopControl":false,
			                    "width":600,
			                    "height":400,
			                    "theme":"Green"
		                    },
		                    "scopes":[
			                    {
				                    "type":"web",
				                    "caption":"&#x57;&#x65;&#x62;",
				                    "searchParam":""
			                    }
		                    ]
	                    }
	                    </script>
                    
                </div>
            </div>   
            -->
        </div>
        
        
    </div>

    <div id="header_message">
        <div id="header_message_wrapper">
            <asp:Literal ID="ltlSiteDescription" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, SiteDescription%>" />
            <asp:Literal ID="ltlSourceCode" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, SourceCode%>" /> <a href="http://code.google.com/p/dropthings/">code.google.com/p/dropthings</a>
        </div>
    </div>

    <div id="Progress" >
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10" DynamicLayout="false" >
        <ProgressTemplate><span><img src="indicator.gif" align="middle" runat="server" alt="<%$Resources:SharedResources, Loading%>" /></span></ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <div id="HelpDiv"></div>
</div>


<script type="text/javascript">
document.getElementById('query').focus();
</script>