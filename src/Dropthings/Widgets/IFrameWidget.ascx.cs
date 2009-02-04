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

public partial class Widgets_IFrameWidget : System.Web.UI.UserControl, IWidget
{
    private IWidgetHost _Host;

    private XElement _State;
    private XElement State
    {
        get
        {
            string state = this._Host.GetState();
            if (string.IsNullOrEmpty(state))
                state = "<state><url>http://www.labpixies.com/campaigns/notes/notes.html</url><width>300</width><height>200</height><scroll>false</scroll></state>";
            if (_State == null) _State = XElement.Parse(state);
            return _State;
        }
    }

    public bool Scrollbar
    {
        get { return bool.Parse((State.Element("scroll") ?? new XElement("scroll", "false")).Value); }
        set
        {
            if (State.Element("scroll") != null) State.Element("scroll").Value = value.ToString();
            else State.Add(new XElement("scroll", value.ToString())); 
        }
    }

    public string Url
    {
        get { return (State.Element("url") ?? new XElement("url", "")).Value; }
        set { State.Element("url").Value = value; }
    }

    public string Width
    {
        get { return (State.Element("width") ?? new XElement("width", "300")).Value; }
        set { State.Element("width").Value = value; }
    }

    public string Height
    {
        get { return (State.Element("height") ?? new XElement("height", "200")).Value; }
        set { State.Element("height").Value = value; }
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
        UrlTextBox.Text = this.Url;
        WidthTextBox.Text = this.Width;
        HeightTextBox.Text = this.Height;
        ScrollCheckBox.Checked = this.Scrollbar;

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
        this.Url = UrlTextBox.Text;
        this.Width = WidthTextBox.Text;
        this.Height = HeightTextBox.Text;
        this.Scrollbar = ScrollCheckBox.Checked;

        this.SaveState();
    }

}
