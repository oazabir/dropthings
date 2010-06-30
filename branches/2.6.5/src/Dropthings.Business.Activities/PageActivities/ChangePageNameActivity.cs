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

    using Dropthings.DataAccess;

    public partial class ChangePageNameActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(ChangePageNameActivity));
        private static DependencyProperty PageNameProperty = DependencyProperty.Register("PageName", typeof(string), typeof(ChangePageNameActivity));

        #endregion Fields

        #region Constructors

        public ChangePageNameActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string PageName
        {
            get { return (string)base.GetValue(PageNameProperty); }
            set { base.SetValue(PageNameProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DatabaseHelper.UpdateObject<Page, int>(DatabaseHelper.SubsystemEnum.Page,
                this.PageId, LinqQueries.CompiledQuery_GetPageById,
                (page) => page.Title = PageName);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}