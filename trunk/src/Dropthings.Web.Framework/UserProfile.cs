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
    using System.Web.Profile;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;

    /// <summary>
    /// Summary description for UserProfile
    /// </summary>
    [Serializable]
    public class UserProfile : System.Web.Profile.ProfileBase
    {
        #region Constructors

        public UserProfile()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion Constructors

        #region Properties

        [SettingsAllowAnonymousAttribute(true)]
        public virtual string Fullname
        {
            get
            {
                return ((string)(this.GetPropertyValue("Fullname")));
            }
            set
            {
                this.SetPropertyValue("Fullname", value);
            }
        }

        [SettingsAllowAnonymousAttribute(true)]
        [DefaultSettingValue("true")]
        public virtual bool IsFirstVisit
        {
            get
            {
                return ((bool)(this.GetPropertyValue("IsFirstVisit")));
            }
            set
            {
                this.SetPropertyValue("IsFirstVisit", value);
            }
        }

        [SettingsAllowAnonymousAttribute(true)]
        public virtual DateTime? LastActivityAt
        {
            get
            {
                return ((DateTime?)(this.GetPropertyValue("LastActivityAt")));
            }
            set
            {
                this.SetPropertyValue("LastActivityAt", value ?? DateTime.Now);
            }
        }

        #endregion Properties
    }
}