#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

/// <summary>
/// Summary description for ActionValidator
/// </summary>
namespace Dropthings.Web.Util
{
    using Dropthings.Util;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;

    public static class ActionValidator
    {
        #region Fields

        private const int DURATION = 10; // 10 min period

        #endregion Fields

        #region Enumerations

        /*
         * Type of actions and their maximum value per period
         *
         */
        public enum ActionTypeEnum
        {
            None = 0,
            FirstVisit = 100, // The most expensive one, choose the value wisely.
            Revisit = 1000,  // Welcome to revisit as many times as user likes
            Postback = 5000,    // Not must of a problem for us
            AddNewWidget = 100,
            AddNewPage = 100,
        }

        #endregion Enumerations

        #region Methods

        public static bool IsValid( ActionTypeEnum actionType )
        {
            if (ConstantHelper.DisableDOSCheck)
                return true;

            HttpContext context = HttpContext.Current;

            // We want crawler to index Dropthings
            //if( context.Request.Browser.Crawler ) return false;

            string key = actionType.ToString() + context.Request.UserHostAddress;

            var hit = (HitInfo)(context.Cache[key] ?? new HitInfo());

            if( hit.Hits > (int)actionType ) return false;
            else hit.Hits ++;

            if( hit.Hits == 1 )
                context.Cache.Add(key, hit, null, DateTime.Now.AddMinutes(DURATION),
                    System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

            return true;
        }

        #endregion Methods

        #region Nested Types

        private class HitInfo
        {
            #region Fields

            public int Hits;

            private DateTime _ExpiresAt = DateTime.Now.AddMinutes(DURATION);

            #endregion Fields

            #region Properties

            public DateTime ExpiresAt
            {
                get { return _ExpiresAt; } set { _ExpiresAt = value; }
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}