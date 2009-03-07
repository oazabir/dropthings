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

namespace Dropthings.Business.Workflows.TabWorkflows
{
    partial class GetColumnsInPageWorkflow
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
            this.GetColumnsInPage = new Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity();
            this.EnsurePageOwner = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // GetColumnsInPage
            // 
            activitybind1.Name = "GetColumnsInPageWorkflow";
            activitybind1.Path = "Response.Columns";
            this.GetColumnsInPage.Name = "GetColumnsInPage";
            activitybind2.Name = "GetColumnsInPageWorkflow";
            activitybind2.Path = "Request.PageId";
            this.GetColumnsInPage.SetBinding(Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.GetColumnsInPage.SetBinding(Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity.ColumnsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // EnsurePageOwner
            // 
            this.EnsurePageOwner.Name = "EnsurePageOwner";
            activitybind3.Name = "GetColumnsInPageWorkflow";
            activitybind3.Path = "Request.PageId";
            activitybind4.Name = "GetColumnsInPageWorkflow";
            activitybind4.Path = "Request.UserName";
            this.EnsurePageOwner.WidgetInstanceId = 0;
            this.EnsurePageOwner.WidgetZoneId = 0;
            this.EnsurePageOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.EnsurePageOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // GetColumnsInPageWorkflow
            // 
            this.Activities.Add(this.EnsurePageOwner);
            this.Activities.Add(this.GetColumnsInPage);
            this.Name = "GetColumnsInPageWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity GetColumnsInPage;
        private Dropthings.Business.Activities.EnsureOwnerActivity EnsurePageOwner;






    }
}
