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

    public sealed partial class GetWidgetWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(GetWidgetWorkflowRequest), typeof(GetWidgetWorkflow));
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(GetWidgetWorkflowResponse), typeof(GetWidgetWorkflow));

        #endregion Fields

        #region Constructors

        public GetWidgetWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public GetWidgetWorkflowRequest Request
        {
            get { return (GetWidgetWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public GetWidgetWorkflowResponse Response
        {
            get { return (GetWidgetWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}