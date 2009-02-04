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

public partial class Widgets_DiggWidget : System.Web.UI.UserControl, IWidget
{
    private IWidgetHost _Host;

    private XElement _State;
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

    public string Topic
    {
        get { return (State.Element("topic") ?? new XElement("topic", "")).Value; }
        set { State.Element("topic").Value = value; }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    private void BindList()
    { 
    
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindDiggData();
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

    private void SaveState()
    {
        
    }

    public void BindDiggData()
    {
        diggXaml.InitParameters = "WidgetId={0}".FormatWith(this._Host.ID)
            + ",State={0}".FormatWith(this.State.Xml());
    }
}
