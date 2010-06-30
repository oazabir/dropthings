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
    using System.Data.Linq;
    using System.Drawing;
    using System.Linq;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.DataAccess;

    public partial class CloneColumnActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty ColumnToCloneProperty = DependencyProperty.Register("ColumnToClone", typeof(Column), typeof(CloneColumnActivity));
        private static DependencyProperty NewColumnProperty = DependencyProperty.Register("NewColumn", typeof(Column), typeof(CloneColumnActivity));
        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(CloneColumnActivity));
        private static DependencyProperty WidgetZoneIdProperty = DependencyProperty.Register("WidgetZoneId", typeof(int), typeof(CloneColumnActivity));

        #endregion Fields

        #region Constructors

        public CloneColumnActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Column ColumnToClone
        {
            get { return (Column)base.GetValue(ColumnToCloneProperty); }
            set { base.SetValue(ColumnToCloneProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Column NewColumn
        {
            get { return (Column)base.GetValue(NewColumnProperty); }
            set { base.SetValue(NewColumnProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        public int WidgetZoneId
        {
            get { return (int)base.GetValue(WidgetZoneIdProperty); }
            set { base.SetValue(WidgetZoneIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            this.NewColumn = DatabaseHelper.Insert<Column>(DatabaseHelper.SubsystemEnum.Column, (col) =>
                {
                    col.PageId = this.PageId;
                    col.WidgetZoneId = this.WidgetZoneId;
                    col.ColumnNo = this.ColumnToClone.ColumnNo;
                    col.ColumnWidth = this.ColumnToClone.ColumnWidth;
               });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}