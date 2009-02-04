// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

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

using System.Linq;
using System.Xml.Linq;
using Dropthings.Widget.Widgets;
using Dropthings.Widget.Framework;
using Dropthings.Web.Framework;


public partial class Widgets_FastRssWidget : System.Web.UI.UserControl, IWidget
{
    private IWidgetHost _Host;
    private XElement _State;
    private XElement State
    {
        get
        {
            if (_State == null) _State = XElement.Parse(this._Host.GetState());
            return _State;
        }
    }

    public string Url
    {
        get { return State.Element("url").Value; }
        set { State.Element("url").Value = value; }
    }

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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this._Host.IsFirstLoad)
        {
            ScriptManager.RegisterClientScriptInclude(this,
                typeof(Widgets_FastRssWidget),
                "FastRssWidget",
                this.ResolveClientUrl(this.AppRelativeTemplateSourceDirectory + "FastRssWidget.js"));

            var cachedJSON = GetCachedJSON();

            ScriptManager.RegisterStartupScript(this, typeof(Widgets_FastRssWidget), "LoadRSS",
                string.Format("var rssLoader{0} = new FastRssWidget( '{1}', '{2}', {3}, {4} ); rssLoader{0}.load();",
                    this._Host.ID, this.Url, this.RssContainer.ClientID, this.Count, cachedJSON ?? "null"), true);
        }
    }

    private string GetCachedJSON()
    {
        if (ProxyAsync.IsUrlInCache(Cache, this.Url))
        {
            var cachedRSS = new ProxyAsync().GetRss(this.Url, this.Count, 10);            
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(cachedRSS);
            return json;
        }
        else
            return null;
    }

    protected void SaveSettings_Click(object sender, EventArgs e)
    {
        (this as IWidget).HideSettings();
    }

    void IWidget.Init(IWidgetHost host)
    {
        this._Host = host;
    }

    void IWidget.ShowSettings()
    {
        SettingsPanel.Visible = true;
        FeedCountDropDownList.SelectedIndex = -1;
        FeedCountDropDownList.Items.FindByText(this.Count.ToString()).Selected = true;
    }
    void IWidget.HideSettings()
    {
        SettingsPanel.Visible = false;
        this.Count = int.Parse(FeedCountDropDownList.SelectedValue);
        this.SaveState();
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
        this._State = null;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!this._Host.IsFirstLoad)
        ScriptManager.RegisterStartupScript(this, typeof(Widgets_FastRssWidget), "LoadRSS",
                string.Format("rssLoader{0}.url = '{1}'; rssLoader{0}.count = {2}; rssLoader{0}.load();",
                    this._Host.ID, this.Url, this.Count), true);
    }
    
}
