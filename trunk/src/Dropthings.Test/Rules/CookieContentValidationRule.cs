namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;

    [DisplayName("Validate Cookie Value")]
    public class CookieContentValidationRule : ValidationRule
    {
        #region Fields

        private string _CookieName = string.Empty;
        private string _CookieValue = string.Empty;
        private bool _StopOnError = true;

        #endregion Fields

        #region Properties

        public string CookieName
        {
            get { return _CookieName; }
            set { _CookieName = value; }
        }

        public string CookieValue
        {
            get { return _CookieValue; }
            set { _CookieValue = value; }
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
                Cookie cookie = e.Request.Cookies[this.CookieName];

                // If no cookie found, it fails
                if (null == cookie)
                {
                    e.IsValid = false;
                    e.Message = this.CookieName + " cookie not found";
                    return;
                }

                string cookieVlaue = cookie.Value;

                if (!(string.Compare(cookieVlaue, this.CookieValue, true) == 0))
                {
                    e.IsValid = false;
                    e.Message = "Cookie content does not match";
                }
            }
        }

        #endregion Methods
    }
}