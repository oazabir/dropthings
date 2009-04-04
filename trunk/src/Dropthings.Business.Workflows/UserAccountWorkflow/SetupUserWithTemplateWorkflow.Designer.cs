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
    partial class SetupUserWithTemplateWorkflow
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
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            this.CloneWidgetInstance = new Dropthings.Business.Activities.CloneWidgetInstanceActivity();
            this.ForEachWidgetInstanceInZoneToClone = new ForEachActivity.ForEach();
            this.GetWidgetInstancesInZoneToClone = new Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity();
            this.CloneColumn = new Dropthings.Business.Activities.CloneColumnActivity();
            this.CloneWidgetZone = new Dropthings.Business.Activities.AddWidgetZoneActivity();
            this.GetWidgetZoneToClone = new Dropthings.Business.Activities.GetWidgetZoneActivity();
            this.ColumnCloneSquence = new System.Workflow.Activities.SequenceActivity();
            this.ForEachColumnsOfPageToClone = new ForEachActivity.ForEach();
            this.GetColumnsOfPageToClone = new Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity();
            this.ClonePage = new Dropthings.Business.Activities.ClonePageActivity();
            this.PageCloneSequence = new System.Workflow.Activities.SequenceActivity();
            this.SetUserCurrentPage = new Dropthings.Business.Activities.GetUserSettingActivity();
            this.GetUserPages = new Dropthings.Business.Activities.GetUserPagesActivity();
            this.ForEachUserPagesToClone = new ForEachActivity.ForEach();
            this.GetUserPagesToClone = new Dropthings.Business.Activities.GetUserPagesActivity();
            this.CloneWithUserExists = new System.Workflow.Activities.IfElseBranchActivity();
            this.CheckIfCloneWithUserExists = new System.Workflow.Activities.IfElseActivity();
            this.GetRoleTemplate = new Dropthings.Business.Activities.UserAccountActivities.GetRoleTemplateActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // CloneWidgetInstance
            // 
            this.CloneWidgetInstance.Name = "CloneWidgetInstance";
            this.CloneWidgetInstance.WidgetInstance = null;
            activitybind1.Name = "CloneWidgetZone";
            activitybind1.Path = "NewWidgetZone.ID";
            this.CloneWidgetInstance.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.CloneWidgetInstanceActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            activitybind2.Name = "GetWidgetInstancesInZoneToClone";
            activitybind2.Path = "WidgetInstances";
            // 
            // ForEachWidgetInstanceInZoneToClone
            // 
            this.ForEachWidgetInstanceInZoneToClone.Activities.Add(this.CloneWidgetInstance);
            this.ForEachWidgetInstanceInZoneToClone.Description = "A generic flow control activity that executes once for each item in a collection." +
                "";
            this.ForEachWidgetInstanceInZoneToClone.Name = "ForEachWidgetInstanceInZoneToClone";
            this.ForEachWidgetInstanceInZoneToClone.Iterating += new System.EventHandler(this.OnForEachWidgetInstance);
            this.ForEachWidgetInstanceInZoneToClone.SetBinding(ForEachActivity.ForEach.ItemsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetWidgetInstancesInZoneToClone
            // 
            this.GetWidgetInstancesInZoneToClone.Name = "GetWidgetInstancesInZoneToClone";
            this.GetWidgetInstancesInZoneToClone.WidgetInstances = null;
            this.GetWidgetInstancesInZoneToClone.WidgetZoneId = 0;
            // 
            // CloneColumn
            // 
            this.CloneColumn.ColumnToClone = null;
            this.CloneColumn.Name = "CloneColumn";
            this.CloneColumn.NewColumn = null;
            activitybind3.Name = "ClonePage";
            activitybind3.Path = "NewPage.ID";
            activitybind4.Name = "CloneWidgetZone";
            activitybind4.Path = "NewWidgetZone.ID";
            this.CloneColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.CloneColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.CloneColumn.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.CloneColumnActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // CloneWidgetZone
            // 
            this.CloneWidgetZone.Name = "CloneWidgetZone";
            this.CloneWidgetZone.NewWidgetZone = null;
            activitybind5.Name = "GetWidgetZoneToClone";
            activitybind5.Path = "WidgetZone.Title";
            this.CloneWidgetZone.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneTitle", typeof(Dropthings.Business.Activities.AddWidgetZoneActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // GetWidgetZoneToClone
            // 
            this.GetWidgetZoneToClone.Name = "GetWidgetZoneToClone";
            this.GetWidgetZoneToClone.WidgetZone = null;
            this.GetWidgetZoneToClone.ZoneId = 0;
            // 
            // ColumnCloneSquence
            // 
            this.ColumnCloneSquence.Activities.Add(this.GetWidgetZoneToClone);
            this.ColumnCloneSquence.Activities.Add(this.CloneWidgetZone);
            this.ColumnCloneSquence.Activities.Add(this.CloneColumn);
            this.ColumnCloneSquence.Activities.Add(this.GetWidgetInstancesInZoneToClone);
            this.ColumnCloneSquence.Activities.Add(this.ForEachWidgetInstanceInZoneToClone);
            this.ColumnCloneSquence.Name = "ColumnCloneSquence";
            activitybind6.Name = "GetColumnsOfPageToClone";
            activitybind6.Path = "Columns";
            // 
            // ForEachColumnsOfPageToClone
            // 
            this.ForEachColumnsOfPageToClone.Activities.Add(this.ColumnCloneSquence);
            this.ForEachColumnsOfPageToClone.Description = "A generic flow control activity that executes once for each item in a collection." +
                "";
            this.ForEachColumnsOfPageToClone.Name = "ForEachColumnsOfPageToClone";
            this.ForEachColumnsOfPageToClone.Iterating += new System.EventHandler(this.OnForEachColumnOfPage);
            this.ForEachColumnsOfPageToClone.SetBinding(ForEachActivity.ForEach.ItemsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // GetColumnsOfPageToClone
            // 
            this.GetColumnsOfPageToClone.Columns = null;
            this.GetColumnsOfPageToClone.Name = "GetColumnsOfPageToClone";
            this.GetColumnsOfPageToClone.PageId = 0;
            // 
            // ClonePage
            // 
            this.ClonePage.Name = "ClonePage";
            this.ClonePage.NewPage = null;
            this.ClonePage.PageToClone = null;
            activitybind7.Name = "GetUserGuid";
            activitybind7.Path = "UserGuid";
            this.ClonePage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserId", typeof(Dropthings.Business.Activities.ClonePageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // PageCloneSequence
            // 
            this.PageCloneSequence.Activities.Add(this.ClonePage);
            this.PageCloneSequence.Activities.Add(this.GetColumnsOfPageToClone);
            this.PageCloneSequence.Activities.Add(this.ForEachColumnsOfPageToClone);
            this.PageCloneSequence.Name = "PageCloneSequence";
            // 
            // SetUserCurrentPage
            // 
            activitybind8.Name = "SetupUserWithTemplateWorkflow";
            activitybind8.Path = "Response.CurrentPage";
            this.SetUserCurrentPage.Name = "SetUserCurrentPage";
            activitybind9.Name = "GetUserGuid";
            activitybind9.Path = "UserGuid";
            activitybind10.Name = "SetupUserWithTemplateWorkflow";
            activitybind10.Path = "Response.UserSetting";
            this.SetUserCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.SetUserCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserSetting", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.SetUserCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("CurrentPage", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // GetUserPages
            // 
            this.GetUserPages.Name = "GetUserPages";
            activitybind11.Name = "SetupUserWithTemplateWorkflow";
            activitybind11.Path = "Response.UserPages";
            activitybind12.Name = "GetUserGuid";
            activitybind12.Path = "UserGuid";
            this.GetUserPages.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("Pages", typeof(Dropthings.Business.Activities.GetUserPagesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.GetUserPages.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserPagesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            activitybind13.Name = "GetUserPagesToClone";
            activitybind13.Path = "Pages";
            // 
            // ForEachUserPagesToClone
            // 
            this.ForEachUserPagesToClone.Activities.Add(this.PageCloneSequence);
            this.ForEachUserPagesToClone.Description = "A generic flow control activity that executes once for each item in a collection." +
                "";
            this.ForEachUserPagesToClone.Name = "ForEachUserPagesToClone";
            this.ForEachUserPagesToClone.Iterating += new System.EventHandler(this.OnForEachPage);
            this.ForEachUserPagesToClone.SetBinding(ForEachActivity.ForEach.ItemsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            // 
            // GetUserPagesToClone
            // 
            this.GetUserPagesToClone.Name = "GetUserPagesToClone";
            this.GetUserPagesToClone.Pages = null;
            activitybind14.Name = "GetRoleTemplate";
            activitybind14.Path = "RoleTemplate.TemplateUserId";
            this.GetUserPagesToClone.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserPagesActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            // 
            // CloneWithUserExists
            // 
            this.CloneWithUserExists.Activities.Add(this.GetUserPagesToClone);
            this.CloneWithUserExists.Activities.Add(this.ForEachUserPagesToClone);
            this.CloneWithUserExists.Activities.Add(this.GetUserPages);
            this.CloneWithUserExists.Activities.Add(this.SetUserCurrentPage);
            ruleconditionreference1.ConditionName = "CloneWithUserGuidNotEmpty";
            this.CloneWithUserExists.Condition = ruleconditionreference1;
            this.CloneWithUserExists.Name = "CloneWithUserExists";
            // 
            // CheckIfCloneWithUserExists
            // 
            this.CheckIfCloneWithUserExists.Activities.Add(this.CloneWithUserExists);
            this.CheckIfCloneWithUserExists.Name = "CheckIfCloneWithUserExists";
            // 
            // GetRoleTemplate
            // 
            this.GetRoleTemplate.Name = "GetRoleTemplate";
            this.GetRoleTemplate.RoleTemplate = null;
            activitybind15.Name = "GetUserGuid";
            activitybind15.Path = "UserGuid";
            this.GetRoleTemplate.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.UserAccountActivities.GetRoleTemplateActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            activitybind16.Name = "SetupUserWithTemplateWorkflow";
            activitybind16.Path = "Response.UserGuid";
            activitybind17.Name = "SetupUserWithTemplateWorkflow";
            activitybind17.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            // 
            // SetupUserWithTemplateWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.GetRoleTemplate);
            this.Activities.Add(this.CheckIfCloneWithUserExists);
            this.Name = "SetupUserWithTemplateWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.UserAccountActivities.GetRoleTemplateActivity GetRoleTemplate;
        private Dropthings.Business.Activities.GetUserPagesActivity GetUserPages;
        private Dropthings.Business.Activities.GetUserSettingActivity SetUserCurrentPage;
        private Dropthings.Business.Activities.GetUserPagesActivity GetUserPagesToClone;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;
        private Dropthings.Business.Activities.CloneColumnActivity CloneColumn;
        private Dropthings.Business.Activities.AddWidgetZoneActivity CloneWidgetZone;
        private Dropthings.Business.Activities.ClonePageActivity ClonePage;
        private Dropthings.Business.Activities.CloneWidgetInstanceActivity CloneWidgetInstance;
        private ForEachActivity.ForEach ForEachWidgetInstanceInZoneToClone;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity GetWidgetInstancesInZoneToClone;
        private Dropthings.Business.Activities.GetWidgetZoneActivity GetWidgetZoneToClone;
        private SequenceActivity ColumnCloneSquence;
        private ForEachActivity.ForEach ForEachColumnsOfPageToClone;
        private Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity GetColumnsOfPageToClone;
        private SequenceActivity PageCloneSequence;
        private IfElseBranchActivity CloneWithUserExists;
        private IfElseActivity CheckIfCloneWithUserExists;
        private ForEachActivity.ForEach ForEachUserPagesToClone;


































































    }
}
