#define REG_USER_TEMPLATE_ON

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSpec;
using Dropthings.Web.Framework;
using Dropthings.Business.Facade.Test.Helper;
using Xunit;
using Dropthings.Data;


namespace Dropthings.Business.Facade.Tests
{
    public class TestSignup
    {
        public TestSignup()
        {
            Facade.BootStrap();
        }

#if REG_USER_TEMPLATE_ON

        [Specification]
        public void User_can_signup_and_have_registered_user_template_tabs()
        {
            var facade = default(Facade);
            var anonUserProfile = default(UserProfile);
            var newlyAddedWidgetId = default(int);
            var registeredUserName = default(string);
            var registeredUserPassword = default(string);

            var regUserName = default(string);
            var regUserTabs = default(List<Tab>);

            "Given registered user template is on and an anonymous user who has made some changes to the page".Context(() =>
                {
                    anonUserProfile = MembershipHelper.CreateNewAnonUser();
                    facade = new Facade(new Context.AppContext(anonUserProfile.UserName, anonUserProfile.UserName));

                    var userSetup = facade.FirstVisitHomeTab(anonUserProfile.UserName, string.Empty, true, false);
                    var newWidgetToAdd = facade.GetWidgetList(anonUserProfile.UserName, Data.Enumerations.WidgetType.PersonalTab).First();
                    var newlyAddedWidget = facade.AddWidgetInstance(newWidgetToAdd.ID, 0, 0, 0);
                    newlyAddedWidgetId = newlyAddedWidget.Id;

                    // Load the anonymous user pages and widgets
                    regUserName = facade.GetUserSettingTemplate().RegisteredUserSettingTemplate.UserName;
                    regUserTabs = facade.GetTabsOfUser(facade.GetUserGuidFromUserName(regUserName));
                });

            "When user signs up".Do(() =>
                {
                    registeredUserName = Guid.NewGuid().ToString();
                    registeredUserPassword = Guid.NewGuid().ToString();
                    facade.RegisterUser(registeredUserName, registeredUserPassword, registeredUserName, false);
                });

            "It should replace the users tabs with the tabs and widgets of the registered user template".Assert(() =>
                {
                    var revisitModel = facade.RepeatVisitHomeTab(registeredUserName, string.Empty, false, false);
                    regUserTabs.Each(regTab =>
                    {
                        var userTab = revisitModel.UserTabs.First(page =>
                                        page.Title == regTab.Title
                                        && page.OrderNo == regTab.OrderNo
                                        && page.PageType == regTab.PageType);

                        facade.GetColumnsInTab(regTab.ID).Each(regColumn =>
                        {
                            var userColumns = facade.GetColumnsInTab(userTab.ID);
                            var userColumn = userColumns.First(column =>
                                            column.ColumnNo == regColumn.ColumnNo);

                            var anonColumnWidgets = facade.GetWidgetInstancesInZoneWithWidget(regColumn.WidgetZone.ID);
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

                });

            "It should allow user to login to access the pages".Assert(() =>
                {
                    Assert.True(facade.Login(registeredUserName, registeredUserPassword));
                });
        }

#else
        [Specification]
        public void User_can_signup_to_save_changes_made_to_anon_pages()
        {
            var facade = default(Facade);
            var anonUserProfile = default(UserProfile);
            var newlyAddedWidgetId = default(int);
            var registeredUserName = default(string);
            var registeredUserPassword = default(string);

            "Given registered user template is off and an anonymous user who has made some changes to the page".Context(() =>
                {
                    anonUserProfile = MembershipHelper.CreateNewAnonUser();
                    facade = new Facade(new Context.AppContext(anonUserProfile.UserName, anonUserProfile.UserName));

                    var userSetup = facade.FirstVisitHomeTab(anonUserProfile.UserName, string.Empty, true, false);
                    var newWidgetToAdd = facade.GetWidgetList(anonUserProfile.UserName, Data.Enumerations.WidgetType.PersonalTab).First();
                    var newlyAddedWidget = facade.AddWidgetInstance(newWidgetToAdd.ID, 0, 0, 0);
                    newlyAddedWidgetId = newlyAddedWidget.Id;
                });

            "When user signs up".Do(() =>
                {
                    registeredUserName = Guid.NewGuid().ToString();
                    registeredUserPassword = Guid.NewGuid().ToString();
                    facade.RegisterUser(registeredUserName, registeredUserPassword, registeredUserName, false);
                });

            "It should preserve the tabs and changes made to the tabs upon revisit".Assert(() =>
                {
                    var revisitModel = facade.RepeatVisitHomeTab(registeredUserName, string.Empty, false, false);
                    var firstColumnWidgetZone = facade.GetColumnsInTab(revisitModel.CurrentTab.ID).First().WidgetZone;
                    var widgets = facade.GetWidgetInstancesInZoneWithWidget(firstColumnWidgetZone.ID);

                    Assert.NotNull(widgets.First(w => w.Id == newlyAddedWidgetId));
                });

            "It should allow user to login to access the pages".Assert(() =>
                {
                    Assert.True(facade.Login(registeredUserName, registeredUserPassword));
                });
        }

#endif
    }
}
