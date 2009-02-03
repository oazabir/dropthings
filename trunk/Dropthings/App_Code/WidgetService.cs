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

            var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        AddWidgetWorkflow,
                        AddWidgetRequest,
                        AddWidgetResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new AddWidgetRequest { WidgetId = widgetId, RowNo = toRow, ColumnNo = 0, ZoneId = toZone, UserName = Profile.UserName }
                        );

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MoveWidgetInstance(int instanceId, int toZoneId, int toRow)
        {
            //new DashboardFacade(Profile.UserName).MoveWidgetInstance(instanceId, toZoneId, toRow);

            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<MoveWidgetInstanceWorkflow, MoveWidgetInstanceWorkflowRequest, MoveWidgetInstanceWorkflowResponse>(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new MoveWidgetInstanceWorkflowRequest { NewZoneId = toZoneId, RowNo = toRow, UserName = Profile.UserName, WidgetInstanceId = instanceId });

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ResizeWidgetInstance(int widgetId, int width, int height)
        {
            //new DashboardFacade(Profile.UserName).ResizeWidgetInstance(widgetId, width, height);

            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ResizeWidgetInstanceWorkflow,
                        ResizeWidgetInstanceRequest,
                        ResizeWidgetInstanceResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new ResizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, Width = width, Hidth = height }
                    );

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MaximizeWidgetInstance(int widgetId)
        {
            //new DashboardFacade(Profile.UserName).MaximizeRestoreWidgetInstance(widgetId, true);
            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        MaximizeWidgetInstanceWorkflow,
                        MaximizeWidgetInstanceRequest,
                        MaximizeWidgetInstanceResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new MaximizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsMaximize = true }
                    );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string ExpandWidgetInstance(int widgetId, string postbackUrl)
        {
            //new DashboardFacade(Profile.UserName).ExpanCollaspeWidgetInstance(widgetId, true);

            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ExpandWidgetInstanceWorkflow,
                        ExpandWidgetInstanceRequest,
                        ExpandWidgetInstanceResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
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

            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ExpandWidgetInstanceWorkflow,
                        ExpandWidgetInstanceRequest,
                        ExpandWidgetInstanceResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new ExpandWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsExpand = false }
                    );

            return postbackUrl;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void RestoreWidgetInstance(int widgetId)
        {
            //new DashboardFacade(Profile.UserName).MaximizeRestoreWidgetInstance(widgetId, false);

            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        MaximizeWidgetInstanceWorkflow,
                        MaximizeWidgetInstanceRequest,
                        MaximizeWidgetInstanceResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new MaximizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsMaximize = false }
                    );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void DeleteWidgetInstance(int widgetId)
        {
            //new DashboardFacade(Profile.UserName).DeleteWidgetInstance(widgetId);

            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        DeleteWidgetInstanceWorkflow,
                        DeleteWidgetInstanceWorkflowRequest,
                        DeleteWidgetInstanceWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new DeleteWidgetInstanceWorkflowRequest { WidgetInstanceId = widgetId, UserName = Profile.UserName }
                        );

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void AssignPermission(string widgetPermissions)
        {
            
            ObjectContainer.Resolve<IWorkflowHelper>()
                .ExecuteWorkflow<
                    AssignWidgetPermissionWorkflow,
                    AssignWidgetPermissionRequest,
                    AssignWidgetPermissionResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new AssignWidgetPermissionRequest { WidgetPermissions = widgetPermissions, UserName = Profile.UserName }
                    );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, XmlSerializeString = true)]
        public string GetWidgetState(int widgetId)
        {
            var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        GetWidgetInstanceStateWorkflow,
                        GetWidgetInstanceStateRequest,
                        GetWidgetInstanceStateResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new GetWidgetInstanceStateRequest { WidgetInstanceId = widgetId, UserName = Profile.UserName }
                        );
            return response.WidgetState;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void SaveWidgetState(int widgetId, string state)
        {
            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        SaveWidgetInstanceStateWorkflow,
                        SaveWidgetInstanceStateRequest,
                        SaveWidgetInstanceStateResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new SaveWidgetInstanceStateRequest { WidgetInstanceId = widgetId, State = state, UserName = Profile.UserName }
                        );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangeWidgetTitle(int widgetId, string newTitle)
        {
            // TODO: Create a workflow to save widget title
        }
    }

}