using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;
using Dropthings.Business.Facade.Test.Helper;
using Xunit;
using SubSpec;
using Dropthings.Model;
using Dropthings.Data;
using Dropthings.Util;
using Dropthings.Web.Framework;

namespace Dropthings.Test.IntegrationTests
{
    public class TestUserVisit
    {
        public TestUserVisit()
        {
            Facade.BootStrap();
        }
        /// <summary>
        /// Ensure the first visit produces the pages and widgets defined in the template user
        /// </summary>
        [Specification]
        public void First_visit_should_create_same_pages_and_widgets_as_the_template_user()
        {
            var profile = default(UserProfile);
            UserSetup userVisitModel = null;
            var facade = default(Facade);
            var anonUserName = default(string);
            var anonPages = default(List<Page>);

           
            "Given anonymous user who has never visited the site before".Context(() => 
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                // Load the anonymous user pages and widgets
                anonUserName = facade.GetUserSettingTemplate().AnonUserSettingTemplate.UserName;
                anonPages = facade.GetPagesOfUser(facade.GetUserGuidFromUserName(anonUserName));

            });

            "when the user visits for the first time".Do(() =>
            {                
                userVisitModel = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "it creates widgets on the newly created page at exact columns and positions as the anon user's pages".Assert(() =>
            {
                anonPages.Each(anonPage =>
                {
                    var userPage = userVisitModel.UserPages.First(page =>
                                    page.Title == anonPage.Title
                                    && page.OrderNo == anonPage.OrderNo
                                    && page.PageType == anonPage.PageType);

                    facade.GetColumnsInPage(anonPage.ID).Each(anonColumn =>
                    {
                        var userColumns = facade.GetColumnsInPage(userPage.ID);
                        var userColumn = userColumns.First(column =>
                                        column.ColumnNo == anonColumn.ColumnNo);

                        var anonColumnWidgets = facade.GetWidgetInstancesInZoneWithWidget(anonColumn.WidgetZone.ID);
                        var userColumnWidgets = facade.GetWidgetInstancesInZoneWithWidget(userColumn.WidgetZone.ID);

                        // Ensure the widgets from the anonymous user template's columns are 
                        // in the same column and row.
                        anonColumnWidgets.Each(anonWidget => Assert.True(userColumnWidgets.Where(userWidget =>
                                userWidget.Title == anonWidget.Title
                                && userWidget.Expanded == anonWidget.Expanded
                                && userWidget.State == anonWidget.State
                                && userWidget.Resized == anonWidget.Resized
                                && userWidget.Height == anonWidget.Height
                                && userWidget.OrderNo == anonWidget.OrderNo).Count() == 1));
                    });
                });

                facade.Dispose();
            });

        }

        [Specification]
        public void Revisit_should_load_the_pages_and_widgets_exactly_the_same()
        {
            var profile = default(UserProfile);

            UserSetup userVisitModel = null;
            UserSetup userRevisitModel = null;

            var facade = default(Facade);

            "Given an anonymous user who visited first".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));
                userVisitModel = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "when the same user visits again".Do(() =>
            {
                userRevisitModel = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "it should load the exact same pages, column and widgets as the first visit produced".Assert(() =>
            {
                userVisitModel.UserPages.Each(firstVisitPage =>
                {
                    Assert.True(userRevisitModel.UserPages.Exists(page => page.ID == firstVisitPage.ID));

                    var revisitPage = userRevisitModel.UserPages.First(page => page.ID == firstVisitPage.ID);
                    var revisitPageColumns = facade.GetColumnsInPage(revisitPage.ID);

                    facade.GetColumnsInPage(firstVisitPage.ID).Each(firstVisitColumn =>
                    {
                        var revisitColumn = revisitPageColumns.First(column => column.ID == firstVisitColumn.ID);

                        var firstVisitWidgets = facade.GetWidgetInstancesInZoneWithWidget(firstVisitColumn.WidgetZone.ID);
                        var revisitWidgets = facade.GetWidgetInstancesInZoneWithWidget(revisitColumn.WidgetZone.ID);

                        firstVisitWidgets.Each(firstVisitWidget =>
                                Assert.True(revisitWidgets.Where(revisitWidget =>
                                        revisitWidget.Id == firstVisitWidget.Id).Count() == 1));
                    });
                });

                facade.Dispose();
            });
        }

        [Specification]
        public void User_can_visit_a_page_directly_and_that_page_becomes_the_default()
        {
            var user = default(UserProfile);
            var facade = default(Facade);
            var userSetup = default(UserSetup);
            var anotherPage = default(Page);

            "Given a user who has more than one tabs".Context(() =>
                {
                    user = MembershipHelper.CreateNewAnonUser();
                    facade = new Facade(new AppContext(user.UserName, user.UserName));
                    userSetup = facade.FirstVisitHomePage(user.UserName, string.Empty, true, false);
                });

            "When the user visits another tab directly".Do(() =>
                {
                    anotherPage = userSetup.UserPages.Where(p => p.ID != userSetup.CurrentPage.ID).FirstOrDefault();
                    if (null == anotherPage)
                    {
                        anotherPage = facade.CreatePage("Test Page", 0);
                        facade.SetCurrentPage(facade.GetUserGuidFromUserName(user.UserName), userSetup.CurrentPage.ID);
                    }

                    facade.RepeatVisitHomePage(user.UserName, anotherPage.UserTabName, true, false);
                });

            "It becomes the default tab".Assert(() =>
                {
                    var revisit = facade.RepeatVisitHomePage(user.UserName, string.Empty, true, false);

                    Assert.Equal(anotherPage.ID, revisit.CurrentPage.ID);
                });
        }
    }
}
