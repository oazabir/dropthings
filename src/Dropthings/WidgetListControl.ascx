<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WidgetListControl.ascx.cs" Inherits="WidgetListControl" %>

   <p class="addcontent_message"><asp:Literal ID="ltlAddItemToPage" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, AddItemToPage%>" /></p>
                                <div class="addcontent_navigation"><asp:LinkButton ID="WidgetListPreviousLinkButton" runat="server" Visible="false" Text="&lt; Previous" OnClick="WidgetListPreviousLinkButton_Click" /> | <asp:LinkButton ID="WidgetListNextButton" runat="server" Visible="false" Text="Next &gt;" OnClick="WidgetListNextButton_Click" /></div>
                                
                                <asp:DataList ID="WidgetDataList" runat="server" RepeatDirection="Vertical" RepeatColumns="5" RepeatLayout="Table" CellPadding="3" CellSpacing="3" EnableViewState="False" ShowFooter="False" ShowHeader="False" Width="100%" >
                                    <ItemTemplate>
                                        <div class="new_widget" id="new_widget_<%# Eval("ID") %>">
                                            <asp:Image ID="Icon" ImageUrl='<%# Eval("Icon") %>' ImageAlign="AbsMiddle" runat="server" />&nbsp;
                                            <asp:LinkButton CommandArgument='<%# Eval("ID") %>' CommandName="AddWidget" ID="AddWidget" runat="server" CssClass="widgetitem"><%# Eval("Name") %></asp:LinkButton>
                                        </div>    
                                    </ItemTemplate>
                                </asp:DataList>  
