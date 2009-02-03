namespace Dropthings.Business.Activities.UserAccountActivities
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Linq;
    using System.Transactions;
    using System.Web.Security;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.DataAccess;

    public partial class UpdateAccountActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty NewEmailProperty = DependencyProperty.Register("NewEmail", typeof(string), typeof(UpdateAccountActivity));
        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(UpdateAccountActivity));

        #endregion Fields

        #region Constructors

        public UpdateAccountActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        public string NewEmail
        {
            get { return (string)base.GetValue(NewEmailProperty); }
            set { base.SetValue(NewEmailProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            MembershipUser memUser = Membership.GetUser(this.UserGuid);

            //aspnet_User user = db.aspnet_Users.Single(u => u.LoweredUserName == this._UserName && u.ApplicationId == DatabaseHelper.ApplicationGuid);
            aspnet_User user = DatabaseHelper.GetSingle<aspnet_User, string>(DatabaseHelper.SubsystemEnum.User,
                memUser.UserName, LinqQueries.CompiledQuery_GetUserFromUserName);

            using (TransactionScope ts = new TransactionScope())
            {
                //update user email
                memUser.Email = this.NewEmail;
                Membership.UpdateUser(memUser);

                //update username
                if (!string.Equals(memUser.UserName, this.NewEmail))
                {
                    user.UserName = this.NewEmail;
                    user.LoweredUserName = this.NewEmail.ToLower();
                }

                DatabaseHelper.UpdateObject<aspnet_User>(DatabaseHelper.SubsystemEnum.User,
                    user, null);

                ts.Complete();
            }

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}