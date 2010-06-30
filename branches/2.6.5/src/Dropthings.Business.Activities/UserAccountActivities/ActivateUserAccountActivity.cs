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
    using Dropthings.Util;
    using Dropthings.Utils;

    public partial class ActivateUserAccount : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        public static readonly DependencyProperty TokenProperty = 
            DependencyProperty.Register("Token", typeof(Token), typeof(ActivateUserAccount));

        private static DependencyProperty ActivationKeyProperty = DependencyProperty.Register("ActivationKey", typeof(string), typeof(ActivateUserAccount));

        #endregion Fields

        #region Constructors

        public ActivateUserAccount()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        public string ActivationKey
        {
            get { return (string)base.GetValue(ActivationKeyProperty); }
            set { base.SetValue(ActivationKeyProperty, value); }
        }

        public Token Token
        {
            get { return (Token)GetValue(TokenProperty); }
            set { SetValue(TokenProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Guid guid = ((ShortGuid)this.ActivationKey).Guid;
            this.Token = DatabaseHelper.GetSingle<Token, Guid>(DatabaseHelper.SubsystemEnum.Token,
                guid, LinqQueries.CompiledQuery_GetTokenByUniqueId);

            if (this.Token != default(Token))
            {
                MembershipUser user = Membership.GetUser(this.Token.UserName);

                if (user != null)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);

                    DatabaseHelper.Delete<Token>(DatabaseHelper.SubsystemEnum.Token, this.Token);
                }
            }

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}