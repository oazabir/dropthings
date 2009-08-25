<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Theme="GreenBlue" EnableSessionState="False" ValidateRequest="false" Trace="False" TraceMode="SortByCategory" %>
<%@ OutputCache Location="None" NoStore="true" %>

<%@ Register Src="~/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<%@ Register src="~/WidgetListControl.ascx" tagname="WidgetListControl" tagprefix="widgets" %>
<%@ Register Src="~/WidgetPage.ascx" TagName="WidgetPage" TagPrefix="widgets" %>
<%@ Register src="~/TabPage.ascx" tagname="TabPage" tagprefix="tab" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Dropthings - Free AJAX Web Portal, Web 2.0 Start Page built on ASP.NET 3.5</title>
    <meta name="Description" content="Dropthings is a free open source Web Portal. Personalize your internet experience by putting widgets on your page. It's built on ASP.NET AJAX, .NET 3.5, Windows Workflow foundation. ">
    <meta name="Keywords" content="AJAX Web Portal Web 2.0 Start Page ASP.NET 3.5 ">    
    <meta name="Page-topic" content="Free Open Source Ajax Start Page" />
</head>
<body>
<form id="default_form" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" ScriptMode="Release">
    <Services>
        <asp:ServiceReference InlineScript="true" Path="PageService.asmx" />
        <asp:ServiceReference InlineScript="true" Path="ProxyAsync.asmx" />
        <asp:ServiceReference InlineScript="true" Path="WidgetService.asmx" />
    </Services>
    <Scripts>
        <asp:ScriptReference Path="~/Scripts/jquery-1.3.2.min.js" />
        <asp:ScriptReference Path="~/Scripts/jquery-ui-1.7.1.custom.min.js" />
        <asp:ScriptReference Path="~/Scripts/jquery.micro_template.js" />
        <asp:ScriptReference Path="~/Scripts/tabscroll.js" />
        <asp:ScriptReference Path="~/Scripts/Myframework.js" />
        <asp:ScriptReference Path="~/Scripts/Ensure.js" />
    </Scripts>    
</asp:ScriptManager>

