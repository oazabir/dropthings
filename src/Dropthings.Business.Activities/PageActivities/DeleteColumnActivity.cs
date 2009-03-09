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
    using System.Drawing;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.DataAccess;

    public partial class DeleteColumnActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        public static readonly DependencyProperty ColumnIdProperty = 
            DependencyProperty.Register("ColumnId", typeof(int), typeof(DeleteColumnActivity));

        #endregion Fields

        #region Constructors

        public DeleteColumnActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public int ColumnId
        {
            get { return (int)GetValue(ColumnIdProperty); }
            set { SetValue(ColumnIdProperty, value); }
        }

        public Column ColumnToDelete
        {
            get { return (Column)GetValue(ColumnToDeleteProperty); }
            set { SetValue(ColumnToDeleteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnToDelete.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnToDeleteProperty =
            DependencyProperty.Register("ColumnToDelete", typeof(Column), typeof(DeleteColumnActivity));

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (this.ColumnId > 0)
            {
                this.ColumnToDelete = DatabaseHelper.GetSingle<Column, int>(DatabaseHelper.SubsystemEnum.Column,
                    this.ColumnId, LinqQueries.CompiledQuery_GetColumnById);

                this.DeleteColumn();
            }
            else
            {
                this.ColumnToDelete = DatabaseHelper.GetSingle<Column, int, int>(DatabaseHelper.SubsystemEnum.Column, this.PageId, this.ColumnNo,
                    LinqQueries.CompiledQuery_GetColumnByPageId_ColumnNo);
                this.ColumnId = this.ColumnToDelete.ID;
                this.DeleteColumn();
            }
            return ActivityExecutionStatus.Closed;
        }

        private void DeleteColumn()
        {
            DatabaseHelper.DeleteByPK<Column, int>(DatabaseHelper.SubsystemEnum.Column, this.ColumnId);
            DatabaseHelper.DeleteByPK<WidgetZone, int>(DatabaseHelper.SubsystemEnum.WidgetZone, this.ColumnToDelete.WidgetZoneId);            
        }

        #endregion Methods



        public int ColumnNo
        {
            get { return (int)GetValue(ColumnNoProperty); }
            set { SetValue(ColumnNoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnNo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnNoProperty =
            DependencyProperty.Register("ColumnNo", typeof(int), typeof(DeleteColumnActivity));




        public int PageId
        {
            get { return (int)GetValue(PageIdProperty); }
            set { SetValue(PageIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIdProperty =
            DependencyProperty.Register("PageId", typeof(int), typeof(DeleteColumnActivity));


    }
}