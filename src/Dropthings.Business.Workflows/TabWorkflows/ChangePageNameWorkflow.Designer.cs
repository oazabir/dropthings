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

namespace Dropthings.Business.Workflows.TabWorkflows
{
	partial class ChangePageNameWorkflow
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
            this.ChangeName = new Dropthings.Business.Activities.ChangePageNameActivity();
            this.GetCurrentPage = new Dropthings.Business.Activities.GetUserSettingActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // ChangeName
            // 
            this.ChangeName.Name = "ChangeName";
            activitybind1.Name = "GetCurrentPage";
            activitybind1.Path = "UserSetting.CurrentPageId";
            activitybind2.Name = "ChangePageNameWorkflow";
            activitybind2.Path = "Request.PageName";
            this.ChangeName.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.ChangePageNameActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ChangeName.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageName", typeof(Dropthings.Business.Activities.ChangePageNameActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetCurrentPage
            // 
            this.GetCurrentPage.CurrentPage = null;
            this.GetCurrentPage.Name = "GetCurrentPage";
            activitybind3.Name = "GetUserGuid";
            activitybind3.Path = "UserGuid";
            this.GetCurrentPage.UserSetting = null;
            this.GetCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind4.Name = "ChangePageNameWorkflow";
            activitybind4.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // ChangePageNameWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.GetCurrentPage);
            this.Activities.Add(this.ChangeName);
            this.Name = "ChangePageNameWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private Dropthings.Business.Activities.ChangePageNameActivity ChangeName;
        private Dropthings.Business.Activities.GetUserSettingActivity GetCurrentPage;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;










    }
}
