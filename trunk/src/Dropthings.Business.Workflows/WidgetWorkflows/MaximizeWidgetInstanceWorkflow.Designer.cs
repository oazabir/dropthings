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
	partial class MaximizeWidgetInstanceWorkflow
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
            this.MaximizeWidget = new Dropthings.Business.Activities.MaximizeWidgetActivity();
            this.EnsureWidgetOwner = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // MaximizeWidget
            // 
            activitybind1.Name = "MaximizeWidgetInstanceWorkflow";
            activitybind1.Path = "Request.IsMaximize";
            this.MaximizeWidget.Name = "MaximizeWidget";
            activitybind2.Name = "MaximizeWidgetInstanceWorkflow";
            activitybind2.Path = "Request.WidgetInstanceId";
            this.MaximizeWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.MaximizeWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.MaximizeWidget.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("IsMaximize", typeof(Dropthings.Business.Activities.MaximizeWidgetActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // EnsureWidgetOwner
            // 
            this.EnsureWidgetOwner.Name = "EnsureWidgetOwner";
            this.EnsureWidgetOwner.PageId = 0;
            activitybind3.Name = "MaximizeWidgetInstanceWorkflow";
            activitybind3.Path = "Request.UserName";
            activitybind4.Name = "MaximizeWidgetInstanceWorkflow";
            activitybind4.Path = "Request.WidgetInstanceId";
            this.EnsureWidgetOwner.WidgetZoneId = 0;
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // MaximizeWidgetInstanceWorkflow
            // 
            this.Activities.Add(this.EnsureWidgetOwner);
            this.Activities.Add(this.MaximizeWidget);
            this.Name = "MaximizeWidgetInstanceWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private Dropthings.Business.Activities.EnsureOwnerActivity EnsureWidgetOwner;
        private Dropthings.Business.Activities.MaximizeWidgetActivity MaximizeWidget;





    }
}
