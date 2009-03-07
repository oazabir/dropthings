#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Widget.Framework
{
    using System;

    /// <summary>
    /// Summary description for RssItem
    /// </summary>
    [Serializable]
    public class RssItem
    {
        #region Constructors

        public RssItem()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion Constructors

        #region Properties

        public string Description
        {
            get; set;
        }

        public string Link
        {
            get; set;
        }

        public string Title
        {
            get; set;
        }

        #endregion Properties
    }
}