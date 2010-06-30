using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Dropthings.Business.Activities.PageActivities
{
    public partial class MoveWidgetFromOnColumnToAnother
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
            this.MoveWidgetInstance = new Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity();
            this.ForEachWidgetInstanceToMove = new ForEachActivity.ForEach();
            this.GetWidgetInstancesOnOldZone = new Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity();
            this.GetWidgetZoneOnNewColumn = new Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn();
            this.GetWidgetZoneOnOldColumn = new Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn();
            // 
            // MoveWidgetInstance
            // 
            this.MoveWidgetInstance.Name = "MoveWidgetInstance";
            this.MoveWidgetInstance.RowNo = 0;
            this.MoveWidgetInstance.WidgetInstanceId = 0;
            activitybind1.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind1.Path = "NewWidgetZone.ID";
            this.MoveWidgetInstance.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetZoneId", typeof(Dropthings.Business.Activities.ChangeWidgetInstancePositionActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            activitybind2.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind2.Path = "OldWidgetInstances";
            // 
            // ForEachWidgetInstanceToMove
            // 
            this.ForEachWidgetInstanceToMove.Activities.Add(this.MoveWidgetInstance);
            this.ForEachWidgetInstanceToMove.Description = "A generic flow control activity that executes once for each item in a collection." +
                "";
            this.ForEachWidgetInstanceToMove.Name = "ForEachWidgetInstanceToMove";
            this.ForEachWidgetInstanceToMove.Iterating += new System.EventHandler(this.ForEachWidgetInstanceToMove_Iterating);
            this.ForEachWidgetInstanceToMove.SetBinding(ForEachActivity.ForEach.ItemsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetWidgetInstancesOnOldZone
            // 
            this.GetWidgetInstancesOnOldZone.Name = "GetWidgetInstancesOnOldZone";
            activitybind3.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind3.Path = "OldWidgetInstances";
            activitybind4.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind4.Path = "OldWidgetZone.ID";
            this.GetWidgetInstancesOnOldZone.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity.WidgetZoneIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.GetWidgetInstancesOnOldZone.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity.WidgetInstancesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // GetWidgetZoneOnNewColumn
            // 
            activitybind5.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind5.Path = "NewColumnNo";
            this.GetWidgetZoneOnNewColumn.Name = "GetWidgetZoneOnNewColumn";
            activitybind6.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind6.Path = "PageId";
            activitybind7.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind7.Path = "NewWidgetZone";
            this.GetWidgetZoneOnNewColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.ColumnNoProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.GetWidgetZoneOnNewColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.GetWidgetZoneOnNewColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.WidgetZoneProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // GetWidgetZoneOnOldColumn
            // 
            activitybind8.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind8.Path = "OldColumno";
            this.GetWidgetZoneOnOldColumn.Name = "GetWidgetZoneOnOldColumn";
            activitybind9.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind9.Path = "PageId";
            activitybind10.Name = "MoveWidgetFromOnColumnToAnother";
            activitybind10.Path = "OldWidgetZone";
            this.GetWidgetZoneOnOldColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.ColumnNoProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.GetWidgetZoneOnOldColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.GetWidgetZoneOnOldColumn.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn.WidgetZoneProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // MoveWidgetFromOnColumnToAnother
            // 
            this.Activities.Add(this.GetWidgetZoneOnOldColumn);
            this.Activities.Add(this.GetWidgetZoneOnNewColumn);
            this.Activities.Add(this.GetWidgetInstancesOnOldZone);
            this.Activities.Add(this.ForEachWidgetInstanceToMove);
            this.Name = "MoveWidgetFromOnColumnToAnother";
            this.CanModifyActivities = false;

        }

        #endregion

        private ChangeWidgetInstancePositionActivity MoveWidgetInstance;
        private ForEachActivity.ForEach ForEachWidgetInstanceToMove;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn GetWidgetZoneOnNewColumn;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity GetWidgetInstancesOnOldZone;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetZoneOnPageColumn GetWidgetZoneOnOldColumn;




















    }
}
