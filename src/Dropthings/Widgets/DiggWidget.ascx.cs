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

public partial class Widgets_DiggWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;
    private XElement _State;

    #endregion Fields

    #region Properties

    public string Topic
    {
        get { return (State.Element("topic") ?? new XElement("topic", "")).Value; }
        set { State.Element("topic").Value = value; }
    }

    private XElement State
    {
        get
        {
            string state = this._Host.GetState();
            if (string.IsNullOrEmpty(state))
                state = "<state><topic>football</topic></state>";
            if (_State == null) _State = XElement.Parse(state);
            return _State;
        }
    }

    #endregion Properties

    #region Methods

    public void BindDiggData()
    {
        Source.Text = ResolveClientUrl("~/ClientBin/Dropthings.DiggSilverlight.xap");
        InitParams.Text = "WidgetId={0}".FormatWith(this._Host.ID)
            + ",State={0}".FormatWith(this.State.Xml());
    }

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
        SettingsPanel.Visible = true;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindDiggData();
    }

    private void BindList()
    {
    }

    private void SaveState()
    {
    }

    #endregion Methods
}