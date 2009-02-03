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
	partial class ActivateAccountWorkflow
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
            this.ActivateAccount = new Dropthings.Business.Activities.UserAccountActivities.ActivateUserAccount();
            // 
            // ActivateAccount
            // 
            activitybind1.Name = "ActivateAccountWorkflow";
            activitybind1.Path = "Request.ActivationKey";
            this.ActivateAccount.Name = "ActivateAccount";
            activitybind2.Name = "ActivateAccountWorkflow";
            activitybind2.Path = "Response.Token";
            this.ActivateAccount.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("ActivationKey", typeof(Dropthings.Business.Activities.UserAccountActivities.ActivateUserAccount)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ActivateAccount.SetBinding(Dropthings.Business.Activities.UserAccountActivities.ActivateUserAccount.TokenProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // ActivateAccountWorkflow
            // 
            this.Activities.Add(this.ActivateAccount);
            this.Name = "ActivateAccountWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private Dropthings.Business.Activities.UserAccountActivities.ActivateUserAccount ActivateUserAccount;
        private Dropthings.Business.Activities.UserAccountActivities.ActivateUserAccount ActivateAccount;


    }
}
