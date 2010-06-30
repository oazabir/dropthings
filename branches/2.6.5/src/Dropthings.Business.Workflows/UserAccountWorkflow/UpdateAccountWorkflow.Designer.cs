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

namespace Dropthings.Business.Workflows.UserAccountWorkflow
{
	partial class UpdateAccountWorkflow
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
            this.UpdateAccount = new Dropthings.Business.Activities.UserAccountActivities.UpdateAccountActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // UpdateAccount
            // 
            this.UpdateAccount.Name = "UpdateAccount";
            activitybind1.Name = "UpdateAccountWorkflow";
            activitybind1.Path = "Request.Email";
            activitybind2.Name = "GetUserGuid";
            activitybind2.Path = "UserGuid";
            this.UpdateAccount.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("NewEmail", typeof(Dropthings.Business.Activities.UserAccountActivities.UpdateAccountActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.UpdateAccount.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.UpdateAccountActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind3.Name = "UpdateAccountWorkflow";
            activitybind3.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // UpdateAccountWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.UpdateAccount);
            this.Name = "UpdateAccountWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private Dropthings.Business.Activities.UserAccountActivities.UpdateAccountActivity UpdateAccount;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;



    }
}
