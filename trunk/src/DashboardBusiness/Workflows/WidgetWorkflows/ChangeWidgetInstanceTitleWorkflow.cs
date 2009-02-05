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

    public sealed partial class ChangeWidgetInstanceTitleWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(ChangeWidgetInstanceTitleWorkflowRequest), typeof(ChangeWidgetInstanceTitleWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(ChangeWidgetInstanceTitleWorkflowResponse), typeof(ChangeWidgetInstanceTitleWorkflow));

        public static DependencyProperty NewTitleProperty = DependencyProperty.Register("NewTitle", typeof(System.String), typeof(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow));
        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow));
        public static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(System.Int32), typeof(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow));

        #endregion Fields

        #region Constructors

        public ChangeWidgetInstanceTitleWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public ChangeWidgetInstanceTitleWorkflowRequest Request
        {
            get { return (ChangeWidgetInstanceTitleWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public ChangeWidgetInstanceTitleWorkflowResponse Response
        {
            get { return (ChangeWidgetInstanceTitleWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}