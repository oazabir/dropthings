using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Dropthings.Web.Util
{
    public class RssHelper
    {
        private static readonly Regex _StripTagEx = new Regex("</?[^>]+>", RegexOptions.Compiled);

        public static IEnumerable<RssItem> ConvertXmlToRss(XElement feed, int count)
        {
            XNamespace ns = "http://www.w3.org/2005/Atom";

            // see if RSS or Atom

            // RSS
            if (feed.Element("channel") != null)
                return (from item in feed.Element("channel").Elements("item")
                        select new RssItem
                        {
                            Title = StripTags(item.Element("title").Value, 200),
                            Link = item.Element("link").Value,
                            Description = StripTags(item.Element("description").Value, 200)
                        }).Take(count);

            // Atom
            else if (feed.Element(ns + "entry") != null)
                return (from item in feed.Elements(ns + "entry")
                        select new RssItem
                        {
                            Title = StripTags(item.Element(ns + "title").Value, 200),
                            Link = item.Element(ns + "link").Attribute("href").Value,
                            Description = StripTags(item.Element(ns + "content").Value, 200)
                        }).Take(count);

            // Invalid
            else
                return null;
        }

        private static string StripTags(string html, int trimAt)
        {
            string plainText = _StripTagEx.Replace(html, string.Empty);
            return plainText.Substring(0, Math.Min(plainText.Length, trimAt));
        }
    }
}
