namespace Dropthings.Business.Workflows.SystemWorkflows
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

    public sealed partial class SetupDefaultRolesWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(SetupDefaultRolesWorkflowRequest), typeof(SetupDefaultRolesWorkflow));
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(SetupDefaultRolesWorkflowResponse), typeof(SetupDefaultRolesWorkflow));

        #endregion Fields

        #region Constructors

        public SetupDefaultRolesWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public SetupDefaultRolesWorkflowRequest Request
        {
            get { return (SetupDefaultRolesWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public SetupDefaultRolesWorkflowResponse Response
        {
            get { return (SetupDefaultRolesWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}