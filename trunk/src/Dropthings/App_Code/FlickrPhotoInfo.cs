// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar
namespace Dropthings.Widget.Widgets.Flickr
{
    public class FlickrPhotoInfo
    {
        #region Fields

        public string Farm;
        public string Id;
        public bool IsFamily;
        public bool IsFriend;
        public bool IsPublic;
        public string Owner;
        public string Secret;
        public string Server;
        public string Title;

        private const string FLICKR_PHOTO_URL = "http://www.flickr.com/photos/";
        private const string FLICKR_SERVER_URL = "http://static.flickr.com/";

        #endregion Fields

        #region Properties

        public string PhotoPageUrl
        {
            get { return FLICKR_PHOTO_URL + this.Owner + '/' + this.Id; }
        }

        #endregion Properties

        #region Methods

        public string PhotoUrl(bool small)
        {
            return FLICKR_SERVER_URL + this.Server + '/' + this.Id + '_' + this.Secret + (small ? "_s.jpg" : "_m.jpg");
        }

        #endregion Methods
    }
}