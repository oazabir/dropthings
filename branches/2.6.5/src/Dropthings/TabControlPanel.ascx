<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TabControlPanel.ascx.cs" Inherits="TabControlPanel" %>
<%@ Register src="~/WidgetListControl.ascx" tagname="WidgetListControl" tagprefix="widgets" %>


<asp:UpdatePanel ID="OnPageMenuUpdatePanel" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
        <div id="onpage_menu_bar" onmouseover="this.className='onpage_menu_bar_hover'" onmouseout="this.className=''">
            <asp:LinkButton CssClass="onpage_menu_action" ID="ShowAddContentPanel" runat="server" Text="<%$Resources:SharedResources, AddStuff%>" OnClick="ShowAddContentPanel_Click"/>
            <asp:LinkButton CssClass="onpage_menu_action" ID="HideAddContentPanel" runat="server" Text="<%$Resources:SharedResources, HideStuff%>" OnClick="HideAddContentPanel_Click" Visible="false" OnClientClick="DropthingsUI.hideWidgetGallery();" />
            <asp:LinkButton ID="ChangeTabTitleLinkButton" CssClass="onpage_menu_action" Text="<%$Resources:SharedResources, ChangeSettings%>" runat="server" OnClick="ChangeTabSettingsLinkButton_Clicked" />
        </div>
        <div id="onpage_menu_panels">
            <asp:Panel ID="ChangeTabSettingsPanel" runat="server" Visible="false" CssClass="onpage_menu_panel">
                <div>
                    <div class="onpage_menu_panel_column">
                        <h1><asp:Literal ID="ltlChangeTabTitle" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ChangeTabTitle%>" /></h1>
                        <p>
                            <asp:Literal ID="ltlTitle" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Title%>" />: <asp:TextBox ID="NewTitleTextBox" runat="server" />
                            <asp:Button ID="SaveNewTitleButton" runat="server" OnClick="SaveNewTitleButton_Clicked" Text="<%$Resources:SharedResources, Save%>" />
                        </p>
                        <asp:Panel ID="pnlTemplateUserSettings" runat="server" Visible="false">
                        <p>
                            <asp:Literal ID="ltlLocked" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Locked%>" />: <asp:CheckBox ID="TabLocked" runat="server" />
                            <asp:Button ID="SaveTabLockSetting" runat="server" OnClick="SaveTabLockSettingButton_Clicked" Text="<%$Resources:SharedResources, Save%>" />
                        </p>
                        <p id="maintenenceOption" runat="server" visible="false">
                            <asp:Literal ID="ltlMaintence" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, MaintenanceModeMessage%>" />: <asp:CheckBox ID="TabMaintanance" runat="server" />
                            <asp:Button ID="SaveTabMaintenenceSetting" runat="server" OnClick="SaveTabMaintenenceSettingButton_Clicked" Text="<%$Resources:SharedResources, Save%>" />
                        </p>
                        <p id="serveAsStartPageOption" runat="server" visible="false">
                            <asp:Literal ID="ltlServeAsStartPage" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ServeAsStartPageMessage%>" />: <asp:CheckBox ID="TabServeAsStartPage" runat="server" />
                            <asp:Button ID="SaveTabServeAsStartPageSetting" runat="server" OnClick="SaveTabServeAsStartPageSettingButton_Clicked" Text="<%$Resources:SharedResources, Save%>" />
                        </p>
                        </asp:Panel>
                    </div>
                    <div class="onpage_menu_panel_column">
                        <h1><asp:Literal ID="ltlDeleteTab" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, DeleteTab%>" /></h1>
                        <p>
                        <asp:Literal ID="ltlDeleteTab2" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, DeleteTab%>" />? <asp:Button ID="DeleteTabLinkButton" runat="server" OnClick="DeleteTabLinkButton_Clicked" Text="<%$Resources:SharedResources, Yes%>" />
                        </p>
                    </div>                                
                    <div class="onpage_menu_panel_column" style="clear:right">
                        <h1><asp:Literal ID="ltlChangeColumn" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ChangeColumn%>" /></h1>
                                        
                        <p><asp:Literal ID="ltlChoiceColumnLayout" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, ChoiceColumnLayout%>" /><br />
                            <asp:ImageButton ID="ImageButton1" ImageUrl="img/Layout1.jpg" runat="server" OnClick="Layout1_Clicked" />
                            <asp:ImageButton ID="ImageButton2" ImageUrl="img/Layout2.jpg" runat="server" OnClick="Layout2_Clicked" />
                            <asp:ImageButton ID="ImageButton3" ImageUrl="img/Layout3.jpg" runat="server" OnClick="Layout3_Clicked" />
                            <asp:ImageButton ID="ImageButton4" ImageUrl="img/Layout4.jpg" runat="server" OnClick="Layout4_Clicked" />                                        
                        </p>                                        
                    </div>
                </div>
                <div style="clear:both"></div>
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
