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

    public partial class ClonePageActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty NewPageProperty = DependencyProperty.Register("NewPage", typeof(Page), typeof(ClonePageActivity));
        private static DependencyProperty PageToCloneProperty = DependencyProperty.Register("PageToClone", typeof(Page), typeof(ClonePageActivity));
        private static DependencyProperty UserIdProperty = DependencyProperty.Register("UserId", typeof(Guid), typeof(ClonePageActivity));

        #endregion Fields

        #region Constructors

        public ClonePageActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Page NewPage
        {
            get { return (Page)base.GetValue(NewPageProperty); }
            set { base.SetValue(NewPageProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Page PageToClone
        {
            get { return (Page)base.GetValue(PageToCloneProperty); }
            set { base.SetValue(PageToCloneProperty, value); }
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
            this.NewPage = DatabaseHelper.Insert<Page>(DatabaseHelper.SubsystemEnum.Page, (wi) =>
                {
                    wi.CreatedDate = DateTime.Now;
                    wi.Title = this.PageToClone.Title;
                    wi.UserId = this.UserId;
                    wi.LastUpdated = this.PageToClone.LastUpdated;
                    wi.VersionNo = this.PageToClone.VersionNo;
                    wi.LayoutType = this.PageToClone.LayoutType;
                    wi.PageType = this.PageToClone.PageType;

               });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}