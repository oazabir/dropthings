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

namespace Dropthings.Business.Workflows.EntryPointWorkflows
{
    partial class FirstVisitWorkflow
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
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding1 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding2 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding3 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding4 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding5 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference3 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference4 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            this.SetException = new System.Workflow.Activities.CodeActivity();
            this.CallLoadUserVisitWorkflow = new Dropthings.Business.Activities.CallWorkflowActivity();
            this.CreateDefaultWidgets = new Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity();
            this.FirstPageFailed = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfCreated = new System.Workflow.Activities.IfElseBranchActivity();
            this.FirstPageCreateCheck = new System.Workflow.Activities.IfElseActivity();
            this.CreateFirstTab = new Dropthings.Business.Activities.CreateNewPageActivity();
            this.CallCloneUserFromTemplateWorkflow = new Dropthings.Business.Activities.CallWorkflowActivity();
            this.IfSettingTemplateDisable = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfCloneAnonProfileEnabled = new System.Workflow.Activities.IfElseBranchActivity();
            this.CheckIfCloneAnonProfileEnabled = new System.Workflow.Activities.IfElseActivity();
            this.AddUserToGuestRole = new Dropthings.Business.Activities.SetUserRolesActivity();
            this.GetUserSettingTemplates = new Dropthings.Business.Activities.GetUserSettingTemplatesActivity();
            this.GetUserGUID = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // SetException
            // 
            this.SetException.Name = "SetException";
            this.SetException.ExecuteCode += new System.EventHandler(this.SetException_ExecuteCode);
            // 
            // CallLoadUserVisitWorkflow
            // 
            this.CallLoadUserVisitWorkflow.Name = "CallLoadUserVisitWorkflow";
            activitybind1.Name = "FirstVisitWorkflow";
            activitybind1.Path = "Request";
            workflowparameterbinding1.ParameterName = "Request";
            workflowparameterbinding1.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            activitybind2.Name = "FirstVisitWorkflow";
            activitybind2.Path = "Response";
            workflowparameterbinding2.ParameterName = "Response";
            workflowparameterbinding2.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.CallLoadUserVisitWorkflow.Parameters.Add(workflowparameterbinding1);
            this.CallLoadUserVisitWorkflow.Parameters.Add(workflowparameterbinding2);
            this.CallLoadUserVisitWorkflow.Type = typeof(Dropthings.Business.Workflows.EntryPointWorkflows.UserVisitWorkflow);
            // 
            // CreateDefaultWidgets
            // 
            this.CreateDefaultWidgets.Name = "CreateDefaultWidgets";
            activitybind3.Name = "CreateFirstTab";
            activitybind3.Path = "NewPageId";
            this.CreateDefaultWidgets.UserName = null;
            this.CreateDefaultWidgets.SetBinding(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // FirstPageFailed
            // 
            this.FirstPageFailed.Activities.Add(this.SetException);
            ruleconditionreference1.ConditionName = "FirstPageIDZeroOrLess";
            this.FirstPageFailed.Condition = ruleconditionreference1;
            this.FirstPageFailed.Name = "FirstPageFailed";
            // 
            // IfCreated
            // 
            this.IfCreated.Activities.Add(this.CreateDefaultWidgets);
            this.IfCreated.Activities.Add(this.CallLoadUserVisitWorkflow);
            ruleconditionreference2.ConditionName = "FirstPageIDNonZero";
            this.IfCreated.Condition = ruleconditionreference2;
            this.IfCreated.Name = "IfCreated";
            // 
            // FirstPageCreateCheck
            // 
            this.FirstPageCreateCheck.Activities.Add(this.IfCreated);
            this.FirstPageCreateCheck.Activities.Add(this.FirstPageFailed);
            this.FirstPageCreateCheck.Name = "FirstPageCreateCheck";
            // 
            // CreateFirstTab
            // 
            this.CreateFirstTab.Description = "Create the first default tab ";
            this.CreateFirstTab.LayoutType = null;
            this.CreateFirstTab.Name = "CreateFirstTab";
            this.CreateFirstTab.NewPage = null;
            this.CreateFirstTab.NewPageId = 0;
            this.CreateFirstTab.Title = "First Page";
            activitybind4.Name = "GetUserGUID";
            activitybind4.Path = "UserGuid";
            this.CreateFirstTab.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserId", typeof(Dropthings.Business.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // CallCloneUserFromTemplateWorkflow
            // 
            this.CallCloneUserFromTemplateWorkflow.Name = "CallCloneUserFromTemplateWorkflow";
            activitybind5.Name = "FirstVisitWorkflow";
            activitybind5.Path = "Request.UserName";
            workflowparameterbinding3.ParameterName = "CloneWithUserName";
            workflowparameterbinding3.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            activitybind6.Name = "FirstVisitWorkflow";
            activitybind6.Path = "Request";
            workflowparameterbinding4.ParameterName = "Request";
            workflowparameterbinding4.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            activitybind7.Name = "FirstVisitWorkflow";
            activitybind7.Path = "Response";
            workflowparameterbinding5.ParameterName = "Response";
            workflowparameterbinding5.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.CallCloneUserFromTemplateWorkflow.Parameters.Add(workflowparameterbinding3);
            this.CallCloneUserFromTemplateWorkflow.Parameters.Add(workflowparameterbinding4);
            this.CallCloneUserFromTemplateWorkflow.Parameters.Add(workflowparameterbinding5);
            this.CallCloneUserFromTemplateWorkflow.Type = typeof(Dropthings.Business.Workflows.UserAccountWorkflow.SetupUserWithTemplateWorkflow);
            // 
            // IfSettingTemplateDisable
            // 
            this.IfSettingTemplateDisable.Activities.Add(this.CreateFirstTab);
            this.IfSettingTemplateDisable.Activities.Add(this.FirstPageCreateCheck);
            ruleconditionreference3.ConditionName = "CloneAnonProfileDisabled";
            this.IfSettingTemplateDisable.Condition = ruleconditionreference3;
            this.IfSettingTemplateDisable.Name = "IfSettingTemplateDisable";
            // 
            // IfCloneAnonProfileEnabled
            // 
            this.IfCloneAnonProfileEnabled.Activities.Add(this.CallCloneUserFromTemplateWorkflow);
            ruleconditionreference4.ConditionName = "CloneAnonProfileEnabled";
            this.IfCloneAnonProfileEnabled.Condition = ruleconditionreference4;
            this.IfCloneAnonProfileEnabled.Name = "IfCloneAnonProfileEnabled";
            // 
            // CheckIfCloneAnonProfileEnabled
            // 
            this.CheckIfCloneAnonProfileEnabled.Activities.Add(this.IfCloneAnonProfileEnabled);
            this.CheckIfCloneAnonProfileEnabled.Activities.Add(this.IfSettingTemplateDisable);
            this.CheckIfCloneAnonProfileEnabled.Name = "CheckIfCloneAnonProfileEnabled";
            // 
            // AddUserToGuestRole
            // 
            this.AddUserToGuestRole.Description = "set user to guest role by default";
            this.AddUserToGuestRole.Name = "AddUserToGuestRole";
            activitybind8.Name = "GetUserSettingTemplates";
            activitybind8.Path = "AnonUserSettingTemplate.RoleNames";
            activitybind9.Name = "FirstVisitWorkflow";
            activitybind9.Path = "Request.UserName";
            this.AddUserToGuestRole.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.SetUserRolesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.AddUserToGuestRole.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RoleName", typeof(Dropthings.Business.Activities.SetUserRolesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
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
            // GetUserGUID
            // 
            this.GetUserGUID.Description = "Get user GUID from user name";
            this.GetUserGUID.Name = "GetUserGUID";
            activitybind10.Name = "FirstVisitWorkflow";
            activitybind10.Path = "Response.UserGuid";
            activitybind11.Name = "FirstVisitWorkflow";
            activitybind11.Path = "Request.UserName";
            this.GetUserGUID.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.GetUserGUID.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // FirstVisitWorkflow
            // 
            this.Activities.Add(this.GetUserGUID);
            this.Activities.Add(this.GetUserSettingTemplates);
            this.Activities.Add(this.AddUserToGuestRole);
            this.Activities.Add(this.CheckIfCloneAnonProfileEnabled);
            this.Name = "FirstVisitWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.GetUserSettingTemplatesActivity GetUserSettingTemplates;
        private Dropthings.Business.Activities.CallWorkflowActivity CallCloneUserFromTemplateWorkflow;
        private IfElseBranchActivity IfCloneAnonProfileEnabled;
        private IfElseBranchActivity IfSettingTemplateDisable;
        private IfElseActivity CheckIfCloneAnonProfileEnabled;
        private Dropthings.Business.Activities.CallWorkflowActivity CallLoadUserVisitWorkflow;
        private Dropthings.Business.Activities.SetUserRolesActivity AddUserToGuestRole;
        private Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity CreateDefaultWidgets;
        private IfElseBranchActivity FirstPageFailed;
        private IfElseBranchActivity IfCreated;
        private IfElseActivity FirstPageCreateCheck;
        private CodeActivity SetException;
        private Dropthings.Business.Activities.CreateNewPageActivity CreateFirstTab;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGUID;















































































































    }
}
