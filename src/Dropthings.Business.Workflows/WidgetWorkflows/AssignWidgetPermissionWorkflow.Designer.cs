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
    partial class AssignWidgetPermissionWorkflow
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
            this.AssignWidgetPermission = new Dropthings.Business.Activities.AssignWidgetPermissionActivity();
            // 
            // AssignWidgetPermission
            // 
            this.AssignWidgetPermission.Name = "AssignWidgetPermission";
            activitybind1.Name = "AssignWidgetPermissionWorkflow";
            activitybind1.Path = "Request.WidgetPermissions";
            this.AssignWidgetPermission.SetBinding(System.Workflow.ComponentModel.DependencyProperty.FromName("WidgetPermissions", typeof(Dropthings.Business.Activities.AssignWidgetPermissionActivity)), ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // AssignWidgetPermissionWorkflow
            // 
            this.Activities.Add(this.AssignWidgetPermission);
            this.Name = "AssignWidgetPermissionWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Dropthings.Business.Activities.AssignWidgetPermissionActivity AssignWidgetPermission;





    }
}
