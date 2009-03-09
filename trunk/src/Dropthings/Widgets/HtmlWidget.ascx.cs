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


public partial class Widgets_HtmlWidget : System.Web.UI.UserControl, IWidget
{
    private IWidgetHost _Host;

    private XElement _State;
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

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    void IWidget.Init(IWidgetHost host)
    {
        this._Host = host;
    }

    void IWidget.ShowSettings()
    {
        this.HtmltextBox.Text = this.State.Value;
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
        var xml = this.State.Xml();
        this._Host.SaveState(xml);
    }

    protected void SaveSettings_Clicked(object sender, EventArgs e)
    {
        this.State.RemoveAll();
        this.State.Add(new XCData(this.HtmltextBox.Text));
        
        this.SaveState();
    }
    
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        this.Output.Text = (this.State.FirstNode as XCData ?? new XCData("")).Value;
    }

    #region IWidget Members


    void IEventListener.AcceptEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion
}
