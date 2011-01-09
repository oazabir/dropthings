using System;
//using Microsoft.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
namespace Dropthings.RestApi
{
    [ServiceContract(Namespace = "http://dropthings.omaralzabir.com", Name = "WidgetService")]    
    public interface IWidgetService
    {
        //[WebHelp(Comment = "Add a new instance of the given Widget at the given zone and row")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void AddWidgetInstance(int widgetId, int toZone, int toRow);

        //[WebHelp(Comment = "Change the title of the given widget instance")]
        [WebInvoke(BodyStyle=WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]        
        void ChangeWidgetTitle(int widgetInstanceId, string newTitle);

        //[WebHelp(Comment = "Collapse the given widget instance")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]        
        void CollaspeWidgetInstance(int widgetInstanceId);


        //[WebHelp(Comment = "Delete the specified widget instance")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void DeleteWidgetInstance(int widgetInstanceId);


        //[WebHelp(Comment = "Expand the specified widget instance")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void ExpandWidgetInstance(int widgetInstanceId);


        //[WebHelp(Comment = "Get content from specified URL and cache the response for specified duration in seconds")]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        string GetWidgetState(int widgetInstanceId);


        //[WebHelp(Comment = "Get content from specified URL and cache the response for specified duration in seconds")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void MaximizeWidgetInstance(int widgetInstanceId);


        //[WebHelp(Comment = "Get content from specified URL and cache the response for specified duration in seconds")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void MoveWidgetInstance(int instanceId, int toZoneId, int toRow);


        //[WebHelp(Comment = "Get content from specified URL and cache the response for specified duration in seconds")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void ResizeWidgetInstance(int widgetInstanceId, int width, int height);


        //[WebHelp(Comment = "Get content from specified URL and cache the response for specified duration in seconds")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void RestoreWidgetInstance(int widgetInstanceId);

        //[WebHelp(Comment = "Get content from specified URL and cache the response for specified duration in seconds")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract()]
        void SaveWidgetState(int widgetInstanceId, string state);
    }
}
