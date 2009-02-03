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

    public partial class ChangeWidgetInstancePositionActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty RowNoProperty = DependencyProperty.Register("RowNo", typeof(int), typeof(ChangeWidgetInstancePositionActivity));
        private static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(ChangeWidgetInstancePositionActivity));

        /*
        private static DependencyProperty ColumnNoProperty = DependencyProperty.Register("ColumnNo", typeof(int), typeof(ChangeWidgetInstancePositionActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int ColumnNo
        {
            get { return (int)base.GetValue(ColumnNoProperty); }
            set { base.SetValue(ColumnNoProperty, value); }
        }
        */
        private static DependencyProperty WidgetZoneIdProperty = DependencyProperty.Register("WidgetZoneId", typeof(int), typeof(ChangeWidgetInstancePositionActivity));

        #endregion Fields

        #region Constructors

        public ChangeWidgetInstancePositionActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int RowNo
        {
            get { return (int)base.GetValue(RowNoProperty); }
            set { base.SetValue(RowNoProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int WidgetInstanceId
        {
            get { return (int)base.GetValue(WidgetInstanceIdProperty); }
            set { base.SetValue(WidgetInstanceIdProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int WidgetZoneId
        {
            get { return (int)base.GetValue(WidgetZoneIdProperty); }
            set { base.SetValue(WidgetZoneIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DatabaseHelper.UpdateObject<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.Page,
                this.WidgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceById,
                (wi) =>
                {
                    var isMovingDown = RowNo > wi.OrderNo;
                    wi.OrderNo = isMovingDown ? RowNo + 1 : RowNo;
                    wi.WidgetZoneId = this.WidgetZoneId;
                });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}