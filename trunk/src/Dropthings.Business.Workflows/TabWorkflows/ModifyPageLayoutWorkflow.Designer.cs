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
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            this.ChangePageLayout = new Dropthings.Business.Activities.ChangePageLayoutActivity();
            this.GetColumnsInPage = new Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity();
            this.DecideColumnNoFromLayoutTypeActivity = new Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity();
            this.GetUserSetting = new Dropthings.Business.Activities.GetUserSettingActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // ChangePageLayout
            // 
            activitybind1.Name = "GetUserSetting";
            activitybind1.Path = "CurrentPage.ID";
            this.ChangePageLayout.Name = "ChangePageLayout";
            activitybind2.Name = "ModifyPageLayoutWorkflow";
            activitybind2.Path = "CurrentPage.ID";
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("LayoutType", typeof(Dropthings.Business.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetColumnsInPage
            // 
            this.GetColumnsInPage.Columns = null;
            this.GetColumnsInPage.Name = "GetColumnsInPage";
            activitybind3.Name = "ModifyPageLayoutWorkflow";
            activitybind3.Path = "CurrentPage.ID";
            this.GetColumnsInPage.SetBinding(Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // DecideColumnNoFromLayoutTypeActivity
            // 
            activitybind4.Name = "ModifyPageLayoutWorkflow";
            activitybind4.Path = "NewColumns";
            activitybind5.Name = "ModifyPageLayoutWorkflow";
            activitybind5.Path = "Request.LayoutType";
            this.DecideColumnNoFromLayoutTypeActivity.Name = "DecideColumnNoFromLayoutTypeActivity";
            this.DecideColumnNoFromLayoutTypeActivity.SetBinding(Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity.ColumnsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.DecideColumnNoFromLayoutTypeActivity.SetBinding(Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity.LayoutTypeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // GetUserSetting
            // 
            activitybind6.Name = "ModifyPageLayoutWorkflow";
            activitybind6.Path = "CurrentPage";
            this.GetUserSetting.Name = "GetUserSetting";
            activitybind7.Name = "GetUserGuid";
            activitybind7.Path = "UserGuid";
            this.GetUserSetting.UserSetting = null;
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("CurrentPage", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind8.Name = "ModifyPageLayoutWorkflow";
            activitybind8.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // ModifyPageLayoutWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.GetUserSetting);
            this.Activities.Add(this.DecideColumnNoFromLayoutTypeActivity);
            this.Activities.Add(this.GetColumnsInPage);
            this.Activities.Add(this.ChangePageLayout);
            this.Name = "ModifyPageLayoutWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity GetColumnsInPage;
        private Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity DecideColumnNoFromLayoutTypeActivity;
        private Dropthings.Business.Activities.GetUserSettingActivity GetUserSetting;
        private Dropthings.Business.Activities.ChangePageLayoutActivity ChangePageLayout;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;

































    }
}
