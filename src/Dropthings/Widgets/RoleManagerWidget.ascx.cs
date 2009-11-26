using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Dropthings.Widget.Framework;
using Dropthings.Widget.Widgets;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;
using Dropthings.Web.Util;

using Dropthings.DataAccess;

public partial class Widgets_RoleManagerWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;
    private XElement _State;
    private bool _hasProcessedHeader;

    #endregion Fields

    #region Properties

    private XElement State
    {
        get
        {
            string state = this._Host.GetState();
            if (string.IsNullOrEmpty(state))
                state = "<state></state>";
            if (_State == null) _State = XElement.Parse(state);
            return _State;
        }
    }

    #endregion Properties

    #region Methods

    void IEventListener.AcceptEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    void IWidget.Closed()
    {
    }

    void IWidget.Collasped()
    {
    }

    void IWidget.Expanded()
    {
    }

    void IWidget.HideSettings()
    {
        SettingsPanel.Visible = false;
    }

    void IWidget.Init(IWidgetHost host)
    {
        this._Host = host;
    }

    void IWidget.Maximized()
    {
    }

    void IWidget.Restored()
    {
    }

    void IWidget.ShowSettings()
    {
        SettingsPanel.Visible = true;
    }
    
    private void SaveRole()
    {
        using (var facade = new Facade(AppContext.GetContext(Context)))
        {
            try
            {
                facade.InsertRole(txtRole.Text.Trim());
                BindList();
            }
            catch (ArgumentException ex)
            {
                WebUtil.ShowMessage(lblMessage, ex.Message, false);
            }
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
         
    }

    protected void SaveClicked(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SaveRole();
        }
    }

    protected void CancelClicked(object sender, EventArgs e)
    {
        BindList();
    }

    protected void AddNewRoleClicked(object sender, EventArgs e)
    {
        this.Multiview.ActiveViewIndex = 1;
    }

    protected void Items_Selecting(object sender, LinqDataSourceSelectEventArgs args)
    {
        using (var facade = new Facade(AppContext.GetContext(Context)))
        {
            args.Result = facade.GetAllRole();
        }
    }

    protected void ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        string roleName = e.CommandArgument.ToString();

        if (!string.IsNullOrEmpty(roleName))
        {
            if (string.Equals(e.CommandName, "DeleteItem"))
            {
                //add delete logic here    
                using (var facade = new Facade(AppContext.GetContext(Context)))
                {
                    try
                    {
                        facade.DeleteRole(roleName);
                        BindList();
                        WebUtil.ShowMessage(lblMessage, "Role has been successfully deleted.", false);
                    }
                    catch (Exception ex)
                    {
                        WebUtil.ShowMessage(lblMessage, ex.Message, true);
                    }
                }
            }
        }
    }

    public void BindList()
    {
        this.lvItems.DataBind();
        this.Multiview.ActiveViewIndex = 0;
    }

    #endregion Methods
}