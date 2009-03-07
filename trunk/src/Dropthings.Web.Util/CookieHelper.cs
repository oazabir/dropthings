#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

/// <summary>
/// Summary description for ActionValidator
/// </summary>
namespace Dropthings.Web.Util
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;

    public static class CookieHelper
    {
        #region Methods

        public static void Clear()
        {
            HttpContext context = HttpContext.Current;

            List<string> cookiesToClear = new List<string>();
            foreach (string cookieName in context.Request.Cookies)
            {
                HttpCookie cookie = context.Request.Cookies[cookieName];
                cookiesToClear.Add(cookie.Name);
            }

            foreach (string name in cookiesToClear)
            {
                HttpCookie cookie = new HttpCookie(name, string.Empty);
                cookie.Expires = DateTime.Today.AddYears(-1);

                context.Response.Cookies.Set(cookie);
            }
        }

        #endregion Methods
    }
}