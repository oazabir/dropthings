using System.Web.Security;

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

            // Get the template user so that its page setup can be cloned for new user
            var roleTemplate = GetRoleTemplate(userGuid);

            if (userSettingTemplate.CloneAnonProfileEnabled)
            {
                if (!roleTemplate.TemplateUserId.IsEmpty())
                {
                    // Get template user pages so that it can be cloned for new user
                    var templateUserPages = this.pageRepository.GetPagesOfUser(roleTemplate.TemplateUserId);
                    templateUserPages.Each(templatePage =>
                    { 
                        if(!templatePage.IsLocked)
                        {
                            ClonePage(userGuid, templatePage);
                        }
                    });

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
                    RepeatVisitHomePage(userName, pageTitle, isAnonymous, DateTime.Now);    // non-recursive. this will hit the outter most else block
                }
                else
                {
                    throw new ApplicationException("First page creation failed");
                }
            }

            var currentPages = this.pageRepository.GetPagesOfUser(userGuid);
            response.UserPages = currentPages;
            response.UserSetting = GetUserSetting(userGuid);
            response.CurrentPage = DecideCurrentPage(userGuid, pageTitle, response.UserSetting.CurrentPageId);
            response.CurrentUserId = userGuid;
            response.RoleTemplate = roleTemplate;
            return response;
        }

        public UserSetup RepeatVisitHomePage(string userName, string pageTitle, bool isAnonymous, DateTime? lastVisited)
        {
            // User is visiting again, so load user's existing page setup
            var response = new UserSetup();
            var userGuid = this.userRepository.GetUserGuidFromUserName(userName);

            var roleTemplate = GetRoleTemplate(userGuid);
             response.RoleTemplate = roleTemplate;

            if (!roleTemplate.TemplateUserId.IsEmpty())
            {
                // Get template user pages so that it can be cloned for new user
                if (roleTemplate.TemplateUserId != userGuid)
                {
                    //IILIAS: No more clone. Page will down for maintenence
                    //clone all recently unlock user
                    //var unLockedPages = this.pageRepository.GetUnlockedPagesOfUser(roleTemplate.TemplateUserId);

                    //unLockedPages.ForEach(p =>
                    //{
                    //    if (p.UnlockedAt > lastVisited)
                    //    {
                    //        ClonePage(userGuid, p);
                    //    }
                    //});

                    //bring only the locked pages which are not in maintenence mode
                    response.UserSharedPages = this.pageRepository.GetLockedPagesOfUser(roleTemplate.TemplateUserId, false);
                }
            }
            
            var pages = this.pageRepository.GetPagesOfUser(userGuid);
            //var userSettingTemplate = GetUserSettingTemplate();
            //Membership.GetUser().l
            
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
                response.CurrentUserId = userGuid;
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