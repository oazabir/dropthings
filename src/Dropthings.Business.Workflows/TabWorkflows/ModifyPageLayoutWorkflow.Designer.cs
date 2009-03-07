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
            this.moveWidgetFromOnColumnToAnother1 = new Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother();
            this.WhileMoreColumnsToRemove = new System.Workflow.Activities.WhileActivity();
            this.SetCounters = new System.Workflow.Activities.CodeActivity();
            this.IfColumnIncreased = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfColumnDecreased = new System.Workflow.Activities.IfElseBranchActivity();
            this.ChangePageLayout = new Dropthings.Business.Activities.ChangePageLayoutActivity();
            this.ColumnDecreasedOrIncreased = new System.Workflow.Activities.IfElseActivity();
            this.GetColumnsInPage = new Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity();
            this.DecideColumnNoFromLayoutTypeActivity = new Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity();
            this.GetUserSetting = new Dropthings.Business.Activities.GetUserSettingActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // moveWidgetFromOnColumnToAnother1
            // 
            this.moveWidgetFromOnColumnToAnother1.Name = "moveWidgetFromOnColumnToAnother1";
            activitybind1.Name = "ModifyPageLayoutWorkflow";
            activitybind1.Path = "NewColumnNo";
            this.moveWidgetFromOnColumnToAnother1.NewWidgetZone = null;
            activitybind2.Name = "ModifyPageLayoutWorkflow";
            activitybind2.Path = "ColumnCounter";
            this.moveWidgetFromOnColumnToAnother1.OldWidgetZone = null;
            activitybind3.Name = "ModifyPageLayoutWorkflow";
            activitybind3.Path = "CurrentPage.ID";
            this.moveWidgetFromOnColumnToAnother1.SetBinding(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.NewColumnNoProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.moveWidgetFromOnColumnToAnother1.SetBinding(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.OldColumnoProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.moveWidgetFromOnColumnToAnother1.SetBinding(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // WhileMoreColumnsToRemove
            // 
            this.WhileMoreColumnsToRemove.Activities.Add(this.moveWidgetFromOnColumnToAnother1);
            ruleconditionreference1.ConditionName = "ColumnCounterIsOverNewColumnCount";
            this.WhileMoreColumnsToRemove.Condition = ruleconditionreference1;
            this.WhileMoreColumnsToRemove.Name = "WhileMoreColumnsToRemove";
            // 
            // SetCounters
            // 
            this.SetCounters.Name = "SetCounters";
            this.SetCounters.ExecuteCode += new System.EventHandler(this.SetCounters_ExecuteCode);
            // 
            // IfColumnIncreased
            // 
            this.IfColumnIncreased.Name = "IfColumnIncreased";
            // 
            // IfColumnDecreased
            // 
            this.IfColumnDecreased.Activities.Add(this.SetCounters);
            this.IfColumnDecreased.Activities.Add(this.WhileMoreColumnsToRemove);
            ruleconditionreference2.ConditionName = "NewColumnLessThanExistingColumn";
            this.IfColumnDecreased.Condition = ruleconditionreference2;
            this.IfColumnDecreased.Name = "IfColumnDecreased";
            // 
            // ChangePageLayout
            // 
            activitybind4.Name = "GetUserSetting";
            activitybind4.Path = "CurrentPage.ID";
            this.ChangePageLayout.Name = "ChangePageLayout";
            activitybind5.Name = "ModifyPageLayoutWorkflow";
            activitybind5.Path = "CurrentPage.ID";
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("LayoutType", typeof(Dropthings.Business.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // ColumnDecreasedOrIncreased
            // 
            this.ColumnDecreasedOrIncreased.Activities.Add(this.IfColumnDecreased);
            this.ColumnDecreasedOrIncreased.Activities.Add(this.IfColumnIncreased);
            this.ColumnDecreasedOrIncreased.Name = "ColumnDecreasedOrIncreased";
            // 
            // GetColumnsInPage
            // 
            activitybind6.Name = "ModifyPageLayoutWorkflow";
            activitybind6.Path = "ExistingColumns";
            this.GetColumnsInPage.Name = "GetColumnsInPage";
            activitybind7.Name = "ModifyPageLayoutWorkflow";
            activitybind7.Path = "CurrentPage.ID";
            this.GetColumnsInPage.SetBinding(Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.GetColumnsInPage.SetBinding(Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity.ColumnsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // DecideColumnNoFromLayoutTypeActivity
            // 
            activitybind8.Name = "ModifyPageLayoutWorkflow";
            activitybind8.Path = "NewColumnDefs";
            activitybind9.Name = "ModifyPageLayoutWorkflow";
            activitybind9.Path = "Request.LayoutType";
            this.DecideColumnNoFromLayoutTypeActivity.Name = "DecideColumnNoFromLayoutTypeActivity";
            this.DecideColumnNoFromLayoutTypeActivity.SetBinding(Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity.ColumnsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.DecideColumnNoFromLayoutTypeActivity.SetBinding(Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity.LayoutTypeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // GetUserSetting
            // 
            activitybind10.Name = "ModifyPageLayoutWorkflow";
            activitybind10.Path = "CurrentPage";
            this.GetUserSetting.Name = "GetUserSetting";
            activitybind11.Name = "GetUserGuid";
            activitybind11.Path = "UserGuid";
            this.GetUserSetting.UserSetting = null;
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("CurrentPage", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind12.Name = "ModifyPageLayoutWorkflow";
            activitybind12.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // ModifyPageLayoutWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.GetUserSetting);
            this.Activities.Add(this.DecideColumnNoFromLayoutTypeActivity);
            this.Activities.Add(this.GetColumnsInPage);
            this.Activities.Add(this.ColumnDecreasedOrIncreased);
            this.Activities.Add(this.ChangePageLayout);
            this.Name = "ModifyPageLayoutWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother moveWidgetFromOnColumnToAnother1;
        private IfElseBranchActivity IfColumnIncreased;
        private IfElseBranchActivity IfColumnDecreased;
        private IfElseActivity ColumnDecreasedOrIncreased;
        private WhileActivity WhileMoreColumnsToRemove;
        private CodeActivity SetCounters;
        private Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity GetColumnsInPage;
        private Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity DecideColumnNoFromLayoutTypeActivity;
        private Dropthings.Business.Activities.GetUserSettingActivity GetUserSetting;
        private Dropthings.Business.Activities.ChangePageLayoutActivity ChangePageLayout;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGuid;
























































    }
}
