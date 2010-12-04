using System;
using Microsoft.ServiceModel.Web;
using System.ServiceModel.Web;
using System.ServiceModel;
namespace Dropthings.RestApi
{
    [ServiceContract(Namespace = "http://dropthings.omaralzabir.com/rest", Name = "ProxyServiceRest")]
    interface IProxyServiceRest : IProxyService
    {
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "GetUrl?url={url}&cacheDuration={cacheDuration}")]
        new IAsyncResult BeginGetUrl(string url, int cacheDuration, AsyncCallback wcfCallback, object wcfState);

        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "GetRss?url={url}&count={count}&cacheDuration={cacheDuration}")]
        new IAsyncResult BeginGetRss(string url, int count, int cacheDuration, AsyncCallback wcfCallback, object wcfState);        
    }
}
