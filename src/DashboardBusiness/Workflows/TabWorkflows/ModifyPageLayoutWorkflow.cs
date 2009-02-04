namespace Dropthings.Business
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

    using Dropthings.Business.Workflows.TabWorkflows;

    public sealed partial class ModifyPageLayoutWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(ModifyTabLayoutWorkflowRequest), typeof(ModifyPageLayoutWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(ModifyTabLayoutWorkflowResponse), typeof(ModifyPageLayoutWorkflow));

        #endregion Fields

        #region Constructors

        public ModifyPageLayoutWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public ModifyTabLayoutWorkflowRequest Request
        {
            get { return (ModifyTabLayoutWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public ModifyTabLayoutWorkflowResponse Response
        {
            get { return (ModifyTabLayoutWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}