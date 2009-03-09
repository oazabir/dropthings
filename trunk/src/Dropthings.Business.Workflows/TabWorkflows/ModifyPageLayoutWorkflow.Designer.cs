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

namespace Dropthings.Business.Workflows.TabWorkflows
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
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference3 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind19 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind20 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind21 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind22 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind23 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind24 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind25 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind26 = new System.Workflow.ComponentModel.ActivityBind();
            this.ChangeWidgetInstancePosition = new Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity();
            this.IncreaseColumnCounter = new System.Workflow.Activities.CodeActivity();
            this.AddNewColumn = new Dropthings.Business.Activities.PageActivities.AddColumnActivity();
            this.SetWidthForColumn = new System.Workflow.Activities.CodeActivity();
            this.DecreaseColumnCounter = new System.Workflow.Activities.CodeActivity();
            this.DeleteTheColumn = new Dropthings.Business.Activities.DeleteColumnActivity();
            this.ForEachWidgetInstance = new ForEachActivity.ForEach();
            this.GetWidgetsOnOldColumn = new Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity();
            this.GetNewWidgetZone = new Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn();
            this.GetOldWidgetZone = new Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn();
            this.AddNewColumns = new System.Workflow.Activities.SequenceActivity();
            this.MoveWidgetAndRemoveColumn = new System.Workflow.Activities.SequenceActivity();
            this.WhileMoreColumnsToAdd = new System.Workflow.Activities.WhileActivity();
            this.WhileMoreColumnsToRemove = new System.Workflow.Activities.WhileActivity();
            this.IfColumnIncreased = new System.Workflow.Activities.IfElseBranchActivity();
            this.IfColumnDecreased = new System.Workflow.Activities.IfElseBranchActivity();
            this.ChangePageLayout = new Dropthings.Business.Activities.ChangePageLayoutActivity();
            this.SetNewColumnWidths = new Dropthings.Business.Activities.PageActivities.ApplyColumnWidthsActivity();
            this.ColumnDecreasedOrIncreased = new System.Workflow.Activities.IfElseActivity();
            this.SetCounters = new System.Workflow.Activities.CodeActivity();
            this.GetColumnsInPage = new Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity();
            this.DecideColumnNoFromLayoutTypeActivity = new Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity();
            this.GetUserSetting = new Dropthings.Business.Activities.GetUserSettingActivity();
            this.GetUserGuid = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // ChangeWidgetInstancePosition
            // 
            this.ChangeWidgetInstancePosition.Name = "ChangeWidgetInstancePosition";
            this.ChangeWidgetInstancePosition.RowNo = 0;
            this.ChangeWidgetInstancePosition.WidgetInstanceId = 0;
            activitybind1.Name = "ModifyPageLayoutWorkflow";
            activitybind1.Path = "NewWidgetZone.ID";
            this.ChangeWidgetInstancePosition.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // IncreaseColumnCounter
            // 
            this.IncreaseColumnCounter.Name = "IncreaseColumnCounter";
            this.IncreaseColumnCounter.ExecuteCode += new System.EventHandler(this.IncreaseColumnCounter_ExecuteCode);
            // 
            // AddNewColumn
            // 
            activitybind2.Name = "ModifyPageLayoutWorkflow";
            activitybind2.Path = "NewColumnNo";
            this.AddNewColumn.Name = "AddNewColumn";
            activitybind3.Name = "ModifyPageLayoutWorkflow";
            activitybind3.Path = "CurrentPage.ID";
            activitybind4.Name = "ModifyPageLayoutWorkflow";
            activitybind4.Path = "NewColumnWidth";
            this.AddNewColumn.SetBinding(Dropthings.Business.Activities.PageActivities.AddColumnActivity.ColumnNoProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.AddNewColumn.SetBinding(Dropthings.Business.Activities.PageActivities.AddColumnActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.AddNewColumn.SetBinding(Dropthings.Business.Activities.PageActivities.AddColumnActivity.WidthProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // SetWidthForColumn
            // 
            this.SetWidthForColumn.Name = "SetWidthForColumn";
            this.SetWidthForColumn.ExecuteCode += new System.EventHandler(this.SetWidthForColumn_ExecuteCode);
            // 
            // DecreaseColumnCounter
            // 
            this.DecreaseColumnCounter.Name = "DecreaseColumnCounter";
            this.DecreaseColumnCounter.ExecuteCode += new System.EventHandler(this.DecreaseColumnCounter_ExecuteCode);
            // 
            // DeleteTheColumn
            // 
            this.DeleteTheColumn.ColumnId = 0;
            activitybind5.Name = "ModifyPageLayoutWorkflow";
            activitybind5.Path = "ColumnCounter";
            this.DeleteTheColumn.ColumnToDelete = null;
            this.DeleteTheColumn.Name = "DeleteTheColumn";
            activitybind6.Name = "ModifyPageLayoutWorkflow";
            activitybind6.Path = "CurrentPage.ID";
            this.DeleteTheColumn.SetBinding(Dropthings.Business.Activities.DeleteColumnActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.DeleteTheColumn.SetBinding(Dropthings.Business.Activities.DeleteColumnActivity.ColumnNoProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            activitybind7.Name = "ModifyPageLayoutWorkflow";
            activitybind7.Path = "WidgetInstancesToMove";
            // 
            // ForEachWidgetInstance
            // 
            this.ForEachWidgetInstance.Activities.Add(this.ChangeWidgetInstancePosition);
            this.ForEachWidgetInstance.Description = "A generic flow control activity that executes once for each item in a collection." +
                "";
            this.ForEachWidgetInstance.Name = "ForEachWidgetInstance";
            this.ForEachWidgetInstance.Iterating += new System.EventHandler(this.ForEachWidgetInstance_Iterating);
            this.ForEachWidgetInstance.SetBinding(ForEachActivity.ForEach.ItemsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // GetWidgetsOnOldColumn
            // 
            this.GetWidgetsOnOldColumn.Name = "GetWidgetsOnOldColumn";
            activitybind8.Name = "ModifyPageLayoutWorkflow";
            activitybind8.Path = "WidgetInstancesToMove";
            activitybind9.Name = "ModifyPageLayoutWorkflow";
            activitybind9.Path = "OldWidgetZone.ID";
            this.GetWidgetsOnOldColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity.WidgetZoneIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.GetWidgetsOnOldColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity.WidgetInstancesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // GetNewWidgetZone
            // 
            activitybind10.Name = "ModifyPageLayoutWorkflow";
            activitybind10.Path = "NewColumnNo";
            this.GetNewWidgetZone.Name = "GetNewWidgetZone";
            activitybind11.Name = "ModifyPageLayoutWorkflow";
            activitybind11.Path = "CurrentPage.ID";
            activitybind12.Name = "ModifyPageLayoutWorkflow";
            activitybind12.Path = "NewWidgetZone";
            this.GetNewWidgetZone.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.ColumnNoProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.GetNewWidgetZone.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.GetNewWidgetZone.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.WidgetZoneProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // GetOldWidgetZone
            // 
            activitybind13.Name = "ModifyPageLayoutWorkflow";
            activitybind13.Path = "ColumnCounter";
            this.GetOldWidgetZone.Name = "GetOldWidgetZone";
            activitybind14.Name = "ModifyPageLayoutWorkflow";
            activitybind14.Path = "CurrentPage.ID";
            activitybind15.Name = "ModifyPageLayoutWorkflow";
            activitybind15.Path = "OldWidgetZone";
            this.GetOldWidgetZone.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.GetOldWidgetZone.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.ColumnNoProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.GetOldWidgetZone.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.WidgetZoneProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            // 
            // AddNewColumns
            // 
            this.AddNewColumns.Activities.Add(this.SetWidthForColumn);
            this.AddNewColumns.Activities.Add(this.AddNewColumn);
            this.AddNewColumns.Activities.Add(this.IncreaseColumnCounter);
            this.AddNewColumns.Name = "AddNewColumns";
            // 
            // MoveWidgetAndRemoveColumn
            // 
            this.MoveWidgetAndRemoveColumn.Activities.Add(this.GetOldWidgetZone);
            this.MoveWidgetAndRemoveColumn.Activities.Add(this.GetNewWidgetZone);
            this.MoveWidgetAndRemoveColumn.Activities.Add(this.GetWidgetsOnOldColumn);
            this.MoveWidgetAndRemoveColumn.Activities.Add(this.ForEachWidgetInstance);
            this.MoveWidgetAndRemoveColumn.Activities.Add(this.DeleteTheColumn);
            this.MoveWidgetAndRemoveColumn.Activities.Add(this.DecreaseColumnCounter);
            this.MoveWidgetAndRemoveColumn.Name = "MoveWidgetAndRemoveColumn";
            // 
            // WhileMoreColumnsToAdd
            // 
            this.WhileMoreColumnsToAdd.Activities.Add(this.AddNewColumns);
            ruleconditionreference1.ConditionName = "ColumnCounterIsBelowNewColumnCount";
            this.WhileMoreColumnsToAdd.Condition = ruleconditionreference1;
            this.WhileMoreColumnsToAdd.Name = "WhileMoreColumnsToAdd";
            // 
            // WhileMoreColumnsToRemove
            // 
            this.WhileMoreColumnsToRemove.Activities.Add(this.MoveWidgetAndRemoveColumn);
            ruleconditionreference2.ConditionName = "ColumnCounterIsOverNewColumnCount";
            this.WhileMoreColumnsToRemove.Condition = ruleconditionreference2;
            this.WhileMoreColumnsToRemove.Name = "WhileMoreColumnsToRemove";
            // 
            // IfColumnIncreased
            // 
            this.IfColumnIncreased.Activities.Add(this.WhileMoreColumnsToAdd);
            this.IfColumnIncreased.Name = "IfColumnIncreased";
            // 
            // IfColumnDecreased
            // 
            this.IfColumnDecreased.Activities.Add(this.WhileMoreColumnsToRemove);
            ruleconditionreference3.ConditionName = "NewColumnLessThanExistingColumn";
            this.IfColumnDecreased.Condition = ruleconditionreference3;
            this.IfColumnDecreased.Name = "IfColumnDecreased";
            // 
            // ChangePageLayout
            // 
            activitybind16.Name = "ModifyPageLayoutWorkflow";
            activitybind16.Path = "Request.LayoutType";
            this.ChangePageLayout.Name = "ChangePageLayout";
            activitybind17.Name = "ModifyPageLayoutWorkflow";
            activitybind17.Path = "CurrentPage.ID";
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("LayoutType", typeof(Dropthings.Business.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            this.ChangePageLayout.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.ChangePageLayoutActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            // 
            // SetNewColumnWidths
            // 
            this.SetNewColumnWidths.Columns = null;
            activitybind18.Name = "ModifyPageLayoutWorkflow";
            activitybind18.Path = "NewColumnDefs";
            this.SetNewColumnWidths.Name = "SetNewColumnWidths";
            activitybind19.Name = "ModifyPageLayoutWorkflow";
            activitybind19.Path = "CurrentPage.ID";
            this.SetNewColumnWidths.SetBinding(Dropthings.Business.Activities.PageActivities.ApplyColumnWidthsActivity.ColumnWidthsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
            this.SetNewColumnWidths.SetBinding(Dropthings.Business.Activities.PageActivities.ApplyColumnWidthsActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
            // 
            // ColumnDecreasedOrIncreased
            // 
            this.ColumnDecreasedOrIncreased.Activities.Add(this.IfColumnDecreased);
            this.ColumnDecreasedOrIncreased.Activities.Add(this.IfColumnIncreased);
            this.ColumnDecreasedOrIncreased.Name = "ColumnDecreasedOrIncreased";
            // 
            // SetCounters
            // 
            this.SetCounters.Name = "SetCounters";
            this.SetCounters.ExecuteCode += new System.EventHandler(this.SetCounters_ExecuteCode);
            // 
            // GetColumnsInPage
            // 
            activitybind20.Name = "ModifyPageLayoutWorkflow";
            activitybind20.Path = "ExistingColumns";
            this.GetColumnsInPage.Name = "GetColumnsInPage";
            activitybind21.Name = "ModifyPageLayoutWorkflow";
            activitybind21.Path = "CurrentPage.ID";
            this.GetColumnsInPage.SetBinding(Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind21)));
            this.GetColumnsInPage.SetBinding(Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity.ColumnsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind20)));
            // 
            // DecideColumnNoFromLayoutTypeActivity
            // 
            activitybind22.Name = "ModifyPageLayoutWorkflow";
            activitybind22.Path = "NewColumnDefs";
            activitybind23.Name = "ModifyPageLayoutWorkflow";
            activitybind23.Path = "Request.LayoutType";
            this.DecideColumnNoFromLayoutTypeActivity.Name = "DecideColumnNoFromLayoutTypeActivity";
            this.DecideColumnNoFromLayoutTypeActivity.SetBinding(Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity.ColumnsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind22)));
            this.DecideColumnNoFromLayoutTypeActivity.SetBinding(Dropthings.Business.Activities.PageActivities.DecideColumnsFromLayoutTypeActivity.LayoutTypeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind23)));
            // 
            // GetUserSetting
            // 
            activitybind24.Name = "ModifyPageLayoutWorkflow";
            activitybind24.Path = "CurrentPage";
            this.GetUserSetting.Name = "GetUserSetting";
            activitybind25.Name = "GetUserGuid";
            activitybind25.Path = "UserGuid";
            this.GetUserSetting.UserSetting = null;
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind25)));
            this.GetUserSetting.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("CurrentPage", typeof(Dropthings.Business.Activities.GetUserSettingActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind24)));
            // 
            // GetUserGuid
            // 
            this.GetUserGuid.Name = "GetUserGuid";
            this.GetUserGuid.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind26.Name = "ModifyPageLayoutWorkflow";
            activitybind26.Path = "Request.UserName";
            this.GetUserGuid.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind26)));
            // 
            // ModifyPageLayoutWorkflow
            // 
            this.Activities.Add(this.GetUserGuid);
            this.Activities.Add(this.GetUserSetting);
            this.Activities.Add(this.DecideColumnNoFromLayoutTypeActivity);
            this.Activities.Add(this.GetColumnsInPage);
            this.Activities.Add(this.SetCounters);
            this.Activities.Add(this.ColumnDecreasedOrIncreased);
            this.Activities.Add(this.SetNewColumnWidths);
            this.Activities.Add(this.ChangePageLayout);
            this.Name = "ModifyPageLayoutWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity SetWidthForColumn;
        private Dropthings.Business.Activities.PageActivities.ApplyColumnWidthsActivity SetNewColumnWidths;
        private Dropthings.Business.Activities.DeleteColumnActivity DeleteTheColumn;
        private Dropthings.Business.Activities.PageActivities.AddColumnActivity AddNewColumn;
        private SequenceActivity AddNewColumns;
        private WhileActivity WhileMoreColumnsToAdd;
        private CodeActivity IncreaseColumnCounter;
        private ForEachActivity.ForEach ForEachWidgetInstance;
        private Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity ChangeWidgetInstancePosition;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn GetNewWidgetZone;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity GetWidgetsOnOldColumn;
        private SequenceActivity MoveWidgetAndRemoveColumn;
        private CodeActivity DecreaseColumnCounter;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn GetOldWidgetZone;
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
