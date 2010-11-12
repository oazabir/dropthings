#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Web.Framework
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Security;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;

    /// <summary>
    /// Summary description for WebServiceBase
    /// </summary>
    [WebService(Namespace = "http://dropthings.omaralzabir.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class WebServiceBase : System.Web.Services.WebService
    {
        #region Fields

        protected UserProfile Profile = HttpContext.Current.Profile as UserProfile;

        #endregion Fields

        #region Constructors

        public WebServiceBase()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion Constructors
    }
}