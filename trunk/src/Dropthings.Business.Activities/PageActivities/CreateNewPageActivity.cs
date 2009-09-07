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
    using System.Data.Linq;
    using System.Drawing;
    using System.Linq;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.Business.Activities.PageActivities;
    using Dropthings.DataAccess;

    public partial class CreateNewPageActivity : Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for NewPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewPageProperty = 
            DependencyProperty.Register("NewPage", typeof(Page), typeof(CreateNewPageActivity));

        private static DependencyProperty LayoutTypeProperty = DependencyProperty.Register("LayoutType", typeof(String), typeof(CreateNewPageActivity));
        private static DependencyProperty NewPageIdProperty = DependencyProperty.Register("NewPageId", typeof(int), typeof(CreateNewPageActivity));
        private static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(String), typeof(CreateNewPageActivity));
        private static DependencyProperty UserIdProperty = DependencyProperty.Register("UserId", typeof(Guid), typeof(CreateNewPageActivity));

        #endregion Fields

        #region Constructors

        public CreateNewPageActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string LayoutType
        {
            get { return (string)base.GetValue(LayoutTypeProperty); }
            set { base.SetValue(LayoutTypeProperty, value); }
        }

        public Page NewPage
        {
            get { return (Page)GetValue(NewPageProperty); }
            set { SetValue(NewPageProperty, value); }
        }

        public int NewPageId
        {
            get { return (int)base.GetValue(NewPageIdProperty); }
            set { base.SetValue(NewPageIdProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set { base.SetValue(TitleProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserId
        {
            get { return (Guid)base.GetValue(UserIdProperty); }
            set { base.SetValue(UserIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            // Create a 3 column all equal size page
            var insertedPage = DatabaseHelper.Insert<Page>(DatabaseHelper.SubsystemEnum.Page, (newPage) =>
                {
                    ObjectBuilder.BuildDefaultPage(newPage,
                        this.UserId, this.Title, Convert.ToInt32(this.LayoutType), 0);
                });

            var page = DatabaseHelper.GetSingle<Page, int>(DatabaseHelper.SubsystemEnum.Page,
                insertedPage.ID, LinqQueries.CompiledQuery_GetPageById);

            for (int i = 0; i < insertedPage.ColumnCount; i++)
            {
                var insertedWidgetZone = DatabaseHelper.Insert<WidgetZone>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                    (newWidgetZone) =>
                    {
                        string title = "Column " + (i + 1);
                        ObjectBuilder.BuildDefaultWidgetZone(newWidgetZone, title, title, 0);
                    });
                var insertedColumn = DatabaseHelper.Insert<Column>(DatabaseHelper.SubsystemEnum.Page,
                    (newColumn) =>
                    {
                        newColumn.ColumnNo = i;
                        newColumn.ColumnWidth = (100 / insertedPage.ColumnCount);
                        newColumn.WidgetZoneId = insertedWidgetZone.ID;
                        newColumn.PageId = insertedPage.ID;
                    });
            }

            NewPageId = page.ID;

            this.NewPage = page;

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}