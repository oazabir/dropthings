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

    public partial class SetCurrentPageActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(SetCurrentPageActivity));
        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(SetCurrentPageActivity));

        #endregion Fields

        #region Constructors

        public SetCurrentPageActivity()
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
        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var userSetting = DatabaseHelper.GetSingle<UserSetting, Guid>(DatabaseHelper.SubsystemEnum.User,
                this.UserGuid, LinqQueries.CompiledQuery_GetUserSettingByUserGuid);

            if (userSetting != null)
            {
                userSetting.CurrentPageId = this.PageId;

                DatabaseHelper.UpdateObject<UserSetting>(DatabaseHelper.SubsystemEnum.User, userSetting, null);
            }
            else
            {
                userSetting = DatabaseHelper.Insert<UserSetting>(DatabaseHelper.SubsystemEnum.Page, (wi) =>
                {
                    wi.UserId = this.UserGuid;
                    wi.CurrentPageId = this.PageId;
                    wi.CreatedDate = DateTime.Now;
                });
            }
            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}