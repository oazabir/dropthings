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
	partial class ResetPasswordWorkflow
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
            this.ResetPassword = new Dropthings.Business.Activities.UserAccountActivities.ResetPasswordActivity();
            // 
            // ResetPassword
            // 
            activitybind1.Name = "ResetPasswordWorkflow";
            activitybind1.Path = "Request.Email";
            this.ResetPassword.Name = "ResetPassword";
            activitybind2.Name = "ResetPasswordWorkflow";
            activitybind2.Path = "Response.NewPassword";
            this.ResetPassword.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Email", typeof(Dropthings.Business.Activities.UserAccountActivities.ResetPasswordActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ResetPassword.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("NewPassword", typeof(Dropthings.Business.Activities.UserAccountActivities.ResetPasswordActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // ResetPasswordWorkflow
            // 
            this.Activities.Add(this.ResetPassword);
            this.Name = "ResetPasswordWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private Dropthings.Business.Activities.UserAccountActivities.ResetPasswordActivity ResetPassword;



    }
}
