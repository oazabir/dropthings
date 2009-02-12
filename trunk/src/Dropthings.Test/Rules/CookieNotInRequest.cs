namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;

    [DisplayName("Ensure Cookie Not In Request")]
    public class CookieNotInRequest : ValidationRule
    {
        #region Fields

        private string _CookieName = string.Empty;
        private bool _StopOnError = true;

        #endregion Fields

        #region Properties

        public string CookieName
        {
            get { return _CookieName; }
            set { _CookieName = value; }
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

                if (cookie != null)
                {
                    e.IsValid = false;
                    e.Message = this.CookieName + " " + "is not removed";
                    return;
                }
                else
                {
                    // Cookie does not exist in request, so it has been removed
                }
            }
        }

        #endregion Methods
    }
}