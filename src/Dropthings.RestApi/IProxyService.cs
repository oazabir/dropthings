using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.ServiceModel.Web;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Xml.Linq;
using Dropthings.Web.Util;

namespace Dropthings.RestApi
{
    [ServiceContract(Namespace = "http://dropthings.omaralzabir.com", Name = "ProxyService")]
    public interface IProxyService
    {
        [WebHelp(Comment = "Get content from specified URL and cache the response for specified duration in seconds")]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetUrl(string url, int cacheDuration, AsyncCallback wcfCallback, object wcfState);
        System.IO.Stream EndGetUrl(IAsyncResult asyncResult);

        [WebHelp(Comment = "Get content from specified URL and cache the response for specified duration in seconds")]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetRss(string url, int count, int cacheDuration, AsyncCallback wcfCallback, object wcfState);
        RssItem[] EndGetRss(IAsyncResult asyncResult);
    }
}
