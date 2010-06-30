namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    [DisplayName("Extract PostBack Names")]
    public class ExtractPostbackNames : ExtractionRule
    {
        #region Fields

        private static Regex _FindPostbackNames = new Regex(@"__doPostBack\('(.*?)'", 
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase
            | RegexOptions.CultureInvariant);

        #endregion Fields

        #region Methods

        /// <summary>
        /// Find all javascript:__doPostback(...) type declarations which indicates
        /// all controls that support postback. It finds all such controls that support
        /// postback and then stores the full client ID of the control in Context
        /// using the last part of the ID as key in Context. For example:
        /// $POSTBACK.1.AddNewWidget = WidgetUpdatePanel001$ctl_002$AddNewWidget
        /// This way you can find a paricular controls full client ID when you know only the
        /// server ID of the control.
        /// </summary>
        /// <param name="bodyHtml">Body HTML</param>
        /// <param name="context">WebTest Context</param>
        public static void ExtractPostBackNames(string bodyHtml, WebTestContext context)
        {
            RuleHelper.NotAlreadyDone(context, "$POSTBACK.EXTRACTED", () =>
                {
                    var matches = _FindPostbackNames.Matches(bodyHtml);
                    foreach (Match match in matches)
                    {
                        string fullID = match.Groups[1].Value;
                        string lastPartOfID = fullID.Substring(fullID.LastIndexOf('$') + 1);
                        string contextKeyName = "$POSTBACK." + lastPartOfID;

                        RuleHelper.PlaceUniqueItem(context, contextKeyName, fullID);
                    }
                });
        }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            string bodyHtml = e.Response.BodyString;

            ExtractPostBackNames(bodyHtml, e.WebTest.Context);
        }

        #endregion Methods
    }
}