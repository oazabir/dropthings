namespace Dropthings.Business.Activities.WidgetActivities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
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

    public partial class GetWidgetInstancesInZoneActivity : Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for WidgetInstances.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetInstancesProperty = 
            DependencyProperty.Register("WidgetInstances", typeof(List<WidgetInstance>), typeof(GetWidgetInstancesInZoneActivity));

        // Using a DependencyProperty as the backing store for WidgetZoneId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetZoneIdProperty = 
            DependencyProperty.Register("WidgetZoneId", typeof(int), typeof(GetWidgetInstancesInZoneActivity));

        #endregion Fields

        #region Constructors

        public GetWidgetInstancesInZoneActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public List<WidgetInstance> WidgetInstances
        {
            get { return (List<WidgetInstance>)GetValue(WidgetInstancesProperty); }
            set { SetValue(WidgetInstancesProperty, value); }
        }

        public int WidgetZoneId
        {
            get { return (int)GetValue(WidgetZoneIdProperty); }
            set { SetValue(WidgetZoneIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var widgetInstances = DatabaseHelper.GetList<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                this.WidgetZoneId,
                LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneIdWithWidget,
                LinqQueries.WidgetInstance_Options_With_Widget);

            this.WidgetInstances = widgetInstances;

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}