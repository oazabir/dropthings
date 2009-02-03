// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using Dropthings.Widget.Widgets;
using Dropthings.Widget.Framework;
using Dropthings.Web.Framework;


public partial class Widgets_FastFlickrWidget : System.Web.UI.UserControl, IWidget
{

    private const string FLICKR_API_KEY = "c705bfbf75e8d40f584c8a946cf0834c";
    private const string MOST_RECENT ="http://www.flickr.com/services/rest/?method=flickr.photos.getRecent&api_key="+FLICKR_API_KEY;
    private const string INTERESTING = "http://www.flickr.com/services/rest/?method=flickr.interestingness.getList&api_key="+FLICKR_API_KEY;
    private const string ENTER_TAG ="http://www.flickr.com/services/rest/?method=flickr.photos.search&api_key="+FLICKR_API_KEY+"&tags=";
    private const string FIND_BY_USERNAME = "http://www.flickr.com/services/rest/?method=flickr.people.findByUsername&api_key="+FLICKR_API_KEY+"&username=";
    private const string FIND_BY_EMAIL = "http://www.flickr.com/services/rest/?method=flickr.people.findByEmail&api_key="+FLICKR_API_KEY+"&find_email=";
    private const string PHOTOS_FROM_FLICKR_USER = "http://www.flickr.com/services/rest/?method=flickr.people.getPublicPhotos&api_key="+FLICKR_API_KEY+"&user_id=";

    private IWidgetHost _Host;

    private int PageIndex
    {
        get 
        { 
            return (int)(ViewState[this.ClientID + "_PageIndex"] ?? 0);
        }
        set { ViewState[this.ClientID + "_PageIndex"] = value; }
    }

    private XElement _State;
    private XElement State
    {
        get
        {
            if( _State == null )
            {
                string stateXml = this._Host.GetState();
                if (string.IsNullOrEmpty(stateXml))
                {
                    //stateXml = "<state><type>MostPopular</type><tag /></state>";
                    _State = new XElement("state",
                        new XElement("type", "MostPopular"),
                        new XElement("tag", ""));
                }
                else
                {
                    _State = XElement.Parse(stateXml);
                }
            }
            return _State;
        }
    }

    public enum PhotoTypeEnum
    {
        MostRecent = 0,
        MostPopular = 1,
        Tag = 2
    }

    public PhotoTypeEnum TypeOfPhoto
    {
        get { return (PhotoTypeEnum)Enum.Parse( typeof( PhotoTypeEnum ), State.Element("type").Value ); }
        set { State.Element("type").Value = value.ToString(); }
    }
    public string PhotoTag
    {
        get { return State.Element("tag").Value; }
        set { State.Element("tag").Value = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this._Host.IsFirstLoad)
        {
            ScriptManager.RegisterClientScriptInclude(this,
            typeof(Widgets_FastFlickrWidget),
            "FastFlickrWidget",
            this.ResolveClientUrl(this.AppRelativeTemplateSourceDirectory + "FastFlickrWidget.js"));

            var cachedJSON = GetCachedJSON();

            // Create a script block which will initialize an instance of FastFlickrWidget javascript class on the 
            // client and then load photo xml by calling Proxy.GetXml
            ScriptManager.RegisterStartupScript(this, typeof(Widgets_FastFlickrWidget), "LoadFlickr",
                string.Format("var flickrLoader{0} = new FastFlickrWidget( '{1}', '{2}', '{3}', '{4}', {5} ); flickrLoader{0}.load();",
                    this._Host.ID, this.GetPhotoUrl(), this.FlickrPhotoPanel.ClientID, 
                    this.ShowPrevious.ClientID, this.ShowNext.ClientID, cachedJSON ?? "null"), true);
        }
    }

    private string GetCachedJSON()
    {
        if (ProxyAsync.IsUrlInCache(Cache, this.GetPhotoUrl()))
        {
            var cachedString = new ProxyAsync().GetString(this.GetPhotoUrl(), 10);
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(cachedString);
            return json;
        }
        else
            return null;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if( !this._Host.IsFirstLoad )
        ScriptManager.RegisterStartupScript(this, typeof(Widgets_FastFlickrWidget), "LoadFlickr",
            string.Format("flickrLoader{0}.url = '{1}'; flickrLoader{0}.load();",
                this._Host.ID, this.GetPhotoUrl(), this.FlickrPhotoPanel.ClientID), true);

        this.ShowPrevious.OnClientClick = string.Format("flickrLoader{0}.previous(); return false;", this._Host.ID);
        this.ShowNext.OnClientClick = string.Format("flickrLoader{0}.next(); return false;", this._Host.ID);
}

    private void LoadState()
    {
        if (this.TypeOfPhoto == PhotoTypeEnum.MostPopular)
        {
            mostInterestingRadioButton.Checked = true;
            mostRecentRadioButton.Checked = false;
            customTagRadioButton.Checked = false;
        }
        else if (this.TypeOfPhoto == PhotoTypeEnum.MostRecent)
        {
            mostRecentRadioButton.Checked = true;
            mostInterestingRadioButton.Checked = false;
            customTagRadioButton.Checked = false;
        }
        else
        {
            mostRecentRadioButton.Checked = false;
            mostInterestingRadioButton.Checked = false;
            customTagRadioButton.Checked = true;
            CustomTagTextBox.Text = this.PhotoTag;
        }
    }

    void IWidget.Init(IWidgetHost host)
    {
        this._Host = host;
    }

    void IWidget.ShowSettings()
    {
        settingsPanel.Visible = true;

        this.LoadState();
        
    }
    void IWidget.HideSettings()
    {
        settingsPanel.Visible = false;
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
    protected void photoTypeRadio_CheckedChanged(object sender, EventArgs e)
    {
        this.SaveState();
    }

    private void SaveState()
    {
        if( mostRecentRadioButton.Checked )
            this.TypeOfPhoto = PhotoTypeEnum.MostRecent;
        else if( mostInterestingRadioButton.Checked )
            this.TypeOfPhoto = PhotoTypeEnum.MostPopular;
        else if( customTagRadioButton.Checked )
        {
            this.TypeOfPhoto = PhotoTypeEnum.Tag;
            this.PhotoTag = this.CustomTagTextBox.Text;
        }

        this._Host.SaveState(this.State.Xml());
        this.PageIndex = 0;
        this._State = null;
    }

    private string GetPhotoUrl()
    {
        string url = MOST_RECENT;
        if (this.TypeOfPhoto == PhotoTypeEnum.Tag)
            url = ENTER_TAG + this.PhotoTag;
        else if (this.TypeOfPhoto == PhotoTypeEnum.MostPopular)
            url = INTERESTING;

        return url;
    }
    
    protected string PageLeftScript()
    {
        return string.Format("flickrLoader{0}.previous()", this.UniqueID);
    }

    protected string PageRightScript()
    {
        return string.Format("flickrLoader{0}.next()", this.UniqueID);
    }

    protected void ShowTagButton_Clicked(object sender, EventArgs e)
    {
        this.PhotoTag = this.CustomTagTextBox.Text;
        this.SaveState();
    }
}
