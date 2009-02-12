namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    #region Enumerations

    public enum CacheModeEnum
    {
        NoCache = 0,
        Private = 1,
        Public = 2
    }

    #endregion Enumerations

    [DisplayName("Cache Header Validation")]
    public class CacheHeaderValidation : ValidationRule
    {
        #region Fields

        private bool _CacheControlNoCache;
        private bool _CacheControlPrivate;
        private bool _CacheControlPublic;
        private CacheModeEnum _CacheMode = CacheModeEnum.NoCache;
        private int _DifferenceThresholdSec = 10;
        private bool _Enabled = true;
        private int _ExpiresAfterSeconds = 0;
        private bool _StopOnError = true;

        #endregion Fields

        #region Properties

        public bool CacheControlNoCache
        {
            get { return _CacheControlNoCache; }
            set
            {
                _CacheControlNoCache = value;
                if (value)
                {
                    _CacheControlPrivate = false;
                    _CacheControlPublic = false;
                    this._CacheMode = CacheModeEnum.NoCache;
                }
            }
        }

        public bool CacheControlPrivate
        {
            get { return _CacheControlPrivate; }
            set
            {
                _CacheControlPrivate = value;
                if (value)
                {
                    _CacheControlNoCache = false;
                    _CacheControlPublic = false;
                    this._CacheMode = CacheModeEnum.Private;
                    this._ExpiresAfterSeconds = 0;
                }
            }
        }

        public bool CacheControlPublic
        {
            get { return _CacheControlPublic; }
            set
            {
                _CacheControlPublic = value;
                if (value)
                {
                    _CacheControlNoCache = false;
                    _CacheControlPrivate = false;
                    this._CacheMode = CacheModeEnum.Public;
                }
            }
        }

        public int DifferenceThresholdSec
        {
            get { return _DifferenceThresholdSec; }
            set { _DifferenceThresholdSec = value; }
        }

        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }

        public int ExpiresAfterSeconds
        {
            get { return _ExpiresAfterSeconds; }
            set { _ExpiresAfterSeconds = value; }
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
                if (!this.Enabled)
                {
                    e.Message = "Cache control validation not enabled";
                    return;
                }

                string expiresHeader = e.Response.Headers["Expires"];
                string cacheControl = e.Response.Headers["Cache-Control"];

                Dictionary<string, string> cacheDirectives = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(cacheControl))
                {
                    string[] tokens = cacheControl.Split(',');
                    foreach (string token in tokens)
                    {
                        if (token.IndexOf('=') > 0)
                        {
                            string[] pair = token.Split('=');
                            cacheDirectives.Add(pair[0].Trim(), pair[1]);
                        }
                        else
                        {
                            cacheDirectives.Add(token.Trim(), string.Empty);
                        }
                    }
                }

                if (this._CacheMode == CacheModeEnum.NoCache)
                {
                    // If expires header is present, then it should be -1
                    if (!string.IsNullOrEmpty(expiresHeader))
                    {
                        if (expiresHeader != "-1")
                        {
                            e.Message = "Expires header must be -1 if present for No Cache mode. Found: " + expiresHeader;
                            e.IsValid = false;
                            return;
                        }
                    }

                    if (cacheDirectives.ContainsKey("no-cache"))
                    {
                        // OK
                    }
                    // If Cache-Control = private, then max-age=0
                    else if (cacheDirectives.ContainsKey("private")
                        && cacheDirectives.ContainsKey("max-age")
                        && cacheDirectives["max-age"] == "0")
                    {
                        // OK
                    }
                    else
                    {
                        e.Message = "Cache-Control must be set to private, max-age=0. Found: " + cacheControl;
                        e.IsValid = false;
                        return;
                    }
                }
                else if (this._CacheMode == CacheModeEnum.Private)
                {
                    if (!cacheDirectives.ContainsKey("private"))
                    {
                        e.Message = "Cache-Control expected to be private, found: " + cacheControl;
                        e.IsValid = false;
                        return;
                    }
                }
                else if (this._CacheMode == CacheModeEnum.Public)
                {
                    if (!cacheDirectives.ContainsKey("public"))
                    {
                        e.Message = "Cache-Control expected to be public, found: " + cacheControl;
                        e.IsValid = false;
                        return;
                    }
                }

                if (this.ExpiresAfterSeconds != 0)
                {
                    if (cacheDirectives.ContainsKey("max-age")
                        && cacheDirectives["max-age"] == this.ExpiresAfterSeconds.ToString())
                    {
                        // OK
                    }
                    else
                    {
                        e.Message = "MaxAge is expected to be " + this.ExpiresAfterSeconds.ToString() + " but found: " + cacheControl;
                        e.IsValid = false;
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(expiresHeader))
                {
                    if (this.ExpiresAfterSeconds > 0)
                    {
                        DateTime nowInGMT = DateTime.Parse( e.Response.Headers["Date"] ); //DateTime.Now.ToUniversalTime();

                        DateTime expireHeaderDate = DateTime.Parse(expiresHeader); //.ToUniversalTime();

                        TimeSpan diff = expireHeaderDate - nowInGMT;
                        if (Math.Abs( diff.TotalSeconds - this.ExpiresAfterSeconds) < this.DifferenceThresholdSec )
                        {
                            e.Message = "Expiration date is set correctly";
                            e.IsValid = true;
                            return;
                        }
                        else
                        {
                            e.Message = "Expires header should be after '" + nowInGMT.AddSeconds(this.ExpiresAfterSeconds).ToString() + "', but found: '" + expireHeaderDate.ToString() + "'. Difference: " + (expireHeaderDate - nowInGMT).ToString();
                            e.IsValid = false;
                            return;
                        }

                        /*
                        DateTime expectedDate = nowInGMT.AddSeconds(this.ExpiresAfterSeconds);

                        // The expires header is generated some milliseconds earlier.
                        // So, the expires header will be couple of milliseconds earlier than
                        // the expecteddate. However, the diff should not be more than 10 sec
                        if ((expectedDate - expireHeaderDate) < TimeSpan.FromSeconds(10))
                        {
                            // OK, the expiration date is after the expire threshold
                        }
                        else
                        {
                            e.Message = "Expires header should be after '" + expectedDate.ToString() + "', but found: '" + expireHeaderDate.ToString() + "'. Difference: " + (expireHeaderDate - expectedDate).ToString();
                            e.IsValid = false;
                            return;
                        }
                         * */
                    }
                }
            }
        }

        #endregion Methods
    }
}