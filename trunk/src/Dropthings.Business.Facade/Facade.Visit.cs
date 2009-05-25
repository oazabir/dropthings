namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Business.Activities;
    using Dropthings.Business.Activities.UserAccountActivities;
    using Dropthings.DataAccess;
    using Dropthings.Model;

    partial class Facade
    {
        #region Constants
        private const string DEFAULT_FIRST_PAGE_NAME = "First Page";
        #endregion

        #region Methods

        public UserSetup FirstVisitHomePage(string userName, string pageTitle, bool isAnonymous)
        {
            // If user does not exist, then this is the very *FIRST VISIT* of the user and user
            // Get template setting that so that we can create pages from templates

            var response = new UserSetup();
            var userGuid = this.userRepository.GetUserGuidFromUserName(userName);

            var userSettingTemplate = GetUserSettingTemplate();
            SetUserRoles(userName, userSettingTemplate.AnonUserSettingTemplate.RoleNames);

            if (userSettingTemplate.CloneAnonProfileEnabled)
            {
                // Get the template user so that its page setup can be cloned for new user
                var roleTemplate = GetRoleTemplate(userGuid);

                if (!roleTemplate.TemplateUserId.IsEmpty())
                {
                    // Get template user pages so that it can be cloned for new user
                    var templateUserPages = this.pageRepository.GetPagesOfUser(roleTemplate.TemplateUserId);
                    templateUserPages.Each(templatePage => ClonePage(userGuid, templatePage));
                }
            }
            else
            {
                // Setup some default pages
                var page = CreatePage(userGuid, string.Empty, null);

                if (page == null)
                    throw new ApplicationException("First page creation failed");
                else
                {
                    CreateDefaultWidgetsOnPage(userName, page.ID);
                    RepeatVisitHomePage(userName, pageTitle, isAnonymous);    // non-recursive. this will hit the outter most else block
                }
            }

            var currentPages = this.pageRepository.GetPagesOfUser(userGuid);
            response.UserPages = currentPages;
            response.UserSetting = GetUserSetting(userGuid);
            response.CurrentPage = DecideCurrentPage(userGuid, pageTitle, response.UserSetting.CurrentPageId);

            return response;
        }

        public UserSetup RepeatVisitHomePage(string userName, string pageTitle, bool isAnonymous)
        {
            // User is visiting again, so load user's existing page setup
            var response = new UserSetup();            
            var userGuid = this.userRepository.GetUserGuidFromUserName(userName);
            var pages = this.pageRepository.GetPagesOfUser(userGuid);

            if (pages != null && pages.Count > 0)
            {
                // User has pages
                response.UserPages = pages;

                var userSetting = GetUserSetting(userGuid);
                response.CurrentPage = DecideCurrentPage(userGuid, pageTitle, userSetting.CurrentPageId);

                if (userSetting.CurrentPageId != response.CurrentPage.ID)
                {
                    this.userSettingRepository.Update(userSetting, (setting) =>
                    {
                        setting.CurrentPageId = response.CurrentPage.ID;
                    }, null);
                }

                response.UserSetting = GetUserSetting(userGuid);
            }
            else
            {
                // User has no pages
                response = FirstVisitHomePage(userName, pageTitle, isAnonymous);
            }

            return response;
        }

        #endregion Methods
    }
}