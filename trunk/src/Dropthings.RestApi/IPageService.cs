using System;
//using Microsoft.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Dropthings.RestApi
{
    [ServiceContract(Namespace = "http://dropthings.omaralzabir.com", Name = "PageService")]   
    public interface IPageService
    {
        //[WebHelp(Comment = "Change page column sizes")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void ChangePageLayout(int newLayout);


        //[WebHelp(Comment = "Reorder a tab")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void MoveTab(int pageId, int orderNo);
    }
}
