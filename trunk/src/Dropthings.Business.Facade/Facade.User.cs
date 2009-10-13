using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dropthings.DataAccess;

using System.Transactions;
using System.Web.Security;
using Dropthings.Configuration;
using OmarALZabir.AspectF;
using Dropthings.Util;

namespace Dropthings.Business.Facade
{
	partial class Facade
    {
        #region Methods

        public void UpdateAccount(string email, string userName)
        {
            
        }

        public MembershipUser GetUser(string userName)
        {
            return AspectF.Define.Cache<MembershipUser>(Services.Get<ICacheResolver>(), CacheSetup.CacheKeys.UserFromUserName(userName))
                .Return<MembershipUser>(() => Membership.GetUser(userName));
        }
        public MembershipUser GetUser(Guid userGuid)
        {
            return AspectF.Define.Cache<MembershipUser>(Services.Get<ICacheResolver>(), CacheSetup.CacheKeys.UserFromUserGuid(userGuid))
                .Return<MembershipUser>(() => Membership.GetUser(userGuid));
        }

        public UserSetting GetUserSetting(Guid userGuid)
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

        public bool TransferOwnership(Guid userGuid, Guid userOldGuid)
        {
            var success = false;

            using (TransactionScope ts = new TransactionScope())
            {
                IEnumerable<Page> pages = this.GetPagesOfUser(userOldGuid);
                
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
            return (this.GetUserGuidFromUserName(userName) != default(Guid));
        }

        public void CreateTemplateUser(string email, bool isActivationRequired, string password, string requestedUserName, string roleName, string templateRoleName)
        {
            var userGuid = this.GetUserGuidFromUserName(requestedUserName);
            if (userGuid.IsEmpty())
            {
                var newUserGuid = CreateUser(requestedUserName, password, email);
                SetUserRoles(requestedUserName, roleName);
                AddUserToRoleTemplate(newUserGuid, templateRoleName);

                var createdPage = CreatePage(newUserGuid, string.Empty, null, 0);

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

        public Guid GetUserGuidFromUserName(string userName)
        {
            return this.userRepository.GetUserGuidFromUserName(userName);
        }

        #endregion
    }
}
