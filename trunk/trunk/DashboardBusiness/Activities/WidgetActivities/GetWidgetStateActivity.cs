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

    public partial class GetWidgetStateActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(GetWidgetStateActivity));
        private static DependencyProperty WidgetStateProperty = DependencyProperty.Register("WidgetState", typeof(string), typeof(GetWidgetStateActivity));

        #endregion Fields

        #region Constructors

        public GetWidgetStateActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int WidgetInstanceId
        {
            get { return (int)base.GetValue(WidgetInstanceIdProperty); }
            set { base.SetValue(WidgetInstanceIdProperty, value); }
        }

        [Browsable(true)]
        public string WidgetState
        {
            get { return (string)base.GetValue(WidgetStateProperty); }
            set { base.SetValue(WidgetStateProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var wi = DatabaseHelper.GetSingle<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                    this.WidgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceById);

            this.WidgetState = wi.State;

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}