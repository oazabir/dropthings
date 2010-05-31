<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TabPage.ascx.cs" Inherits="TabPage" %>
<%@ Register Src="WidgetContainer.ascx" TagName="WidgetContainer" TagPrefix="widget" %>
<%@ Register src="WidgetPanels.ascx" tagname="WidgetPanels" tagprefix="uc3" %>
<asp:UpdatePanel ID="TabUpdatePanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
       <div id="tab_container" class="tab_container">
            <div class="tab_container_header" id="tabHeader">
                <ul class="tabs tab-strip" runat="server" id="tabList">
                    <li class="tab inactivetab"><asp:LinkButton id="Page1Tab" runat="server" Text="Page 1"></asp:LinkButton></li>
                    <li class="tab activetab"><asp:LinkButton id="Page2Tab" runat="server" Text="Page 2"></asp:LinkButton>
                    </li>
                </ul>
           </div>
           <div class="tab_container_options">
                <div class="newtabscrolling" style="display:none;"><asp:LinkButton id="addNewTabLinkButton1" runat="server" CssClass="newtab_add newtab_add_block" OnClick="addNewTabLinkButton_Click"></asp:LinkButton></div>
           </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    jQuery(document).ready(function () {
        DropthingsUI.init("<%= this.EnableTabSorting %>");
    });
</script>

