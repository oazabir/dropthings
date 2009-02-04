namespace Dropthings.Business.Activities.UserAccountActivities
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

    public partial class GetAspnetUserActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        public static readonly DependencyProperty aspnet_UserProperty = 
            DependencyProperty.Register("aspnet_User", typeof(aspnet_User), typeof(GetAspnetUserActivity));

        private static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(string), typeof(GetAspnetUserActivity));

        #endregion Fields

        #region Constructors

        public GetAspnetUserActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        public string UserName
        {
            get { return (string)base.GetValue(UserNameProperty); }
            set { base.SetValue(UserNameProperty, value); }
        }

        public aspnet_User aspnet_User
        {
            get { return (aspnet_User)GetValue(aspnet_UserProperty); }
            set { SetValue(aspnet_UserProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            this.aspnet_User = DatabaseHelper.GetSingle<aspnet_User, string>(DatabaseHelper.SubsystemEnum.User,
                    this.UserName, LinqQueries.CompiledQuery_GetUserFromUserName);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}