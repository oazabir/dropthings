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
            var anonTabs = default(List<Tab>);

           
            "Given anonymous user who has never visited the site before".Context(() => 
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                // Load the anonymous user pages and widgets
                anonUserName = facade.GetUserSettingTemplate().AnonUserSettingTemplate.UserName;
                anonTabs = facade.GetTabsOfUser(facade.GetUserGuidFromUserName(anonUserName));

            });

            "When the user visits for the first time".Do(() =>
            {                
                userVisitModel = facade.FirstVisitHomeTab(profile.UserName, string.Empty, true, false);
            });

            "It creates widgets on the newly created page at exact columns and positions as the anon user's pages".Assert(() =>
            {
                anonTabs.Each(anonTab =>
                {
                    var userTab = userVisitModel.UserTabs.First(page =>
                                    page.Title == anonTab.Title
                                    && page.OrderNo == anonTab.OrderNo
                                    && page.PageType == anonTab.PageType);

                    facade.GetColumnsInTab(anonTab.ID).Each(anonColumn =>
                    {
                        var userColumns = facade.GetColumnsInTab(userTab.ID);
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
                userVisitModel = facade.FirstVisitHomeTab(profile.UserName, string.Empty, true, false);
            });

            "When the same user visits again".Do(() =>
            {
                userRevisitModel = facade.RepeatVisitHomeTab(profile.UserName, string.Empty, true, false);
            });

            "It should load the exact same pages, column and widgets as the first visit produced".Assert(() =>
            {
                userVisitModel.UserTabs.Each(firstVisitTab =>
                {
                    Assert.True(userRevisitModel.UserTabs.Exists(page => page.ID == firstVisitTab.ID));

                    var revisitTab = userRevisitModel.UserTabs.First(page => page.ID == firstVisitTab.ID);
                    var revisitTabColumns = facade.GetColumnsInTab(revisitTab.ID);

                    facade.GetColumnsInTab(firstVisitTab.ID).Each(firstVisitColumn =>
                    {
                        var revisitColumn = revisitTabColumns.First(column => column.ID == firstVisitColumn.ID);

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
            var anotherTab = default(Tab);

            "Given a user who has more than one tabs".Context(() =>
                {
                    user = MembershipHelper.CreateNewAnonUser();
                    facade = new Facade(new AppContext(user.UserName, user.UserName));
                    userSetup = facade.FirstVisitHomeTab(user.UserName, string.Empty, true, false);
                });

            "When the user visits another tab directly".Do(() =>
                {
                    anotherTab = userSetup.UserTabs.Where(p => p.ID != userSetup.CurrentTab.ID).FirstOrDefault();
                    if (null == anotherTab)
                    {
                        anotherTab = facade.CreateTab("Test Tab", 0);
                        facade.SetCurrentTab(facade.GetUserGuidFromUserName(user.UserName), userSetup.CurrentTab.ID);
                    }

                    facade.RepeatVisitHomeTab(user.UserName, anotherTab.UserTabName, true, false);
                });

            "It becomes the default tab".Assert(() =>
                {
                    var revisit = facade.RepeatVisitHomeTab(user.UserName, string.Empty, true, false);

                    Assert.Equal(anotherTab.ID, revisit.CurrentTab.ID);
                });
        }
    }
}
