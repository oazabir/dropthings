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

    public partial class GetWidgetZoneOnPageColumn : Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for ColumnNo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnNoProperty = 
            DependencyProperty.Register("ColumnNo", typeof(int), typeof(GetWidgetZoneOnPageColumn));

        // Using a DependencyProperty as the backing store for PageId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIdProperty = 
            DependencyProperty.Register("PageId", typeof(int), typeof(GetWidgetZoneOnPageColumn));

        // Using a DependencyProperty as the backing store for WidgetZone.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetZoneProperty = 
            DependencyProperty.Register("WidgetZone", typeof(WidgetZone), typeof(GetWidgetZoneOnPageColumn));

        #endregion Fields

        #region Constructors

        public GetWidgetZoneOnPageColumn()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public int ColumnNo
        {
            get { return (int)GetValue(ColumnNoProperty); }
            set { SetValue(ColumnNoProperty, value); }
        }

        public int PageId
        {
            get { return (int)GetValue(PageIdProperty); }
            set { SetValue(PageIdProperty, value); }
        }

        public WidgetZone WidgetZone
        {
            get { return (WidgetZone)GetValue(WidgetZoneProperty); }
            set { SetValue(WidgetZoneProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var widgetZone = DatabaseHelper.GetSingle<WidgetZone, int, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                this.PageId, this.ColumnNo,
                LinqQueries.CompiledQuery_GetWidgetZoneByPageId_ColumnNo);

            this.WidgetZone = widgetZone;

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}