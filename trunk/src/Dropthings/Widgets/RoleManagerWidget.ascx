<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoleManagerWidget.ascx.cs" Inherits="Widgets_RoleManagerWidget" %>
<asp:LinqDataSource ID="ldsItems" runat="server" OnSelecting="Items_Selecting" />
<asp:Panel ID="SettingsPanel" runat="server" Visible="false">
</asp:Panel>
<div>
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>
<asp:MultiView ID="Multiview" runat="server" ActiveViewIndex="0">
     <asp:View runat="server" ID="RoleListView">
        <div style="float:right; margin:5px;clear:both">
            <asp:ImageButton ID="btnAdd" runat="server" SkinID="sknImageAdd" ToolTip="Add Role" OnClick="AddNewRoleClicked"></asp:ImageButton>
        </div>
        <div class="grid">
            <asp:ListView ID="lvItems" DataSourceID="ldsItems" runat="server" OnItemCommand="ItemCommand">
                <LayoutTemplate>                                         
                    <table id="RoleList" runat="server" class="datatable" cellpadding="0" cellspacing="0">
                        <tr id="itemPlaceholder" runat="server" />
                    </table>  
                    <div class="pager">
                        <asp:DataPager ID="pagerBottom" runat="server" PageSize="1">
                            <Fields>
                                <asp:TemplatePagerField><PagerTemplate></PagerTemplate></asp:TemplatePagerField>
                                <asp:NextPreviousPagerField ButtonCssClass="command" FirstPageText="«" PreviousPageText="‹" RenderDisabledButtonsAsLabels="true" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" /> 
                                <asp:NumericPagerField ButtonCount="7" NumericButtonCssClass="command" CurrentPageLabelCssClass="current" NextPreviousButtonCssClass="command" />
                                <asp:NextPreviousPagerField ButtonCssClass="command" LastPageText="»" NextPageText="›" RenderDisabledButtonsAsLabels="true" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" />                                            
                            </Fields>
                        </asp:DataPager>
                    </div>                                                  
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="item" runat="server" class="row">
                        <td><%# Eval("roleName")%></td>
                        <td style="text-align: left; white-space: nowrap;">
                            <asp:ImageButton ID="btnDelete" runat="server" SkinID="sknImageDelete" ToolTip="Delete" CommandName="DeleteItem" CommandArgument=<%# Eval("roleName")%>></asp:ImageButton>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
     </asp:View>
     <asp:View runat="server" ID="RoleEditor">
        <div class="form">
            <p>
                <asp:Label ID="ltlRole" EnableViewState="false" runat="server" Text="RoleName" />
                <asp:TextBox ID="txtRole" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="btnCancel" runat="server" Text="<%$Resources:SharedResources, Cancel%>" EnableViewState="false" OnClick="CancelClicked" />
                <asp:Button ID="btnSubmit" runat="server" Text="<%$Resources:SharedResources, Submit%>" EnableViewState="false" OnClick="SaveClicked" />
            </p>
        </div>
     </asp:View>
</asp:MultiView>