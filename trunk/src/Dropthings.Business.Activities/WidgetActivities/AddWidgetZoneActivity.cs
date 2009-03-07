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

    public partial class AddWidgetZoneActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty NewWidgetZoneProperty = DependencyProperty.Register("NewWidgetZone", typeof(WidgetZone), typeof(AddWidgetZoneActivity));
        private static DependencyProperty WidgetZoneTitleProperty = DependencyProperty.Register("WidgetZoneTitle", typeof(string), typeof(AddWidgetZoneActivity));

        #endregion Fields

        #region Constructors

        public AddWidgetZoneActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public WidgetZone NewWidgetZone
        {
            get { return (WidgetZone)base.GetValue(NewWidgetZoneProperty); }
            set { base.SetValue(NewWidgetZoneProperty, value); }
        }

        public string WidgetZoneTitle
        {
            get { return (string)base.GetValue(WidgetZoneTitleProperty); }
            set { base.SetValue(WidgetZoneTitleProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            this.NewWidgetZone = DatabaseHelper.Insert<WidgetZone>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                  (newWidgetZone) =>
                  {
                      string title = this.WidgetZoneTitle;
                      ObjectBuilder.BuildDefaultWidgetZone(newWidgetZone, title, title, 0);
                  });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}