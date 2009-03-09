using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Dropthings.Widget.Widgets;
using Dropthings.Widget.Framework;

public partial class Widgets_WeatherWidget : System.Web.UI.UserControl, IWidget
{
    private string weatherLocation = "http://xml.weather.yahoo.com/forecastrss?p=";
    private string zipCode = "22202";
    private IWidgetHost _Host;

    public IWidgetHost Host
    {
        get { return _Host; }
        set { _Host = value; }
    }
	
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) this.LoadContentView(sender, e);        
    }

    protected void LoadContentView(object sender, EventArgs e)
    {
        this.Multiview.ActiveViewIndex = 1;
        this.MultiviewTimer.Enabled = false;

        if (!Page.IsPostBack)
        {
            if (this.Host.GetState().Trim().Length == 0)
            {
                //lblWeather.Text = GetWeatherData();
            }
            else
            {
                //lblWeather.Text = this.Host.GetState();
                zipCode = this.Host.GetState();
            }
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        lblWeather.Text = GetWeatherData();
    }

    void IWidget.Init(IWidgetHost host)
    {
        this.Host = host;
    }

    void IWidget.ShowSettings()
    {
        pnlSettings.Visible = true;
    }
    void IWidget.HideSettings()
    {
        pnlSettings.Visible = false;
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

    public string GetWeatherData()
    {
        string url = weatherLocation + zipCode;

        XmlDocument doc = Cache[url] as XmlDocument ?? (new XmlDocument());
        try
        {
            if (!doc.HasChildNodes) doc.Load(url);
        }
        catch
        {
            return string.Empty;
        }
        if (null == Cache[url]) Cache[url] = doc;

        XmlElement root = doc.DocumentElement;
        XmlNodeList nodes = root.SelectNodes("/rss/channel/item");
        string data = "";
        foreach (XmlNode node in nodes)
        {
            data  = data + node["title"].InnerText;
            data  = data + node["description"].InnerText;
        }
        return data;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        zipCode = txtZipCode.Text;
        lblWeather.Text = GetWeatherData();
        this.Host.SaveState(zipCode);
        (this as IWidget).HideSettings();
    }

    #region IWidget Members


    void IEventListener.AcceptEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion
}
