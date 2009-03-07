namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Linq;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs;

    public sealed partial class AssignWidgetPermissionWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(AssignWidgetPermissionRequest), typeof(AssignWidgetPermissionWorkflow));
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(AssignWidgetPermissionResponse), typeof(AssignWidgetPermissionWorkflow));

        #endregion Fields

        #region Constructors

        public AssignWidgetPermissionWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public AssignWidgetPermissionRequest Request
        {
            get { return (AssignWidgetPermissionRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public AssignWidgetPermissionResponse Response
        {
            get { return (AssignWidgetPermissionResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}