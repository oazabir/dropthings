namespace Dropthings.Business.Workflows.TabWorkflows
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

    public sealed partial class GetColumnsInPageWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(GetColumnsInPageWorkflowRequest), typeof(GetColumnsInPageWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(GetColumnsInPageWorkflowResponse), typeof(GetColumnsInPageWorkflow));

        #endregion Fields

        #region Constructors

        public GetColumnsInPageWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public GetColumnsInPageWorkflowRequest Request
        {
            get { return (GetColumnsInPageWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public GetColumnsInPageWorkflowResponse Response
        {
            get { return (GetColumnsInPageWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}