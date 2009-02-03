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

    public partial class AddUserToRoleTemplateActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty RoleNameProperty = DependencyProperty.Register("RoleName", typeof(string), typeof(AddUserToRoleTemplateActivity));
        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(AddUserToRoleTemplateActivity));

        #endregion Fields

        #region Constructors

        public AddUserToRoleTemplateActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        public string RoleName
        {
            get { return (string)base.GetValue(RoleNameProperty); }
            set { base.SetValue(RoleNameProperty, value); }
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
            var RoleTemplates = DatabaseHelper.GetList<RoleTemplate>(DatabaseHelper.SubsystemEnum.User,
                    LinqQueries.CompiledQuery_GetRoleTemplates);
            int priority = RoleTemplates.Count == 0 ? 0 : RoleTemplates.Max(r => r.Priority);

            var aspnet_Role = DatabaseHelper.GetSingle<aspnet_Role, string>(DatabaseHelper.SubsystemEnum.User,
                    RoleName, LinqQueries.CompiledQuery_GetRoleByRoleName);
            var RoleTemplate = DatabaseHelper.GetSingle<RoleTemplate, string>(DatabaseHelper.SubsystemEnum.User,
                RoleName, LinqQueries.CompiledQuery_GetRoleTemplateByRoleName);

            if (RoleTemplate == null)
            {
                var insertedRoleTemplate = DatabaseHelper.Insert<RoleTemplate>(DatabaseHelper.SubsystemEnum.User, (newRoleTemplate) =>
                {
                    newRoleTemplate.RoleId = aspnet_Role.RoleId;
                    newRoleTemplate.TemplateUserId = this.UserGuid;
                    newRoleTemplate.Priority = priority + 1;
                });
            }

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}