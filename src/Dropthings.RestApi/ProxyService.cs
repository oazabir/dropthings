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
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            WebRequestState myState = new WebRequestState(wcfCallback, wcfState) 
            { 
                Request = request,
                CacheDuration = cacheDuration,
                ContentType = WebOperationContext.Current.IncomingRequest.ContentType
            };
            IAsyncResult asyncResult = request.BeginGetResponse(new AsyncCallback(HttpGetCallback), myState);
            return new CustomAsyncResult<WebRequestState>(asyncResult, myState);                
        }

        public Stream EndGetUrl(IAsyncResult asyncResult)
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
            
            return myState.Response.GetResponseStream();
        }

        private void SetCaching(WebOperationContext context, DateTime lastModifiedDate, Int32 maxCacheAge)
        {
            // set CacheControl header
            HttpResponseHeader cacheHeader = HttpResponseHeader.CacheControl;
            String cacheControlValue = String.Format("max-age={0}, must-revalidate", maxCacheAge);
            context.OutgoingResponse.Headers.Add(cacheHeader, cacheControlValue);

            // set cache validation 
            context.OutgoingResponse.LastModified = lastModifiedDate;
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
            return BeginGetUrl(url, cacheDuration, wcfCallback, wcfState);
        }
        public RssItem[] EndGetRss(IAsyncResult asyncResult)
        {            
            var result = RssHelper.ConvertXmlToRss(XElement.Load(new XmlTextReader(EndGetUrl(asyncResult))), 10).ToArray();
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return result;
        }

        internal class WebRequestState : CustomStateBase
        {
            public HttpWebRequest Request;
            public HttpWebResponse Response;
            public Exception Error;
            public int CacheDuration;
            public string ContentType;

            public WebRequestState(AsyncCallback originalCallback, object originalState) 
                : base(originalCallback, originalState) 
            { 
            }            
        }
    }
 }