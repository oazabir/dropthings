using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;
using Dropthings.DataAccess;
using Dropthings.Util;
using OmarALZabir.AspectF;

public partial class Admin_ManageWidgets : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.LoadWidgets();           
    }

    private void LoadWidgets()
    {
        using (Facade facade = new Facade(AppContext.GetContext(Context)))
        {
            Widgets.DataSource = facade.GetAllWidgets();
            Widgets.DataBind();
        }
    }

    protected void SaveWidget_Clicked(object sender, EventArgs e)
    {
        try
        {
            var control = LoadControl(Field_Url.Value);
        }
        catch
        {
            Error.Text = "Cannot load the widget from the specified location. Make sure you have given a correct relative path to an .ascx";
            Error.Visible = true;
            return;
        }

        using (Facade facade = new Facade(AppContext.GetContext(Context)))
        {
            var widgetId = int.Parse(Field_ID.Value.Trim());
            if (widgetId == 0)
            {
                var newlyAddedWidget = facade.AddWidget(
                    Field_Name.Value,
                    Field_Url.Value,
                    Field_Icon.Value,
                    Field_Description.Value,
                    Field_DefaultState.Value,
                    Field_IsDefault.Checked,
                    Field_IsLocked.Checked,
                    int.Parse(Field_OrderNo.Value),
                    Field_RoleName.Value,
                    int.Parse(Field_WidgetType.Value));
                
                widgetId = newlyAddedWidget.ID;
                SetWidgetRoles(widgetId);   
                this.EditWidget(newlyAddedWidget);
            }
            else
            {
                facade.UpdateWidget(widgetId,
                    Field_Name.Value,
                    Field_Url.Value,
                    Field_Icon.Value,
                    Field_Description.Value,
                    Field_DefaultState.Value,
                    Field_IsDefault.Checked,
                    Field_IsLocked.Checked,
                    int.Parse(Field_OrderNo.Value),
                    Field_RoleName.Value,
                    int.Parse(Field_WidgetType.Value));

                SetWidgetRoles(widgetId);   
            }

            // Flush all widget cache
            CacheSetup.CacheKeys.AllWidgetsKeys().Each(key => Services.Get<ICache>().Remove(key));
                     
            this.LoadWidgets();
            EditForm.Visible = false;
        }        
    }

    private void SetWidgetRoles(int widgetId)
    {
        var roles = new List<string>();
        foreach (ListItem item in WidgetRoles.Items)
            if (item.Selected)
                roles.Add(item.Text);
        using (Facade facade = new Facade(AppContext.GetContext(Context)))
        {
            facade.AssignWidgetRoles(widgetId, roles.ToArray());
        }        
    }

    protected void AddNew_Clicked(object sender, EventArgs e)
    {
        EditForm.Visible = true;
        ClearForm(role => false);
    }

    private void ClearForm(Func<string, bool> isRoleChecked)
    {
        Field_DefaultState.Value = "";
        Field_Description.Value = "";
        Field_Url.Value = "";
        Field_Icon.Value = "widgets/FlickrIcon.gif";
        Field_ID.Value = "0";
        Field_IsDefault.Checked = false;
        Field_IsLocked.Checked = false;
        Field_Name.Value = "NewWidget";
        Field_OrderNo.Value = "0";
        Field_RoleName.Value = "guest";
        Field_WidgetType.Value = "0";

        WidgetRoles.Items.Clear();
        using (Facade facade = new Facade())
        {
            facade.GetAllRole().Each(role =>
                {   
                    ListItem newRoleItem = new ListItem(role.RoleName, role.RoleId.ToString());
                    newRoleItem.Selected = isRoleChecked(role.RoleName);                    
                    WidgetRoles.Items.Add(newRoleItem);

                });
        }
    }
    protected void Widgets_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            Widget widget = (Widgets.DataSource as List<Widget>)[e.Item.ItemIndex];

            EditWidget(widget);
        }
    }

    private void EditWidget(Widget widget)
    {
        using (Facade facade = new Facade(AppContext.GetContext(Context)))
        {
            ClearForm(role => facade.IsWidgetInRole(widget.ID, role));
        }

        EditForm.Visible = true;

        Field_DefaultState.Value = widget.DefaultState;
        Field_Description.Value = widget.Description;
        Field_Url.Value = widget.Url;
        Field_Icon.Value = widget.Icon;
        Field_ID.Value = widget.ID.ToString();
        Field_IsDefault.Checked = widget.IsDefault;
        Field_IsLocked.Checked = widget.IsLocked;
        Field_Name.Value = widget.Name;
        Field_OrderNo.Value = widget.OrderNo.ToString();
        Field_RoleName.Value = widget.RoleName.ToString();
        Field_WidgetType.Value = widget.WidgetType.ToString();
    }
}
