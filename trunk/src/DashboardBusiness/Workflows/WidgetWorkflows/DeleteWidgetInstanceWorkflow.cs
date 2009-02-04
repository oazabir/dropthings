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
    using Dropthings.DataAccess;

    public sealed partial class DeleteWidgetInstanceWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(DeleteWidgetInstanceWorkflowRequest), typeof(DeleteWidgetInstanceWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(DeleteWidgetInstanceWorkflowResponse), typeof(DeleteWidgetInstanceWorkflow));

        public static DependencyProperty WidgetInstanceProperty = DependencyProperty.Register("WidgetInstance", typeof(WidgetInstance), typeof(Dropthings.Business.DeleteWidgetInstanceWorkflow));

        #endregion Fields

        #region Constructors

        public DeleteWidgetInstanceWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        /*
        public static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(System.Int32), typeof(DashboardBusiness.DeleteWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 PageId
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.PageIdProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.PageIdProperty, value);
            }
        }

        public static DependencyProperty ColumnNoProperty = DependencyProperty.Register("ColumnNo", typeof(System.Int32), typeof(DashboardBusiness.DeleteWidgetInstanceWorkflow));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 ColumnNo
        {
            get
            {
                return ((int)(base.GetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.ColumnNoProperty)));
            }
            set
            {
                base.SetValue(DashboardBusiness.DeleteWidgetInstanceWorkflow.ColumnNoProperty, value);
            }
        }
        */
        public DeleteWidgetInstanceWorkflowRequest Request
        {
            get { return (DeleteWidgetInstanceWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public DeleteWidgetInstanceWorkflowResponse Response
        {
            get { return (DeleteWidgetInstanceWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public WidgetInstance WidgetInstance
        {
            get
            {
                return ((WidgetInstance)(base.GetValue(Dropthings.Business.DeleteWidgetInstanceWorkflow.WidgetInstanceProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.DeleteWidgetInstanceWorkflow.WidgetInstanceProperty, value);
            }
        }

        #endregion Properties
    }
}