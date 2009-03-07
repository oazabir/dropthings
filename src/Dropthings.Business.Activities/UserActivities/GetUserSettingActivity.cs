namespace Dropthings.Business.Activities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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

    public partial class GetUserSettingActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(Page), typeof(GetUserSettingActivity));
        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(GetUserSettingActivity));
        private static DependencyProperty UserSettingProperty = DependencyProperty.Register("UserSetting", typeof(UserSetting), typeof(GetUserSettingActivity));

        #endregion Fields

        #region Constructors

        public GetUserSettingActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Page CurrentPage
        {
            get { return (Page)base.GetValue(CurrentPageProperty); }
            set { base.SetValue(CurrentPageProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }

        public UserSetting UserSetting
        {
            get { return (UserSetting)base.GetValue(UserSettingProperty); }
            set { base.SetValue(UserSettingProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using( new TimedLog(UserGuid.ToString(), "Activity: Get User Setting") )
            {
                var userSetting = DatabaseHelper.GetSingle<UserSetting, Guid>(DatabaseHelper.SubsystemEnum.User,
                    this.UserGuid, LinqQueries.CompiledQuery_GetUserSettingByUserGuid);

                if (userSetting == default(UserSetting))
                {
                    // No setting saved before. Create default setting

                    userSetting = DatabaseHelper.Insert<UserSetting>(DatabaseHelper.SubsystemEnum.User,
                        (newSetting) =>
                        {
                            newSetting.UserId = UserGuid;
                            newSetting.CreatedDate = DateTime.Now;
                            newSetting.CurrentPageId = DatabaseHelper.GetQueryResult<int, Guid, int>(
                                DatabaseHelper.SubsystemEnum.Page,
                                this.UserGuid, LinqQueries.CompiledQuery_GetPageIdByUserGuid,
                                (query) => query.First<int>());
                        });
                }

                this.UserSetting = userSetting;

                // Get users current page and if not available, get the first page
                this.CurrentPage = DatabaseHelper.GetSingle<Page, int>(DatabaseHelper.SubsystemEnum.Page,
                    userSetting.CurrentPageId, LinqQueries.CompiledQuery_GetPageById)
                    ?? DatabaseHelper.GetQueryResult<Page, Guid, Page>(
                                DatabaseHelper.SubsystemEnum.Page,
                                this.UserGuid, LinqQueries.CompiledQuery_GetPagesByUserId,
                                (query) => query.First<Page>());

                return ActivityExecutionStatus.Closed;
            }
        }

        #endregion Methods
    }
}