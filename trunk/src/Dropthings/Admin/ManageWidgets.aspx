<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageWidgets.aspx.cs" Inherits="Admin_ManageWidgets" ValidateRequest="false" %>
<%@ OutputCache Location="None" NoStore="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ManageWidget</title>
</head>

<style>
    label { width: 200px }
    td { padding: 5px; }
</style>
    
<body>
    <form id="form1" runat="server">
    <div>
    
        <h1>Manage Widgets</h1>
        
        <asp:Button ID="CreateNew" runat="server" Text="Add New" OnClick="AddNew_Clicked"/>
        
        <h2>Edit Widget</h2>
        <asp:Panel ID="EditForm" runat="server" Visible="false">
        <asp:Label ID="Error" runat="server" EnableViewState="false" ForeColor="Red" Font-Size="20pt" />
        <div>
            <label>ID: </label>
            <input id="Field_ID" runat="server" type="text" disabled="disabled"  />
            <br />
            <label>Name: </label>
            <input id="Field_Name" maxlength="255" runat="server" type="text"  />
            <br />
            <label>Description: </label>
            <input id="Field_Description" maxlength="255" runat="server" type="text"  />
            <br />
            <label>Url: </label>
            <input id="Field_Url" maxlength="255" runat="server" type="text"  />
            <br />
            <label>Default State: </label>
            <input id="Field_DefaultState" maxlength="255" runat="server" type="text"  />
            <br />
            <label>Icon: </label>
            <input id="Field_Icon" maxlength="255" runat="server" type="text"  />
            <br />
            <label>OrderNo: </label>
            <input id="Field_OrderNo" maxlength="2" value="1" runat="server" type="text"  />
            <br />
            <label>Default Role Name: </label>
            <input id="Field_RoleName" maxlength="255" value="guest" runat="server" type="text"  />
            <br />
            <label>IsLocked: </label>
            <input id="Field_IsLocked" runat="server" type="checkbox"  />
            <br />
            <label>IsDefault: </label>
            <input id="Field_IsDefault" runat="server" type="checkbox"  />
            <br />
            <label>WidgetType: </label>
            <input id="Field_WidgetType" maxlength="2" value="0" runat="server" type="text"  />
            <br />
            <label>Roles that can use the widget:</label>
            <asp:CheckBoxList ID="WidgetRoles" runat="server"></asp:CheckBoxList>
            <br />
            <asp:Button ID="SaveWidget" runat="server" OnClick="SaveWidget_Clicked" Text="Save" />
            <asp:Button Text="Delete" ID="DeleteWidget" OnClick="DeleteWidget_Clicked" runat="server" />
        </div>
        </asp:Panel>
        
        <asp:DataGrid ID="Widgets" runat="server" BackColor="LightGoldenrodYellow" 
            BorderColor="Tan" BorderWidth="1px" ForeColor="Black" 
            GridLines="None" AutoGenerateColumns="False" 
            onitemcommand="Widgets_ItemCommand">
            <FooterStyle BackColor="Tan" />
            <SelectedItemStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                HorizontalAlign="Center" />
            <AlternatingItemStyle BackColor="PaleGoldenrod" />            
            <Columns>
                <asp:ButtonColumn ButtonType="LinkButton" CommandName="Edit" Text="Edit" />
                <asp:BoundColumn DataField="ID" HeaderText="ID" />
                <asp:BoundColumn DataField="Name" HeaderText="Name" ItemStyle-Font-Bold="true" />
                <asp:BoundColumn DataField="Url" HeaderText="Url" />
                <asp:BoundColumn DataField="Description" HeaderText="Description" />
                <asp:TemplateColumn HeaderText="State">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" 
                            Text='<%# DataBinder.Eval(Container, "DataItem.DefaultState") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" 
                            Text='<%# DataBinder.Eval(Container, "DataItem.DefaultState") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Icon" HeaderText="Icon" />
                <asp:BoundColumn DataField="OrderNo" HeaderText="Order" />
                <asp:BoundColumn DataField="RoleName" HeaderText="Role" />
                <asp:BoundColumn DataField="IsLocked" HeaderText="Locked?" />
                <asp:BoundColumn DataField="IsDefault" HeaderText="Default?" />
                <asp:BoundColumn DataField="VersionNo" HeaderText="Ver" />
                <asp:BoundColumn DataField="WidgetType" HeaderText="Type" />
                
            </Columns>
            
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            
        </asp:DataGrid>
        
        
    </div>
    </form>
</body>
</html>
