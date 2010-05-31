using System.Web.Security;

namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Data;
    using Dropthings.Model;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    partial class Facade
    {
        #region Constants
        private const string DEFAULT_FIRST_PAGE_NAME = "First Tab";
        #endregion

        #region Methods

        public UserSetup FirstVisitHomeTab(string userName, string pageTitle, bool isAnonymous, bool isFirstVisitAfterLogin)
        {
            return AspectF.Define
                //.Transaction()
                .Return<UserSetup>(() =>
            {
                // If user does not exist, then this is the very *FIRST VISIT* of the user and user
                // Get template setting that so that we can create pages from templates
                var response = new UserSetup();
                var userGuid = this.GetUserGuidFromUserName(userName);

                var userSettingTemplate = GetUserSettingTemplate();
                SetUserRoles(userName, new string[] { userSettingTemplate.AnonUserSettingTemplate.RoleNames });

                if (userSettingTemplate.CloneAnonProfileEnabled)
                {
                    // Get the template user so that its page setup can be cloned for new user
                    var templateUserGuid = this.GetUserGuidFromUserName(userSettingTemplate.AnonUserSettingTemplate.UserName);
                    var roleTemplate = GetRoleTemplate(templateUserGuid);

                    if (roleTemplate != default(RoleTemplate))
                    {
                        // Get template user pages so that it can be cloned for new user
                        var templateUserTabs = this.GetTabsOfUser(roleTemplate.AspNetUser.UserId);

                        foreach (Tab templateTab in templateUserTabs)
                        {
                            if (!templateTab.IsLocked)
                            {
                                CloneTab(userGuid, templateTab);
                            }
                        }

                        if (roleTemplate.AspNetUser.UserId != userGuid)
                        {
                            //bring only the locked pages which are not in maintenence mode
                            response.UserSharedTabs = this.pageRepository.GetLockedTabsOfUser(roleTemplate.AspNetUser.UserId, false);
                        }

                        response.RoleTemplate = roleTemplate;
                        response.IsRoleTemplateForRegisterUser = CheckRoleTemplateIsRegisterUserTemplate(roleTemplate);                
                    }
                }
                else
                {
                    // Setup some default pages
                    var page = CreateTab(userGuid, string.Empty, 0, 0);

                    if (page != null && page.ID > 0)
                    {
                        CreateDefaultWidgetsOnTab(userName, page.ID);
                        RepeatVisitHomeTab(userName, pageTitle, isAnonymous, isFirstVisitAfterLogin);    // non-recursive. this will hit the outter most else block
                    }
                    else
                    {
                        throw new ApplicationException("First page creation failed");
                    }
                }

                var currentTabs = this.GetTabsOfUser(userGuid);
                response.UserTabs = currentTabs;
                response.UserSetting = GetUserSetting(userGuid);
                response.CurrentTab = DecideCurrentTab(userGuid, pageTitle, response.UserSetting.CurrentTab.ID, isAnonymous, isFirstVisitAfterLogin);
                response.CurrentUserId = userGuid;
                return response;
            });
        }

        public UserSetup RepeatVisitHomeTab(string userName, string pageTitle, bool isAnonymous, bool isFirstVisitAfterLogin)
        {
            return AspectF.Define
                //.Transaction()
                .Return<UserSetup>(() =>
            {
                // User is visiting again, so load user's existing page setup
                var response = new UserSetup();
                var userGuid = this.GetUserGuidFromUserName(userName);

                var roleTemplate = GetRoleTemplate(userGuid);
                response.RoleTemplate = roleTemplate;
                response.IsRoleTemplateForRegisterUser = CheckRoleTemplateIsRegisterUserTemplate(roleTemplate);

                if (!roleTemplate.AspNetUser.UserId.IsEmpty())
                {
                    // Get template user pages so that it can be cloned for new user
                    if (roleTemplate.AspNetUser.UserId != userGuid)
                    {
                        //bring only the locked pages which are not in maintenence mode
                        response.UserSharedTabs = this.pageRepository.GetLockedTabsOfUser(roleTemplate.AspNetUser.UserId, false);
                    }
                }

                var pages = this.GetTabsOfUser(userGuid);

                if (pages != null && pages.Count() > 0)
                {
                    // User has pages
                    response.UserTabs = pages;

                    var userSetting = GetUserSetting(userGuid);
                    response.CurrentTab = DecideCurrentTab(userGuid, pageTitle, userSetting.CurrentTab.ID, isAnonymous, isFirstVisitAfterLogin);

                    if (userSetting.CurrentTab.ID != response.CurrentTab.ID)
                    {
                        //userSetting.CurrentTab.ID = response.CurrentTab.ID;
                        //this.userSettingRepository.Update(userSetting);
                        SetCurrentTab(userGuid, response.CurrentTab.ID);
                    }

                    response.UserSetting = GetUserSetting(userGuid);
                    response.CurrentUserId = userGuid;
                }
                else
                {
                    // User has no pages
                    response = FirstVisitHomeTab(userName, pageTitle, isAnonymous, isFirstVisitAfterLogin);
                }

                return response;
            });
        }

        #endregion Methods
    }
}