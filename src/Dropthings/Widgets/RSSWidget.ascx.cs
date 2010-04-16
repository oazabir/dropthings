#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;

using Dropthings.Web.Framework;
using Dropthings.Widget.Framework;

public partial class Widgets_RSSWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;
    private XElement _State;

    #endregion Fields

    #region Properties

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

    public string Url
    {
        get { return State.Element("url").Value; }
        set { State.Element("url").Value = value; }
    }

    private XElement State
    {
        get
        {
            if( _State == null ) _State = XElement.Parse(this._Host.GetState());
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

        if (userClicked)
        {
            this.Count = int.Parse(FeedCountDropDownList.SelectedValue);
            this.Url = FeedUrl.Text;
            this.SaveState();
            this.ShowFeeds();
        }
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
            FeedUrl.Text = this.Url;
            FeedCountDropDownList.SelectedIndex = -1;
            FeedCountDropDownList.Items.FindByText(this.Count.ToString()).Selected = true;
        }
    }

    protected void LoadRSSView(object sender, EventArgs e)
    {
        this.ShowFeeds();
        this.RSSMultiview.ActiveViewIndex = 1;
        this.RSSWidgetTimer.Enabled = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // If we already have the URL in cache, then we can directly render it instead of fetching the content
        // after a async postback
        if (Page.IsPostBack || ProxyAsync.IsUrlInCache(this.Url) )
            this.LoadRSSView(sender, e);
    }

    protected void SaveSettings_Click( object sender, EventArgs e )
    {
        this._Host.HideSettings(true);
    }

    private void SaveState()
    {
        var xml = this.State.Xml();
        this._Host.SaveState(xml);
    }

    private void ShowFeeds()
    {
        string url = State.Element("url").Value;
        int count = State.Element("count") == null ? 3 : int.Parse( State.Element("count").Value );

        using (var proxy = new Dropthings.Web.Framework.ProxyAsync())
        {
            var items = proxy.GetRss(url, count, 10);
			if (null == items)
			{
				Message.Text = "There was a problem loading the feed";
				Message.Visible = true;
			}
			else
			{
				FeedList.DataSource = items;
			}
        }

        FeedList.DataBind();

        //Cache[url] = feed;
    }

    #endregion Methods
}