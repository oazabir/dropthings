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
using Dropthings.Widget.Widgets.Flickr;

public partial class FlickrWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private const string ENTER_TAG = "http://www.flickr.com/services/rest/?method=flickr.photos.search&api_key="+FLICKR_API_KEY+"&tags=";
    private const string FIND_BY_EMAIL = "http://www.flickr.com/services/rest/?method=flickr.people.findByEmail&api_key="+FLICKR_API_KEY+"&find_email=";
    private const string FIND_BY_USERNAME = "http://www.flickr.com/services/rest/?method=flickr.people.findByUsername&api_key="+FLICKR_API_KEY+"&username=";
    private const string FLICKR_API_KEY = "c705bfbf75e8d40f584c8a946cf0834c";
    private const string INTERESTING = "http://www.flickr.com/services/rest/?method=flickr.interestingness.getList&api_key="+FLICKR_API_KEY;
    private const string MOST_RECENT = "http://www.flickr.com/services/rest/?method=flickr.photos.getRecent&api_key="+FLICKR_API_KEY;
    private const string PHOTOS_FROM_FLICKR_USER = "http://www.flickr.com/services/rest/?method=flickr.people.getPublicPhotos&api_key="+FLICKR_API_KEY+"&user_id=";

    private int Columns = 3;
    private int Rows = 3;
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

    void IWidget.HideSettings()
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

    void IWidget.ShowSettings()
    {
        settingsPanel.Visible = true;

        this.LoadState();
    }

    protected void LoadPhotoView(object sender, EventArgs e)
    {
        this.ShowPictures(this.PageIndex);

        this.FlickrWidgetMultiview.ActiveViewIndex = 1;
        this.FlickrWidgetTimer.Enabled = false;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (Page.IsPostBack)
            this.ShowPictures(this.PageIndex);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || ProxyAsync.IsUrlInCache(this.GetPhotoUrl()))
            this.LoadPhotoView(this, e);
    }

    protected void ShowNext_Click(object sender, EventArgs e)
    {
        this.PageIndex ++;
        this.ShowPictures(this.PageIndex);
    }

    protected void ShowPrevious_Click(object sender, EventArgs e)
    {
        this.PageIndex --;
        this.ShowPictures(this.PageIndex);
    }

    protected void ShowTagButton_Clicked(object sender, EventArgs e)
    {
        this.PhotoTag = this.CustomTagTextBox.Text;
        this.SaveState();
    }

    protected void photoTypeRadio_CheckedChanged(object sender, EventArgs e)
    {
        this.SaveState();
        this.ShowPictures(this.PageIndex);
    }

    private string GetPhotoUrl()
    {
        string url = MOST_RECENT;

        if (this.TypeOfPhoto == PhotoTypeEnum.Tag)
            url = ENTER_TAG + this.PhotoTag;
        else if (this.TypeOfPhoto == PhotoTypeEnum.MostPopular)
            url = INTERESTING;
        else
            url = MOST_RECENT;

        return url;
    }

    private string LoadPictures()
    {
        string cachedXml = new ProxyAsync().GetXml(GetPhotoUrl(), 10);
        return cachedXml;
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
        this._State = null;
    }

    private void ShowPictures(int pageIndex)
    {
        var xml = this.LoadPictures();
        if( string.IsNullOrEmpty(xml) ) return;
        var xroot = XElement.Parse(xml);
        var photos = (from photo in xroot.Element("photos").Elements("photo")
                    select new PhotoInfo
                    {
                        Id = (string)photo.Attribute("id"),
                        Owner = (string)photo.Attribute("owner"),
                        Title = (string)photo.Attribute("title"),
                        Secret = (string)photo.Attribute("secret"),
                        Server = (string)photo.Attribute("server"),
                        Farm = (string)photo.Attribute("Farm"),
                        /*IsPublic = (bool)photo.Attribute("ispublic"),
                        IsFriend = (bool)photo.Attribute("isfriend"),
                        IsFamily = (bool)photo.Attribute("isfamily")*/
                    }).Skip(pageIndex*Columns*Rows).Take(Columns*Rows);

        HtmlTable table = new HtmlTable();
        table.Align = "center";
        var row = 0;
        var col = 0;
        var count = 0;
        foreach( var photo in photos )
        {
            if( col == 0 )
                table.Rows.Add( new HtmlTableRow() );

            var cell = new HtmlTableCell();

            var div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "preview");

            var img = new HtmlImage();
            img.Src = photo.PhotoUrl(true);
            //img.Width = img.Height = 75;
            img.Border = 0;
            img.Attributes.Add("class", "preview");
            //img.Attributes.Add("onmouseover", "Zoom.larger(this, 150, 150)");
            //img.Attributes.Add("onmouseout", "Zoom.smaller(this, 150, 150)");

            var link = new HtmlGenericControl("a");
            link.Attributes["href"] = photo.PhotoPageUrl;
            link.Attributes["target"] = "_blank";
            link.Attributes["title"] = photo.Title;

            link.Controls.Add(img);
            div.Controls.Add(link);
            cell.Controls.Add(div);

            table.Rows[row].Cells.Add(cell);

            col ++;
            if( col == Columns )
            {
                col = 0; row ++;
            }

            count ++;
        }

        photoPanel.Controls.Clear();
        photoPanel.Controls.Add(table);

        if( pageIndex == 0 )
        {
            this.ShowPrevious.Visible = false;
            this.ShowNext.Visible = true;
        }
        else
        {
            this.ShowPrevious.Visible = true;
        }
        if( count < Columns*Rows )
        {
            this.ShowNext.Visible = false;
        }
    }

    #endregion Methods
}