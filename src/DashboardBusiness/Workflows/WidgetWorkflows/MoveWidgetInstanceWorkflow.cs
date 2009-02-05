#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs;

    public sealed partial class MoveWidgetInstanceWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(MoveWidgetInstanceWorkflowRequest), typeof(MoveWidgetInstanceWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(MoveWidgetInstanceWorkflowResponse), typeof(MoveWidgetInstanceWorkflow));

        public Dropthings.DataAccess.WidgetInstance OldWidgetInstance = new Dropthings.DataAccess.WidgetInstance();

        #endregion Fields

        #region Constructors

        public MoveWidgetInstanceWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public MoveWidgetInstanceWorkflowRequest Request
        {
            get { return (MoveWidgetInstanceWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public MoveWidgetInstanceWorkflowResponse Response
        {
            get { return (MoveWidgetInstanceWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}