namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    using Dropthings.Data;
    using Dropthings.Model;
    using System.Web.Security;
    using Dropthings.Configuration;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    partial class Facade
    {
        #region Methods

        public RoleTemplate GetRoleTemplate(string templateUserName)
        {
            var userGuid = this.userRepository.GetUserGuidFromUserName(templateUserName);
            var template = this.roleTemplateRepository.GetRoleTemplatesByUserId(userGuid);

            return template;
        }

        public bool CheckRoleTemplateIsRegisterUserTemplate(RoleTemplate template)
        {
            //MembershipUser user = this.GetUser(template.AspNetUser.UserId);
            AspNetUser user = userRepository.GetUserByUserGuid(template.AspNetUser.UserId);
            
            UserTemplateSetting settingTemplate = GetUserSettingTemplate();

            return settingTemplate.RegisteredUserSettingTemplate.UserName.Equals(user.UserName);
        }

        public bool CheckRoleTemplateIsAnonymousUserTemplate(RoleTemplate template)
        {
            //MembershipUser user = this.GetUser(template.AspNetUser.UserId);
            AspNetUser user = userRepository.GetUserByUserGuid(template.AspNetUser.UserId);

            UserTemplateSetting settingTemplate = GetUserSettingTemplate();

            return settingTemplate.AnonUserSettingTemplate.UserName.Equals(user.UserName);
        }

        public UserTemplateSetting GetUserSettingTemplate()
        {
            return AspectF.Define.Cache<UserTemplateSetting>(Services.Get<ICache>(), CacheKeys.TemplateKeys.UserTemplateSetting())
                .Return<UserTemplateSetting>(() =>
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
                    });
        }

        #endregion Methods

    }
}