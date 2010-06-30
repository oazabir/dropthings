namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;

    [DisplayName("Ensure Cookie Set to Expire")]
    public class CookieSetToExpire : ValidationRule
    {
        #region Fields

        private string _CookieName = string.Empty;
        private string _Domain;
        private bool _StopOnError = true;

        #endregion Fields

        #region Properties

        public string CookieName
        {
            get { return _CookieName; }
            set { _CookieName = value; }
        }

        public string Domain
        {
            get { return _Domain; }
            set { _Domain = value; }
        }

        public bool StopOnError
        {
            get { return _StopOnError; }
            set { _StopOnError = value; }
        }

        #endregion Properties

        #region Methods

        public override void Validate(object sender, ValidationEventArgs e)
        {
            using (new RuleCheck(e, this.StopOnError))
            {
                Cookie cookie = e.Response.Cookies[this.CookieName];

                if (cookie != null)
                {
                    if (cookie.Expires < DateTime.Now)
                    {
                        // Success, cookie is set to expire

                        // Check if cookie domain matches if domain is specified
                        if (!string.IsNullOrEmpty(this.Domain))
                        {
                            if (cookie.Domain != this.Domain)
                            {
                                e.IsValid = false;
                                e.Message = "Cookie is set to expire on domain '" + cookie.Domain + "' but it needs to expire on '" + this.Domain + "'";
                            }
                        }
                        else
                        {
                            // If domain is not specified, then cookie must expire on current domain
                            if (!string.IsNullOrEmpty(cookie.Domain) && cookie.Domain != e.Response.ResponseUri.Host)
                            {
                                e.IsValid = false;
                                e.Message = "Cookie is set to expire on domain '" + cookie.Domain + "' but it needs to expire on '" + e.Response.ResponseUri.Host + "'";
                            }
                        }
                    }
                    else
                    {
                        // Fail, cookie is not set to expire
                        e.IsValid = false;
                        e.Message = this.CookieName + " " + "is not removed";
                        return;
                    }
                }
                else
                {
                    // Fail, Cookie does not exist in response
                    e.IsValid = false;
                    e.Message = this.CookieName + " " + "is not present in response";
                    return;

                }
            }
        }

        #endregion Methods
    }
}