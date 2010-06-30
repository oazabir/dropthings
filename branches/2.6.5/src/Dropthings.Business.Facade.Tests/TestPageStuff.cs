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
using Dropthings.Data;

namespace Dropthings.Business.Facade.Tests
{
    public class TestTabStuff
    {
        public TestTabStuff()
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

                    facade.FirstVisitHomeTab(profile.UserName, string.Empty, true, false);
                });

            "When user adds a new page".Do(() =>
                {
                    var newTab = facade.CreateTab("Some New Tab", 0);
                });

            "It should add a new blank page as current page".Assert(() =>
                {
                    var userTabSetup = facade.RepeatVisitHomeTab(profile.UserName, string.Empty, true, false);
                    Assert.Equal("Some New Tab", userTabSetup.CurrentTab.Title);
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

                userSetup = facade.FirstVisitHomeTab(profile.UserName, string.Empty, true, false);
            });

            "When user changes title of current page".Do(() =>
            {
                facade.ChangeTabName(newName);
            });

            "It should persist and on next visit the new page title should reflect".Assert(() =>
            {
                var userTabSetup = facade.RepeatVisitHomeTab(profile.UserName, string.Empty, true, false);
                Assert.Equal(newName, userTabSetup.CurrentTab.Title);
            });
        }

        [Specification]
        public void User_should_be_able_set_the_same_page_title_again()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);
            var userSetup = default(UserSetup);
            "Given a new user".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                userSetup = facade.FirstVisitHomeTab(profile.UserName, string.Empty, true, false);
            });

            "When user changes title of current page".Do(() =>
            {
                facade.ChangeTabName(userSetup.CurrentTab.Title);
            });

            "It should persist and on next visit the page title should remain the same".Assert(() =>
            {
                var userTabSetup = facade.RepeatVisitHomeTab(profile.UserName, string.Empty, true, false);
                Assert.Equal(userTabSetup.CurrentTab.Title, userTabSetup.CurrentTab.Title);
            });
        }

        [Specification]
        public void User_should_not_be_able_to_use_same_page_title_in_more_than_one_page()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);
            var userSetup = default(UserSetup);
            var newName = Guid.NewGuid().ToString();
            var newTitle = default(string);
            "Given a new user".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                userSetup = facade.FirstVisitHomeTab(profile.UserName, string.Empty, true, false);
            });

            "When user changes title of one page to the title of another page".Do(() =>
            {
                // Create a new page
                var newTab = facade.CreateTab(Guid.NewGuid().ToString(), 0);
                newTitle = newTab.Title;
                // Change back to the old page
                facade.SetCurrentTab(facade.GetUserGuidFromUserName(profile.UserName), userSetup.CurrentTab.ID);
                // Set the same title as the newly created page
                facade.ChangeTabName(newTitle);
            });

            "It should automatically change the title to some unique title by adding some number".Assert(() =>
            {
                var userTabSetup = facade.RepeatVisitHomeTab(profile.UserName, string.Empty, true, false);
                Assert.NotEqual(newTitle, userTabSetup.CurrentTab.Title);
                
                // Ensure there's no two pages with same title
                Assert.False(userTabSetup.UserTabs.Exists(p1 => userTabSetup.UserTabs.Exists(p2 => p1.Title == p2.Title 
                    && p1.ID != p2.ID)));
            });
        }


        [Specification]
        public void User_should_be_able_delete_a_page()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);
            var userSetup = default(UserSetup);
            var deletedTabId = default(int);

            "Given a new user".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                userSetup = facade.FirstVisitHomeTab(profile.UserName, string.Empty, true, false);
            });

            "When user deletes the current page".Do(() =>
            {
                facade.DeleteTab(userSetup.CurrentTab.ID);
            });

            "It should not appear on next visit".Assert(() =>
            {
                var userTabSetup = facade.RepeatVisitHomeTab(profile.UserName, string.Empty, true, false);
                Assert.DoesNotContain<int>(deletedTabId, 
                    facade.GetTabsOfUser(userTabSetup.CurrentUserId).Select(p => p.ID));
            });
        }

        [Specification]
        public void User_changes_3_column_view_to_2_column_view()
        {
            var facade = default(Facade);
            var user = default(UserProfile);
            var userSetup = default(UserSetup);
            var widgets = default(List<Widget>);

            var threeColumnLayoutNo = 0;
            var twoColumnLayoutNo = 2;

            var widgetMap = new Dictionary<int, List<int>>();

            "Given a page with 3 columns and widgets on all 3 columns".Context(() =>
                {
                    user = MembershipHelper.CreateNewAnonUser();
                    facade = new Facade(new AppContext(user.UserName, user.UserName));

                    userSetup = facade.FirstVisitHomeTab(user.UserName, string.Empty, true, false);
                    facade.CreateTab("Test Tab for Widget Delete", threeColumnLayoutNo);

                    // Add all the widgets on each column
                    widgets = facade.GetWidgetList(user.UserName, Data.Enumerations.WidgetType.PersonalTab);
                    for (int columnNo = 0; columnNo < 3; columnNo++)
                    {
                        widgetMap[columnNo] = new List<int>();
                        for (int i = 0; i < widgets.Count; i++)
                        {
                            var wi = facade.AddWidgetInstance(widgets[i].ID, i, columnNo, 0);
                            widgetMap[columnNo].Add(wi.Id);
                        }
                    }                    
                });

            "When the page is switched to 2 column mode".Do(() =>
                {
                    facade.ModifyTabLayout(twoColumnLayoutNo);                    
                });

            "It should move all the widgets on 3rd column to 2nd column after the existing 2nd column widgets".Assert(() =>
                {
                    userSetup = facade.RepeatVisitHomeTab(user.UserName, string.Empty, true, false);
                    Assert.Equal(2, userSetup.CurrentTab.ColumnCount);

                    var columns = facade.GetColumnsInTab(userSetup.CurrentTab.ID);
                    Assert.Equal(2, columns.Count);

                    var firstColumn = columns[0];
                    var secondColumn = columns[1];

                    // Ensure column widgets are all correct                    

                    // Ensure first column has the same number of widgets as it was created
                    var firstColumnWidgets = facade.GetWidgetInstancesInZoneWithWidget(firstColumn.WidgetZone.ID);
                    Assert.Equal(widgetMap[firstColumn.ColumnNo].Count, firstColumnWidgets.Count());
                    firstColumnWidgets.Each(wi => Assert.True(widgetMap[firstColumn.ColumnNo].Contains(wi.Id)));

                    // Ensure second column has both second and third column widgets
                    var secondColumnWidgets = facade.GetWidgetInstancesInZoneWithWidget(secondColumn.WidgetZone.ID);
                    Assert.Equal(widgetMap[secondColumn.ColumnNo].Count + widgetMap[2].Count,
                        secondColumnWidgets.Count());

                    var secondColumnIdEnumerator = widgetMap[secondColumn.ColumnNo].GetEnumerator();
                    var secondColumnWidgetEnumerator = secondColumnWidgets.GetEnumerator();

                    // Ensure the second column's original widgets are in the exact same position
                    while (secondColumnIdEnumerator.MoveNext())
                    {
                        secondColumnWidgetEnumerator.MoveNext();
                        Assert.Equal(secondColumnIdEnumerator.Current, secondColumnWidgetEnumerator.Current.Id);
                    }

                    // Ensure the third column widgets are added after the second column widgets
                    var thirdColumnIdEnumerator = widgetMap[2].GetEnumerator();
                    while (thirdColumnIdEnumerator.MoveNext())
                    {
                        secondColumnWidgetEnumerator.MoveNext();
                        Assert.Equal(thirdColumnIdEnumerator.Current, secondColumnWidgetEnumerator.Current.Id);
                    }
                });

        }

        [Specification]
        public void User_changes_3_column_view_to_1_column_view()
        {
            var facade = default(Facade);
            var user = default(UserProfile);
            var userSetup = default(UserSetup);
            var widgets = default(List<Widget>);

            var threeColumnLayoutNo = 0;
            var oneColumnLayoutNo = 4;

            var widgetMap = new Dictionary<int, List<int>>();

            "Given a page with 3 columns and widgets on all 3 columns".Context(() =>
            {
                user = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(user.UserName, user.UserName));

                userSetup = facade.FirstVisitHomeTab(user.UserName, string.Empty, true, false);
                facade.CreateTab("Test Tab for Widget Delete", threeColumnLayoutNo);

                // Add all the widgets on each column
                widgets = facade.GetWidgetList(user.UserName, Data.Enumerations.WidgetType.PersonalTab);
                for (int columnNo = 0; columnNo < 3; columnNo++)
                {
                    widgetMap[columnNo] = new List<int>();
                    for (int i = 0; i < widgets.Count; i++)
                    {
                        var wi = facade.AddWidgetInstance(widgets[i].ID, i, columnNo, 0);
                        widgetMap[columnNo].Add(wi.Id);
                    }
                }
            });

            "When the page is switched to 2 column mode".Do(() =>
            {
                facade.ModifyTabLayout(oneColumnLayoutNo);
            });

            "It should move all the widgets on 3rd column to 2nd column after the existing 2nd column widgets".Assert(() =>
            {
                userSetup = facade.RepeatVisitHomeTab(user.UserName, string.Empty, true, false);
                Assert.Equal(1, userSetup.CurrentTab.ColumnCount);

                var columns = facade.GetColumnsInTab(userSetup.CurrentTab.ID);
                Assert.Equal(1, columns.Count);

                var firstColumn = columns[0];
                
                // Ensure first column has the same number of widgets as it was created
                var firstColumnWidgets = facade.GetWidgetInstancesInZoneWithWidget(firstColumn.WidgetZone.ID);
                Assert.Equal(widgetMap[0].Count + widgetMap[1].Count + widgetMap[2].Count,
                    firstColumnWidgets.Count());

                var idEnumerator = Enumerable.Concat(
                    Enumerable.Concat(widgetMap[0].AsEnumerable(), widgetMap[1].AsEnumerable()),
                    widgetMap[2].AsEnumerable()).GetEnumerator();

                var firstColumnWidgetEnumerator = firstColumnWidgets.GetEnumerator();
                while (idEnumerator.MoveNext())
                {
                    firstColumnWidgetEnumerator.MoveNext();
                    Assert.Equal(idEnumerator.Current, firstColumnWidgetEnumerator.Current.Id);
                }
            });

        }

        [Specification]
        public void User_can_change_page_layout_from_1_to_2()
        {
            var user = default(UserProfile);
            var facade = default(Facade);
            var userSetup = default(UserSetup);

            var oneColumnLayoutNo = 4;
            var twoColumnLayoutNo = 2;

            "Given a user with a one column page layout".Context(() =>
                {
                    user = MembershipHelper.CreateNewAnonUser();
                    facade = new Facade(new AppContext(user.UserName, user.UserName));

                    userSetup = facade.FirstVisitHomeTab(user.UserName, string.Empty, true, false);
                    facade.CreateTab("Test Tab 1 column", oneColumnLayoutNo);
                });

            "When user changes the page layout to two columns".Do(() =>
                {
                    facade.ModifyTabLayout(twoColumnLayoutNo);
                });

            "It should introduce a blank column after the two columns".Assert(() =>
                {
                    userSetup = facade.RepeatVisitHomeTab(user.UserName, string.Empty, true, false);
                    var columns = facade.GetColumnsInTab(userSetup.CurrentTab.ID);
                    Assert.Equal(2, columns.Count);

                    var secondColumn = columns[1];
                    var widgetsOnSecondColumn = facade.GetWidgetInstancesInZoneWithWidget(secondColumn.WidgetZone.ID);
                    Assert.Equal(0, widgetsOnSecondColumn.Count());
                });
        }
    }
}
