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

    public partial class SetUserRolesActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty RoleNameProperty = DependencyProperty.Register("RoleName", typeof(String), typeof(SetUserRolesActivity));
        private static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(string), typeof(SetUserRolesActivity));

        #endregion Fields

        #region Constructors

        public SetUserRolesActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public String RoleName
        {
            get { return (String)base.GetValue(RoleNameProperty); }
            set { base.SetValue(RoleNameProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string UserName
        {
            get { return (string)base.GetValue(UserNameProperty); }
            set { base.SetValue(UserNameProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (System.Web.Security.Roles.Enabled && !string.IsNullOrEmpty(this.RoleName))
            {
                string[] roles = RoleName.Split(new char[] { ',', ':' });

                for (int i = 0; i < roles.Length; i++)
                {
                    if (!System.Web.Security.Roles.IsUserInRole(roles[i]))
                    {
                        System.Web.Security.Roles.AddUserToRole(this.UserName, roles[i]);
                    }
                }
            }

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}