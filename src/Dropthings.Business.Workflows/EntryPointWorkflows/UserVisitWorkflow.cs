#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Workflows.EntryPointWorkflows
{
    using System;
    using System.Collections.Generic;
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

    using Dropthings.DataAccess;

    public sealed partial class UserVisitWorkflow : SequentialWorkflowActivity, IUserVisitWorkflow
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(IUserVisitWorkflowRequest), typeof(UserVisitWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(IUserVisitWorkflowResponse), typeof(UserVisitWorkflow));

        public static DependencyProperty ReturnPages_MethodInvoking1Event = DependencyProperty.Register("ReturnPages_MethodInvoking1", typeof(System.EventHandler), typeof(UserVisitWorkflow));

        #endregion Fields

        #region Constructors

        public UserVisitWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Events

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Handlers")]
        public event EventHandler ReturnPages_MethodInvoking1
        {
            add
            {
                base.AddHandler(ReturnPages_MethodInvoking1Event, value);
            }
            remove
            {
                base.RemoveHandler(ReturnPages_MethodInvoking1Event, value);
            }
        }

        #endregion Events

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

        private void ReturnUserPageSetup_ExecuteCode(object sender, EventArgs e)
        {
            this.Response.UserPages = this.GetUserPages.Pages;
            this.Response.UserSetting = this.GetUserSetting.UserSetting;
            //this.Response.WidgetInstances = this.GetWidgetsInCurrentPage.WidgetInstances;
        }

        private void SetException_ExecuteCode(object sender, EventArgs e)
        {
            throw new ApplicationException("Not a valid user");
        }

        private void UserHasPageCode_ExecuteCode(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("User has pages");
        }

        #endregion Methods
    }
}