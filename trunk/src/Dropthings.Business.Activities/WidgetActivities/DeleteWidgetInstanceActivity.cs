#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Activities
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

    using Dropthings.DataAccess;

    public partial class DeleteWidgetInstanceActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        public static readonly DependencyProperty WidgetInstanceIdProperty = 
            DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(DeleteWidgetInstanceActivity));

        #endregion Fields

        #region Constructors

        public DeleteWidgetInstanceActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public int WidgetInstanceId
        {
            get { return (int)GetValue(WidgetInstanceIdProperty); }
            set { SetValue(WidgetInstanceIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DatabaseHelper.DeleteByPK<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance, new int[] { this.WidgetInstanceId });
            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}