namespace Dropthings.Test.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    using Dropthings.Test.Rules;

    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Microsoft.VisualStudio.TestTools.WebTesting.Rules;

    public class AsyncPostbackRequestPlugin : WebTestRequestPlugin
    {
        #region Properties

        public string ControlName
        {
            get; set;
        }

        public string UpdatePanelName
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            base.PostRequest(sender, e);
        }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            base.PreRequest(sender, e);

            e.Request.Headers.Add("x-microsoftajax", "Delta=true");
            FormPostHttpBody formBody = e.Request.Body as FormPostHttpBody;
            if (null == formBody)
            {
                formBody = (e.Request.Body = new FormPostHttpBody()) as FormPostHttpBody;
                e.Request.Method = "POST";
            }

            string controlName = RuleHelper.ResolveContextValue(e.WebTest.Context, this.ControlName);
            string updatePanelName = RuleHelper.ResolveContextValue(e.WebTest.Context, this.UpdatePanelName);

            // Post all input hidden fields
            string[] hiddenFieldKeyNames = e.WebTest.Context["$HIDDEN1"] as string[];
            if (null != hiddenFieldKeyNames)
            {
                foreach (string hiddenFieldKeyName in hiddenFieldKeyNames)
                    formBody.FormPostParameters.Add(hiddenFieldKeyName, e.WebTest.Context["$HIDDEN1." + hiddenFieldKeyName] as string);

                RuleHelper.SetParameter(formBody.FormPostParameters, "TheScriptManager$ScriptManager1", updatePanelName + "|" + controlName, true);
                RuleHelper.SetParameter(formBody.FormPostParameters, "__EVENTTARGET", controlName, true);
                RuleHelper.SetParameter(formBody.FormPostParameters, "__ASYNCPOST", "true", true);
            }
        }

        #endregion Methods
    }
}