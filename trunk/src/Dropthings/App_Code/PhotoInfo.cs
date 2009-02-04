// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

namespace Dropthings.Widget.Widgets.Flickr
{
    public class PhotoInfo
    {
        private const string FLICKR_SERVER_URL = "http://static.flickr.com/";
        private const string FLICKR_PHOTO_URL = "http://www.flickr.com/photos/";

        public string Id;
        public string Owner;
        public string Title;
        public string Secret;
        public string Server;
        public string Farm;
        public bool IsPublic;
        public bool IsFriend;
        public bool IsFamily;
        public string PhotoUrl(bool small)
        {
            return FLICKR_SERVER_URL + this.Server + '/' + this.Id + '_' + this.Secret + (small ? "_s.jpg" : "_m.jpg");
        }
        public string PhotoPageUrl
        {
            get { return FLICKR_PHOTO_URL + this.Owner + '/' + this.Id; }
        }
    }
}