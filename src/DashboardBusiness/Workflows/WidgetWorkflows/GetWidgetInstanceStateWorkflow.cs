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

    public sealed partial class GetWidgetInstanceStateWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(GetWidgetInstanceStateRequest), typeof(GetWidgetInstanceStateWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(GetWidgetInstanceStateResponse), typeof(GetWidgetInstanceStateWorkflow));

        #endregion Fields

        #region Constructors

        public GetWidgetInstanceStateWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public GetWidgetInstanceStateRequest Request
        {
            get { return (GetWidgetInstanceStateRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public GetWidgetInstanceStateResponse Response
        {
            get { return (GetWidgetInstanceStateResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}