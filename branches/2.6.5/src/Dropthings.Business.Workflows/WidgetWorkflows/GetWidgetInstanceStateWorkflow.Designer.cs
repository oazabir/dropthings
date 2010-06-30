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
    partial class GetWidgetInstanceStateWorkflow
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
            this.GetWidgetState = new Dropthings.Business.Activities.GetWidgetStateActivity();
            this.EnsureOwner = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // GetWidgetState
            // 
            this.GetWidgetState.Name = "GetWidgetState";
            activitybind1.Name = "GetWidgetInstanceStateWorkflow";
            activitybind1.Path = "Request.WidgetInstanceId";
            activitybind2.Name = "GetWidgetInstanceStateWorkflow";
            activitybind2.Path = "Response.WidgetState";
            this.GetWidgetState.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.GetWidgetStateActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.GetWidgetState.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetState", typeof(Dropthings.Business.Activities.GetWidgetStateActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // EnsureOwner
            // 
            this.EnsureOwner.Name = "EnsureOwner";
            this.EnsureOwner.PageId = 0;
            activitybind3.Name = "GetWidgetInstanceStateWorkflow";
            activitybind3.Path = "Request.UserName";
            activitybind4.Name = "GetWidgetInstanceStateWorkflow";
            activitybind4.Path = "Request.WidgetInstanceId";
            this.EnsureOwner.WidgetZoneId = 0;
            this.EnsureOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.EnsureOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // GetWidgetInstanceStateWorkflow
            // 
            this.Activities.Add(this.EnsureOwner);
            this.Activities.Add(this.GetWidgetState);
            this.Name = "GetWidgetInstanceStateWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.EnsureOwnerActivity EnsureOwner;
        private Dropthings.Business.Activities.GetWidgetStateActivity GetWidgetState;







    }
}
