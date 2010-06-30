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

    public partial class SaveWidgetStateActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for ModifiedWidgetInstance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModifiedWidgetInstanceProperty = 
            DependencyProperty.Register("ModifiedWidgetInstance", typeof(WidgetInstance), typeof(SaveWidgetStateActivity));

        private static DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(string), typeof(SaveWidgetStateActivity));
        private static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(SaveWidgetStateActivity));

        #endregion Fields

        #region Constructors

        public SaveWidgetStateActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public WidgetInstance ModifiedWidgetInstance
        {
            get { return (WidgetInstance)GetValue(ModifiedWidgetInstanceProperty); }
            set { SetValue(ModifiedWidgetInstanceProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string State
        {
            get { return (string)base.GetValue(StateProperty); }
            set { base.SetValue(StateProperty, value); }
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
            DatabaseHelper.UpdateObject<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                this.WidgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceById,
                (wi) =>
                {
                    wi.State = this.State;
                    this.ModifiedWidgetInstance = wi;
                });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}