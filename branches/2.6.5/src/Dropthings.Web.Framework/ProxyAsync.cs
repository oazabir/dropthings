#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Web.Framework
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Xml;
    using System.Xml.Linq;

    using AJAXASMXHandler;

    using Dropthings.Widget.Framework;
    using Dropthings.Util;
    using OmarALZabir.AspectF;
    
    /// <summary>
    /// Summary description for Proxy
    /// </summary>
    [WebService(Namespace = "http://www.dropthings.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class ProxyAsync : System.Web.Services.WebService
    {
        #region Fields

        private const string CACHE_KEY = "ProxyAsync.";

        private static Regex _StripTagEx = new Regex("</?[^>]+>", RegexOptions.Compiled);

        #endregion Fields

        #region Constructors

        public ProxyAsync()
        {
        }

        #endregion Constructors

        #region Methods

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static bool IsUrlInCache(string url)
        {
            var data = Services.Get<ICache>().Get(CACHE_KEY + url);
            return (null != data);
        }

        [ScriptMethod]
        public IAsyncResult BeginGetString(string url, int cacheDuration, AsyncCallback cb, object state)
        {
            return AspectF.Define
                .Log(Services.Get<ILogger>(), "BeginGetString Url: {0} cache: {1}", url, cacheDuration)
                .Return<IAsyncResult>(() =>
                {
                    // See if the response from the URL is already cached on server
                    string cachedContent = Services.Get<ICache>().Get(CACHE_KEY + url) as string;
                    if (!string.IsNullOrEmpty(cachedContent))
                    {
                        this.CacheResponse(Context, cacheDuration);
                        return new AsmxHandlerSyncResult(cachedContent);
                    }

                    HttpWebRequest request = this.CreateHttpWebRequest(url);
                    // As we will stream the response, don't want to automatically decompress the content
                    request.AutomaticDecompression = DecompressionMethods.None;

                    GetStringState myState = new GetStringState(state);
                    myState.Request = request;
                    myState.Url = url;
                    myState.CacheDuration = cacheDuration;

                    return request.BeginGetResponse(cb, myState);
                });
        }

        [ScriptMethod]
        public IAsyncResult BeginGetXml(string url, int cacheDuration, AsyncCallback cb, object state)
        {
            return BeginGetString(url, cacheDuration, cb, state);
        }

        [ScriptMethod]
        public string EndGetString(IAsyncResult result)
        {
            GetStringState state = result.AsyncState as GetStringState;

            return AspectF.Define
                .MustBeNonNull(state)
                .Log(Services.Get<ILogger>(), "EndGetString Url: {0} cache: {1}", state.Url, state.CacheDuration)
                .Return<string>(() =>
                {   
                    MemoryStream responseBuffer = new MemoryStream();

                    HttpWebRequest request = state.Request;
                    using (HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            // produce cache headers for response caching
                            this.CacheResponse(state.Context, state.CacheDuration);

                            string contentLength = response.GetResponseHeader("Content-Length") ?? "-1";
                            state.Context.Response.AppendHeader("Content-Length", contentLength);

                            string contentEncoding = response.GetResponseHeader("Content-Encoding") ?? "";
                            state.Context.Response.AppendHeader("Content-Encoding", contentEncoding);

                            state.Context.Response.ContentType = response.ContentType;

                            const int BUFFER_SIZE = 4 * 1024;
                            byte[] buffer = new byte[BUFFER_SIZE];
                            int dataReceived;
                            while ((dataReceived = stream.Read(buffer, 0, BUFFER_SIZE)) > 0)
                            {
                                if (!state.Context.Response.IsClientConnected) return string.Empty;

                                // Transmit to client (browser) immediately
                                byte[] outBuffer = new byte[dataReceived];
                                Array.Copy(buffer, outBuffer, dataReceived);

                                state.Context.Response.BinaryWrite(outBuffer);
                                //state.Context.Response.Flush();

                                // Store in buffer so that we can cache the whole stuff
                                responseBuffer.Write(buffer, 0, dataReceived);
                            }

							responseBuffer.Position = 0;
                            // If the content is compressed, decompress it
                            Stream contentStream = contentEncoding == "gzip" ?
                                (new GZipStream(responseBuffer, CompressionMode.Decompress) as Stream)
                                :
                                (contentEncoding == "deflate" ?
                                    (new DeflateStream(responseBuffer, CompressionMode.Decompress) as Stream)
                                    :
                                    (responseBuffer as Stream));

                            // Cache the decompressed content so that we can return it next time
                            using (StreamReader reader = new StreamReader(contentStream, true))
                            {
                                string content = reader.ReadToEnd();

                                Services.Get<ICache>().Add(CACHE_KEY + state.Url, content);
                            }

                            state.Context.Response.Flush();

                            return null;
                        }
                    }
                });
        }

        [ScriptMethod]
        public string EndGetXml(IAsyncResult result)
        {
            return EndGetString(result);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public object GetRss(string url, int count, int cacheDuration)
        {
            var feed = Services.Get<ICache>().Get(CACHE_KEY + url) as XElement;
            if (feed == null)
            {
                // We have failed to load the RSS before. So, let's not try again.
                if (string.Empty == (Services.Get<ICache>().Get(CACHE_KEY + url) as string)) return null;

                try
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                    request.Timeout = 15000;
                    using (WebResponse response = request.GetResponse())
                    {
                        using (XmlTextReader reader = new XmlTextReader(new StreamReader(response.GetResponseStream(), true)))
                        {
                            feed = XElement.Load(reader);
                        }
                    }

                    if (feed == null) return null;
                    Services.Get<ICache>().Add(CACHE_KEY + url, feed, TimeSpan.FromMinutes(15));

                }
                catch(Exception x)
                {
                    Debug.WriteLine(x.ToString());
                    // Let's remember that we failed to load this RSS feed and we will not try to load it again
                    // in next 15 mins
                    Services.Get<ICache>().Add(CACHE_KEY + url, string.Empty, TimeSpan.FromMinutes(15));
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
            finally
            {
                this.CacheResponse(Context, cacheDuration);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetString(string url, int cacheDuration)
        {
            return AspectF.Define
                .MustBeNonNull(url)
                .HowLong(Services.Get<ILogger>(), "Begin:GetString " + url, "End:GetString " + url + " {0}")
                .Cache<string>(Services.Get<ICache>(), CACHE_KEY + url)
                .Return<string>(() =>
                {
                    using (WebClient client = new WebClient())
                    {
                        var content = client.DownloadString(url);
                        return content;
                    }
                });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml)]
        public string GetXml(string url, int cacheDuration)
        {
            return GetString(url, cacheDuration);
        }

        private void CacheResponse(HttpContext context, int durationInMinutes)
        {
            TimeSpan duration = TimeSpan.FromMinutes(durationInMinutes);

            // With the new AJAX ASMX handler, there's no need for this hack to set maxAge value
            /*FieldInfo maxAge = HttpContext.Current.Response.Cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);
            maxAge.SetValue(HttpContext.Current.Response.Cache, duration);*/

            if (context.Request.Url.AbsolutePath.EndsWith(".asmx"))
            {
                HttpCachePolicy cache = context.Response.Cache;
                cache.SetCacheability(HttpCacheability.Public);
                cache.SetExpires(DateTime.Now.Add(duration));
                cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
                cache.SetMaxAge(duration);
            }
        }

        private HttpWebRequest CreateHttpWebRequest(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Headers.Add("Accept-Encoding", "gzip");
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.MaximumAutomaticRedirections = 2;
            request.MaximumResponseHeadersLength = 4 * 1024;
            request.ReadWriteTimeout = 1 * 1000;
            request.Timeout = 5 * 1000;

            return request;
        }

        private string StripTags(string html, int trimAt)
        {
            string plainText = _StripTagEx.Replace(html, string.Empty);
            return plainText.Substring(0, Math.Min(plainText.Length, trimAt));
        }

        #endregion Methods

        #region Nested Types

        private class GetStringState : AsyncWebMethodState
        {
            #region Fields

            public int CacheDuration;
            public HttpWebRequest Request;
            public string Url;

            #endregion Fields

            #region Constructors

            public GetStringState(object state)
                : base(state)
            {
            }

            #endregion Constructors
        }

        #endregion Nested Types
    }
}