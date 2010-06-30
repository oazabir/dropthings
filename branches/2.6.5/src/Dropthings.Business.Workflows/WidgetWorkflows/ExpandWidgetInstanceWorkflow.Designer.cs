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
    partial class ExpandWidgetInstanceWorkflow
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
            this.ExpandWidget = new Dropthings.Business.Activities.ExpandWidgetActivity();
            this.EnsureOwner = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // ExpandWidget
            // 
            activitybind1.Name = "ExpandWidgetInstanceWorkflow";
            activitybind1.Path = "Request.IsExpand";
            activitybind2.Name = "ExpandWidgetInstanceWorkflow";
            activitybind2.Path = "Response.WidgetInstanceAffected";
            this.ExpandWidget.Name = "ExpandWidget";
            activitybind3.Name = "ExpandWidgetInstanceWorkflow";
            activitybind3.Path = "Request.WidgetInstanceId";
            this.ExpandWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("IsExpand", typeof(Dropthings.Business.Activities.ExpandWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ExpandWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.ExpandWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.ExpandWidget.SetBinding(Dropthings.Business.Activities.ExpandWidgetActivity.ModifiedWidgetInstanceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // EnsureOwner
            // 
            this.EnsureOwner.Name = "EnsureOwner";
            this.EnsureOwner.PageId = 0;
            activitybind4.Name = "ExpandWidgetInstanceWorkflow";
            activitybind4.Path = "Request.UserName";
            activitybind5.Name = "ExpandWidgetInstanceWorkflow";
            activitybind5.Path = "Request.WidgetInstanceId";
            this.EnsureOwner.WidgetZoneId = 0;
            this.EnsureOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.EnsureOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // ExpandWidgetInstanceWorkflow
            // 
            this.Activities.Add(this.EnsureOwner);
            this.Activities.Add(this.ExpandWidget);
            this.Name = "ExpandWidgetInstanceWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.EnsureOwnerActivity EnsureOwner;
        private Dropthings.Business.Activities.ExpandWidgetActivity ExpandWidget;





    }
}
