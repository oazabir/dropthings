namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;

    [DisplayName("Check Cookie From Response")]
    public class CookieValidationRule : ValidationRule
    {
        #region Fields

        private string _CookieName;
        private string _CookieValueToMatch = string.Empty;
        private string _Domain;
        private bool _Exists = true;
        private int _Index;
        private bool _IsPersistent;
        private bool _MatchValue = false;
        private bool _StopOnError = true;

        #endregion Fields

        #region Properties

        public string CookieName
        {
            get { return _CookieName;}
            set { _CookieName = value;}
        }

        public string CookieValueToMatch
        {
            get { return _CookieValueToMatch; }
            set { _CookieValueToMatch = value; }
        }

        public string Domain
        {
            get { return _Domain; }
            set { _Domain = value; }
        }

        [DefaultValue(true)]
        public bool Exists
        {
            get { return _Exists; }
            set { _Exists = value; }
        }

        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        public bool IsPersistent
        {
            get { return _IsPersistent; }
            set { _IsPersistent = value; }
        }

        public bool MatchValue
        {
            get { return _MatchValue; }
            set { _MatchValue = value; }
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
                Cookie cookie = RuleHelper.GetCookie(this.CookieName, this.Domain, e.Response.Cookies);
                if (!this.Exists)
                {
                    if (null != cookie)
                    {
                        e.IsValid = false;
                        e.Message = this.CookieName + " must not be in response";
                        return;
                    }
                }
                else
                {
                    // If no cookie found, it fails
                    if (null == cookie)
                    {
                        e.IsValid = false;
                        e.Message = this.CookieName + " cookie not found";
                        return;
                    }

                    // Cookie will have expires set in future if it's a persistent cookie
                    if (this.IsPersistent)
                    {
                        if (DateTime.Now > cookie.Expires)
                        {
                            e.IsValid = false;
                            e.Message = "Cookie does not expire in future. It has expired on: " + cookie.Expires.ToString();
                            return;
                        }
                    }
                    else
                    {
                        // Cookie must be non-persistent

                        if (cookie.TimeStamp < cookie.Expires)
                        {
                            e.IsValid = false;
                            e.Message = "Cookie must not be persistent. It has expiry on: " + cookie.Expires.ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(this.Domain))
                    {
                        if (this.Domain != cookie.Domain)
                        {
                            e.IsValid = false;
                            e.Message = "Cookie domain does not match. Found: " + cookie.Domain + " expected: " + this.Domain;
                            return;
                        }
                    }
                    else
                    {
                        if (cookie.Domain != string.Empty && cookie.Domain != e.Response.ResponseUri.Host)
                        {
                            e.IsValid = false;
                            e.Message = "Cookie must not have any domain or should be '" + e.Response.ResponseUri.Host + "', but it has '" + cookie.Domain + "'";
                        }
                    }

                    if (this.MatchValue)
                    {
                        if (cookie.Value != this.CookieValueToMatch)
                        {
                            e.IsValid = false;
                            e.Message = "Cookie value does not match. Expected: [" + this.CookieValueToMatch + "] Found [" + cookie.Value + "]";
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}