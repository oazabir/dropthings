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

    public partial class AddColumnActivity : Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for ColumnNo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnNoProperty = 
            DependencyProperty.Register("ColumnNo", typeof(int), typeof(AddColumnActivity));

        // Using a DependencyProperty as the backing store for PageId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIdProperty = 
            DependencyProperty.Register("PageId", typeof(int), typeof(AddColumnActivity));

        // Using a DependencyProperty as the backing store for Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidthProperty = 
            DependencyProperty.Register("Width", typeof(int), typeof(AddColumnActivity));

        #endregion Fields

        #region Constructors

        public AddColumnActivity()
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

        public int Width
        {
            get { return (int)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var insertedWidgetZone = DatabaseHelper.Insert<WidgetZone>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                    (newWidgetZone) =>
                    {
                        string title = "Column " + (this.ColumnNo + 1);
                        ObjectBuilder.BuildDefaultWidgetZone(newWidgetZone, title, title, 0);
                    });
            var insertedColumn = DatabaseHelper.Insert<Column>(DatabaseHelper.SubsystemEnum.Page, (newColumn) =>
            {
                newColumn.ColumnNo = this.ColumnNo;
                newColumn.ColumnWidth = this.Width;
                newColumn.WidgetZoneId = insertedWidgetZone.ID;
                newColumn.PageId = this.PageId;
            });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}