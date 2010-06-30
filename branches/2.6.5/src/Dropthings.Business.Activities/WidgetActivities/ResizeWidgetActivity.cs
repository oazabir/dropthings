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

    public partial class ResizeWidgetActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for ModifiedWidgetInstance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModifiedWidgetInstanceProperty = 
            DependencyProperty.Register("ModifiedWidgetInstance", typeof(WidgetInstance), typeof(ResizeWidgetActivity));

        private static DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(int), typeof(ResizeWidgetActivity));
        private static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(ResizeWidgetActivity));
        private static DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(int), typeof(ResizeWidgetActivity));

        #endregion Fields

        #region Constructors

        public ResizeWidgetActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int Height
        {
            get { return (int)base.GetValue(HeightProperty); }
            set { base.SetValue(HeightProperty, value); }
        }

        public WidgetInstance ModifiedWidgetInstance
        {
            get { return (WidgetInstance)GetValue(ModifiedWidgetInstanceProperty); }
            set { SetValue(ModifiedWidgetInstanceProperty, value); }
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
        public int Width
        {
            get { return (int)base.GetValue(WidthProperty); }
            set { base.SetValue(WidthProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DatabaseHelper.UpdateObject<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                this.WidgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceById,
                (wi) =>
                {
                    wi.Width = this.Width;
                    wi.Height = this.Height;
                    wi.Resized = true;
                    this.ModifiedWidgetInstance = wi;

                });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}