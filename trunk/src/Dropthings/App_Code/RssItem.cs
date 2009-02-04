// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace Dropthings.Widget.Widgets.RSS
{
    /// <summary>
    /// Summary description for RssItem
    /// </summary>
    [Serializable]
    public class RssItem
    {
        public RssItem()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
    }
}