namespace Dropthings.Test.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    using Dropthings.Test.Rules;

    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;

    public class ASPNETWebTestPlugin : WebTestPlugin
    {
        #region Fields

        public const string STEP_NO_KEY = "$WEBTEST.StepNo";

        private const string TEMPORARY_STORE_EXTRACTION_RULE_KEY = "$TEMP.ExtractionRules";

        #endregion Fields

        #region Methods

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            base.PostRequest(sender, e);

            int stepNo = (int)e.WebTest.Context[STEP_NO_KEY];
            e.WebTest.Context[STEP_NO_KEY] = stepNo + 1;

            // When cookies are issues domain wide like .static.dropthings.com, it does not get
            // added to the Cookie container properly so that the cookie is sent out on static.dropthings.com
            // visit
            foreach (Cookie cookie in e.Response.Cookies)
            {
                if (cookie.Domain.StartsWith("."))
                {
                    CookieContainer container = e.WebTest.Context.CookieContainer;
                    cookie.Domain = cookie.Domain.TrimStart('.');
                    //container.Add(new Uri(e.Response.ResponseUri.GetLeftPart(UriPartial.Authority)), cookie);
                    container.Add(cookie);
                }
            }
        }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            base.PreRequest(sender, e);
            e.Request.ExtractValues += new EventHandler<ExtractionEventArgs>(Request_ExtractValues);

            if (!e.WebTest.Context.ContainsKey(STEP_NO_KEY))
                e.WebTest.Context[STEP_NO_KEY] = 1;
        }

        void Request_ExtractValues(object sender, ExtractionEventArgs e)
        {
            RuleHelper.WhenAspNetResponse(e.Response, () =>
                {
                    RuleHelper.NotAlreadyExtracted<ExtractHiddenFields>(e.Request.ExtractionRuleReferences, () =>
                    {
                        // Extract all hidden fields so that they can be used in next async/sync postback
                        // Hidden fields like __EVENTVALIDATION, __VIEWSTATE changes after every async/sync
                        // postback. So, these fields need to be kept up-to-date to make subsequent async/sync
                        // postback
                        var extractionRule = new ExtractHiddenFields();
                        extractionRule.Required = true;
                        extractionRule.HtmlDecode = true;
                        extractionRule.ContextParameterName = "1";
                        extractionRule.Extract(sender, e);
                    });

                    // Extract all INPUT/SELECT elements so that they can be posted in (async)postback
                    RuleHelper.NotAlreadyExtracted<ExtractFormElements>(e.Request.ExtractionRuleReferences, () =>
                        {
                            new ExtractFormElements().Extract(sender, e);
                        });

                    // Extract all __doPostBack(...) so that the ID of the control can be used to make async
                    // postbacks
                    RuleHelper.NotAlreadyExtracted<ExtractPostbackNames>(e.Request.ExtractionRuleReferences, () =>
                        {
                            new ExtractPostbackNames().Extract(sender, e);
                        });

                    // Extract all updatepanels so that during async postback, the updatepanel name can be derived
                    RuleHelper.NotAlreadyExtracted<ExtractUpdatePanels>(e.Request.ExtractionRuleReferences, () =>
                        {
                            new ExtractUpdatePanels().Extract(sender, e);
                        });
                });
        }

        void Request_ValidateResponse(object sender, ValidationEventArgs e)
        {
        }

        #endregion Methods
    }
}