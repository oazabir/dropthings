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
using System.Xml;
using System.Net;
using System.IO;
using System.Net.Sockets;
using Dropthings.Widget.Framework;
using Dropthings.Web.Framework;

public partial class Widgets_RSSWidget : System.Web.UI.UserControl, IWidget
{
    private IWidgetHost _Host;

    protected void Page_Load(object sender, EventArgs e)
    {
        // If we already have the URL in cache, then we can directly render it instead of fetching the content
        // after a async postback
        if (Page.IsPostBack || ProxyAsync.IsUrlInCache(Cache, this.Url) ) 
            this.LoadRSSView(sender, e);
    }

    protected void LoadRSSView(object sender, EventArgs e)
    {
        this.ShowFeeds();
        this.RSSMultiview.ActiveViewIndex = 1;
        this.RSSWidgetTimer.Enabled = false;
    }

    private XElement _State;
    private XElement State
    {
        get
        {
            if( _State == null ) _State = XElement.Parse(this._Host.GetState());
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
        get { return State.Element("count") == null ? 3 : int.Parse( State.Element("count").Value ); }
        set 
        { 
            if( State.Element("count") == null ) 
                State.Add( new XElement("count", value) );
            else 
                State.Element("count").Value = value.ToString(); 
        }        
    }

    private void ShowFeeds()
    {
        string url = State.Element("url").Value;
        int count = State.Element("count") == null ? 3 : int.Parse( State.Element("count").Value );
        
        /*
        var feed = Cache[url] as XElement;
        if( feed == null )
        {
            if( Cache[url] == string.Empty ) return;
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                
                request.Timeout = 15000;
                using( WebResponse response = request.GetResponse() )
                {
                    XmlTextReader reader = new XmlTextReader( response.GetResponseStream() );

                    feed = XElement.Load(reader);

                    if( feed == null ) return;

                    Cache.Insert(url, feed, null, DateTime.MaxValue, TimeSpan.FromMinutes(15));
                }
           
            }
            catch
            {
                Cache[url] = string.Empty;
                return;
            }
        }

        XNamespace ns = "http://www.w3.org/2005/Atom";
        
        // see if RSS or Atom
        if( feed.Element("channel" ) != null )
            FeedList.DataSource = (from item in feed.Element("channel").Elements("item")
                              select new 
                              { 
                                  title = item.Element("title").Value, 
                                  link = item.Element("link").Value,
                                  description = item.Element("description").Value
                              }).Take(this.Count);
            
        else if( feed.Element(ns + "entry") != null )
            FeedList.DataSource = (from item in feed.Elements(ns + "entry")
                              select new 
                              { 
                                  title = item.Element(ns + "title").Value, 
                                  link = item.Element(ns + "link").Attribute("href").Value,
                                  description = item.Element(ns + "content").Value
                              }).Take(this.Count);

        */

        using (var proxy = new Dropthings.Web.Framework.ProxyAsync())
        {
            var items = proxy.GetRss(url, count, 10);
            FeedList.DataSource = items;
        }

        FeedList.DataBind();

        //Cache[url] = feed;
    }

    void IWidget.Init(IWidgetHost host)
    {
        this._Host = host;
    }

    void IWidget.ShowSettings()
    {
        SettingsPanel.Visible = true;
        FeedUrl.Text = this.Url;
        FeedCountDropDownList.SelectedIndex = -1;
        FeedCountDropDownList.Items.FindByText( this.Count.ToString() ).Selected = true;
    }
    void IWidget.HideSettings()
    {
        SettingsPanel.Visible = false;
        this.Count = int.Parse( FeedCountDropDownList.SelectedValue );
        this.Url = FeedUrl.Text;
        this.SaveState();
        this.ShowFeeds();
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

    protected void SaveSettings_Click( object sender, EventArgs e )
    {
        this._Host.HideSettings();
    }

    private void SaveState()
    {
        var xml = this.State.Xml();
        this._Host.SaveState(xml);
    }
}
