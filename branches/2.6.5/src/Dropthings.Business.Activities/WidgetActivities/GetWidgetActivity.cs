namespace Dropthings.Business.Activities.WidgetActivities
{
    using System;
    using System.Collections;
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

    public partial class GetWidgetActivity : Activity
    {
        #region Fields

        public static readonly DependencyProperty WidgetIdProperty = 
            DependencyProperty.Register("WidgetId", typeof(int), typeof(GetWidgetActivity));

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetProperty = 
            DependencyProperty.Register("Widget", typeof(Widget), typeof(GetWidgetActivity));

        #endregion Fields

        #region Constructors

        public GetWidgetActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Widget Widget
        {
            get { return (Widget)GetValue(WidgetProperty); }
            set { SetValue(WidgetProperty, value); }
        }

        public int WidgetId
        {
            get { return (int)GetValue(WidgetIdProperty); }
            set { SetValue(WidgetIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            this.Widget = DatabaseHelper.GetSingle<Widget, int>(DatabaseHelper.SubsystemEnum.Widget,
                this.WidgetId, LinqQueries.CompiledQuery_GetWidgetById);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}