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
    partial class ChangeWidgetInstanceTitleWorkflow
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
            this.ChangeTitle = new Dropthings.Business.Activities.WidgetActivities.ChangeWidgetInstanceTitle();
            this.EnsureWidgetOwner = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // ChangeTitle
            // 
            this.ChangeTitle.Name = "ChangeTitle";
            activitybind1.Name = "ChangeWidgetInstanceTitleWorkflow";
            activitybind1.Path = "NewTitle";
            activitybind2.Name = "ChangeWidgetInstanceTitleWorkflow";
            activitybind2.Path = "WidgetInstanceId";
            this.ChangeTitle.SetBinding(Dropthings.Business.Activities.WidgetActivities.ChangeWidgetInstanceTitle.NewTitleProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ChangeTitle.SetBinding(Dropthings.Business.Activities.WidgetActivities.ChangeWidgetInstanceTitle.WidgetInstanceIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // EnsureWidgetOwner
            // 
            this.EnsureWidgetOwner.Name = "EnsureWidgetOwner";
            this.EnsureWidgetOwner.PageId = 0;
            activitybind3.Name = "ChangeWidgetInstanceTitleWorkflow";
            activitybind3.Path = "UserName";
            activitybind4.Name = "ChangeWidgetInstanceTitleWorkflow";
            activitybind4.Path = "WidgetInstanceId";
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetInstanceId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.EnsureWidgetOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // ChangeWidgetInstanceTitleWorkflow
            // 
            this.Activities.Add(this.EnsureWidgetOwner);
            this.Activities.Add(this.ChangeTitle);
            this.Name = "ChangeWidgetInstanceTitleWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.WidgetActivities.ChangeWidgetInstanceTitle ChangeTitle;
        private Dropthings.Business.Activities.EnsureOwnerActivity EnsureWidgetOwner;






    }
}
