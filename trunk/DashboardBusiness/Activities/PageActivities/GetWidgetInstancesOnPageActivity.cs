namespace Dropthings.Business.Activities.PageActivities
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

    public partial class GetWidgetInstancesOnPageActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for PageId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIdProperty = 
            DependencyProperty.Register("PageId", typeof(int), typeof(GetWidgetInstancesOnPageActivity));

        // Using a DependencyProperty as the backing store for WidgetInstances.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetInstancesProperty = 
            DependencyProperty.Register("WidgetInstances", typeof(List<WidgetInstance>), typeof(GetWidgetInstancesOnPageActivity));

        #endregion Fields

        #region Constructors

        public GetWidgetInstancesOnPageActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public int PageId
        {
            get { return (int)GetValue(PageIdProperty); }
            set { SetValue(PageIdProperty, value); }
        }

        public List<WidgetInstance> WidgetInstances
        {
            get { return (List<WidgetInstance>)GetValue(WidgetInstancesProperty); }
            set { SetValue(WidgetInstancesProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            this.WidgetInstances = DatabaseHelper.GetList<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                this.PageId, LinqQueries.CompiledQuery_GetWidgetInstancesByPageId);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}