<script type="text/javascript">if( typeof Proxy == "undefined" ) window.Proxy = Dropthings.Web.Framework.ProxyAsync;</script>

    <div id="container">
        <!-- Render header first so that user can start typing search criteria while the huge runtime and other scripts download -->
        <uc1:Header ID="Header1" runat="server" />

        <tab:TabPage ID="UserTabPage" runat="server" />
        
        <div id="onpage_menu">
            <div id="onpage_menu_wrapper">
                <asp:UpdatePanel ID="OnPageMenuUpdatePanel" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                        <div id="onpage_menu_bar" onmouseover="this.className='onpage_menu_bar_hover'" onmouseout="this.className=''">
                            <asp:LinkButton CssClass="onpage_menu_action" ID="ShowAddContentPanel" runat="server" Text="<%$Resources:SharedResources, AddStuff%>" OnClick="ShowAddContentPanel_Click"/>
                            <asp:LinkButton CssClass="onpage_menu_action" ID="HideAddContentPanel" runat="server" Text="<%$Resources:SharedResources, HideStuff%>" OnClick="HideAddContentPanel_Click" Visible="false" OnClientClick="DropthingsUI.hideWidgetGallery();" />
                            <asp:LinkButton ID="ChangePageTitleLinkButton" CssClass="onpage_menu_action" Text="<%$Resources:SharedResources, ChangeSettings%>" runat="server" OnClick="ChangeTabSettingsLinkButton_Clicked" />
                        </div>
                        <div id="onpage_menu_panels">
                            <asp:Panel ID="ChangePageSettingsPanel" runat="server" Visible="false" CssClass="onpage_menu_panel">
                                <div class="onpage_menu_panel_column">
                                    <h1><asp:Literal ID="ltlChangeTabTitle" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ChangeTabTitle%>" /></h1>
                                    <p>
                                        <asp:Literal ID="ltlTitle" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Title%>" />: <asp:TextBox ID="NewTitleTextBox" runat="server" />
                                        <asp:Button ID="SaveNewTitleButton" runat="server" OnClick="SaveNewTitleButton_Clicked" Text="<%$Resources:SharedResources, Save%>" />
                                    </p>
                                    <p>
                                        <asp:Literal ID="ltlLocked" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Locked%>" />: <asp:CheckBox ID="TabLocked" runat="server" />
                                        <asp:Button ID="SaveTabLockSetting" runat="server" OnClick="SaveTabLockSettingButton_Clicked" Text="<%$Resources:SharedResources, Save%>" />
                                    </p>
                                </div>
                                
                                <div class="onpage_menu_panel_column">
                                    <h1><asp:Literal ID="ltlDeleteTab" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, DeleteTab%>" /></h1>
                                    <p>
                                    <asp:Literal ID="ltlDeleteTab2" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, DeleteTab%>" />? <asp:Button ID="DeleteTabLinkButton" runat="server" OnClick="DeleteTabLinkButton_Clicked" Text="<%$Resources:SharedResources, Yes%>" />
                                    </p>
                                </div>                                
                                <div class="onpage_menu_panel_column">
                                    <h1><asp:Literal ID="ltlChangeColumn" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ChangeColumn%>" /></h1>
                                    
                                    <p><asp:Literal ID="ltlChoiceColumnLayout" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ChoiceColumnLayout%>" /><br />
                                    <input id="SelectLayoutPopup_Type1" type="image" value="1"  src="img/Layout1.jpg" onclick="DropthingsUI.Actions.changePageLayout(1)" /> 
                                    <input id="SelectLayoutPopup_Type2" type="image" value="2" src="img/Layout2.jpg" onclick="DropthingsUI.Actions.changePageLayout(2)" />         
                                    <input id="SelectLayoutPopup_Type3" type="image" value="3" src="img/Layout3.jpg" onclick="DropthingsUI.Actions.changePageLayout(3)" />      
                                    <input id="SelectLayoutPopup_Type4" type="image" value="4" src="img/Layout4.jpg" onclick="DropthingsUI.Actions.changePageLayout(4)" />
                                    </p>                                    
                                    
                                </div>
                            </asp:Panel>
                            <div id="Widget_Gallery" style="display:none">
                                <asp:Panel ID="AddContentPanel" runat="Server" CssClass="onpage_menu_panel widget_showcase" Visible="false">                                
                                    <widgets:WidgetListControl ID="WidgetListControlAdd" runat="server" />                    
                                    
                                    <div class="clear"></div>
                                </asp:Panel>            
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="clear"></div>
        <div id="contents">
            <div id="contents_wrapper">
                <div id="widget_area">
                    <div id="widget_area_wrapper">
                        <widgets:WidgetPage runat="server" ID="WidgetPage" />                        
                    </div>
                </div>
            </div>
        </div>
                        
        <uc2:Footer ID="Footer1" runat="server"></uc2:Footer>
                    
    </div>
    

    <!-- Fades the UI -->
    <div id="blockUI" style="display: none; background-color: black;
    width: 100%; height: 100px; position: absolute; left: 0px; top: 0px; z-index: 50000;     
    -moz-opacity:0.5;opacity:0.5;filter:alpha(opacity=50);"
    onclick="return false" onmousedown="return false" onmousemove="return false"
    onmouseup="return false" ondblclick="return false">&nbsp;</div>        
    
    <textarea id="TraceConsole" rows="10" cols="80" style="display:none"></textarea>
            
    <textarea id="DeleteConfirmPopupPlaceholder" style="display:none">
    &lt;div id="DeleteConfirmPopup"&gt;
        &lt;h1&gt;Delete a Widget&lt;/h1&gt;
        &lt;p&gt;Are you sure you want to delete the widget?&lt;/p&gt;
        &lt;input id="DeleteConfirmPopup_Yes" type="button" value="Yes" /&gt;&lt;input id="DeleteConfirmPopup_No" type="button" value="No" /&gt;
    &lt;/div&gt;    
    </textarea>    
    
     <textarea id="DeletePageConfirmPopupPlaceholder" style="display:none">
    &lt;div id="DeletePageConfirmPopup"&gt;
        &lt;h1&gt;Delete a Page&lt;/h1&gt;
        &lt;p&gt;Are you sure you want to delete the page?&lt;/p&gt;
        &lt;input id="DeletePageConfirmPopup_Yes" type="button" value="Yes" /&gt;&lt;input id="DeletePageConfirmPopup_No" type="button" value="No" /&gt;
    &lt;/div&gt;    
    </textarea>
    
    <textarea id="LayoutPickerPopupPlaceholder" style="display:none">
    
    </textarea>
    
    <!-- Template for a new widget placeholder -->
    <!-- Begin template -->
    <div class="nodisplay">
        <div ID="new_widget_template" class="widget">
            <div class="widget_header">
                <table class="widget_header_table" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td class="widget_title"><a class="widget_title_label"><!=json.title !></a></td>
                            <td class="widget_edit"><a class="widget_edit">edit</a></td>
                            <td class="widget_button"><a class="widget_close widget_box">x</a></td>
                        </tr>
                    </tbody>
                </table>            
            </div>
            <div ID="WidgetResizeFrame" class="widget_resize_frame" >
                <div class="widget_body">
                    Loading widget...
                </div>
            </div>            
        </div>
    </div>
    <!-- End template -->
</form>

</body>


<script type="text/javascript">
     jQuery(document).ready(function() {
        DropthingsUI.init();
    });
</script>

</html>
