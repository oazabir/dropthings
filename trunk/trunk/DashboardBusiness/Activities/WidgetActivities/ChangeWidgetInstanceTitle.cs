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

    public partial class ChangeWidgetInstanceTitle : Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for NewTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewTitleProperty = 
            DependencyProperty.Register("NewTitle", typeof(string), typeof(ChangeWidgetInstanceTitle));

        // Using a DependencyProperty as the backing store for WidgetInstanceId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetInstanceIdProperty = 
            DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(ChangeWidgetInstanceTitle));

        #endregion Fields

        #region Constructors

        public ChangeWidgetInstanceTitle()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string NewTitle
        {
            get { return (string)GetValue(NewTitleProperty); }
            set { SetValue(NewTitleProperty, value); }
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
            DatabaseHelper.UpdateObject<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.Page,
                this.WidgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceById,
                (wi) =>
                {
                    wi.Title = this.NewTitle;
                });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}