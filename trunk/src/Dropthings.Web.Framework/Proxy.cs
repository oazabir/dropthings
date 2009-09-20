#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

/// <summary>
/// Summary description for Proxy
/// </summary>
namespace Dropthings.Web.Framework
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Xml;
    using System.Xml.Linq;

    using Dropthings.Widget.Framework;
    using Dropthings.Util;

    [WebService(Namespace = "http://www.dropthings.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class Proxy : System.Web.Services.WebService
    {
        #region Constructors

        public Proxy()
        {
        }

        #endregion Constructors

        #region Methods

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public object GetRss(string url, int count, int cacheDuration)
        {
            return AspectF.Define
                .Retry()
                .HowLong(Logger.Writer,
                    string.Format("[begin] GetRss\tUrl:{0}\tcount:{1}\tcacheDuration{2}", url, count, cacheDuration),
                    "[end] GetRss Url:" + url + " {0}")
                .Return<object>(() =>
                {

                    var feed = Context.Cache[url] as XElement;
                    if (feed == null)
                    {
                        if (string.IsNullOrEmpty(Context.Cache[url] as string)) return null;
                        try
                        {
                            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                            request.Timeout = 15000;
                            using (WebResponse response = request.GetResponse())
                            {
                                using (XmlTextReader reader = new XmlTextReader(response.GetResponseStream()))
                                {
                                    feed = XElement.Load(reader);
                                }
                            }

                            if (feed == null) return null;
                            Context.Cache.Insert(url, feed, null, DateTime.MaxValue, TimeSpan.FromMinutes(15));

                        }
                        catch
                        {
                            Context.Cache[url] = string.Empty;
                            return null;
                        }
                    }

                    XNamespace ns = "http://www.w3.org/2005/Atom";

                    // see if RSS or Atom

                    try
                    {
                        // RSS
                        if (feed.Element("channel") != null)
                            return (from item in feed.Element("channel").Elements("item")
                                    select new RssItem
                                    {
                                        Title = item.Element("title").Value,
                                        Link = item.Element("link").Value,
                                        Description = item.Element("description").Value
                                    }).Take(count);

                        // Atom
                        else if (feed.Element(ns + "entry") != null)
                            return (from item in feed.Elements(ns + "entry")
                                    select new RssItem
                                    {
                                        Title = item.Element(ns + "title").Value,
                                        Link = item.Element(ns + "link").Attribute("href").Value,
                                        Description = item.Element(ns + "content").Value
                                    }).Take(count);

                        // Invalid
                        else
                            return null;
                    }
                    finally
                    {
                        this.CacheResponse(cacheDuration);
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetString(string url, int cacheDuration)
        {
            return AspectF.Define
                .Retry()
                .HowLong(Logger.Writer,
                    string.Format("[begin] GetString\tUrl:{0}\tcacheDuration{2}", url, cacheDuration),
                    "[end] GetString Url:" + url + " {0}")
                .Return<string>(() =>
                {
                    // See if the response from the URL is already cached on server
                    string cachedContent = Context.Cache[url] as string;
                    if (!string.IsNullOrEmpty(cachedContent))
                    {
                        this.CacheResponse(cacheDuration);
                        return cachedContent;
                    }
                    else
                    {
                        using (WebClient client = new WebClient())
                        {
                            string response = client.DownloadString(url);
                            Context.Cache.Insert(url, response, null,
                                Cache.NoAbsoluteExpiration,
                                TimeSpan.FromMinutes(cacheDuration),
                                CacheItemPriority.Normal, null);

                            // produce cache headers for response caching
                            this.CacheResponse(cacheDuration);

                            return response;
                        }
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml)]
        public string GetXml(string url, int cacheDuration)
        {
            return GetString(url, cacheDuration);
        }

        private void CacheResponse(int durationInMinutes)
        {
            TimeSpan duration = TimeSpan.FromMinutes(durationInMinutes);

            // With the new AJAX ASMX handler, there's no need for this hack to set maxAge value
            /*FieldInfo maxAge = HttpContext.Current.Response.Cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);
            maxAge.SetValue(HttpContext.Current.Response.Cache, duration);*/

            // Only when this method is being used from a webservice call.
            if (HttpContext.Current.Request.Url.AbsolutePath.EndsWith(".asmx"))
            {
                HttpCachePolicy cache = HttpContext.Current.Response.Cache;
                cache.SetCacheability(HttpCacheability.Public);
                cache.SetExpires(DateTime.Now.Add(duration));
                cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
                cache.SetMaxAge(duration);
            }
        }

        #endregion Methods
    }
}