#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business
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

    using Dropthings.Business.Workflows.WidgetWorkflows;

    public sealed partial class AddWidgetWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(AddWidgetRequest), typeof(AddWidgetWorkflow));
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(AddWidgetResponse), typeof(AddWidgetWorkflow));

        public Dropthings.DataAccess.WidgetZone NewWidget_WidgetZone = new Dropthings.DataAccess.WidgetZone();

        #endregion Fields

        #region Constructors

        public AddWidgetWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public AddWidgetRequest Request
        {
            get { return (AddWidgetRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public AddWidgetResponse Response
        {
            get { return (AddWidgetResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}