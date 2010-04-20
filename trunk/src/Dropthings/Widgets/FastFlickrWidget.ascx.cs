#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
using Dropthings.Widget.Widgets;
using Dropthings.Web.Util;
using Dropthings.Util;

public partial class Widgets_FastFlickrWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private const string ENTER_TAG = "http://www.flickr.com/services/rest/?method=flickr.photos.search&api_key="+FLICKR_API_KEY+"&tags=";
    private const string FIND_BY_EMAIL = "http://www.flickr.com/services/rest/?method=flickr.people.findByEmail&api_key="+FLICKR_API_KEY+"&find_email=";
    private const string FIND_BY_USERNAME = "http://www.flickr.com/services/rest/?method=flickr.people.findByUsername&api_key="+FLICKR_API_KEY+"&username=";
    private const string FLICKR_API_KEY = "c705bfbf75e8d40f584c8a946cf0834c";
    private const string INTERESTING = "http://www.flickr.com/services/rest/?method=flickr.interestingness.getList&api_key="+FLICKR_API_KEY;
    private const string MOST_RECENT = "http://www.flickr.com/services/rest/?method=flickr.photos.getRecent&api_key="+FLICKR_API_KEY;
    private const string PHOTOS_FROM_FLICKR_USER = "http://www.flickr.com/services/rest/?method=flickr.people.getPublicPhotos&api_key="+FLICKR_API_KEY+"&user_id=";

    private IWidgetHost _Host;
    private XElement _State;

    #endregion Fields

    #region Enumerations

    public enum PhotoTypeEnum
    {
        MostRecent = 0,
        MostPopular = 1,
        Tag = 2
    }

    #endregion Enumerations

    #region Properties

    public string PhotoTag
    {
        get { return State.Element("tag").Value; }
        set { State.Element("tag").Value = value; }
    }

    public PhotoTypeEnum TypeOfPhoto
    {
        get { return (PhotoTypeEnum)Enum.Parse( typeof( PhotoTypeEnum ), State.Element("type").Value ); }
        set { State.Element("type").Value = value.ToString(); }
    }

    private int PageIndex
    {
        get
        {
            return (int)(ViewState[this.ClientID + "_PageIndex"] ?? 0);
        }
        set { ViewState[this.ClientID + "_PageIndex"] = value; }
    }

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
        settingsPanel.Visible = false;
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
        settingsPanel.Visible = true;
        if (userClicked)
        {
            this.LoadState();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        var cachedJSON = GetCachedJSON();

        var scriptToLoad = ResolveClientUrl("~/Widgets/FastFlickrWidget.js?v=" + ConstantHelper.ScriptVersionNo);
        var startUpCode = string.Format("window.flickrLoader{0} = new FastFlickrWidget('{1}', '{2}', '{3}', '{4}', {5}); window.flickrLoader{0}.load();",
                this._Host.ID, this.GetPhotoUrl(), this.FlickrPhotoPanel.ClientID,
                this.ShowPrevious.ClientID, this.ShowNext.ClientID, cachedJSON ?? "null");

        WidgetHelper.RegisterWidgetScript(this, scriptToLoad, startUpCode);

        this.ShowPrevious.OnClientClick = string.Format("window.flickrLoader{0}.previous(); return false;", this._Host.ID);
        this.ShowNext.OnClientClick = string.Format("window.flickrLoader{0}.next(); return false;", this._Host.ID);
    }

    protected string PageLeftScript()
    {
        return string.Format("window.flickrLoader{0}.previous()", this.UniqueID);
    }

    protected string PageRightScript()
    {
        return string.Format("window.flickrLoader{0}.next()", this.UniqueID);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void ShowTagButton_Clicked(object sender, EventArgs e)
    {
        this.PhotoTag = this.CustomTagTextBox.Text;
        this.SaveState();
        settingsPanel.Visible = true;
    }

    protected void photoTypeRadio_CheckedChanged(object sender, EventArgs e)
    {
        this.SaveState();
        settingsPanel.Visible = true;        
    }

    private string GetCachedJSON()
    {
        if (ProxyAsync.IsUrlInCache(this.GetPhotoUrl()))
        {
            var cachedString = new ProxyAsync().GetString(this.GetPhotoUrl(), 10);
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(cachedString);
            return json;
        }
        else
            return null;
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
    }

    #endregion Methods
}