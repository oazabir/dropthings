using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSpec;
using Dropthings.Web.Framework;
using Dropthings.Business.Facade.Test.Helper;
using Dropthings.Business.Facade.Context;
using Xunit;
using Dropthings.Model;

namespace Dropthings.Business.Facade.Tests
{
    public class TestPageStuff
    {
        public TestPageStuff()
        {
            Facade.BootStrap();
        }
        
        [Specification]
        public void User_should_be_able_to_add_new_page()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);

            "Given a new user".Context(() =>
                {
                    profile = MembershipHelper.CreateNewAnonUser();
                    facade = new Facade(new AppContext(string.Empty, profile.UserName));

                    facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
                });

            "When user adds a new page".Do(() =>
                {
                    var newPage = facade.CreatePage("Some New Page", 0);
                });

            "It should add a new blank page as current page".Assert(() =>
                {
                    var userPageSetup = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);
                    Assert.Equal("Some New Page", userPageSetup.CurrentPage.Title);
                });
        }

        [Specification]
        public void User_should_be_able_change_page_title()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);
            var userSetup = default(UserSetup);
            var newName = Guid.NewGuid().ToString();
            "Given a new user".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                userSetup = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "When user changes title of current page".Do(() =>
            {
                facade.ChangePageName(newName);
            });

            "It should persist and on next visit the new page title should reflect".Assert(() =>
            {
                var userPageSetup = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);
                Assert.Equal(newName, userPageSetup.CurrentPage.Title);
            });
        }

        [Specification]
        public void User_should_be_able_delete_a_page()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);
            var userSetup = default(UserSetup);
            var deletedPageId = default(int);

            "Given a new user".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                userSetup = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "When user deletes the current page".Do(() =>
            {
                facade.DeletePage(userSetup.CurrentPage.ID);
            });

            "It should not appear on next visit".Assert(() =>
            {
                var userPageSetup = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);
                Assert.DoesNotContain<int>(deletedPageId, 
                    facade.GetPagesOfUser(userPageSetup.CurrentUserId).Select(p => p.ID));
            });
        }
    }
}
