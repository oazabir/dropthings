namespace Dropthings.Business.Activities.UserAccountActivities
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Linq;
    using System.Web.Security;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.DataAccess;
    using Dropthings.Utils;

    public partial class CreateUserTokenActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty EmailProperty = DependencyProperty.Register("Email", typeof(string), typeof(CreateUserTokenActivity));
        private static DependencyProperty UnlockKeyProperty = DependencyProperty.Register("UnlockKey", typeof(string), typeof(CreateUserTokenActivity));
        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(CreateUserTokenActivity));

        #endregion Fields

        #region Constructors

        public CreateUserTokenActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        public string Email
        {
            get { return (string)base.GetValue(EmailProperty); }
            set { base.SetValue(EmailProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        public string UnlockKey
        {
            get { return (string)base.GetValue(UnlockKeyProperty); }
            set { base.SetValue(UnlockKeyProperty, value); }
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
            Token insertedToken = DatabaseHelper.Insert<Token>(DatabaseHelper.SubsystemEnum.Token, (token) =>
            {
                token.UserId = this.UserGuid;
                token.UserName = this.Email;
                token.UniqueID = Guid.NewGuid();
            });

            ShortGuid shortGuid = insertedToken.UniqueID;
            this.UnlockKey = shortGuid.Value;

            MembershipUser newUser = Membership.GetUser(this.Email);
            newUser.IsApproved = false;
            Membership.UpdateUser(newUser);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}