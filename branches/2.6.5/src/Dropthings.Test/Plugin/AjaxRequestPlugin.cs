namespace Dropthings.Test.Plugins
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using Dropthings.Test.Rules;

    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;

    public class AjaxRequestPlugin : WebTestRequestPlugin
    {
        #region Fields

        private static readonly Regex _FindContextParamJSON = new Regex(@"{{\w+}}", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        #endregion Fields

        #region Methods

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            base.PreRequest(sender, e);

            if (e.Request.Method == "POST")
            {
                FormPostHttpBody form = e.Request.Body as FormPostHttpBody;
                FormPostParameter param = form.FormPostParameters[0];

                string value = RuleHelper.ResolveContextValue(e.WebTest.Context, param.Value);

                string json = param.Name == "JSON" ? param.Value : this.BuildJSON(e.WebTest.Context, value);
                e.Request.Body = new JsonRequestBody(json);

            }
        }

        private string BuildJSON(WebTestContext context, string json)
        {
            // Replace all context params
            return _FindContextParamJSON.Replace(json, new MatchEvaluator((match) => context[match.Value] as string));
        }

        #endregion Methods
    }

    public class JsonRequestBody : IHttpBody
    {
        #region Fields

        private string _JSON;

        #endregion Fields

        #region Constructors

        public JsonRequestBody(string json)
        {
            this._JSON = json;
        }

        #endregion Constructors

        #region Properties

        public string ContentType
        {
            get { return "application/json; charset=utf-8"; }
        }

        public string JSON
        {
            get { return _JSON; }
            set { _JSON = value; }
        }

        #endregion Properties

        #region Methods

        public object Clone()
        {
            return new JsonRequestBody(this._JSON);
        }

        public void WriteHttpBody(WebTestRequest request, Stream bodyStream)
        {
            // Convert .NET unicode string to UTF8 byte array
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(this._JSON);
            byte[] utf = System.Text.Encoding.Convert(Encoding.Unicode, request.Encoding, unicodeBytes);
            bodyStream.Write(utf, 0, utf.Length);
        }

        #endregion Methods
    }
}