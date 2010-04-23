#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

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

using Dropthings.Web.Framework;
using Dropthings.Widget.Framework;
using Dropthings.Widget.Widgets;
using Dropthings.Web.Util;
using Dropthings.Util;

public partial class Widgets_FastRssWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;
    private XElement _State;

    #endregion Fields

    #region Properties

    public int Count
    {
        get { return State.Element("count") == null ? 3 : int.Parse(State.Element("count").Value); }
        set
        {
            if (State.Element("count") == null)
                State.Add(new XElement("count", value));
            else
                State.Element("count").Value = value.ToString();
        }
    }

    public string Url
    {
        get { return State.Element("url").Value; }
        set { State.Element("url").Value = value; }
    }

    private XElement State
    {
        get
        {
            if (_State == null)
            {
                string stateXml = this._Host.GetState();
                if (string.IsNullOrEmpty(stateXml))
                {
                    //stateXml = "<state><type>MostPopular</type><tag /></state>";
                    _State = new XElement("state",
                        new XElement("url", "MostPopular"),
                        new XElement("count", ""));
                }
                else
                {
                    _State = XElement.Parse(stateXml);
                }
            }
            return _State;
        }
    }

    #endregion Properties

    #region Methods

    void IEventListener.AcceptEvent(object sender, EventArgs e)
    {
        
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
        this.Count = int.Parse(FeedCountDropDownList.SelectedValue);
        this.SaveState();
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
        if (userClicked)
        {
            FeedCountDropDownList.SelectedIndex = -1;
            FeedCountDropDownList.Items.FindByText(this.Count.ToString()).Selected = true;            
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        var cachedJSON = GetCachedJSON();

        var scriptToLoad = ResolveClientUrl("~/Widgets/FastRssWidget.js?v=" + ConstantHelper.ScriptVersionNo);
        var startUpCode = string.Format("window.rssLoader{0} = new FastRssWidget( '{1}', '{2}', {3}, {4} ); window.rssLoader{0}.load();",
                this._Host.ID, this.Url, this.RssContainer.ClientID, this.Count, cachedJSON ?? "null");

        WidgetHelper.RegisterWidgetScript(this, scriptToLoad, startUpCode);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SaveSettings_Click(object sender, EventArgs e)
    {
        _Host.HideSettings(true);
    }

    private string GetCachedJSON()
    {
        if (ProxyAsync.IsUrlInCache(this.Url))
        {
            var cachedRSS = new ProxyAsync().GetRss(this.Url, this.Count, 10);
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(cachedRSS);
            return json;
        }
        else
            return null;
    }

    private void SaveState()
    {
        var xml = this.State.Xml();
        this._Host.SaveState(xml);
        this._State = null;
    }

    #endregion Methods
}