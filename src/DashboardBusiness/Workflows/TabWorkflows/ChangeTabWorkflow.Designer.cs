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
    partial class ChangeTabWorkflow
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
            this.SetCurrentPage = new Dropthings.Business.Activities.SetCurrentPageActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            this.EnsurePageOwner = new Dropthings.Business.Activities.EnsureOwnerActivity();
            // 
            // SetCurrentPage
            // 
            this.SetCurrentPage.Name = "SetCurrentPage";
            activitybind1.Name = "ChangeTabWorkflow";
            activitybind1.Path = "Request.PageID";
            activitybind2.Name = "GetUserGuid";
            activitybind2.Path = "UserGuid";
            this.SetCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.SetCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind3.Name = "ChangeTabWorkflow";
            activitybind3.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // EnsurePageOwner
            // 
            this.EnsurePageOwner.Name = "EnsurePageOwner";
            activitybind4.Name = "ChangeTabWorkflow";
            activitybind4.Path = "Request.PageID";
            activitybind5.Name = "ChangeTabWorkflow";
            activitybind5.Path = "Request.UserName";
            this.EnsurePageOwner.WidgetInstanceId = 0;
            this.EnsurePageOwner.WidgetZoneId = 0;
            this.EnsurePageOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.EnsurePageOwner.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.EnsureOwnerActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // ChangeTabWorkflow
            // 
            this.Activities.Add(this.EnsurePageOwner);
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.SetCurrentPage);
            this.Name = "ChangeTabWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.EnsureOwnerActivity EnsurePageOwner;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;
        private Dropthings.Business.Activities.SetCurrentPageActivity SetCurrentPage;














    }
}
