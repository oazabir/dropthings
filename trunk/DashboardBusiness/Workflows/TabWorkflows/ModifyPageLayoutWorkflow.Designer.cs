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
	partial class ModifyPageLayoutWorkflow
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
            this.ChangePageLayout = new Dropthings.Business.Activities.ChangePageLayoutActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // ChangePageLayout
            // 
            activitybind1.Name = "ModifyPageLayoutWorkflow";
            activitybind1.Path = "Request.PageID";
            this.ChangePageLayout.Name = "ChangePageLayout";
            activitybind2.Name = "ModifyPageLayoutWorkflow";
            activitybind2.Path = "Request.PageID";
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("LayoutType", typeof(Dropthings.Business.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind3.Name = "ModifyPageLayoutWorkflow";
            activitybind3.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // ModifyPageLayoutWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.ChangePageLayout);
            this.Name = "ModifyPageLayoutWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private Dropthings.Business.Activities.ChangePageLayoutActivity ChangePageLayout;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;
















    }
}
