using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

using Dropthings.Widget.Framework;
using Dropthings.Widget.Widgets;
using Dropthings.Util;
using OmarALZabir.AspectF;

public partial class Widgets_WeatherWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;
    private string weatherLocation = "http://xml.weather.yahoo.com/forecastrss?p=";
    private string zipCode = "22202";

    #endregion Fields

    #region Properties

    public IWidgetHost Host
    {
        get { return _Host; }
        set { _Host = value; }
    }

    #endregion Properties

    #region Methods

    public string GetWeatherData()
    {
        string url = weatherLocation + zipCode;

        XmlDocument doc = Services.Get<ICacheResolver>().Get(url) as XmlDocument ?? (new XmlDocument());
        try
        {
            if (!doc.HasChildNodes) doc.Load(url);
        }
        catch
        {
            return string.Empty;
        }
        if (null == Services.Get<ICacheResolver>().Get(url))
            Services.Get<ICacheResolver>().Add(url, doc);

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
        pnlSettings.Visible = false;
    }

    void IWidget.Init(IWidgetHost host)
    {
        this.Host = host;
    }

    void IWidget.Maximized()
    {
    }

    void IWidget.Restored()
    {
    }

    void IWidget.ShowSettings()
    {
        pnlSettings.Visible = true;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) this.LoadContentView(sender, e);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        zipCode = txtZipCode.Text;
        lblWeather.Text = GetWeatherData();
        this.Host.SaveState(zipCode);
        (this as IWidget).HideSettings();
    }

    #endregion Methods
}