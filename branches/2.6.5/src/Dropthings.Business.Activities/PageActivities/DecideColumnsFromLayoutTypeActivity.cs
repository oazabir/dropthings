namespace Dropthings.Business.Activities.PageActivities
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

    public partial class DecideColumnsFromLayoutTypeActivity : Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for ColumnWidgets.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty = 
            DependencyProperty.Register("Columns", typeof(int[]), typeof(DecideColumnsFromLayoutTypeActivity));

        // Using a DependencyProperty as the backing store for LayoutType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LayoutTypeProperty = 
            DependencyProperty.Register("LayoutType", typeof(int), typeof(DecideColumnsFromLayoutTypeActivity));

        #endregion Fields

        #region Constructors

        public DecideColumnsFromLayoutTypeActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public int[] Columns
        {
            get { return (int[])GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public int LayoutType
        {
            get { return (int)GetValue(LayoutTypeProperty); }
            set { SetValue(LayoutTypeProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            this.Columns = Page.GetColumnWidths(this.LayoutType);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}