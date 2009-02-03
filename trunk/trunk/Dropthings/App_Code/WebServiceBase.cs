// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Web.Services;
using System.Web.Services.Protocols;

using System.Web.Script.Services;

namespace Dropthings.Web.Framework
{
    /// <summary>
    /// Summary description for WebServiceBase
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class WebServiceBase : System.Web.Services.WebService
    {
        public WebServiceBase()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected UserProfile Profile = HttpContext.Current.Profile as UserProfile;

    }
}