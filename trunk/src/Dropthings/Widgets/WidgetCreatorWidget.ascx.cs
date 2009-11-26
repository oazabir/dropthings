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

public partial class Widgets_WidgetCreatorWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;
    private XElement _State;

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

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        using (Facade facade = new Facade(AppContext.GetContext(Context)))
        {
            facade.AddWidget(txtTitle.Text.Trim(), txtUrl.Text.Trim(), txtDescription.Text.Trim(), chkIsDefault.Checked);
            WebUtil.ShowMessage(lblMessage, "Widget successfully added.", false);
            Clear();
        }
    }

    private void Clear()
    {
        txtTitle.Text = txtUrl.Text = txtDescription.Text = string.Empty;
        chkIsDefault.Checked = false;
    }

    #endregion Methods
}