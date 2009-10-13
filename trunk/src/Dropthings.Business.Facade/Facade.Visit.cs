using System.Web.Security;

namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;
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
            // If user does not exist, then this is the very *FIRST VISIT* of the user and user
            // Get template setting that so that we can create pages from templates
            var response = new UserSetup();
            var userGuid = this.GetUserGuidFromUserName(userName);

            var userSettingTemplate = GetUserSettingTemplate();
            SetUserRoles(userName, userSettingTemplate.AnonUserSettingTemplate.RoleNames);

            // Get the template user so that its page setup can be cloned for new user
            var roleTemplate = GetRoleTemplate(userGuid);

            if (userSettingTemplate.CloneAnonProfileEnabled)
            {
                if (!roleTemplate.TemplateUserId.IsEmpty())
                {
                    // Get template user pages so that it can be cloned for new user
                    var templateUserPages = this.GetPagesOfUser(roleTemplate.TemplateUserId);

                    foreach (Page templatePage in templateUserPages)
                    { 
                        if(!templatePage.IsLocked)
                        {
                            ClonePage(userGuid, templatePage);
                        }
                    }

                    if (roleTemplate.TemplateUserId != userGuid)
                    {
                        //bring only the locked pages which are not in maintenence mode
                        response.UserSharedPages = this.pageRepository.GetLockedPagesOfUser(roleTemplate.TemplateUserId, false);
                    }
                }
            }
            else
            {
                // Setup some default pages
                var page = CreatePage(userGuid, string.Empty, null, 0);

                if (page != null && page.ID > 0)
                {
                    CreateDefaultWidgetsOnPage(userName, page.ID);
                    RepeatVisitHomePage(userName, pageTitle, isAnonymous, DateTime.Now, isFirstVisitAfterLogin);    // non-recursive. this will hit the outter most else block
                }
                else
                {
                    throw new ApplicationException("First page creation failed");
                }
            }

            var currentPages = this.GetPagesOfUser(userGuid);
            response.UserPages = currentPages;
            response.UserSetting = GetUserSetting(userGuid);
            response.CurrentPage = DecideCurrentPage(userGuid, pageTitle, response.UserSetting.CurrentPageId, isAnonymous, isFirstVisitAfterLogin);
            response.CurrentUserId = userGuid;
            response.RoleTemplate = roleTemplate;
            response.IsRoleTemplateForRegisterUser = CheckRoleTemplateIsRegisterUserTemplate(roleTemplate);
            return response;
        }

        public UserSetup RepeatVisitHomePage(string userName, string pageTitle, bool isAnonymous, DateTime? lastVisited, bool isFirstVisitAfterLogin)
        {
            // User is visiting again, so load user's existing page setup
            var response = new UserSetup();
            var userGuid = this.GetUserGuidFromUserName(userName);

            var roleTemplate = GetRoleTemplate(userGuid);
            response.RoleTemplate = roleTemplate;
            response.IsRoleTemplateForRegisterUser = CheckRoleTemplateIsRegisterUserTemplate(roleTemplate);

            if (!roleTemplate.TemplateUserId.IsEmpty())
            {
                // Get template user pages so that it can be cloned for new user
                if (roleTemplate.TemplateUserId != userGuid)
                {
                    //bring only the locked pages which are not in maintenence mode
                    response.UserSharedPages = this.pageRepository.GetLockedPagesOfUser(roleTemplate.TemplateUserId, false);
                }
            }

            var pages = this.GetPagesOfUser(userGuid);
            
            if (pages != null && pages.Count() > 0)
            {
                // User has pages
                response.UserPages = pages;

                var userSetting = GetUserSetting(userGuid);
                response.CurrentPage = DecideCurrentPage(userGuid, pageTitle, userSetting.CurrentPageId, isAnonymous, isFirstVisitAfterLogin);

                if (userSetting.CurrentPageId != response.CurrentPage.ID)
                {
                    this.userSettingRepository.Update(userSetting, (setting) =>
                    {
                        setting.CurrentPageId = response.CurrentPage.ID;
                    }, null);
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
        }

        #endregion Methods
    }
}