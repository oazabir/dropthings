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
    partial class UserVisitWorkflow
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
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding1 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.WorkflowParameterBinding workflowparameterbinding2 = new System.Workflow.ComponentModel.WorkflowParameterBinding();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference3 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference4 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference5 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind19 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind20 = new System.Workflow.ComponentModel.ActivityBind();
            this.ReloadUserSetting = new Dropthings.Business.Activities.GetUserSettingActivity();
            this.ChangeCurrentPage = new Dropthings.Business.Activities.SetCurrentPageActivity();
            this.GetUserPagesRetry = new Dropthings.Business.Activities.GetUserPagesActivity();
            this.CallWorkflowSetupUserWithTemplate = new Dropthings.Business.Activities.CallWorkflowActivity();
            this.IfCurrentPageIsDifferentThanUserSetting = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfSettingTemplateEnabled = new System.Workflow.Activities.IfElseBranchActivity();
            this.ReturnUserPageSetup = new System.Workflow.Activities.CodeActivity();
            this.CheckIfCurrentPageHasChanged = new System.Workflow.Activities.IfElseActivity();
            this.DecideCurrentPage = new Dropthings.Business.Activities.DecideCurrentPageActivity();
            this.GetUserSetting = new Dropthings.Business.Activities.GetUserSettingActivity();
            this.CheckIfSettingTemplateEnabled = new System.Workflow.Activities.IfElseActivity();
            this.GetUserSettingTemplates = new Dropthings.Business.Activities.GetUserSettingTemplatesActivity();
            this.IfUserHasPages = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfUserHasNoPages = new System.Workflow.Activities.IfElseBranchActivity();
            this.CheckIfUserHasPages = new System.Workflow.Activities.IfElseActivity();
            this.CheckIfUserHasNoPages = new System.Workflow.Activities.IfElseActivity();
            this.GetUserPages = new Dropthings.Business.Activities.GetUserPagesActivity();
            this.ifElseBranchActivity2 = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfUserGuidNotEmpty = new System.Workflow.Activities.IfElseBranchActivity();
            this.CheckUserGuid = new System.Workflow.Activities.IfElseActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // ReloadUserSetting
            // 
            activitybind1.Name = "UserVisitWorkflow";
            activitybind1.Path = "Response.CurrentPage";
            this.ReloadUserSetting.Name = "ReloadUserSetting";
            activitybind2.Name = "GetUserGuid";
            activitybind2.Path = "UserGuid";
            activitybind3.Name = "UserVisitWorkflow";
            activitybind3.Path = "Response.UserSetting";
            this.ReloadUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("CurrentPage", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ReloadUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserSetting", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.ReloadUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // ChangeCurrentPage
            // 
            this.ChangeCurrentPage.Name = "ChangeCurrentPage";
            activitybind4.Name = "DecideCurrentPage";
            activitybind4.Path = "CurrentPageId";
            activitybind5.Name = "GetUserGuid";
            activitybind5.Path = "UserGuid";
            this.ChangeCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.ChangeCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // GetUserPagesRetry
            // 
            this.GetUserPagesRetry.Name = "GetUserPagesRetry";
            activitybind6.Name = "UserVisitWorkflow";
            activitybind6.Path = "Response.UserPages";
            activitybind7.Name = "GetUserGuid";
            activitybind7.Path = "UserGuid";
            this.GetUserPagesRetry.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Pages", typeof(Dropthings.Business.Activities.GetUserPagesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.GetUserPagesRetry.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserPagesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // CallWorkflowSetupUserWithTemplate
            // 
            this.CallWorkflowSetupUserWithTemplate.Name = "CallWorkflowSetupUserWithTemplate";
            activitybind8.Name = "UserVisitWorkflow";
            activitybind8.Path = "Request";
            workflowparameterbinding1.ParameterName = "Request";
            workflowparameterbinding1.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            activitybind9.Name = "UserVisitWorkflow";
            activitybind9.Path = "Response";
            workflowparameterbinding2.ParameterName = "Response";
            workflowparameterbinding2.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.CallWorkflowSetupUserWithTemplate.Parameters.Add(workflowparameterbinding1);
            this.CallWorkflowSetupUserWithTemplate.Parameters.Add(workflowparameterbinding2);
            this.CallWorkflowSetupUserWithTemplate.Type = typeof(Dropthings.Business.Workflows.UserAccountWorkflow.SetupUserWithTemplateWorkflow);
            // 
            // IfCurrentPageIsDifferentThanUserSetting
            // 
            this.IfCurrentPageIsDifferentThanUserSetting.Activities.Add(this.ChangeCurrentPage);
            this.IfCurrentPageIsDifferentThanUserSetting.Activities.Add(this.ReloadUserSetting);
            ruleconditionreference1.ConditionName = "CurrentPageHasChanged";
            this.IfCurrentPageIsDifferentThanUserSetting.Condition = ruleconditionreference1;
            this.IfCurrentPageIsDifferentThanUserSetting.Name = "IfCurrentPageIsDifferentThanUserSetting";
            // 
            // IfSettingTemplateEnabled
            // 
            this.IfSettingTemplateEnabled.Activities.Add(this.CallWorkflowSetupUserWithTemplate);
            this.IfSettingTemplateEnabled.Activities.Add(this.GetUserPagesRetry);
            ruleconditionreference2.ConditionName = "SettingTemplateEnabled";
            this.IfSettingTemplateEnabled.Condition = ruleconditionreference2;
            this.IfSettingTemplateEnabled.Name = "IfSettingTemplateEnabled";
            // 
            // ReturnUserPageSetup
            // 
            this.ReturnUserPageSetup.Name = "ReturnUserPageSetup";
            this.ReturnUserPageSetup.ExecuteCode += new System.EventHandler(this.ReturnUserPageSetup_ExecuteCode);
            // 
            // CheckIfCurrentPageHasChanged
            // 
            this.CheckIfCurrentPageHasChanged.Activities.Add(this.IfCurrentPageIsDifferentThanUserSetting);
            this.CheckIfCurrentPageHasChanged.Name = "CheckIfCurrentPageHasChanged";
            // 
            // DecideCurrentPage
            // 
            activitybind10.Name = "UserVisitWorkflow";
            activitybind10.Path = "Response.CurrentPage";
            activitybind11.Name = "UserVisitWorkflow";
            activitybind11.Path = "Response.CurrentPage.ID";
            this.DecideCurrentPage.Name = "DecideCurrentPage";
            activitybind12.Name = "UserVisitWorkflow";
            activitybind12.Path = "Request.PageName";
            activitybind13.Name = "GetUserGuid";
            activitybind13.Path = "UserGuid";
            this.DecideCurrentPage.SetBinding(Dropthings.Business.Activities.DecideCurrentPageActivity.CurrentPageProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.DecideCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageName", typeof(Dropthings.Business.Activities.DecideCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.DecideCurrentPage.SetBinding(Dropthings.Business.Activities.DecideCurrentPageActivity.UserGuidProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.DecideCurrentPage.SetBinding(Dropthings.Business.Activities.DecideCurrentPageActivity.CurrentPageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // GetUserSetting
            // 
            activitybind14.Name = "UserVisitWorkflow";
            activitybind14.Path = "Response.CurrentPage";
            this.GetUserSetting.Name = "GetUserSetting";
            activitybind15.Name = "GetUserGuid";
            activitybind15.Path = "UserGuid";
            activitybind16.Name = "UserVisitWorkflow";
            activitybind16.Path = "Response.UserSetting";
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("CurrentPage", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserSetting", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            // 
            // CheckIfSettingTemplateEnabled
            // 
            this.CheckIfSettingTemplateEnabled.Activities.Add(this.IfSettingTemplateEnabled);
            this.CheckIfSettingTemplateEnabled.Name = "CheckIfSettingTemplateEnabled";
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
            // IfUserHasPages
            // 
            this.IfUserHasPages.Activities.Add(this.GetUserSetting);
            this.IfUserHasPages.Activities.Add(this.DecideCurrentPage);
            this.IfUserHasPages.Activities.Add(this.CheckIfCurrentPageHasChanged);
            this.IfUserHasPages.Activities.Add(this.ReturnUserPageSetup);
            ruleconditionreference3.ConditionName = "UserHasPages";
            this.IfUserHasPages.Condition = ruleconditionreference3;
            this.IfUserHasPages.Name = "IfUserHasPages";
            // 
            // IfUserHasNoPages
            // 
            this.IfUserHasNoPages.Activities.Add(this.GetUserSettingTemplates);
            this.IfUserHasNoPages.Activities.Add(this.CheckIfSettingTemplateEnabled);
            ruleconditionreference4.ConditionName = "UserHasNoPages";
            this.IfUserHasNoPages.Condition = ruleconditionreference4;
            this.IfUserHasNoPages.Name = "IfUserHasNoPages";
            // 
            // CheckIfUserHasPages
            // 
            this.CheckIfUserHasPages.Activities.Add(this.IfUserHasPages);
            this.CheckIfUserHasPages.Name = "CheckIfUserHasPages";
            // 
            // CheckIfUserHasNoPages
            // 
            this.CheckIfUserHasNoPages.Activities.Add(this.IfUserHasNoPages);
            this.CheckIfUserHasNoPages.Name = "CheckIfUserHasNoPages";
            // 
            // GetUserPages
            // 
            this.GetUserPages.Name = "GetUserPages";
            activitybind17.Name = "UserVisitWorkflow";
            activitybind17.Path = "Response.UserPages";
            activitybind18.Name = "GetUserGuid";
            activitybind18.Path = "UserGuid";
            this.GetUserPages.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserPagesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
            this.GetUserPages.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Pages", typeof(Dropthings.Business.Activities.GetUserPagesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            // 
            // ifElseBranchActivity2
            // 
            this.ifElseBranchActivity2.Name = "ifElseBranchActivity2";
            // 
            // IfUserGuidNotEmpty
            // 
            this.IfUserGuidNotEmpty.Activities.Add(this.GetUserPages);
            this.IfUserGuidNotEmpty.Activities.Add(this.CheckIfUserHasNoPages);
            this.IfUserGuidNotEmpty.Activities.Add(this.CheckIfUserHasPages);
            ruleconditionreference5.ConditionName = "UserGuidNotEmpty";
            this.IfUserGuidNotEmpty.Condition = ruleconditionreference5;
            this.IfUserGuidNotEmpty.Name = "IfUserGuidNotEmpty";
            // 
            // CheckUserGuid
            // 
            this.CheckUserGuid.Activities.Add(this.IfUserGuidNotEmpty);
            this.CheckUserGuid.Activities.Add(this.ifElseBranchActivity2);
            this.CheckUserGuid.Name = "CheckUserGuid";
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Description = "name";
            this.GetUserGuid.Name = "GetUserGuid";
            activitybind19.Name = "UserVisitWorkflow";
            activitybind19.Path = "Response.UserGuid";
            activitybind20.Name = "UserVisitWorkflow";
            activitybind20.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind20)));
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
            // 
            // UserVisitWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.CheckUserGuid);
            this.Name = "UserVisitWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.GetUserSettingTemplatesActivity GetUserSettingTemplates;
        private Dropthings.Business.Activities.CallWorkflowActivity CallWorkflowSetupUserWithTemplate;
        private IfElseBranchActivity IfSettingTemplateEnabled;
        private IfElseActivity CheckIfSettingTemplateEnabled;
        private Dropthings.Business.Activities.GetUserPagesActivity GetUserPagesRetry;
        private Dropthings.Business.Activities.GetUserSettingActivity ReloadUserSetting;
        private Dropthings.Business.Activities.SetCurrentPageActivity ChangeCurrentPage;
        private IfElseBranchActivity IfCurrentPageIsDifferentThanUserSetting;
        private CodeActivity ReturnUserPageSetup;
        private IfElseActivity CheckIfCurrentPageHasChanged;
        private Dropthings.Business.Activities.DecideCurrentPageActivity DecideCurrentPage;
        private Dropthings.Business.Activities.GetUserSettingActivity GetUserSetting;
        private IfElseBranchActivity IfUserHasNoPages;
        private IfElseActivity CheckIfUserHasNoPages;
        private IfElseBranchActivity IfUserHasPages;
        private IfElseActivity CheckIfUserHasPages;
        private Dropthings.Business.Activities.GetUserPagesActivity GetUserPages;
        private IfElseBranchActivity ifElseBranchActivity2;
        private IfElseBranchActivity IfUserGuidNotEmpty;
        private IfElseActivity CheckUserGuid;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;









































































































































    }
}
