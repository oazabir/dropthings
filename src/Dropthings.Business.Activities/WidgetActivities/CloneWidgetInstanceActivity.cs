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

    public partial class CloneWidgetInstanceActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty WidgetInstanceProperty = DependencyProperty.Register("WidgetInstance", typeof(WidgetInstance), typeof(CloneWidgetInstanceActivity));
        private static DependencyProperty WidgetZoneIdProperty = DependencyProperty.Register("WidgetZoneId", typeof(int), typeof(CloneWidgetInstanceActivity));

        #endregion Fields

        #region Constructors

        public CloneWidgetInstanceActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public WidgetInstance WidgetInstance
        {
            get { return (WidgetInstance)base.GetValue(WidgetInstanceProperty); }
            set { base.SetValue(WidgetInstanceProperty, value); }
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
            this.WidgetInstance = DatabaseHelper.Insert<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance, (wi) =>
                {
                    wi.CreatedDate = this.WidgetInstance.CreatedDate;
                    wi.Expanded = this.WidgetInstance.Expanded;
                    wi.Height = this.WidgetInstance.Height;
                    wi.LastUpdate = this.WidgetInstance.LastUpdate;
                    wi.Maximized = this.WidgetInstance.Maximized;
                    wi.OrderNo = this.WidgetInstance.OrderNo;
                    wi.Resized = this.WidgetInstance.Resized;
                    wi.State = this.WidgetInstance.State;
                    wi.Title = this.WidgetInstance.Title;
                    wi.VersionNo = this.WidgetInstance.VersionNo;
                    wi.WidgetId = this.WidgetInstance.WidgetId;
                    wi.WidgetZoneId = this.WidgetZoneId;
                    wi.Width = this.WidgetInstance.Width;
               });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}