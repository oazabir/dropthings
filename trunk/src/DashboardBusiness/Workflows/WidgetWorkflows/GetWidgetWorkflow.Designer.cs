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

namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    partial class GetWidgetWorkflow
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
            this.GetWidgetById = new Dropthings.Business.Activities.WidgetActivities.GetWidgetActivity();
            // 
            // GetWidgetById
            // 
            this.GetWidgetById.Name = "GetWidgetById";
            activitybind1.Name = "GetWidgetWorkflow";
            activitybind1.Path = "Response.Widget";
            activitybind2.Name = "GetWidgetWorkflow";
            activitybind2.Path = "Request.WidgetId";
            this.GetWidgetById.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetActivity.WidgetIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.GetWidgetById.SetBinding(Dropthings.Business.Activities.WidgetActivities.GetWidgetActivity.WidgetProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // GetWidgetWorkflow
            // 
            this.Activities.Add(this.GetWidgetById);
            this.Name = "GetWidgetWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.WidgetActivities.GetWidgetActivity GetWidgetById;





    }
}
