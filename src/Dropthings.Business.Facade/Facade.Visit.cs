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
        private const string DEFAULT_FIRST_PAGE_NAME = "First Page";
        #endregion

        #region Methods

        public UserSetup FirstVisitHomePage(string userName, string pageTitle, bool isAnonymous, bool isFirstVisitAfterLogin)
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
                        var templateUserPages = this.GetPagesOfUser(roleTemplate.aspnet_Users.UserId);

                        foreach (Page templatePage in templateUserPages)
                        {
                            if (!templatePage.IsLocked)
                            {
                                ClonePage(userGuid, templatePage);
                            }
                        }

                        if (roleTemplate.aspnet_Users.UserId != userGuid)
                        {
                            //bring only the locked pages which are not in maintenence mode
                            response.UserSharedPages = this.pageRepository.GetLockedPagesOfUser(roleTemplate.aspnet_Users.UserId, false);
                        }

                        response.RoleTemplate = roleTemplate;
                        response.IsRoleTemplateForRegisterUser = CheckRoleTemplateIsRegisterUserTemplate(roleTemplate);                
                    }
                }
                else
                {
                    // Setup some default pages
                    var page = CreatePage(userGuid, string.Empty, 0, 0);

                    if (page != null && page.ID > 0)
                    {
                        CreateDefaultWidgetsOnPage(userName, page.ID);
                        RepeatVisitHomePage(userName, pageTitle, isAnonymous, isFirstVisitAfterLogin);    // non-recursive. this will hit the outter most else block
                    }
                    else
                    {
                        throw new ApplicationException("First page creation failed");
                    }
                }

                var currentPages = this.GetPagesOfUser(userGuid);
                response.UserPages = currentPages;
                response.UserSetting = GetUserSetting(userGuid);
                response.CurrentPage = DecideCurrentPage(userGuid, pageTitle, response.UserSetting.Page.ID, isAnonymous, isFirstVisitAfterLogin);
                response.CurrentUserId = userGuid;
                return response;
            });
        }

        public UserSetup RepeatVisitHomePage(string userName, string pageTitle, bool isAnonymous, bool isFirstVisitAfterLogin)
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

                if (!roleTemplate.aspnet_Users.UserId.IsEmpty())
                {
                    // Get template user pages so that it can be cloned for new user
                    if (roleTemplate.aspnet_Users.UserId != userGuid)
                    {
                        //bring only the locked pages which are not in maintenence mode
                        response.UserSharedPages = this.pageRepository.GetLockedPagesOfUser(roleTemplate.aspnet_Users.UserId, false);
                    }
                }

                var pages = this.GetPagesOfUser(userGuid);

                if (pages != null && pages.Count() > 0)
                {
                    // User has pages
                    response.UserPages = pages;

                    var userSetting = GetUserSetting(userGuid);
                    response.CurrentPage = DecideCurrentPage(userGuid, pageTitle, userSetting.Page.ID, isAnonymous, isFirstVisitAfterLogin);

                    if (userSetting.Page.ID != response.CurrentPage.ID)
                    {
                        userSetting.Page.ID = response.CurrentPage.ID;
                        this.userSettingRepository.Update(userSetting);
                    }

                    response.UserSetting = GetUserSetting(userGuid);
                    response.CurrentUserId = userGuid;
                }
                else
                {
                    // User has no pages
                    response = FirstVisitHomePage(userName, pageTitle, isAnonymous, isFirstVisitAfterLogin);
                }

                return response;
            });
        }

        #endregion Methods
    }
}