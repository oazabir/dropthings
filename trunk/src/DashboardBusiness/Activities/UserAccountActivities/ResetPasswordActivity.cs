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

    public partial class ResetPasswordActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty EmailProperty = DependencyProperty.Register("Email", typeof(string), typeof(ResetPasswordActivity));
        private static DependencyProperty NewPasswordProperty = DependencyProperty.Register("NewPassword", typeof(string), typeof(ResetPasswordActivity));

        #endregion Fields

        #region Constructors

        public ResetPasswordActivity()
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
        public string NewPassword
        {
            get { return (string)base.GetValue(NewPasswordProperty); }
            set { base.SetValue(NewPasswordProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var userName = Membership.GetUserNameByEmail(this.Email);

            if (!string.IsNullOrEmpty(userName))
            {
                var user = Membership.GetUser(userName, false);

                this.NewPassword = user.ResetPassword();
            }

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}