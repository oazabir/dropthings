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
    partial class UserRegistrationWorkflow
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
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding1 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding2 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding3 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference3 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            this.CreateUserToken = new Dropthings.Business.Activities.UserAccountActivities.CreateUserTokenActivity();
            this.TransferOwnership = new Dropthings.Business.Activities.UserAccountActivities.TransferOwnershipActivity();
            this.GetOldAspnetUser = new Dropthings.Business.Activities.UserAccountActivities.GetAspnetUserActivity();
            this.CallCloneUserFromTemplateWorkflow = new Dropthings.Business.Activities.CallWorkflowActivity();
            this.SetUserName = new System.Workflow.Activities.CodeActivity();
            this.IfActivationRequired = new System.Workflow.Activities.IfElseBranchActivity();
            this.SettingTemplateDisabled = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfCloneRegisteredProfileEnabled = new System.Workflow.Activities.IfElseBranchActivity();
            this.ChckIfActivationRequired = new System.Workflow.Activities.IfElseActivity();
            this.CheckIfCloneRegisteredProfileEnabled = new System.Workflow.Activities.IfElseActivity();
            this.AddUserToRegisteredRole = new Dropthings.Business.Activities.SetUserRolesActivity();
            this.GetUserSettingTemplates = new Dropthings.Business.Activities.GetUserSettingTemplatesActivity();
            this.CreateNewUser = new Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity();
            // 
            // CreateUserToken
            // 
            activitybind1.Name = "UserRegistrationWorkflow";
            activitybind1.Path = "Request.Email";
            this.CreateUserToken.Name = "CreateUserToken";
            activitybind2.Name = "UserRegistrationWorkflow";
            activitybind2.Path = "Response.UnlockKey";
            activitybind3.Name = "CreateNewUser";
            activitybind3.Path = "NewUserGuid";
            this.CreateUserToken.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Email", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserTokenActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.CreateUserToken.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserTokenActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.CreateUserToken.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UnlockKey", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserTokenActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // TransferOwnership
            // 
            this.TransferOwnership.Name = "TransferOwnership";
            activitybind4.Name = "CreateNewUser";
            activitybind4.Path = "NewUserGuid";
            activitybind5.Name = "GetOldAspnetUser";
            activitybind5.Path = "aspnet_User.UserId";
            this.TransferOwnership.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserNewGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.TransferOwnershipActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.TransferOwnership.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserOldGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.TransferOwnershipActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // GetOldAspnetUser
            // 
            this.GetOldAspnetUser.aspnet_User = null;
            this.GetOldAspnetUser.Name = "GetOldAspnetUser";
            activitybind6.Name = "UserRegistrationWorkflow";
            activitybind6.Path = "Request.UserName";
            this.GetOldAspnetUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.UserAccountActivities.GetAspnetUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // CallCloneUserFromTemplateWorkflow
            // 
            this.CallCloneUserFromTemplateWorkflow.Name = "CallCloneUserFromTemplateWorkflow";
            activitybind7.Name = "GetUserSettingTemplateActivity1";
            activitybind7.Path = "RegisteredUserTemplate";
            workflowparameterbinding1.ParameterName = "CloneWithUserName";
            workflowparameterbinding1.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            activitybind8.Name = "UserRegistrationWorkflow";
            activitybind8.Path = "Request";
            workflowparameterbinding2.ParameterName = "Request";
            workflowparameterbinding2.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            activitybind9.Name = "UserRegistrationWorkflow";
            activitybind9.Path = "Response";
            workflowparameterbinding3.ParameterName = "Response";
            workflowparameterbinding3.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.CallCloneUserFromTemplateWorkflow.Parameters.Add(workflowparameterbinding1);
            this.CallCloneUserFromTemplateWorkflow.Parameters.Add(workflowparameterbinding2);
            this.CallCloneUserFromTemplateWorkflow.Parameters.Add(workflowparameterbinding3);
            this.CallCloneUserFromTemplateWorkflow.Type = typeof(Dropthings.Business.Workflows.UserAccountWorkflow.SetupUserWithTemplateWorkflow);
            // 
            // SetUserName
            // 
            this.SetUserName.Name = "SetUserName";
            this.SetUserName.ExecuteCode += new System.EventHandler(this.SetUsernameForSetupUserFromTemplateWotkflow_ExecuteCode);
            // 
            // IfActivationRequired
            // 
            this.IfActivationRequired.Activities.Add(this.CreateUserToken);
            ruleconditionreference1.ConditionName = "ActivationRequired";
            this.IfActivationRequired.Condition = ruleconditionreference1;
            this.IfActivationRequired.Name = "IfActivationRequired";
            // 
            // SettingTemplateDisabled
            // 
            this.SettingTemplateDisabled.Activities.Add(this.GetOldAspnetUser);
            this.SettingTemplateDisabled.Activities.Add(this.TransferOwnership);
            ruleconditionreference2.ConditionName = "CloneRegisteredProfileDisabled";
            this.SettingTemplateDisabled.Condition = ruleconditionreference2;
            this.SettingTemplateDisabled.Name = "SettingTemplateDisabled";
            // 
            // IfCloneRegisteredProfileEnabled
            // 
            this.IfCloneRegisteredProfileEnabled.Activities.Add(this.SetUserName);
            this.IfCloneRegisteredProfileEnabled.Activities.Add(this.CallCloneUserFromTemplateWorkflow);
            ruleconditionreference3.ConditionName = "CloneRegisteredProfileEnabled";
            this.IfCloneRegisteredProfileEnabled.Condition = ruleconditionreference3;
            this.IfCloneRegisteredProfileEnabled.Name = "IfCloneRegisteredProfileEnabled";
            // 
            // ChckIfActivationRequired
            // 
            this.ChckIfActivationRequired.Activities.Add(this.IfActivationRequired);
            this.ChckIfActivationRequired.Name = "ChckIfActivationRequired";
            // 
            // CheckIfCloneRegisteredProfileEnabled
            // 
            this.CheckIfCloneRegisteredProfileEnabled.Activities.Add(this.IfCloneRegisteredProfileEnabled);
            this.CheckIfCloneRegisteredProfileEnabled.Activities.Add(this.SettingTemplateDisabled);
            this.CheckIfCloneRegisteredProfileEnabled.Name = "CheckIfCloneRegisteredProfileEnabled";
            // 
            // AddUserToRegisteredRole
            // 
            this.AddUserToRegisteredRole.Name = "AddUserToRegisteredRole";
            activitybind10.Name = "GetUserSettingTemplates";
            activitybind10.Path = "RegisteredUserSettingTemplate.RoleNames";
            activitybind11.Name = "UserRegistrationWorkflow";
            activitybind11.Path = "Request.RequestedUsername";
            this.AddUserToRegisteredRole.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.SetUserRolesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.AddUserToRegisteredRole.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RoleName", typeof(Dropthings.Business.Activities.SetUserRolesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // GetUserSettingTemplates
            // 
            this.GetUserSettingTemplates.AllUserSettingTemplate = null;
            this.GetUserSettingTemplates.AnonUserSettingTemplate = null;
            this.GetUserSettingTemplates.CloneAnonProfileEnabled = false;
            this.GetUserSettingTemplates.CloneRegisteredProfileEnabled = false;
            this.GetUserSettingTemplates.Name = "GetUserSettingTemplates";
            this.GetUserSettingTemplates.RegisteredUserSettingTemplate = null;
            // 
            // CreateNewUser
            // 
            activitybind12.Name = "UserRegistrationWorkflow";
            activitybind12.Path = "Request.Email";
            this.CreateNewUser.Name = "CreateNewUser";
            activitybind13.Name = "UserRegistrationWorkflow";
            activitybind13.Path = "Response.UserGuid";
            activitybind14.Name = "UserRegistrationWorkflow";
            activitybind14.Path = "Request.Password";
            activitybind15.Name = "UserRegistrationWorkflow";
            activitybind15.Path = "Request.RequestedUsername";
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Email", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Password", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("NewUserGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RequestedUsername", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            // 
            // UserRegistrationWorkflow
            // 
            this.Activities.Add(this.CreateNewUser);
            this.Activities.Add(this.GetUserSettingTemplates);
            this.Activities.Add(this.AddUserToRegisteredRole);
            this.Activities.Add(this.CheckIfCloneRegisteredProfileEnabled);
            this.Activities.Add(this.ChckIfActivationRequired);
            this.Name = "UserRegistrationWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.GetUserSettingTemplatesActivity GetUserSettingTemplates;
        private CodeActivity SetUserName;
        private Dropthings.Business.Activities.CallWorkflowActivity CallCloneUserFromTemplateWorkflow;
        private IfElseBranchActivity SettingTemplateDisabled;
        private IfElseBranchActivity IfCloneRegisteredProfileEnabled;
        private IfElseActivity CheckIfCloneRegisteredProfileEnabled;
        private Dropthings.Business.Activities.SetUserRolesActivity AddUserToRegisteredRole;
        private Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity CreateNewUser;
        private IfElseBranchActivity IfActivationRequired;
        private IfElseActivity ChckIfActivationRequired;
        private Dropthings.Business.Activities.UserAccountActivities.CreateUserTokenActivity CreateUserToken;
        private Dropthings.Business.Activities.UserAccountActivities.GetAspnetUserActivity GetOldAspnetUser;
        private Dropthings.Business.Activities.UserAccountActivities.TransferOwnershipActivity TransferOwnership;


































    }
}
