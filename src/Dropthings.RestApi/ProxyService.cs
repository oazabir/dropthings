/// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using Microsoft.ServiceModel.Web;
using System.Linq;
using System.Net;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Xml.Linq;
using System.Xml;
using Dropthings.Web.Util;
using Dropthings.Util;
using OmarALZabir.AspectF;
using System.Text;
using System.Net.Sockets;

[assembly: ContractNamespace("http://dropthings.omaralzabir.com", ClrNamespace = "Dropthings.RestApi")]

namespace Dropthings.RestApi
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public partial class ProxyService : IProxyService, IProxyServiceRest
    {
        /// <summary>
        /// Returns the content from destination URL and caches the response in the browser for specified seconds.
        /// </summary>
        /// <param name="url">URL to fetch</param>
        /// <param name="cacheDuration">Cache duration</param>
        /// <returns></returns>
        public IAsyncResult BeginGetUrl(string url, int cacheDuration, AsyncCallback wcfCallback, object wcfState)
        {
            return StartGetUrl(url, cacheDuration, 0, wcfCallback, wcfState);
        }

        private IAsyncResult StartGetUrl(string url, int cacheDuration, int count, AsyncCallback wcfCallback, object wcfState)
        {
            /// If the url already exists in cache then there's no need to fetch it from the source.
            /// We can just return the response immediately from cache
            if (!ConstantHelper.DisableCache && Services.Get<ICache>().Contains(url))
            {
                WebRequestState myState = new WebRequestState(wcfCallback, wcfState)
                {
                    Url = url,
                    CacheDuration = cacheDuration,
                    ContentType = WebOperationContext.Current.IncomingRequest.ContentType,
                    Count = count
                };

                // Trigger the completion of the request immediately 
                var completedState = new CompletedAsyncResult<WebRequestState>(myState, wcfState);
                wcfCallback(completedState);
                return completedState;
            }
            else
            {
                /// The content does not exist in cache and we need to get it from the
                /// original source 
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                WebRequestState myState = new WebRequestState(wcfCallback, wcfState)
                {
                    Request = request,
                    ContentType = WebOperationContext.Current.IncomingRequest.ContentType,
                    Url = url,
                    CacheDuration = cacheDuration,
                    Count = count
                };
                IAsyncResult asyncResult = request.BeginGetResponse(new AsyncCallback(HttpGetCallback), myState);
                return new CustomAsyncResult<WebRequestState>(asyncResult, myState);
            }
        }

        public Stream EndGetUrl(IAsyncResult asyncResult)
        {
            return CompleteGetUrl(asyncResult);
        }

        private Stream CompleteGetUrl(IAsyncResult asyncResult)
        {
            if (asyncResult is CompletedAsyncResult<WebRequestState>)
            {
                /// The content is already in cache. So, return the item from 
                /// cache
                WebRequestState myState = (asyncResult as CompletedAsyncResult<WebRequestState>).Data;
                string content = Services.Get<ICache>().Get(myState.Url) as string;
                return new MemoryStream(Encoding.UTF8.GetBytes(content));
            }
            else
            {
                CustomAsyncResult<WebRequestState> myAsyncResult = (CustomAsyncResult<WebRequestState>)asyncResult;
                WebRequestState myState = myAsyncResult.AdditionalData;

                if (myState.Error != null)
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, myState.Error.Message,
                        myState.Error);

                var outResponse = WebOperationContext.Current.OutgoingResponse;
                outResponse.ContentLength = myState.Response.ContentLength;
                outResponse.ContentType = myState.Response.ContentType;
                SetCaching(WebOperationContext.Current, DateTime.Now, myState.CacheDuration);

                var contentEncoding = myState.Response.ContentEncoding;
                if (myState.CacheDuration > 0)
                    return new StreamWrapper(myState.Response.GetResponseStream(),
                        (int)(outResponse.ContentLength > 0 ? outResponse.ContentLength : 8 * 1024),
                        buffer =>
                        {
                            Encoding enc;
                            try
                            {
                                if (string.IsNullOrEmpty(contentEncoding))
                                    enc = Encoding.UTF8;
                                else
                                    enc = Encoding.GetEncoding(contentEncoding);
                            }
                            catch
                            {
                                enc = Encoding.GetEncoding(1252);
                            }

                            if (!ConstantHelper.DisableCache)
                                Services.Get<ICache>().Add(myState.Url, enc.GetString(buffer));
                        });
                else
                    return myState.Response.GetResponseStream();
            }
        }

        private void SetCaching(WebOperationContext context, DateTime lastModifiedDate, Int32 maxCacheAge)
        {
            // set CacheControl header
            HttpResponseHeader cacheHeader = HttpResponseHeader.CacheControl;
            String cacheControlValue = String.Format("max-age={0}, must-revalidate", maxCacheAge);
            context.OutgoingResponse.Headers.Add(cacheHeader, cacheControlValue);

            // set cache validation 
            context.OutgoingResponse.LastModified = lastModifiedDate;

            // No ETag, want this to be cached on browser for good.
            //String eTag = context.IncomingRequest.UriTemplateMatch.RequestUri.ToString() + lastModifiedDate.ToString();
            //context.OutgoingResponse.ETag = eTag;
        }

        void HttpGetCallback(IAsyncResult asyncResult)
        {
            WebRequestState myState = (WebRequestState)asyncResult.AsyncState;
            try
            {
                myState.Response = (HttpWebResponse)myState.Request.EndGetResponse(asyncResult);
            }
            catch (WebException e)
            {
                myState.Error = e;
                myState.Response = (HttpWebResponse)e.Response;
            }
            myState.UserCallback(new CustomAsyncResult<WebRequestState>(asyncResult, myState));
        }

        public IAsyncResult BeginGetRss(string url, int count, int cacheDuration, AsyncCallback wcfCallback, object wcfState)
        {
            return StartGetUrl(url, cacheDuration, count, wcfCallback, wcfState);
        }
        public RssItem[] EndGetRss(IAsyncResult asyncResult)
        {
            CustomAsyncResult<WebRequestState> myAsyncResult = (CustomAsyncResult<WebRequestState>)asyncResult;
            WebRequestState myState = myAsyncResult.AdditionalData;

            var result = RssHelper.ConvertXmlToRss(XElement.Load(new XmlTextReader(CompleteGetUrl(asyncResult))), myState.Count).ToArray();
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return result;            
        }

        internal class WebRequestState : CustomStateBase
        {
            public HttpWebRequest Request;
            public HttpWebResponse Response;
            public Exception Error;
            public string Url;
            public int CacheDuration;
            public string ContentType;
            public int Count;

            public WebRequestState(AsyncCallback originalCallback, object originalState) 
                : base(originalCallback, originalState) 
            { 
            }            
        }
    }
 }