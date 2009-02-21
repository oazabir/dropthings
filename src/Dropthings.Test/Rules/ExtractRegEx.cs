namespace MyOfficeTest.Rules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    [DisplayName("Extract RegEx Value")]
    public class ExtractRegEx : ExtractionRule
    {
        #region Properties

        public string Expression
        {
            get; set;
        }

        public int GroupIndex
        {
            get; set;
        }

        public string GroupName
        {
            get; set;
        }

        public bool MatchAll
        {
            get; set;
        }

        public int MatchIndex
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            Regex exp = new Regex(this.Expression);
            MatchCollection matches = exp.Matches(e.Response.BodyString);

            Action<Match, string> storeMatch = new Action<Match, string>((match, keyName) =>
                {
                    if (string.IsNullOrEmpty(this.GroupName))
                    {
                        Group group = match.Groups[this.GroupIndex];
                        e.WebTest.Context[keyName] = group.Value;
                    }
                    else
                    {
                        Group group = match.Groups[this.GroupName];
                        e.WebTest.Context[keyName] = group.Value;
                    }
                });

            if (!this.MatchAll)
            {
                if (this.MatchIndex < matches.Count)
                {
                    Match match = matches[MatchIndex];
                    storeMatch(match, this.ContextParameterName);
                }
                else
                {
                    e.Message = "Match index is higher than matches found or no match found";
                    e.Success = false;
                }
            }
            else
            {
                int counter = 1;
                foreach (Match match in matches)
                {
                    storeMatch(match, this.ContextParameterName + "." + (counter++));
                }

                e.WebTest.Context[this.ContextParameterName + ".$COUNT"] = counter;
            }
        }

        #endregion Methods
    }
}