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

    public sealed partial class CreateTemplateUserWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(CreateTemplateUserWorkflowRequest), typeof(CreateTemplateUserWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(CreateTemplateUserWorkflowResponse), typeof(CreateTemplateUserWorkflow));

        #endregion Fields

        #region Constructors

        public CreateTemplateUserWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public CreateTemplateUserWorkflowRequest Request
        {
            get { return (CreateTemplateUserWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public CreateTemplateUserWorkflowResponse Response
        {
            get { return (CreateTemplateUserWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties

        #region Methods

        private void SetException_ExecuteCode(object sender, EventArgs e)
        {
            throw new ApplicationException("First page creation failed");
        }

        #endregion Methods
    }
}