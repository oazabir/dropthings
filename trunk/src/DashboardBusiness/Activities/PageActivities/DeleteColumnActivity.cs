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

    public partial class DeleteColumnActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        public static readonly DependencyProperty ColumnIdProperty = 
            DependencyProperty.Register("ColumnId", typeof(int), typeof(DeleteColumnActivity));

        #endregion Fields

        #region Constructors

        public DeleteColumnActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public int ColumnId
        {
            get { return (int)GetValue(ColumnIdProperty); }
            set { SetValue(ColumnIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DatabaseHelper.DeleteByPK<Column, int>(DatabaseHelper.SubsystemEnum.Column, new int[] { this.ColumnId });
            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}