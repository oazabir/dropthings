// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Dropthings.Business
{
    partial class DeleteWidgetInstanceWorkflow
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            this.ReorderAllWidgetsOnSameColumn = new Dropthings.Business.Activities.ReorderWidgetInstancesOnWidgetZoneActivity();
            this.DeleteSpecifiedWidgetInstance = new Dropthings.Business.Activities.DeleteWidgetInstanceActivity();
            this.GetWidgetInstance = new Dropthings.Business.Activities.WidgetActivities.GetWidgetInstanceActivity();
            this.EnsureWidgetInstanceOwnership = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // ReorderAllWidgetsOnSameColumn
            // 
            this.ReorderAllWidgetsOnSameColumn.Name = "ReorderAllWidgetsOnSameColumn";
            activitybind1.Name = "GetWidgetInstance";
            activitybind1.Path = "WidgetInstance.WidgetZoneId";
            this.ReorderAllWidgetsOnSameColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.ReorderWidgetInstancesOnWidgetZoneActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // DeleteSpecifiedWidgetInstance
            // 
            this.DeleteSpecifiedWidgetInstance.Name = "DeleteSpecifiedWidgetInstance";
            activitybind2.Name = "DeleteWidgetInstanceWorkflow";
            activitybind2.Path = "Request.WidgetInstanceId";
            this.DeleteSpecifiedWidgetInstance.SetBinding(Dropthings.Business.Activities.DeleteWidgetInstanceActivity.WidgetInstanceIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetWidgetInstance
            // 
            this.GetWidgetInstance.Name = "GetWidgetInstance";
            activitybind3.Name = "DeleteWidgetInstanceWorkflow";
            activitybind3.Path = "WidgetInstance";
            activitybind4.Name = "DeleteWidgetInstanceWorkflow";
            activitybind4.Path = "Request.WidgetInstanceId";
            this.GetWidgetInstance.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetInstanceActivity.WidgetInstanceIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.GetWidgetInstance.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetInstanceActivity.WidgetInstanceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // EnsureWidgetInstanceOwnership
            // 
            this.EnsureWidgetInstanceOwnership.Name = "EnsureWidgetInstanceOwnership";
            this.EnsureWidgetInstanceOwnership.PageId = 0;
            activitybind5.Name = "DeleteWidgetInstanceWorkflow";
            activitybind5.Path = "Request.UserName";
            activitybind6.Name = "DeleteWidgetInstanceWorkflow";
            activitybind6.Path = "Request.WidgetInstanceId";
            this.EnsureWidgetInstanceOwnership.WidgetZoneId = 0;
            this.EnsureWidgetInstanceOwnership.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.EnsureWidgetInstanceOwnership.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // DeleteWidgetInstanceWorkflow
            // 
            this.Activities.Add(this.EnsureWidgetInstanceOwnership);
            this.Activities.Add(this.GetWidgetInstance);
            this.Activities.Add(this.DeleteSpecifiedWidgetInstance);
            this.Activities.Add(this.ReorderAllWidgetsOnSameColumn);
            this.Name = "DeleteWidgetInstanceWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.EnsureOwnerActivity EnsureWidgetInstanceOwnership;
        private Dropthings.Business.Activities.DeleteWidgetInstanceActivity DeleteSpecifiedWidgetInstance;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetInstanceActivity GetWidgetInstance;
        private Dropthings.Business.Activities.ReorderWidgetInstancesOnWidgetZoneActivity ReorderAllWidgetsOnSameColumn;
































    }
}
