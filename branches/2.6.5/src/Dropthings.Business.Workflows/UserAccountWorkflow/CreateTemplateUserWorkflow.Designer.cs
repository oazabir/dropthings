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
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference3 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            this.SetException = new System.Workflow.Activities.CodeActivity();
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
            // SetException
            // 
            this.SetException.Name = "SetException";
            this.SetException.ExecuteCode += new System.EventHandler(this.SetException_ExecuteCode);
            // 
            // CreateDeafultWidgetsOnPage
            // 
            this.CreateDeafultWidgetsOnPage.Name = "CreateDeafultWidgetsOnPage";
            activitybind1.Name = "CreateFirstTab";
            activitybind1.Path = "NewPage.ID";
            activitybind2.Name = "CreateTemplateUserWorkflow";
            activitybind2.Path = "Request.RequestedUsername";
            this.CreateDeafultWidgetsOnPage.SetBinding(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity.UserNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.CreateDeafultWidgetsOnPage.SetBinding(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // SetUserCurrentPage
            // 
            this.SetUserCurrentPage.CurrentPage = null;
            this.SetUserCurrentPage.Name = "SetUserCurrentPage";
            activitybind3.Name = "CreateNewUser";
            activitybind3.Path = "NewUserGuid";
            this.SetUserCurrentPage.UserSetting = null;
            this.SetUserCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
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
            this.IfCreated.Activities.Add(this.SetUserCurrentPage);
            this.IfCreated.Activities.Add(this.CreateDeafultWidgetsOnPage);
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
            activitybind4.Name = "CreateTemplateUserWorkflow";
            activitybind4.Path = "Response.UserGuid";
            this.CreateFirstTab.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserId", typeof(Dropthings.Business.Activities.CreateNewPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // AddUserToRoleTemplate
            // 
            this.AddUserToRoleTemplate.Name = "AddUserToRoleTemplate";
            activitybind5.Name = "CreateTemplateUserWorkflow";
            activitybind5.Path = "Request.TemplateRoleName";
            activitybind6.Name = "CreateTemplateUserWorkflow";
            activitybind6.Path = "Response.UserGuid";
            this.AddUserToRoleTemplate.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RoleName", typeof(Dropthings.Business.Activities.UserAccountActivities.AddUserToRoleTemplateActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.AddUserToRoleTemplate.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.AddUserToRoleTemplateActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // AddUserToRole
            // 
            this.AddUserToRole.Name = "AddUserToRole";
            activitybind7.Name = "CreateTemplateUserWorkflow";
            activitybind7.Path = "Request.RoleName";
            activitybind8.Name = "CreateTemplateUserWorkflow";
            activitybind8.Path = "Request.RequestedUsername";
            this.AddUserToRole.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RoleName", typeof(Dropthings.Business.Activities.SetUserRolesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.AddUserToRole.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.SetUserRolesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // CreateNewUser
            // 
            activitybind9.Name = "CreateTemplateUserWorkflow";
            activitybind9.Path = "Request.Email";
            this.CreateNewUser.Name = "CreateNewUser";
            activitybind10.Name = "CreateTemplateUserWorkflow";
            activitybind10.Path = "Response.UserGuid";
            activitybind11.Name = "CreateTemplateUserWorkflow";
            activitybind11.Path = "Request.Password";
            activitybind12.Name = "CreateTemplateUserWorkflow";
            activitybind12.Path = "Request.RequestedUsername";
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("RequestedUsername", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Email", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("NewUserGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.CreateNewUser.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Password", typeof(Dropthings.Business.Activities.UserAccountActivities.CreateUserActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // UserNotExists
            // 
            this.UserNotExists.Activities.Add(this.CreateNewUser);
            this.UserNotExists.Activities.Add(this.AddUserToRole);
            this.UserNotExists.Activities.Add(this.AddUserToRoleTemplate);
            this.UserNotExists.Activities.Add(this.CreateFirstTab);
            this.UserNotExists.Activities.Add(this.FirstPageCreateCheck);
            ruleconditionreference3.ConditionName = "UserNotExists";
            this.UserNotExists.Condition = ruleconditionreference3;
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
            activitybind13.Name = "CreateTemplateUserWorkflow";
            activitybind13.Path = "Response.UserGuid";
            activitybind14.Name = "CreateTemplateUserWorkflow";
            activitybind14.Path = "Request.RequestedUsername";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
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
        private CodeActivity SetException;
        private IfElseBranchActivity FirstPageFailed;
        private IfElseBranchActivity IfCreated;
        private IfElseActivity FirstPageCreateCheck;
        private Dropthings.Business.Activities.SetUserRolesActivity AddUserToRole;











































































    }
}
