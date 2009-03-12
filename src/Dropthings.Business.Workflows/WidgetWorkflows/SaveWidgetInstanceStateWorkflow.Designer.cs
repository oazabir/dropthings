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
    partial class SaveWidgetInstanceStateWorkflow
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
            this.SaveWidgetState = new Dropthings.Business.Activities.SaveWidgetStateActivity();
            this.EnsureOwner = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // SaveWidgetState
            // 
            activitybind1.Name = "SaveWidgetInstanceStateWorkflow";
            activitybind1.Path = "Response.WidgetInstanceAffected";
            this.SaveWidgetState.Name = "SaveWidgetState";
            activitybind2.Name = "SaveWidgetInstanceStateWorkflow";
            activitybind2.Path = "Request.State";
            activitybind3.Name = "SaveWidgetInstanceStateWorkflow";
            activitybind3.Path = "Request.WidgetInstanceId";
            this.SaveWidgetState.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.SaveWidgetStateActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.SaveWidgetState.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("State", typeof(Dropthings.Business.Activities.SaveWidgetStateActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.SaveWidgetState.SetBinding(Dropthings.Business.Activities.SaveWidgetStateActivity.ModifiedWidgetInstanceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // EnsureOwner
            // 
            this.EnsureOwner.Name = "EnsureOwner";
            this.EnsureOwner.PageId = 0;
            activitybind4.Name = "SaveWidgetInstanceStateWorkflow";
            activitybind4.Path = "Request.UserName";
            activitybind5.Name = "SaveWidgetInstanceStateWorkflow";
            activitybind5.Path = "Request.WidgetInstanceId";
            this.EnsureOwner.WidgetZoneId = 0;
            this.EnsureOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.EnsureOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // SaveWidgetInstanceStateWorkflow
            // 
            this.Activities.Add(this.EnsureOwner);
            this.Activities.Add(this.SaveWidgetState);
            this.Name = "SaveWidgetInstanceStateWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.EnsureOwnerActivity EnsureOwner;
        private Dropthings.Business.Activities.SaveWidgetStateActivity SaveWidgetState;







    }
}
