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

    public partial class CreateUserActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty EmailProperty = DependencyProperty.Register("Email", typeof(string), typeof(CreateUserActivity));
        private static DependencyProperty NewUserGuidProperty = DependencyProperty.Register("NewUserGuid", typeof(Guid), typeof(CreateUserActivity));
        private static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(CreateUserActivity));
        private static DependencyProperty RequestedUsernameProperty = DependencyProperty.Register("RequestedUsername", typeof(string), typeof(CreateUserActivity));

        #endregion Fields

        #region Constructors

        public CreateUserActivity()
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

        public Guid NewUserGuid
        {
            get { return (Guid)base.GetValue(NewUserGuidProperty); }
            set { base.SetValue(NewUserGuidProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        public string Password
        {
            get { return (string)base.GetValue(PasswordProperty); }
            set { base.SetValue(PasswordProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        public string RequestedUsername
        {
            get { return (string)base.GetValue(RequestedUsernameProperty); }
            set { base.SetValue(RequestedUsernameProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            MembershipUser newUser = Membership.CreateUser(this.RequestedUsername, this.Password, this.Email);
            this.NewUserGuid = (Guid)newUser.ProviderUserKey;
            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}