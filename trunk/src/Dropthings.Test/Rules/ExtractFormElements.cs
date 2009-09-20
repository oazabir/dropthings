namespace Dropthings.Test.Rules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    [DisplayName("Extract Form Elements")]
    public class ExtractFormElements : ExtractionRule
    {
        #region Fields

        public const string FORM_ELEMENT_KEYS = "$FORM_ELEMENTS";
        public const string INPUT_PREFIX = "$INPUT.";
        public const string SELECT_PREFIX = "$SELECT.";
        public const string VALUE_SUFFIX = ".VALUE";

        private static Regex _FindInputTags = new Regex(
            @"<(input)\s*[^>]*(name)=""(?<name>([^""]*))""\s*[^>]*(value)="""
            + @"(?<value>([^""]*))""",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );

        private static Regex _FindSelectTags = new Regex(
            @"<select[^>]*>(.*?)</select>",
            RegexOptions.IgnoreCase
            | RegexOptions.Singleline
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );

        private static Regex _ExtractSelectTags = new Regex(
            @"<select\s*[^>]*name=""(?<name>([^""]*))""\s*[^>]*.*<option\s"
            + @"*[^>]*selected=""[^""]*""\s*[^>]*value=""(?<value>([^""]*))""",
            RegexOptions.IgnoreCase
            | RegexOptions.Singleline
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );

        #endregion Fields

        #region Methods

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            string body = e.Response.BodyString;

            List<string> formElements = new List<string>();
            var processMatches = new Action<MatchCollection, string>((matches, prefix) =>
            {
                foreach (Match match in matches)
                {
                    var extractMatches = _ExtractSelectTags.Matches(match.Value);

                    foreach (Match extractMatch in extractMatches)
                    {
                        string name = extractMatch.Groups["name"].Value;
                        string value = extractMatch.Groups["value"].Value;

                        string lastPartOfName = name.Substring(name.LastIndexOf('$') + 1);
                        string keyName = RuleHelper.PlaceUniqueItem(e.WebTest.Context, prefix + lastPartOfName, name);
                        e.WebTest.Context[keyName + VALUE_SUFFIX] = value;

                        // Create a name value pair in context as it is using the form element's name
                        e.WebTest.Context[name] = value;
                        formElements.Add(name);
                    }
                }
            });

            processMatches(_FindInputTags.Matches(body), INPUT_PREFIX);
            processMatches(_FindSelectTags.Matches(body), SELECT_PREFIX);

            e.WebTest.Context[FORM_ELEMENT_KEYS] = formElements.ToArray();
        }

        #endregion Methods
    }
}