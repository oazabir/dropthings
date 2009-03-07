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

namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    partial class MoveWidgetInstanceWorkflow
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
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            this.PullWidgetsUpInOldColumn = new Dropthings.Business.Activities.ReorderWidgetInstancesOnWidgetZoneActivity();
            this.ChangeColumnAndRowOfWidget = new Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity();
            this.PushWidgetsDownInNewColumn = new Dropthings.Business.Activities.PushDownWidgetInstancesOnWidgetZoneActivity();
            this.LoadWidgetInstance = new Dropthings.Business.Activities.LoadWidgetActivity();
            this.EnsureWidgetOwner = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // PullWidgetsUpInOldColumn
            // 
            this.PullWidgetsUpInOldColumn.Name = "PullWidgetsUpInOldColumn";
            activitybind1.Name = "MoveWidgetInstanceWorkflow";
            activitybind1.Path = "OldWidgetInstance.WidgetZoneId";
            this.PullWidgetsUpInOldColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.ReorderWidgetInstancesOnWidgetZoneActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // ChangeColumnAndRowOfWidget
            // 
            this.ChangeColumnAndRowOfWidget.Name = "ChangeColumnAndRowOfWidget";
            activitybind2.Name = "MoveWidgetInstanceWorkflow";
            activitybind2.Path = "Request.RowNo";
            activitybind3.Name = "MoveWidgetInstanceWorkflow";
            activitybind3.Path = "Request.WidgetInstanceId";
            activitybind4.Name = "MoveWidgetInstanceWorkflow";
            activitybind4.Path = "Request.NewZoneId";
            this.ChangeColumnAndRowOfWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RowNo", typeof(Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.ChangeColumnAndRowOfWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.ChangeColumnAndRowOfWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // PushWidgetsDownInNewColumn
            // 
            this.PushWidgetsDownInNewColumn.Name = "PushWidgetsDownInNewColumn";
            activitybind5.Name = "MoveWidgetInstanceWorkflow";
            activitybind5.Path = "Request.RowNo";
            activitybind6.Name = "MoveWidgetInstanceWorkflow";
            activitybind6.Path = "Request.WidgetInstanceId";
            activitybind7.Name = "MoveWidgetInstanceWorkflow";
            activitybind7.Path = "Request.NewZoneId";
            this.PushWidgetsDownInNewColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Position", typeof(Dropthings.Business.Activities.PushDownWidgetInstancesOnWidgetZoneActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.PushWidgetsDownInNewColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.PushDownWidgetInstancesOnWidgetZoneActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.PushWidgetsDownInNewColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.PushDownWidgetInstancesOnWidgetZoneActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // LoadWidgetInstance
            // 
            this.LoadWidgetInstance.Name = "LoadWidgetInstance";
            activitybind8.Name = "MoveWidgetInstanceWorkflow";
            activitybind8.Path = "OldWidgetInstance";
            activitybind9.Name = "MoveWidgetInstanceWorkflow";
            activitybind9.Path = "Request.WidgetInstanceId";
            this.LoadWidgetInstance.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.LoadWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.LoadWidgetInstance.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstance", typeof(Dropthings.Business.Activities.LoadWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // EnsureWidgetOwner
            // 
            this.EnsureWidgetOwner.Name = "EnsureWidgetOwner";
            this.EnsureWidgetOwner.PageId = 0;
            activitybind10.Name = "MoveWidgetInstanceWorkflow";
            activitybind10.Path = "Request.UserName";
            activitybind11.Name = "MoveWidgetInstanceWorkflow";
            activitybind11.Path = "Request.WidgetInstanceId";
            this.EnsureWidgetOwner.WidgetZoneId = 0;
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // MoveWidgetInstanceWorkflow
            // 
            this.Activities.Add(this.EnsureWidgetOwner);
            this.Activities.Add(this.LoadWidgetInstance);
            this.Activities.Add(this.PushWidgetsDownInNewColumn);
            this.Activities.Add(this.ChangeColumnAndRowOfWidget);
            this.Activities.Add(this.PullWidgetsUpInOldColumn);
            this.Name = "MoveWidgetInstanceWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.EnsureOwnerActivity EnsureWidgetOwner;
        private Dropthings.Business.Activities.ReorderWidgetInstancesOnWidgetZoneActivity PullWidgetsUpInOldColumn;
        private Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity ChangeColumnAndRowOfWidget;
        private Dropthings.Business.Activities.LoadWidgetActivity LoadWidgetInstance;
        private Dropthings.Business.Activities.PushDownWidgetInstancesOnWidgetZoneActivity PushWidgetsDownInNewColumn;






































    }
}
