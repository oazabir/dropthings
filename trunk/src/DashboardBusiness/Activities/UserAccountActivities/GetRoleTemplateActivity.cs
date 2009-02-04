namespace Dropthings.Business.Activities.UserAccountActivities
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Configuration;
    using System.Drawing;
    using System.Globalization;
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

    public partial class GetRoleTemplateActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty RoleTemplateProperty = DependencyProperty.Register("RoleTemplate", typeof(RoleTemplate), typeof(GetRoleTemplateActivity));
        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(GetRoleTemplateActivity));

        #endregion Fields

        #region Constructors

        public GetRoleTemplateActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public RoleTemplate RoleTemplate
        {
            get { return (RoleTemplate)base.GetValue(RoleTemplateProperty); }
            set { base.SetValue(RoleTemplateProperty, value); }
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
            //will return the highest priority role template
            var template = DatabaseHelper.GetList<RoleTemplate, Guid>(DatabaseHelper.SubsystemEnum.User,
                    this.UserGuid, LinqQueries.CompiledQuery_GetRoleTemplatesByUserId).FirstOrDefault();

            if (template == null)
            {
                UserSettingTemplateSettingsSection settings = (UserSettingTemplateSettingsSection)ConfigurationManager.GetSection(UserSettingTemplateSettingsSection.SectionName);
                UserSettingTemplateElement anonUserSettingTemplate = settings.UserSettingTemplates[UserSettingTemplateSettingsSection.AnonTemplateKey];
                //as template is null system will look for guest template
                template = DatabaseHelper.GetSingle<RoleTemplate, string>(DatabaseHelper.SubsystemEnum.User, anonUserSettingTemplate.UserName,
                    LinqQueries.CompiledQuery_GetRoleTemplateByTemplateUserName);
            }

            this.RoleTemplate = template;

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}