namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;
    using Dropthings.Model;
    using System.Web.Security;

    partial class Facade
    {
        #region Methods

        public RoleTemplate GetRoleTemplate(Guid userGuid)
        {
            var template = this.roleTemplateRepository.GetRoleTemplatesByUserId(userGuid);

            if (template == null)
            {
                var setting = GetUserSettingTemplate();
                //as template is null system will look for guest template
                template = this.roleTemplateRepository.GetRoleTemplateByTemplateUserName(setting.AnonUserSettingTemplate.UserName);
            }

            return template;
        }

        public bool CheckRoleTemplateIsRegisterUserTemplate(RoleTemplate template)
        {
            MembershipUser user = Membership.GetUser(template.TemplateUserId);

            UserTemplateSetting settingTemplate = GetUserSettingTemplate();

            return settingTemplate.RegisteredUserSettingTemplate.UserName.Equals(user.UserName);
        }

        public bool CheckRoleTemplateIsAnonymousUserTemplate(RoleTemplate template)
        {
            MembershipUser user = Membership.GetUser(template.TemplateUserId);

            UserTemplateSetting settingTemplate = GetUserSettingTemplate();

            return settingTemplate.AnonUserSettingTemplate.UserName.Equals(user.UserName);
        }

        public UserTemplateSetting GetUserSettingTemplate()
        {
            UserSettingTemplateSettingsSection settings = (UserSettingTemplateSettingsSection)ConfigurationManager.GetSection(UserSettingTemplateSettingsSection.SectionName);
            var setting = new UserTemplateSetting
            {
                CloneAnonProfileEnabled = settings.CloneAnonProfileEnabled,
                CloneRegisteredProfileEnabled = settings.CloneRegisteredProfileEnabled,
                AnonUserSettingTemplate = settings.UserSettingTemplates[UserSettingTemplateSettingsSection.AnonTemplateKey],
                RegisteredUserSettingTemplate = settings.UserSettingTemplates[UserSettingTemplateSettingsSection.RegTemplateKey],
                AllUserSettingTemplate = new List<UserSettingTemplateElement>()
            };

            foreach (UserSettingTemplateElement element in settings.UserSettingTemplates)
            {
                setting.AllUserSettingTemplate.Add(element);
            }

            return setting;
        }

        #endregion Methods
    }
}