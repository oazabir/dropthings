#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

using System.Runtime.Serialization;

namespace Dropthings.Web.Util
{
    using System;

    /// <summary>
    /// Summary description for RssItem
    /// </summary>
    [DataContract]
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

        [DataMember()]
        public string Description
        {
            get; set;
        }

        [DataMember()]
        public string Link
        {
            get; set;
        }

        [DataMember()]
        public string Title
        {
            get; set;
        }

        #endregion Properties
    }
}