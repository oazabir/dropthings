using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dropthings.DataAccess;

using System.Transactions;
using System.Web.Security;

namespace Dropthings.Business.Facade
{
	partial class Facade
    {
        #region Methods
        public UserSetting GetUserSetting(Guid userGuid)
        {
            using (new TimedLog(userGuid.ToString(), "Activity: Get User Setting"))
            {
                var userSetting = this.userSettingRepository.GetUserSettingByUserGuid(userGuid);

                if (userSetting == default(UserSetting))
                {
                    // No setting saved before. Create default setting

                    userSetting = this.userSettingRepository.Insert(
                        newSetting =>
                        {
                            newSetting.UserId = userGuid;
                            newSetting.CreatedDate = DateTime.Now;
                            newSetting.CurrentPageId = this.pageRepository.GetPageIdByUserGuid(userGuid).First();
                        });
                }

                return userSetting;
            }
        }

        public bool TransferOwnership(Guid userGuid, Guid userOldGuid)
        {
            var success = false;

            using (TransactionScope ts = new TransactionScope())
            {
                List<Page> pages = this.pageRepository.GetPagesOfUser(userOldGuid);
                
                this.pageRepository.UpdateList(pages, (page) =>
                {
                    page.UserId = userGuid;
                }, null);
                
                var userSetting = GetUserSetting(userOldGuid);
                
                // Delete setting for the anonymous user and create new setting for the new user 
                this.userSettingRepository.Delete(userSetting);

                this.userSettingRepository.Insert((newSetting) =>
                {
                    newSetting.UserId = userGuid;
                    newSetting.CurrentPageId = userSetting.CurrentPageId;
                    newSetting.CreatedDate = DateTime.Now;
                });

                ts.Complete();
            }

            return success;
        }

        public bool UserExists(string userName)
        {
            return (this.userRepository.GetUserGuidFromUserName(userName) != default(Guid));
        }

        public void CreateTemplateUser(string email, bool isActivationRequired, string password, string requestedUserName, string roleName, string templateRoleName)
        {
            var userGuid = this.userRepository.GetUserGuidFromUserName(requestedUserName);
            if (userGuid.IsEmpty())
            {
                var newUserGuid = CreateUser(requestedUserName, password, email);
                SetUserRoles(requestedUserName, roleName);
                AddUserToRoleTemplate(newUserGuid, templateRoleName);

                var createdPage = CreatePage(newUserGuid, string.Empty, null);

                if (createdPage != null && createdPage.ID > 0)
                {
                    var userSetting = GetUserSetting(newUserGuid);
                    CreateDefaultWidgetsOnPage(requestedUserName, createdPage.ID);
                }
                else
                {
                    throw new ApplicationException("First page creation failed");
                }
            }
        }

        public void SetupDefaultSetting()
        {
            SetupDefaultRoles();

            var settings = (UserSettingTemplateSettingsSection)System.Configuration.ConfigurationManager.GetSection(UserSettingTemplateSettingsSection.SectionName);

            foreach (UserSettingTemplateElement setting in settings.UserSettingTemplates)
            {
                CreateTemplateUser(setting.UserName, false, setting.Password, setting.UserName, setting.RoleNames, setting.TemplateRoleName);
            }
        }

        #endregion
    }
}
