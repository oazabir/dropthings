#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Activities
{
    using System;
    using System.Collections.Generic;
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

    public partial class ReorderWidgetInstancesOnWidgetZoneActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(ReorderWidgetInstancesOnWidgetZoneActivity));

        /*
        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        private static DependencyProperty ColumnNoProperty = DependencyProperty.Register("ColumnNo", typeof(int), typeof(ReorderWidgetInstanceOnColumnActivity));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int ColumnNo
        {
            get { return (int)base.GetValue(ColumnNoProperty); }
            set { base.SetValue(ColumnNoProperty, value); }
        }
        */
        private static DependencyProperty WidgetZoneIdProperty = DependencyProperty.Register("WidgetZoneId", typeof(int), typeof(ReorderWidgetInstancesOnWidgetZoneActivity));

        #endregion Fields

        #region Constructors

        public ReorderWidgetInstancesOnWidgetZoneActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

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
            var list = DatabaseHelper.GetList<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                this.WidgetZoneId, LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneId);

            int orderNo = 0;
            foreach (WidgetInstance wi in list)
            {
                wi.OrderNo = orderNo++;
            }

            DatabaseHelper.UpdateList<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance, list, null, null);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}