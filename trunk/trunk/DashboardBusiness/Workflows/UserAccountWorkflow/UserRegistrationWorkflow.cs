namespace Dropthings.Business.Workflows.UserAccountWorkflow
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Configuration;
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

    public sealed partial class UserRegistrationWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(UserRegistrationWorkflowRequest), typeof(UserRegistrationWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(UserRegistrationWorkflowResponse), typeof(UserRegistrationWorkflow));

        #endregion Fields

        #region Constructors

        public UserRegistrationWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public UserRegistrationWorkflowRequest Request
        {
            get { return (UserRegistrationWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public UserRegistrationWorkflowResponse Response
        {
            get { return (UserRegistrationWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties

        #region Methods

        private void SetUsernameForSetupUserFromTemplateWotkflow_ExecuteCode(object sender, EventArgs e)
        {
            this.Request.UserName = this.Request.RequestedUsername;
        }

        #endregion Methods
    }
}