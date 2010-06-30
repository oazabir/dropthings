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
	partial class DeletePageWorkflow
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
            this.DeleteWidgetInstance = new Dropthings.Business.Activities.DeleteWidgetInstanceActivity();
            this.DeleteWidgetZone = new Dropthings.Business.Activities.DeleteWidgetZoneActivity();
            this.DeleteColumn = new Dropthings.Business.Activities.DeleteColumnActivity();
            this.DeleteEachWidgetInstance = new ForEachActivity.ForEach();
            this.GetWidgetInstancesInZone = new Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity();
            this.GetWidgetZone = new Dropthings.Business.Activities.GetWidgetZoneActivity();
            this.DeleteSequence = new System.Workflow.Activities.SequenceActivity();
            this.SetCurrentPage = new Dropthings.Business.Activities.SetCurrentPageActivity();
            this.DecideCurrentPage = new Dropthings.Business.Activities.DecideCurrentPageActivity();
            this.DeletePage = new Dropthings.Business.Activities.DeletePageActivity();
            this.DeleteEachColumn = new ForEachActivity.ForEach();
            this.GetColumnsOfPage = new Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity();
            this.GetUserGUID = new Dropthings.Business.Activities.GetUserGuidActivity();
            // 
            // DeleteWidgetInstance
            // 
            this.DeleteWidgetInstance.Name = "DeleteWidgetInstance";
            this.DeleteWidgetInstance.WidgetInstanceId = 0;
            // 
            // DeleteWidgetZone
            // 
            this.DeleteWidgetZone.Name = "DeleteWidgetZone";
            activitybind1.Name = "GetWidgetZone";
            activitybind1.Path = "WidgetZone.ID";
            this.DeleteWidgetZone.SetBinding(Dropthings.Business.Activities.DeleteWidgetZoneActivity.WidgetZoneIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // DeleteColumn
            // 
            this.DeleteColumn.ColumnId = 0;
            this.DeleteColumn.Name = "DeleteColumn";
            activitybind2.Name = "GetWidgetInstancesInZone";
            activitybind2.Path = "WidgetInstances";
            // 
            // DeleteEachWidgetInstance
            // 
            this.DeleteEachWidgetInstance.Activities.Add(this.DeleteWidgetInstance);
            this.DeleteEachWidgetInstance.Description = "A generic flow control activity that executes once for each item in a collection." +
                "";
            this.DeleteEachWidgetInstance.Name = "DeleteEachWidgetInstance";
            this.DeleteEachWidgetInstance.Iterating += new System.EventHandler(this.OnForEachWidgetInstance);
            this.DeleteEachWidgetInstance.SetBinding(ForEachActivity.ForEach.ItemsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // GetWidgetInstancesInZone
            // 
            this.GetWidgetInstancesInZone.Name = "GetWidgetInstancesInZone";
            this.GetWidgetInstancesInZone.WidgetInstances = null;
            activitybind3.Name = "GetWidgetZone";
            activitybind3.Path = "WidgetZone.ID";
            this.GetWidgetInstancesInZone.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity.WidgetZoneIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // GetWidgetZone
            // 
            this.GetWidgetZone.Name = "GetWidgetZone";
            this.GetWidgetZone.WidgetZone = null;
            this.GetWidgetZone.ZoneId = 0;
            // 
            // DeleteSequence
            // 
            this.DeleteSequence.Activities.Add(this.GetWidgetZone);
            this.DeleteSequence.Activities.Add(this.GetWidgetInstancesInZone);
            this.DeleteSequence.Activities.Add(this.DeleteEachWidgetInstance);
            this.DeleteSequence.Activities.Add(this.DeleteColumn);
            this.DeleteSequence.Activities.Add(this.DeleteWidgetZone);
            this.DeleteSequence.Name = "DeleteSequence";
            // 
            // SetCurrentPage
            // 
            this.SetCurrentPage.Description = "Set the current page after deleting the current page";
            this.SetCurrentPage.Name = "SetCurrentPage";
            activitybind4.Name = "DeletePageWorkflow";
            activitybind4.Path = "CurrentPageId";
            activitybind5.Name = "GetUserGUID";
            activitybind5.Path = "UserGuid";
            this.SetCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.SetCurrentPage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.SetCurrentPageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // DecideCurrentPage
            // 
            activitybind6.Name = "DeletePageWorkflow";
            activitybind6.Path = "Response.NewCurrentPage";
            activitybind7.Name = "DeletePageWorkflow";
            activitybind7.Path = "CurrentPageId";
            this.DecideCurrentPage.Name = "DecideCurrentPage";
            this.DecideCurrentPage.PageName = null;
            activitybind8.Name = "GetUserGUID";
            activitybind8.Path = "UserGuid";
            this.DecideCurrentPage.SetBinding(Dropthings.Business.Activities.DecideCurrentPageActivity.CurrentPageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.DecideCurrentPage.SetBinding(Dropthings.Business.Activities.DecideCurrentPageActivity.CurrentPageProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.DecideCurrentPage.SetBinding(Dropthings.Business.Activities.DecideCurrentPageActivity.UserGuidProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // DeletePage
            // 
            this.DeletePage.Name = "DeletePage";
            activitybind9.Name = "DeletePageWorkflow";
            activitybind9.Path = "Request.PageID";
            activitybind10.Name = "GetUserGUID";
            activitybind10.Path = "UserGuid";
            this.DeletePage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("PageId", typeof(Dropthings.Business.Activities.DeletePageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.DeletePage.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserGuid", typeof(Dropthings.Business.Activities.DeletePageActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            activitybind11.Name = "DeletePageWorkflow";
            activitybind11.Path = "Columns";
            // 
            // DeleteEachColumn
            // 
            this.DeleteEachColumn.Activities.Add(this.DeleteSequence);
            this.DeleteEachColumn.Description = "A generic flow control activity that executes once for each item in a collection." +
                "";
            this.DeleteEachColumn.Name = "DeleteEachColumn";
            this.DeleteEachColumn.Iterating += new System.EventHandler(this.OnForEachColumn);
            this.DeleteEachColumn.SetBinding(ForEachActivity.ForEach.ItemsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // GetColumnsOfPage
            // 
            activitybind12.Name = "DeletePageWorkflow";
            activitybind12.Path = "Columns";
            this.GetColumnsOfPage.Name = "GetColumnsOfPage";
            activitybind13.Name = "DeletePageWorkflow";
            activitybind13.Path = "Request.PageID";
            this.GetColumnsOfPage.SetBinding(Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity.PageIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.GetColumnsOfPage.SetBinding(Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity.ColumnsProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // GetUserGUID
            // 
            this.GetUserGUID.Name = "GetUserGUID";
            this.GetUserGUID.UserGuid = new System.Guid("00000000-0000-0000-0000-000000000000");
            activitybind14.Name = "DeletePageWorkflow";
            activitybind14.Path = "Request.UserName";
            this.GetUserGUID.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("UserName", typeof(Dropthings.Business.Activities.GetUserGuidActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            // 
            // DeletePageWorkflow
            // 
            this.Activities.Add(this.GetUserGUID);
            this.Activities.Add(this.GetColumnsOfPage);
            this.Activities.Add(this.DeleteEachColumn);
            this.Activities.Add(this.DeletePage);
            this.Activities.Add(this.DecideCurrentPage);
            this.Activities.Add(this.SetCurrentPage);
            this.Name = "DeletePageWorkflow";
            this.CanModifyActivities = false;

		}

		#endregion

        private Dropthings.Business.Activities.DeleteColumnActivity DeleteColumn;
        private Dropthings.Business.Activities.DeleteWidgetZoneActivity DeleteWidgetZone;
        private Dropthings.Business.Activities.WidgetActivities.GetWidgetInstancesInZoneActivity GetWidgetInstancesInZone;
        private SequenceActivity DeleteSequence;
        private Dropthings.Business.Activities.PageActivities.GetColumnsOfPageActivity GetColumnsOfPage;
        private Dropthings.Business.Activities.GetWidgetZoneActivity GetWidgetZone;
        private ForEachActivity.ForEach DeleteEachColumn;
        private Dropthings.Business.Activities.DeleteWidgetInstanceActivity DeleteWidgetInstance;
        private ForEachActivity.ForEach DeleteEachWidgetInstance;
        private Dropthings.Business.Activities.SetCurrentPageActivity SetCurrentPage;
        private Dropthings.Business.Activities.DecideCurrentPageActivity DecideCurrentPage;
        private Dropthings.Business.Activities.DeletePageActivity DeletePage;
        private Dropthings.Business.Activities.GetUserGuidActivity GetUserGUID;



















































































    }
}
