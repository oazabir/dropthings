namespace Dropthings.Business.Workflows.UserAccountWorkflow
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

    using Dropthings.Business.Workflows.UserAccountWorkflows;

    public sealed partial class UpdateAccountWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(UpdateAccountWorkflowRequest), typeof(UpdateAccountWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(UpdateAccountWorkflowResponse), typeof(UpdateAccountWorkflow));

        #endregion Fields

        #region Constructors

        public UpdateAccountWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public UpdateAccountWorkflowRequest Request
        {
            get { return (UpdateAccountWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public UpdateAccountWorkflowResponse Response
        {
            get { return (UpdateAccountWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}