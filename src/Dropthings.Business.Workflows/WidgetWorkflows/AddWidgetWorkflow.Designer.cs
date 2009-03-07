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
using Dropthings.DataAccess;

namespace Dropthings.Business
{
    partial class AddWidgetWorkflow
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
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            this.GetWidgetZoneOnColumn = new Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn();
            this.GetWidgetZone = new Dropthings.Business.Activities.GetWidgetZoneActivity();
            this.ColumnSpecified = new System.Workflow.Activities.IfElseBranchActivity();
            this.ZoneIdSpecified = new System.Workflow.Activities.IfElseBranchActivity();
            this.AddWidgetOnCurrentPage = new Dropthings.Business.Activities.AddWidgetOnWidgetZone();
            this.PushDownColumn = new Dropthings.Business.Activities.PushDownWidgetInstancesOnWidgetZoneActivity();
            this.CheckIfZoneIdSpecified = new System.Workflow.Activities.IfElseActivity();
            this.GetThePageToAddWidget = new Dropthings.Business.Activities.GetUserSettingActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // GetWidgetZoneOnColumn
            // 
            activitybind1.Name = "AddWidgetWorkflow";
            activitybind1.Path = "Request.ColumnNo";
            this.GetWidgetZoneOnColumn.Name = "GetWidgetZoneOnColumn";
            activitybind2.Name = "GetThePageToAddWidget";
            activitybind2.Path = "CurrentPage.ID";
            activitybind3.Name = "AddWidgetWorkflow";
            activitybind3.Path = "NewWidget_WidgetZone";
            this.GetWidgetZoneOnColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.ColumnNoProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.GetWidgetZoneOnColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.GetWidgetZoneOnColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.WidgetZoneProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // GetWidgetZone
            // 
            this.GetWidgetZone.Name = "GetWidgetZone";
            activitybind4.Name = "AddWidgetWorkflow";
            activitybind4.Path = "NewWidget_WidgetZone";
            activitybind5.Name = "AddWidgetWorkflow";
            activitybind5.Path = "Request.ZoneId";
            this.GetWidgetZone.SetBinding(Dropthings.Business.Activities.GetWidgetZoneActivity.ZoneIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.GetWidgetZone.SetBinding(Dropthings.Business.Activities.GetWidgetZoneActivity.WidgetZoneProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // ColumnSpecified
            // 
            this.ColumnSpecified.Activities.Add(this.GetWidgetZoneOnColumn);
            this.ColumnSpecified.Name = "ColumnSpecified";
            // 
            // ZoneIdSpecified
            // 
            this.ZoneIdSpecified.Activities.Add(this.GetWidgetZone);
            ruleconditionreference1.ConditionName = "Condition1";
            this.ZoneIdSpecified.Condition = ruleconditionreference1;
            this.ZoneIdSpecified.Name = "ZoneIdSpecified";
            // 
            // AddWidgetOnCurrentPage
            // 
            this.AddWidgetOnCurrentPage.Name = "AddWidgetOnCurrentPage";
            activitybind6.Name = "AddWidgetWorkflow";
            activitybind6.Path = "Response.NewWidget";
            activitybind7.Name = "AddWidgetWorkflow";
            activitybind7.Path = "Request.RowNo";
            activitybind8.Name = "AddWidgetWorkflow";
            activitybind8.Path = "Request.WidgetId";
            activitybind9.Name = "AddWidgetWorkflow";
            activitybind9.Path = "NewWidget_WidgetZone.ID";
            this.AddWidgetOnCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetId", typeof(Dropthings.Business.Activities.AddWidgetOnWidgetZone)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.AddWidgetOnCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("NewWidget", typeof(Dropthings.Business.Activities.AddWidgetOnWidgetZone)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.AddWidgetOnCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Position", typeof(Dropthings.Business.Activities.AddWidgetOnWidgetZone)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.AddWidgetOnCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.AddWidgetOnWidgetZone)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // PushDownColumn
            // 
            this.PushDownColumn.Name = "PushDownColumn";
            activitybind10.Name = "AddWidgetWorkflow";
            activitybind10.Path = "Request.RowNo";
            this.PushDownColumn.WidgetInstanceId = 0;
            activitybind11.Name = "AddWidgetWorkflow";
            activitybind11.Path = "NewWidget_WidgetZone.ID";
            this.PushDownColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Position", typeof(Dropthings.Business.Activities.PushDownWidgetInstancesOnWidgetZoneActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.PushDownColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.PushDownWidgetInstancesOnWidgetZoneActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // CheckIfZoneIdSpecified
            // 
            this.CheckIfZoneIdSpecified.Activities.Add(this.ZoneIdSpecified);
            this.CheckIfZoneIdSpecified.Activities.Add(this.ColumnSpecified);
            this.CheckIfZoneIdSpecified.Name = "CheckIfZoneIdSpecified";
            // 
            // GetThePageToAddWidget
            // 
            this.GetThePageToAddWidget.CurrentPage = null;
            this.GetThePageToAddWidget.Name = "GetThePageToAddWidget";
            activitybind12.Name = "GetUserGuid";
            activitybind12.Path = "UserGuid";
            this.GetThePageToAddWidget.UserSetting = null;
            this.GetThePageToAddWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind13.Name = "AddWidgetWorkflow";
            activitybind13.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            // 
            // AddWidgetWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.GetThePageToAddWidget);
            this.Activities.Add(this.CheckIfZoneIdSpecified);
            this.Activities.Add(this.PushDownColumn);
            this.Activities.Add(this.AddWidgetOnCurrentPage);
            this.Name = "AddWidgetWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private IfElseBranchActivity ColumnSpecified;
        private IfElseBranchActivity ZoneIdSpecified;
        private IfElseActivity CheckIfZoneIdSpecified;
        private Dropthings.Business.Activities.GetWidgetZoneActivity GetWidgetZone;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn GetWidgetZoneOnColumn;
        private Dropthings.Business.Activities.PushDownWidgetInstancesOnWidgetZoneActivity PushDownColumn;
        private Dropthings.Business.Activities.AddWidgetOnWidgetZone AddWidgetOnCurrentPage;
        private Dropthings.Business.Activities.GetUserSettingActivity GetThePageToAddWidget;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;
































































    }
}
