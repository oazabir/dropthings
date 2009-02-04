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

    public partial class AddWidgetOnWidgetZone : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty NewWidgetProperty = DependencyProperty.Register("NewWidget", typeof(WidgetInstance), typeof(AddWidgetOnWidgetZone));
        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(AddWidgetOnWidgetZone));

        /*
        private static DependencyProperty ColumnNoProperty = DependencyProperty.Register("ColumnNo", typeof(int), typeof(AddWidgetOnPage));

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int ColumnNo
        {
            get { return (int)base.GetValue(ColumnNoProperty); }
            set { base.SetValue(ColumnNoProperty, value); }
        }
        */
        private static DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(int), typeof(AddWidgetOnWidgetZone));

        /*
        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }
        */
        private static DependencyProperty WidgetIdProperty = DependencyProperty.Register("WidgetId", typeof(int), typeof(AddWidgetOnWidgetZone));
        private static DependencyProperty WidgetZoneIdProperty = DependencyProperty.Register("WidgetZoneId", typeof(int), typeof(AddWidgetOnWidgetZone));

        #endregion Fields

        #region Constructors

        public AddWidgetOnWidgetZone()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public WidgetInstance NewWidget
        {
            get { return (WidgetInstance)base.GetValue(NewWidgetProperty); }
            set { base.SetValue(NewWidgetProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int Position
        {
            get { return (int)base.GetValue(PositionProperty); }
            set { base.SetValue(PositionProperty, value); }
        }

        public int WidgetId
        {
            get { return (int)base.GetValue(WidgetIdProperty); }
            set { base.SetValue(WidgetIdProperty, value); }
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
            Widget w = DatabaseHelper.GetSingle<Widget, int>(DatabaseHelper.SubsystemEnum.Widget,
                            this.WidgetId,
                            LinqQueries.CompiledQuery_GetWidgetById);

            this.NewWidget = DatabaseHelper.Insert<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance, (wi) =>
                {
                    ObjectBuilder.BuildDefaultWidgetInstance(wi,
                        w.Name, this.WidgetZoneId, this.Position, w.ID, w.DefaultState);
                });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}