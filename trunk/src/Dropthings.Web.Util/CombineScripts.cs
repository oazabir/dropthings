/// <summary>
/// Summary description for CombineScripts
/// </summary>
namespace Dropthings.Web.Util
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml;
    using System.Xml.Linq;
    using Dropthings.Util;

    public class CombineScripts
    {
        #region Fields

        private static Regex _FindScriptTags = new Regex(@"<script\s*src\s*=\s*""(?<url>.[^""]+)"".[^>]*>\s*</script>", RegexOptions.Compiled);

        #endregion Fields

        #region Methods

        /// <summary>
        /// Combine script references using file sets defined in a configuration file.
        /// It will replace multiple script references using one 
        /// </summary>
        public static string CombineScriptBlocks(string scripts, string baseUrl)
        {
            List<UrlMapSet> sets = LoadSets(baseUrl);
            string output = scripts;

            foreach (UrlMapSet mapSet in sets)
            {
                int setStartPos = -1;
                List<string> names = new List<string>();

                output = _FindScriptTags.Replace(output, new MatchEvaluator(delegate(Match match)
                {
                    string url = match.Groups["url"].Value;

                    UrlMap urlMatch = mapSet.Urls.Find(
                        new Predicate<UrlMap>(
                            delegate(UrlMap map)
                            {
                                return map.Url == url;
                            }));

                    if (null != urlMatch)
                    {
                        // Remember the first script tag that matched in this UrlMapSet because
                        // this is where the combined script tag will be inserted
                        if (setStartPos < 0) setStartPos = match.Index;

                        names.Add(urlMatch.Name);
                        return string.Empty;
                    }
                    else
                    {
                        return match.Value;
                    }

                }));

                if (setStartPos >= 0)
                {
                    string setName = string.Empty;
                    // if the set says always include all urls within it whenever a single match is found,
                    // then generate the full set
                    if (mapSet.IsIncludeAll)
                    {
                        // No need send the individual url names when the full set needs to be included
                        setName = string.Empty;
                    }
                    else
                    {
                        names.Sort();
                        setName = string.Join(",", names.ToArray());
                    }

                    string urlPrefix = HttpContext.Current.Request.Path.Substring(0, HttpContext.Current.Request.Path.LastIndexOf('/') + 1);
                    string newScriptTag = "<script type=\"text/javascript\" src=\"Scripts.ashx?" + 
                        HttpUtility.UrlEncode(mapSet.Name) + "=" + HttpUtility.UrlEncode(setName) 
                        + "&" + HttpUtility.UrlEncode(urlPrefix) 
                        + "&" + HttpUtility.UrlEncode(ConstantHelper.ScriptVersionNo) + "\"></script>";

                    output = output.Insert(setStartPos, newScriptTag);
                }
            }

            return output;
        }

        public static List<UrlMapSet> LoadSets(string baseUrl)
        {
            const string CACHE_KEY = "CombineScript.SetDefinition";

            List<UrlMapSet> sets = HttpContext.Current.Cache[CACHE_KEY] as List<UrlMapSet> ?? new List<UrlMapSet>();

            if (sets.Count == 0)
            {
                using (XmlReader reader = new XmlTextReader(new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/FileSets.xml"))))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if ("set" == reader.Name)
                        {
                            string setName = reader.GetAttribute("name");
                            string isIncludeAll = reader.GetAttribute("includeAll");

                            UrlMapSet mapSet = new UrlMapSet();
                            mapSet.Name = setName;
                            if (isIncludeAll == "true")
                                mapSet.IsIncludeAll = true;

                            while (reader.Read())
                            {
                                if ("url" == reader.Name)
                                {
                                    string urlName = reader.GetAttribute("name");
                                    string url = reader.ReadElementContentAsString();
                                    string fullUrl = url.Replace("~", baseUrl);
                                    mapSet.Urls.Add(new UrlMap(urlName, fullUrl));
                                }
                                else if ("set" == reader.Name)
                                    break;
                            }

                            sets.Add(mapSet);
                        }
                    }
                }

                HttpContext.Current.Cache[CACHE_KEY] = sets;
            }

            return sets;
        }

        #endregion Methods
    }

    public class UrlMap
    {
        #region Fields

        public string Name;
        public string Url;

        #endregion Fields

        #region Constructors

        public UrlMap(string name, string url)
        {
            this.Name = name;
            this.Url = url;
        }

        #endregion Constructors
    }

    public class UrlMapSet
    {
        #region Fields

        public bool IsIncludeAll;
        public string Name;
        public List<UrlMap> Urls = new List<UrlMap>();

        #endregion Fields
    }
}