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
    partial class CreateTemplateUserWorkflow
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
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference3 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference4 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference5 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            this.SetException2 = new System.Workflow.Activities.CodeActivity();
            this.SecondPageFailed = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfSecondPageCreated = new System.Workflow.Activities.IfElseBranchActivity();
            this.SetException = new System.Workflow.Activities.CodeActivity();
            this.SecondPageCheck = new System.Workflow.Activities.IfElseActivity();
            this.CreateSecondPage = new Dropthings.Business.Activities.CreateNewPageActivity();
            this.CreateDeafultWidgetsOnPage = new Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity();
            this.SetUserCurrentPage = new Dropthings.Business.Activities.GetUserSettingActivity();
            this.FirstPageFailed = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfCreated = new System.Workflow.Activities.IfElseBranchActivity();
            this.FirstPageCreateCheck = new System.Workflow.Activities.IfElseActivity();
            this.CreateFirstTab = new Dropthings.Business.Activities.CreateNewPageActivity();
            this.AddUserToRoleTemplate = new Dropthings.Business.Activities.UserAccountActivities.AddUserToRoleTemplateActivity();
            this.AddUserToRole = new Dropthings.Business.Activities.SetUserRolesActivity();
            this.CreateNewUser = new Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity();
            this.UserNotExists = new System.Workflow.Activities.IfElseBranchActivity();
            this.CheckIfUserExists = new System.Workflow.Activities.IfElseActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // SetException2
            // 
            this.SetException2.Name = "SetException2";
            this.SetException2.ExecuteCode += new System.EventHandler(this.SetException_ExecuteCode);
            // 
            // SecondPageFailed
            // 
            this.SecondPageFailed.Activities.Add(this.SetException2);
            ruleconditionreference1.ConditionName = "SecondPageIDZeroOrLess";
            this.SecondPageFailed.Condition = ruleconditionreference1;
            this.SecondPageFailed.Name = "SecondPageFailed";
            // 
            // IfSecondPageCreated
            // 
            ruleconditionreference2.ConditionName = "SecondPageIDNonZero";
            this.IfSecondPageCreated.Condition = ruleconditionreference2;
            this.IfSecondPageCreated.Name = "IfSecondPageCreated";
            // 
            // SetException
            // 
            this.SetException.Name = "SetException";
            this.SetException.ExecuteCode += new System.EventHandler(this.SetException_ExecuteCode);
            // 
            // SecondPageCheck
            // 
            this.SecondPageCheck.Activities.Add(this.IfSecondPageCreated);
            this.SecondPageCheck.Activities.Add(this.SecondPageFailed);
            this.SecondPageCheck.Name = "SecondPageCheck";
            // 
            // CreateSecondPage
            // 
            this.CreateSecondPage.LayoutType = null;
            this.CreateSecondPage.Name = "CreateSecondPage";
            this.CreateSecondPage.NewPage = null;
            this.CreateSecondPage.NewPageId = 0;
            this.CreateSecondPage.Title = "Second Page";
            activitybind1.Name = "CreateTemplateUserWorkflow";
            activitybind1.Path = "Response.UserGuid";
            this.CreateSecondPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserId", typeof(Dropthings.Business.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // CreateDeafultWidgetsOnPage
            // 
            this.CreateDeafultWidgetsOnPage.Name = "CreateDeafultWidgetsOnPage";
            activitybind2.Name = "CreateFirstTab";
            activitybind2.Path = "NewPage.ID";
            activitybind3.Name = "CreateTemplateUserWorkflow";
            activitybind3.Path = "Request.RequestedUsername";
            this.CreateDeafultWidgetsOnPage.SetBinding(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity.UserNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.CreateDeafultWidgetsOnPage.SetBinding(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // SetUserCurrentPage
            // 
            this.SetUserCurrentPage.CurrentPage = null;
            this.SetUserCurrentPage.Name = "SetUserCurrentPage";
            activitybind4.Name = "CreateNewUser";
            activitybind4.Path = "NewUserGuid";
            this.SetUserCurrentPage.UserSetting = null;
            this.SetUserCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // FirstPageFailed
            // 
            this.FirstPageFailed.Activities.Add(this.SetException);
            ruleconditionreference3.ConditionName = "FirstPageIDZeroOrLess";
            this.FirstPageFailed.Condition = ruleconditionreference3;
            this.FirstPageFailed.Name = "FirstPageFailed";
            // 
            // IfCreated
            // 
            this.IfCreated.Activities.Add(this.SetUserCurrentPage);
            this.IfCreated.Activities.Add(this.CreateDeafultWidgetsOnPage);
            this.IfCreated.Activities.Add(this.CreateSecondPage);
            this.IfCreated.Activities.Add(this.SecondPageCheck);
            ruleconditionreference4.ConditionName = "FirstPageIDNonZero";
            this.IfCreated.Condition = ruleconditionreference4;
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
            activitybind5.Name = "CreateTemplateUserWorkflow";
            activitybind5.Path = "Response.UserGuid";
            this.CreateFirstTab.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserId", typeof(Dropthings.Business.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // AddUserToRoleTemplate
            // 
            this.AddUserToRoleTemplate.Name = "AddUserToRoleTemplate";
            activitybind6.Name = "CreateTemplateUserWorkflow";
            activitybind6.Path = "Request.TemplateRoleName";
            activitybind7.Name = "CreateTemplateUserWorkflow";
            activitybind7.Path = "Response.UserGuid";
            this.AddUserToRoleTemplate.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RoleName", typeof(Dropthings.Business.Activities.UserAccountActivities.AddUserToRoleTemplateActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.AddUserToRoleTemplate.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.AddUserToRoleTemplateActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // AddUserToRole
            // 
            this.AddUserToRole.Name = "AddUserToRole";
            activitybind8.Name = "CreateTemplateUserWorkflow";
            activitybind8.Path = "Request.RoleName";
            activitybind9.Name = "CreateTemplateUserWorkflow";
            activitybind9.Path = "Request.RequestedUsername";
            this.AddUserToRole.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RoleName", typeof(Dropthings.Business.Activities.SetUserRolesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.AddUserToRole.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.SetUserRolesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // CreateNewUser
            // 
            activitybind10.Name = "CreateTemplateUserWorkflow";
            activitybind10.Path = "Request.Email";
            this.CreateNewUser.Name = "CreateNewUser";
            activitybind11.Name = "CreateTemplateUserWorkflow";
            activitybind11.Path = "Response.UserGuid";
            activitybind12.Name = "CreateTemplateUserWorkflow";
            activitybind12.Path = "Request.Password";
            activitybind13.Name = "CreateTemplateUserWorkflow";
            activitybind13.Path = "Request.RequestedUsername";
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RequestedUsername", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Email", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("NewUserGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Password", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // UserNotExists
            // 
            this.UserNotExists.Activities.Add(this.CreateNewUser);
            this.UserNotExists.Activities.Add(this.AddUserToRole);
            this.UserNotExists.Activities.Add(this.AddUserToRoleTemplate);
            this.UserNotExists.Activities.Add(this.CreateFirstTab);
            this.UserNotExists.Activities.Add(this.FirstPageCreateCheck);
            ruleconditionreference5.ConditionName = "UserNotExists";
            this.UserNotExists.Condition = ruleconditionreference5;
            this.UserNotExists.Name = "UserNotExists";
            // 
            // CheckIfUserExists
            // 
            this.CheckIfUserExists.Activities.Add(this.UserNotExists);
            this.CheckIfUserExists.Name = "CheckIfUserExists";
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            activitybind14.Name = "CreateTemplateUserWorkflow";
            activitybind14.Path = "Response.UserGuid";
            activitybind15.Name = "CreateTemplateUserWorkflow";
            activitybind15.Path = "Request.RequestedUsername";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            // 
            // CreateTemplateUserWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.CheckIfUserExists);
            this.Name = "CreateTemplateUserWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.UserAccountActivities.AddUserToRoleTemplateActivity AddUserToRoleTemplate;
        private Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity CreateDeafultWidgetsOnPage;
        private Dropthings.Business.Activities.GetUserSettingActivity SetUserCurrentPage;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;
        private IfElseBranchActivity UserNotExists;
        private IfElseActivity CheckIfUserExists;
        private Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity CreateNewUser;
        private Dropthings.Business.Activities.CreateNewPageActivity CreateFirstTab;
        private CodeActivity SetException2;
        private IfElseBranchActivity SecondPageFailed;
        private IfElseBranchActivity IfSecondPageCreated;
        private CodeActivity SetException;
        private IfElseActivity SecondPageCheck;
        private Dropthings.Business.Activities.CreateNewPageActivity CreateSecondPage;
        private IfElseBranchActivity FirstPageFailed;
        private IfElseBranchActivity IfCreated;
        private IfElseActivity FirstPageCreateCheck;
        private Dropthings.Business.Activities.SetUserRolesActivity AddUserToRole;










































































    }
}
