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

namespace Dropthings.Business
{
    partial class AddNewTabWorkflow
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
            this.SetNewPageAsCurrent = new Dropthings.Business.Activities.SetCurrentPageActivity();
            this.CreateNewPage = new Dropthings.Business.Activities.CreateNewPageActivity();
            this.DecideUniquePageTitle = new Dropthings.Business.Activities.PageActivities.DecideUniquePageNameActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // SetNewPageAsCurrent
            // 
            this.SetNewPageAsCurrent.Name = "SetNewPageAsCurrent";
            activitybind1.Name = "CreateNewPage";
            activitybind1.Path = "NewPageId";
            activitybind2.Name = "GetUserGuid";
            activitybind2.Path = "UserGuid";
            this.SetNewPageAsCurrent.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.SetNewPageAsCurrent.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // CreateNewPage
            // 
            activitybind3.Name = "AddNewTabWorkflow";
            activitybind3.Path = "Request.LayoutType";
            this.CreateNewPage.Name = "CreateNewPage";
            activitybind4.Name = "AddNewTabWorkflow";
            activitybind4.Path = "Response.NewPage";
            this.CreateNewPage.NewPageId = 0;
            activitybind5.Name = "DecideUniquePageTitle";
            activitybind5.Path = "NewPageTitle";
            activitybind6.Name = "GetUserGuid";
            activitybind6.Path = "UserGuid";
            this.CreateNewPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserId", typeof(Dropthings.Business.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.CreateNewPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("LayoutType", typeof(Dropthings.Business.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.CreateNewPage.SetBinding(Dropthings.Business.Activities.CreateNewPageActivity.NewPageProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.CreateNewPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Title", typeof(Dropthings.Business.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // DecideUniquePageTitle
            // 
            this.DecideUniquePageTitle.Name = "DecideUniquePageTitle";
            this.DecideUniquePageTitle.NewPageTitle = null;
            activitybind7.Name = "AddNewTabWorkflow";
            activitybind7.Path = "Response.UserGuid";
            this.DecideUniquePageTitle.SetBinding(Dropthings.Business.Activities.PageActivities.DecideUniquePageNameActivity.UserGuidProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            activitybind8.Name = "AddNewTabWorkflow";
            activitybind8.Path = "Response.UserGuid";
            activitybind9.Name = "AddNewTabWorkflow";
            activitybind9.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // AddNewTabWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.DecideUniquePageTitle);
            this.Activities.Add(this.CreateNewPage);
            this.Activities.Add(this.SetNewPageAsCurrent);
            this.Name = "AddNewTabWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.PageActivities.DecideUniquePageNameActivity DecideUniquePageTitle;
        private Dropthings.Business.Activities.SetCurrentPageActivity SetNewPageAsCurrent;
        private Dropthings.Business.Activities.CreateNewPageActivity CreateNewPage;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;

















    }
}
