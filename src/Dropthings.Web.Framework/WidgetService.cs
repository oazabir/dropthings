#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Web.Framework
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Workflow.Runtime;

    using Dropthings.Business;
    using Dropthings.Business.Container;
    using Dropthings.Business.Workflows;
    using Dropthings.Business.Workflows.TabWorkflows;
    using Dropthings.Business.Workflows.WidgetWorkflows;
    using Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs;
    using Dropthings.DataAccess;

    using Dropthings.Business.Facade;
    using Dropthings.Business.Facade.Context;

    /// <summary>
    /// Summary description for WidgetService
    /// </summary>
    public class WidgetService : WebServiceBase
    {
        #region Constructors

        public WidgetService()
        {
            //Uncomment the following line if using designed components
            //InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void AddWidgetInstance(int widgetId, int toZone, int toRow)
        {
            //WidgetInstance widget = new DashboardFacade(Profile.UserName).AddWidget(widgetId, 0, toRow, toZone);

            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.AddWidget(widgetId, toRow, 0, toZone);
            }

            // -- Workflow way. Obselete.
            //var response = WorkflowHelper.Run<AddWidgetWorkflow, AddWidgetRequest, AddWidgetResponse>(
            //    new AddWidgetRequest { WidgetId = widgetId, RowNo = toRow, ColumnNo = 0, ZoneId = toZone, UserName = Profile.UserName } );

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void AssignPermission(string widgetPermissions)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.AssignWidgetPermission(widgetPermissions);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<AssignWidgetPermissionWorkflow, AssignWidgetPermissionRequest, AssignWidgetPermissionResponse>(
            //        new AssignWidgetPermissionRequest { WidgetPermissions = widgetPermissions, UserName = Profile.UserName }
            //    );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangePageLayout(int newLayout)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.ModifyPageLayout(newLayout);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<ModifyPageLayoutWorkflow, ModifyTabLayoutWorkflowRequest, ModifyTabLayoutWorkflowResponse>(
            //    new ModifyTabLayoutWorkflowRequest{ LayoutType = newLayout, UserName = Profile.UserName }
            //);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangeWidgetTitle(int widgetId, string newTitle)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.ChangeWidgetInstanceTitle(widgetId, newTitle);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<ChangeWidgetInstanceTitleWorkflow, ChangeWidgetInstanceTitleWorkflowRequest, ChangeWidgetInstanceTitleWorkflowResponse>(
            //    new ChangeWidgetInstanceTitleWorkflowRequest { WidgetInstanceId = widgetId, UserName = Profile.UserName, NewTitle = newTitle }
            //);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string CollaspeWidgetInstance(int widgetId, string postbackUrl)
        {
            //new DashboardFacade(Profile.UserName).ExpanCollaspeWidgetInstance(widgetId, false);

            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.ExpandWidget(widgetId, false);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<ExpandWidgetInstanceWorkflow, ExpandWidgetInstanceRequest, ExpandWidgetInstanceResponse>(
            //        new ExpandWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsExpand = false }
            //    );

            return postbackUrl;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void DeleteWidgetInstance(int widgetId)
        {
            //new DashboardFacade(Profile.UserName).DeleteWidgetInstance(widgetId);

            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.DeleteWidgetInstance(widgetId);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<DeleteWidgetInstanceWorkflow, DeleteWidgetInstanceWorkflowRequest, DeleteWidgetInstanceWorkflowResponse>(
            //        new DeleteWidgetInstanceWorkflowRequest { WidgetInstanceId = widgetId, UserName = Profile.UserName }
            //    );

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string ExpandWidgetInstance(int widgetId, string postbackUrl)
        {
            //new DashboardFacade(Profile.UserName).ExpanCollaspeWidgetInstance(widgetId, true);

            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.ExpandWidget(widgetId, true);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<ExpandWidgetInstanceWorkflow, ExpandWidgetInstanceRequest, ExpandWidgetInstanceResponse>(
            //        new ExpandWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsExpand = true }
            //    );

            Context.Cache.Remove(Profile.UserName);
            return postbackUrl;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, XmlSerializeString = true)]
        public string GetWidgetState(int widgetId)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                return facade.GetWidgetInstanceState(widgetId);
            }

            // -- Workflow way. Obselete.
            //var response = WorkflowHelper.Run<GetWidgetInstanceStateWorkflow, GetWidgetInstanceStateRequest, GetWidgetInstanceStateResponse>(
            //        new GetWidgetInstanceStateRequest { WidgetInstanceId = widgetId, UserName = Profile.UserName }
            //    );

            //return response.WidgetState;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MaximizeWidgetInstance(int widgetId)
        {
            //new DashboardFacade(Profile.UserName).MaximizeRestoreWidgetInstance(widgetId, true);

            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.MaximizeWidget(widgetId, true);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<MaximizeWidgetInstanceWorkflow, MaximizeWidgetInstanceRequest, MaximizeWidgetInstanceResponse>(
            //        new MaximizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsMaximize = true }
            //    );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MoveWidgetInstance(int instanceId, int toZoneId, int toRow)
        {
            //new DashboardFacade(Profile.UserName).MoveWidgetInstance(instanceId, toZoneId, toRow);

            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.MoveWidgetInstance(instanceId, toZoneId, toRow);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<MoveWidgetInstanceWorkflow, MoveWidgetInstanceWorkflowRequest, MoveWidgetInstanceWorkflowResponse>(
            //            new MoveWidgetInstanceWorkflowRequest { NewZoneId = toZoneId, RowNo = toRow, UserName = Profile.UserName, WidgetInstanceId = instanceId });

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ResizeWidgetInstance(int widgetId, int width, int height)
        {
            //new DashboardFacade(Profile.UserName).ResizeWidgetInstance(widgetId, width, height);

            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.ResizeWidgetInstance(widgetId, width, height);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<ResizeWidgetInstanceWorkflow, ResizeWidgetInstanceRequest, ResizeWidgetInstanceResponse>(
            //        new ResizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, Width = width, Hidth = height }
            //    );

            Context.Cache.Remove(Profile.UserName);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void RestoreWidgetInstance(int widgetId)
        {
            //new DashboardFacade(Profile.UserName).MaximizeRestoreWidgetInstance(widgetId, false);
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.MaximizeWidget(widgetId, false);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<MaximizeWidgetInstanceWorkflow, MaximizeWidgetInstanceRequest, MaximizeWidgetInstanceResponse>(
            //        new MaximizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = widgetId, IsMaximize = false }
            //    );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void SaveWidgetState(int widgetId, string state)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.SaveWidgetInstanceState(widgetId, state);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<SaveWidgetInstanceStateWorkflow, SaveWidgetInstanceStateRequest, SaveWidgetInstanceStateResponse>(
            //         new SaveWidgetInstanceStateRequest { WidgetInstanceId = widgetId, State = state, UserName = Profile.UserName }
            //    );
        }

        #endregion Methods
    }
}