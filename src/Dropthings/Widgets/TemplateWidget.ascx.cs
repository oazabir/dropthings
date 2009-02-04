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
using Dropthings.Widget.Widgets;
using Dropthings.Widget.Framework;


public partial class Widgets_TemplateWidget : System.Web.UI.UserControl, IWidget
{
    private IWidgetHost _Host;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    void IWidget.Init(IWidgetHost host)
    {
        this._Host = host;
    }

    void IWidget.ShowSettings()
    {
        SettingsPanel.Visible = true;
    }
    void IWidget.HideSettings()
    {
        SettingsPanel.Visible = false;
    }
    void IWidget.Expanded()
    {
    }
    void IWidget.Collasped()
    {
    }
    void IWidget.Restored()
    {
    }
    void IWidget.Maximized()
    {
    }
    void IWidget.Closed()
    {
    }
}
