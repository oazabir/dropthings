// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.Web.Script.Services;

using Dropthings.Business;
using Dropthings.DataAccess;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows.WidgetWorkflows;
using System.Workflow.Runtime;
using Dropthings.Business.Workflows;
using System.Collections.Specialized;
using Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs;

namespace Dropthings.Web.Framework
{
    /// <summary>
    /// Summary description for WidgetService
    /// </summary>

    public class WidgetService : WebServiceBase
    {

        public WidgetService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void AddWidgetInstance(int widgetId, int toZone, int toRow)
        {
            //WidgetInstance widget = new DashboardFacade(Profile.UserName).AddWidget(widgetId, 0, toRow, toZone);

            var response = RunWorkflow.Run<AddWidgetWorkflow, AddWidgetRequest, AddWidgetResponse>(
                new AddWidgetRequest { WidgetId = widgetId, RowNo = toRow, ColumnNo = 0, ZoneId = toZone, UserName = Profile.UserName } );

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MoveWidgetInstance(int instanceId, int toZoneId, int toRow)
        {
            //new DashboardFacade(Profile.UserName).MoveWidgetInstance(instanceId, toZoneId, toRow);

            RunWorkflow.Run<MoveWidgetInstanceWorkflow, MoveWidgetInstanceWorkflowRequest, MoveWidgetInstanceWorkflowResponse>(
                        new MoveWidgetInstanceWorkflowRequest { NewZoneId = toZoneId, RowNo = toRow, UserName = Profile.UserName, WidgetInstanceId = instanceId });

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ResizeWidgetInstance(int widgetId, int width, int height)
        {
            //new DashboardFacade(Profile.UserName).ResizeWidgetInstance(widgetId, width, height);

            RunWorkflow.Run<ResizeWidgetInstanceWorkflow, ResizeWidgetInstanceRequest, ResizeWidgetInstanceResponse>(
                    new ResizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, Width = width, Hidth = height }
                );

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MaximizeWidgetInstance(int widgetId)
        {
            //new DashboardFacade(Profile.UserName).MaximizeRestoreWidgetInstance(widgetId, true);
            RunWorkflow.Run<MaximizeWidgetInstanceWorkflow, MaximizeWidgetInstanceRequest, MaximizeWidgetInstanceResponse>(
                    new MaximizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsMaximize = true }
                );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string ExpandWidgetInstance(int widgetId, string postbackUrl)
        {
            //new DashboardFacade(Profile.UserName).ExpanCollaspeWidgetInstance(widgetId, true);

            RunWorkflow.Run<ExpandWidgetInstanceWorkflow, ExpandWidgetInstanceRequest, ExpandWidgetInstanceResponse>(
                    new ExpandWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsExpand = true }
                );

            Context.Cache.Remove(Profile.UserName);
            return postbackUrl;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string CollaspeWidgetInstance(int widgetId, string postbackUrl)
        {
            //new DashboardFacade(Profile.UserName).ExpanCollaspeWidgetInstance(widgetId, false);

            RunWorkflow.Run<ExpandWidgetInstanceWorkflow, ExpandWidgetInstanceRequest, ExpandWidgetInstanceResponse>(
                    new ExpandWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsExpand = false }
                );

            return postbackUrl;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void RestoreWidgetInstance(int widgetId)
        {
            //new DashboardFacade(Profile.UserName).MaximizeRestoreWidgetInstance(widgetId, false);

            RunWorkflow.Run<MaximizeWidgetInstanceWorkflow, MaximizeWidgetInstanceRequest, MaximizeWidgetInstanceResponse>(
                    new MaximizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsMaximize = false }
                );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void DeleteWidgetInstance(int widgetId)
        {
            //new DashboardFacade(Profile.UserName).DeleteWidgetInstance(widgetId);

            RunWorkflow.Run<DeleteWidgetInstanceWorkflow, DeleteWidgetInstanceWorkflowRequest, DeleteWidgetInstanceWorkflowResponse>(
                    new DeleteWidgetInstanceWorkflowRequest { WidgetInstanceId = widgetId, UserName = Profile.UserName }
                );

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void AssignPermission(string widgetPermissions)
        {            
            RunWorkflow.Run<AssignWidgetPermissionWorkflow, AssignWidgetPermissionRequest, AssignWidgetPermissionResponse>(
                    new AssignWidgetPermissionRequest { WidgetPermissions = widgetPermissions, UserName = Profile.UserName }
                );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, XmlSerializeString = true)]
        public string GetWidgetState(int widgetId)
        {
            var response = RunWorkflow.Run<GetWidgetInstanceStateWorkflow, GetWidgetInstanceStateRequest, GetWidgetInstanceStateResponse>(
                    new GetWidgetInstanceStateRequest { WidgetInstanceId = widgetId, UserName = Profile.UserName }
                );
            return response.WidgetState;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void SaveWidgetState(int widgetId, string state)
        {
            RunWorkflow.Run<SaveWidgetInstanceStateWorkflow, SaveWidgetInstanceStateRequest, SaveWidgetInstanceStateResponse>(
                     new SaveWidgetInstanceStateRequest { WidgetInstanceId = widgetId, State = state, UserName = Profile.UserName }
                );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangeWidgetTitle(int widgetId, string newTitle)
        {
            RunWorkflow.Run<ChangeWidgetInstanceTitleWorkflow, ChangeWidgetInstanceTitleWorkflowRequest, ChangeWidgetInstanceTitleWorkflowResponse>(
                new ChangeWidgetInstanceTitleWorkflowRequest { WidgetInstanceId = widgetId, UserName = Profile.UserName, NewTitle = newTitle }
            );
        }

    }

}