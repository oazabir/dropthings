using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Collections.Generic;
using Dropthings.DataAccess;

namespace Dropthings.Business.Activities.PageActivities
{
	public partial class ApplyColumnWidthsActivity: Activity
	{
		public ApplyColumnWidthsActivity()
		{
			InitializeComponent();
		}



        public int PageId
        {
            get { return (int)GetValue(PageIdProperty); }
            set { SetValue(PageIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIdProperty =
            DependencyProperty.Register("PageId", typeof(int), typeof(ApplyColumnWidthsActivity));




        public List<Column> Columns
        {
            get { return (List<Column>)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(List<Column>), typeof(ApplyColumnWidthsActivity));


        public int[] ColumnWidths
        {
            get { return (int[])GetValue(ColumnWidthsProperty); }
            set { SetValue(ColumnWidthsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnWidths.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnWidthsProperty =
            DependencyProperty.Register("ColumnWidths", typeof(int[]), typeof(ApplyColumnWidthsActivity));


        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var columns = DatabaseHelper.GetList<Column, int>(DatabaseHelper.SubsystemEnum.Page,
                this.PageId,
                LinqQueries.CompiledQuery_GetColumnsByPageId);

            this.Columns = columns;
            
            DatabaseHelper.UpdateList<Column>(DatabaseHelper.SubsystemEnum.Column, columns,
                null,
                (col) => col.ColumnWidth = this.ColumnWidths[col.ColumnNo]);

            return ActivityExecutionStatus.Closed;
        }

	}
}
