using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dropthings.Data;

using System.Transactions;
using System.Web.Security;
using Dropthings.Configuration;
using OmarALZabir.AspectF;
using Dropthings.Util;
using System.Web.Profile;
using System.Data.Objects.DataClasses;

namespace Dropthings.Business.Facade
{
	partial class Facade
    {
        #region Methods

        public bool Login(string userName, string password)
        {
            return Membership.ValidateUser(userName, password);
        }

        public void UpdateAccount(string email, string userName)
        {
            
        }

        public MembershipUser GetUser(string userName)
        {
            return AspectF.Define.Cache<MembershipUser>(Services.Get<ICache>(), CacheKeys.UserKeys.UserFromUserName(userName))
                .Return<MembershipUser>(() => Membership.GetUser(userName));
        }
        public MembershipUser GetUser(Guid userGuid)
        {
            return AspectF.Define.Cache<MembershipUser>(Services.Get<ICache>(), CacheKeys.UserKeys.UserFromUserGuid(userGuid))
                .Return<MembershipUser>(() => Membership.GetUser(userGuid));
        }

        public Guid CreateUser(string registeredUserName, string password, string email)
        {
            MembershipUser newUser = Membership.CreateUser(registeredUserName, password, email);
            return (Guid)newUser.ProviderUserKey;
        }

        public MembershipUser CreateUser(string userName, string password, string email, bool isApproved, out MembershipCreateStatus status)
        {
            return Membership.CreateUser(userName, password, email, null, null, isApproved, out status);
        }

        public void UpdateUser(MembershipUser member)
        {
            Membership.UpdateUser(member);
        }

        public void DeleteUser(string useName)
        {
            Membership.DeleteUser(useName, true);
        }

        public UserSetting GetUserSetting(Guid userGuid)
        {
            var userSetting = this.userSettingRepository.GetUserSettingByUserGuid(userGuid);

            if (userSetting == default(UserSetting))
            {
                // No setting saved before. Create default setting

                userSetting = this.userSettingRepository.Insert(new UserSetting
                    {
                        AspNetUser = new AspNetUser { UserId = userGuid },
                        CreatedDate = DateTime.Now,
                        CurrentTab = new Tab { ID = this.pageRepository.GetTabIdByUserGuid(userGuid).First() }
                    });
            }

            return userSetting;        
        }

        public bool TransferOwnership(Guid userGuid, Guid userOldGuid)
        {
            var success = false;

            using (TransactionScope ts = new TransactionScope())
            {
                // TODO: Since changing the page object will change the object
                // directly into the cache, next time getting the same pages will
                // return the new user ID for the pages. We need to clone the pages.
                IEnumerable<Tab> pages = this.pageRepository.GetTabsOfUser(userOldGuid);

                var newUser = this.userRepository.GetUserByUserGuid(userGuid);
                pages.Each(page =>
                    {
                        page.AspNetUser = newUser;
                        page.AspNetUserReference = new EntityReference<AspNetUser>
                        {
                            EntityKey = newUser.EntityKey
                        };
                    });
                this.pageRepository.UpdateList(pages);
                
                var userSetting = GetUserSetting(userOldGuid);
                
                // Delete setting for the anonymous user and create new setting for the new user 
                this.userSettingRepository.Delete(userSetting);

                this.userSettingRepository.Insert(new UserSetting
                {
                    AspNetUser = new AspNetUser { UserId = userGuid },
                    CurrentTab = new Tab { ID = userSetting.CurrentTab.ID },
                    CreatedDate = DateTime.Now
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
                SetUserRoles(requestedUserName, new string[] { roleName });
                AddUserToRoleTemplate(newUserGuid, templateRoleName);

                var createdTab = CreateTab(newUserGuid, string.Empty, 0, 0);

                if (createdTab != null && createdTab.ID > 0)
                {
                    var userSetting = GetUserSetting(newUserGuid);
                    CreateDefaultWidgetsOnTab(requestedUserName, createdTab.ID);
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

        public bool IsUserAnonymous(string userName)
        {
            var user = this.userRepository.GetUserByUserGuid(this.userRepository.GetUserGuidFromUserName(userName));
            if (user == null)
                throw new ApplicationException("User does not exist:" + userName);
            else
                return user.IsAnonymous;
        }

        #endregion
    }
}
