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
    partial class ResizeWidgetInstanceWorkflow
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
            this.ResizeWidget = new Dropthings.Business.Activities.ResizeWidgetActivity();
            this.EnsureWidgetOwner = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // ResizeWidget
            // 
            activitybind1.Name = "ResizeWidgetInstanceWorkflow";
            activitybind1.Path = "Request.Hidth";
            activitybind2.Name = "ResizeWidgetInstanceWorkflow";
            activitybind2.Path = "Response.WidgetInstanceAffected";
            this.ResizeWidget.Name = "ResizeWidget";
            activitybind3.Name = "ResizeWidgetInstanceWorkflow";
            activitybind3.Path = "Request.WidgetInstanceId";
            activitybind4.Name = "ResizeWidgetInstanceWorkflow";
            activitybind4.Path = "Request.Width";
            this.ResizeWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Height", typeof(Dropthings.Business.Activities.ResizeWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ResizeWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.ResizeWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.ResizeWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Width", typeof(Dropthings.Business.Activities.ResizeWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.ResizeWidget.SetBinding(Dropthings.Business.Activities.ResizeWidgetActivity.ModifiedWidgetInstanceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // EnsureWidgetOwner
            // 
            this.EnsureWidgetOwner.Name = "EnsureWidgetOwner";
            this.EnsureWidgetOwner.PageId = 0;
            activitybind5.Name = "ResizeWidgetInstanceWorkflow";
            activitybind5.Path = "Request.UserName";
            activitybind6.Name = "ResizeWidgetInstanceWorkflow";
            activitybind6.Path = "Request.WidgetInstanceId";
            this.EnsureWidgetOwner.WidgetZoneId = 0;
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // ResizeWidgetInstanceWorkflow
            // 
            this.Activities.Add(this.EnsureWidgetOwner);
            this.Activities.Add(this.ResizeWidget);
            this.Name = "ResizeWidgetInstanceWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.EnsureOwnerActivity EnsureWidgetOwner;
        private Dropthings.Business.Activities.ResizeWidgetActivity ResizeWidget;









    }
}
