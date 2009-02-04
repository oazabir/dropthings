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

    public partial class ExpandWidgetActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty IsExpandProperty = DependencyProperty.Register("IsExpand", typeof(Boolean), typeof(ExpandWidgetActivity));
        private static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(ExpandWidgetActivity));

        #endregion Fields

        #region Constructors

        public ExpandWidgetActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public bool IsExpand
        {
            get { return (bool)base.GetValue(IsExpandProperty); }
            set { base.SetValue(IsExpandProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int WidgetInstanceId
        {
            get { return (int)base.GetValue(WidgetInstanceIdProperty); }
            set { base.SetValue(WidgetInstanceIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var wi = DatabaseHelper.GetSingle<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                    this.WidgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceById);

            wi.Expanded = this.IsExpand;

            DatabaseHelper.UpdateObject<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                wi, null);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}