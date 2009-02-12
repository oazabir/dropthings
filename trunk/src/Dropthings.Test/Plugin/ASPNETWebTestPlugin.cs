namespace Dropthings.Test.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

                    RuleHelper.NotAlreadyExtracted<ExtractPostbackNames>(e.Request.ExtractionRuleReferences, () =>
                    {
                        ExtractPostbackNames.ExtractPostBackNames(e.Response.BodyString, e.WebTest.Context);
                    });
                    RuleHelper.NotAlreadyExtracted<ExtractUpdatePanels>(e.Request.ExtractionRuleReferences, () =>
                    {
                        ExtractUpdatePanels.ExtractUpdatePanelNames(e.Response.BodyString, e.WebTest.Context);
                    });
                });
        }

        void Request_ValidateResponse(object sender, ValidationEventArgs e)
        {
        }

        #endregion Methods
    }
}