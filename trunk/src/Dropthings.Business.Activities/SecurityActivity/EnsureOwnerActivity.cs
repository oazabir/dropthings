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

    public partial class EnsureOwnerActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for WidgetZoneId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetZoneIdProperty = 
            DependencyProperty.Register("WidgetZoneId", typeof(int), typeof(EnsureOwnerActivity));

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(EnsureOwnerActivity));
        private static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(string), typeof(EnsureOwnerActivity));
        private static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(int), typeof(EnsureOwnerActivity));

        #endregion Fields

        #region Constructors

        public EnsureOwnerActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string UserName
        {
            get { return (string)base.GetValue(UserNameProperty); }
            set { base.SetValue(UserNameProperty, value); }
        }

        public int WidgetInstanceId
        {
            get { return (int)base.GetValue(WidgetInstanceIdProperty); }
            set { base.SetValue(WidgetInstanceIdProperty, value); }
        }

        public int WidgetZoneId
        {
            get { return (int)GetValue(WidgetZoneIdProperty); }
            set { SetValue(WidgetZoneIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {
                if (this.PageId == 0 && this.WidgetInstanceId == 0 && this.WidgetZoneId == 0)
                {
                    throw new ApplicationException("Nothing specified to check. Must have one of these: PageID, WidgetInstanceID, WidgetZoneID");
                }

                if (this.PageId > 0)
                {
                    // Get the user who is the owner of the page. Then see if the current user is the same
                    var ownerName = DatabaseHelper.GetSingle<string, int>(DatabaseHelper.SubsystemEnum.Page,
                        this.PageId, LinqQueries.CompiledQuery_GetPageOwnerName);

                    if (!this.UserName.ToLower().Equals(ownerName))
                        throw new ApplicationException(string.Format("User {0} is not the owner of the page {1}", this.UserName, this.PageId));
                }
                else if (this.WidgetZoneId > 0)
                {
                    // Get the user who is the owner of the widget. Then see if the current user is the same
                    var owners = DatabaseHelper.GetList<string, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                        this.WidgetZoneId, LinqQueries.CompiledQuery_GetWidgetZoneOwnerName);
                    var ownerName = DatabaseHelper.GetSingle<string, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                        this.WidgetZoneId, LinqQueries.CompiledQuery_GetWidgetZoneOwnerName);

                    if (!this.UserName.ToLower().Equals(ownerName))
                        throw new ApplicationException(string.Format("User {0} is not the owner of the widget zone {1}", this.UserName, this.WidgetInstanceId));
                }

                else if (this.WidgetInstanceId > 0)
                {
                    // Get the user who is the owner of the widget. Then see if the current user is the same
                    var ownerName = DatabaseHelper.GetSingle<string, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                        this.WidgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceOwnerName);

                    if (!this.UserName.ToLower().Equals(ownerName))
                        throw new ApplicationException(string.Format("User {0} is not the owner of the widget instance {1}", this.UserName, this.WidgetInstanceId));
                }
            }
            catch (Exception e)
            {
            }

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}