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

    public partial class GetWidgetInstanceActivity : Activity
    {
        #region Fields

        public static readonly DependencyProperty WidgetInstanceIdProperty = 
            DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(GetWidgetInstanceActivity));

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetInstanceProperty = 
            DependencyProperty.Register("WidgetInstance", typeof(WidgetInstance), typeof(GetWidgetInstanceActivity));

        #endregion Fields

        #region Constructors

        public GetWidgetInstanceActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public WidgetInstance WidgetInstance
        {
            get { return (WidgetInstance)GetValue(WidgetInstanceProperty); }
            set { SetValue(WidgetInstanceProperty, value); }
        }

        public int WidgetInstanceId
        {
            get { return (int)GetValue(WidgetInstanceIdProperty); }
            set { SetValue(WidgetInstanceIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            this.WidgetInstance = DatabaseHelper.GetSingle<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                this.WidgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceById);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}