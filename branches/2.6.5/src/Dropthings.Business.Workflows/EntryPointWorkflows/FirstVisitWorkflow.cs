#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Workflows.EntryPointWorkflows
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Configuration;
    using System.Drawing;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.Business.Activities;
    using Dropthings.Business.Workflows;
    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.Business.Workflows.UserAccountWorkflow;

    public sealed partial class FirstVisitWorkflow : SequentialWorkflowActivity, IFirstVisitWorkflow
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(IUserVisitWorkflowRequest), typeof(FirstVisitWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(IUserVisitWorkflowResponse), typeof(FirstVisitWorkflow));

        #endregion Fields

        #region Constructors

        public FirstVisitWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public IUserVisitWorkflowRequest Request
        {
            get { return (IUserVisitWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public IUserVisitWorkflowResponse Response
        {
            get { return (IUserVisitWorkflowResponse)GetValue(ResponseProperty); }
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