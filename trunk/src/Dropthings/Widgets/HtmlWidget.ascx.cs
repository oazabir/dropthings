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

public partial class Widgets_HtmlWidget : System.Web.UI.UserControl, IWidget
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

    void IWidget.HideSettings(bool userClicked)
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

    void IWidget.ShowSettings(bool userClicked)
    {
        this.HtmltextBox.Text = this.State.Value;
        SettingsPanel.Visible = true;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        this.Output.Text = (this.State.FirstNode as XCData ?? new XCData("")).Value;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SaveSettings_Clicked(object sender, EventArgs e)
    {
        this.State.RemoveAll();
        this.State.Add(new XCData(this.HtmltextBox.Text));

        this.SaveState();
    }

    private void SaveState()
    {
        var xml = this.State.Xml();
        this._Host.SaveState(xml);
    }

    #endregion Methods
}