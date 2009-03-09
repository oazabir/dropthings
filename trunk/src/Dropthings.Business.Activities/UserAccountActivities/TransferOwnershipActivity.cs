namespace Dropthings.Business.Activities.UserAccountActivities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Linq;
    using System.Transactions;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.DataAccess;

    public partial class TransferOwnershipActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty UserNewGuidProperty = DependencyProperty.Register("UserNewGuid", typeof(Guid), typeof(TransferOwnershipActivity));
        private static DependencyProperty UserOldGuidProperty = DependencyProperty.Register("UserOldGuid", typeof(Guid), typeof(TransferOwnershipActivity));

        #endregion Fields

        #region Constructors

        public TransferOwnershipActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        public Guid UserNewGuid
        {
            get { return (Guid)base.GetValue(UserNewGuidProperty); }
            set { base.SetValue(UserNewGuidProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        public Guid UserOldGuid
        {
            get { return (Guid)base.GetValue(UserOldGuidProperty); }
            set { base.SetValue(UserOldGuidProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                List<Page> pages = DatabaseHelper.GetList<Page, Guid>(DatabaseHelper.SubsystemEnum.Page,
                    this.UserOldGuid, LinqQueries.CompiledQuery_GetPagesByUserId);

                foreach (Page page in pages)
                {
                    page.UserId = this.UserNewGuid;
                }

                DatabaseHelper.UpdateList<Page>(DatabaseHelper.SubsystemEnum.Page, pages,
                    null, null);

                // Delete setting for the anonymous user and create new setting for the new user
                UserSetting setting = DatabaseHelper.GetSingle<UserSetting, Guid>(DatabaseHelper.SubsystemEnum.User,
                    this.UserOldGuid, LinqQueries.CompiledQuery_GetUserSettingByUserGuid);
                DatabaseHelper.Delete<UserSetting>(DatabaseHelper.SubsystemEnum.User, setting);

                DatabaseHelper.Insert<UserSetting>(DatabaseHelper.SubsystemEnum.User, (newSetting) =>
                {
                    newSetting.UserId = this.UserNewGuid;
                    newSetting.CurrentPageId = setting.CurrentPageId;
                    newSetting.CreatedDate = DateTime.Now;
                });

                ts.Complete();
            }

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}