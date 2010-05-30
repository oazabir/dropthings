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
        public void User_should_be_able_set_the_same_page_title_again()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);
            var userSetup = default(UserSetup);
            "Given a new user".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                userSetup = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "When user changes title of current page".Do(() =>
            {
                facade.ChangePageName(userSetup.CurrentPage.Title);
            });

            "It should persist and on next visit the page title should remain the same".Assert(() =>
            {
                var userPageSetup = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);
                Assert.Equal(userPageSetup.CurrentPage.Title, userPageSetup.CurrentPage.Title);
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

                userSetup = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "When user changes title of one page to the title of another page".Do(() =>
            {
                // Create a new page
                var newPage = facade.CreatePage(Guid.NewGuid().ToString(), 0);
                newTitle = newPage.Title;
                // Change back to the old page
                facade.SetCurrentPage(facade.GetUserGuidFromUserName(profile.UserName), userSetup.CurrentPage.ID);
                // Set the same title as the newly created page
                facade.ChangePageName(newTitle);
            });

            "It should automatically change the title to some unique title by adding some number".Assert(() =>
            {
                var userPageSetup = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);
                Assert.NotEqual(newTitle, userPageSetup.CurrentPage.Title);
                
                // Ensure there's no two pages with same title
                Assert.False(userPageSetup.UserPages.Exists(p1 => userPageSetup.UserPages.Exists(p2 => p1.Title == p2.Title 
                    && p1.ID != p2.ID)));
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

                    userSetup = facade.FirstVisitHomePage(user.UserName, string.Empty, true, false);
                    facade.CreatePage("Test Page for Widget Delete", threeColumnLayoutNo);

                    // Add all the widgets on each column
                    widgets = facade.GetWidgetList(user.UserName, Data.Enumerations.WidgetTypeEnum.PersonalPage);
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
                    facade.ModifyPageLayout(twoColumnLayoutNo);                    
                });

            "It should move all the widgets on 3rd column to 2nd column after the existing 2nd column widgets".Assert(() =>
                {
                    userSetup = facade.RepeatVisitHomePage(user.UserName, string.Empty, true, false);
                    Assert.Equal(2, userSetup.CurrentPage.ColumnCount);

                    var columns = facade.GetColumnsInPage(userSetup.CurrentPage.ID);
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

                userSetup = facade.FirstVisitHomePage(user.UserName, string.Empty, true, false);
                facade.CreatePage("Test Page for Widget Delete", threeColumnLayoutNo);

                // Add all the widgets on each column
                widgets = facade.GetWidgetList(user.UserName, Data.Enumerations.WidgetTypeEnum.PersonalPage);
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
                facade.ModifyPageLayout(oneColumnLayoutNo);
            });

            "It should move all the widgets on 3rd column to 2nd column after the existing 2nd column widgets".Assert(() =>
            {
                userSetup = facade.RepeatVisitHomePage(user.UserName, string.Empty, true, false);
                Assert.Equal(1, userSetup.CurrentPage.ColumnCount);

                var columns = facade.GetColumnsInPage(userSetup.CurrentPage.ID);
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

                    userSetup = facade.FirstVisitHomePage(user.UserName, string.Empty, true, false);
                    facade.CreatePage("Test Page 1 column", oneColumnLayoutNo);
                });

            "When user changes the page layout to two columns".Do(() =>
                {
                    facade.ModifyPageLayout(twoColumnLayoutNo);
                });

            "It should introduce a blank column after the two columns".Assert(() =>
                {
                    userSetup = facade.RepeatVisitHomePage(user.UserName, string.Empty, true, false);
                    var columns = facade.GetColumnsInPage(userSetup.CurrentPage.ID);
                    Assert.Equal(2, columns.Count);

                    var secondColumn = columns[1];
                    var widgetsOnSecondColumn = facade.GetWidgetInstancesInZoneWithWidget(secondColumn.WidgetZone.ID);
                    Assert.Equal(0, widgetsOnSecondColumn.Count());
                });
        }
    }
}
