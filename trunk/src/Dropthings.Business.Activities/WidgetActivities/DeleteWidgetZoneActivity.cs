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

    public partial class DeleteWidgetZoneActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        public static readonly DependencyProperty WidgetZoneIdProperty = 
            DependencyProperty.Register("WidgetZoneId", typeof(int), typeof(DeleteWidgetZoneActivity));

        #endregion Fields

        #region Constructors

        public DeleteWidgetZoneActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public int WidgetZoneId
        {
            get { return (int)GetValue(WidgetZoneIdProperty); }
            set { SetValue(WidgetZoneIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DatabaseHelper.DeleteByPK<WidgetZone, int>(DatabaseHelper.SubsystemEnum.WidgetZone, new int[] { this.WidgetZoneId });
            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}