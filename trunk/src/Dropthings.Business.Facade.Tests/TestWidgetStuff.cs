using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSpec;
using Dropthings.Model;
using Dropthings.Business.Facade.Test.Helper;
using Dropthings.Business.Facade.Context;
using Dropthings.Web.Framework;
using Dropthings.Data;
using Xunit;

namespace Dropthings.Business.Facade.Tests
{
    public class TestWidgetStuff
    {
        public TestWidgetStuff()
        {
            Facade.BootStrap();
        }

        [Specification]
        public void Widget_should_be_able_to_move_to_another_column()
        {
            var profile = default(UserProfile);
            UserSetup userVisitModel = default(UserSetup);
            var facade = default(Facade);
            var widgetInstance = default(WidgetInstance);
            var userColumns = default(List<Column>);
            var secondColumn = default(Column);
            var noOfWidgetsOnSeconColumn = default(int);

            "Given a new user and a widget on user's page".Context(() =>
                {
                    profile = MembershipHelper.CreateNewAnonUser();
                    facade = new Facade(new AppContext(string.Empty, profile.UserName));
                    userVisitModel = facade.FirstVisitHomePage(profile.UserName, "Test", true, false);

                    userColumns = facade.GetColumnsInPage(userVisitModel.CurrentPage.ID);
                    var firstColumn = userColumns.First();
                    var widgetsOnColumn = facade.GetWidgetInstancesInZoneWithWidget(firstColumn.WidgetZone.ID);

                    widgetInstance = widgetsOnColumn.First();
                });

            "When the widget is moved to another column".Do(() =>
                {
                    secondColumn = userColumns.ElementAt(1);
                    noOfWidgetsOnSeconColumn = facade.GetWidgetInstancesInZoneWithWidget(secondColumn.WidgetZone.ID).Count();
            
                    facade.MoveWidgetInstance(widgetInstance.Id, secondColumn.WidgetZone.ID, 1);                    
                });

            "It should remain there permanently".Assert(() =>
                {
                    var newWidgetsOnSecondColumn = facade.GetWidgetInstancesInZoneWithWidget(secondColumn.WidgetZone.ID);

                    var widgetAfterMove = newWidgetsOnSecondColumn.Where(wi => wi.OrderNo == 1).FirstOrDefault();
                    Assert.NotNull(widgetAfterMove);
                    Assert.Equal(widgetInstance.Id, widgetAfterMove.Id);
                });

            "It should push down other widgets where it is dropped".Assert(() =>
                {
                    var newWidgetsOnSecondColumn = facade.GetWidgetInstancesInZoneWithWidget(secondColumn.WidgetZone.ID);
                    // There should be 1 widget before it
                    Assert.Equal(1, newWidgetsOnSecondColumn.Where(wi => wi.OrderNo < 1).Count());
                    // There should be N-1 widgets after it where N = before move number of columns
                    Assert.Equal(noOfWidgetsOnSeconColumn - 1, newWidgetsOnSecondColumn.Where(wi => wi.OrderNo > 1).Count());
                });
        }


        [Specification]
        public void User_should_be_able_to_delete_an_widget_from_page()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);
            var wiId = default(int);
            var userSetup = default(UserSetup);
            
            "Given a new user".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                userSetup = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "When user adds a new page".Do(() =>
            {
                var widgets = facade.GetWidgetInstancesInZoneWithWidget(
                    facade.GetColumnsInPage(userSetup.CurrentPage.ID).First().WidgetZone.ID);

                wiId = widgets.First().Id;
                facade.DeleteWidgetInstance(wiId);
            });

            "It should add a new blank page as current page".Assert(() =>
            {
                var userPageSetup = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);

                var widgets = facade.GetWidgetInstancesInZoneWithWidget(
                    facade.GetColumnsInPage(userSetup.CurrentPage.ID).First().WidgetZone.ID);

                Assert.Equal(0, widgets.Where(wi => wi.Id == wiId).Count());
            });
        }

        [Specification]
        public void User_should_be_able_to_change_widget_state()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);
            var wiId = default(int);
            var userSetup = default(UserSetup);
            var newState = Guid.NewGuid().ToString();

            "Given a new user".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                userSetup = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "When user changes a widget's state and saves it".Do(() =>
            {
                var widgets = facade.GetWidgetInstancesInZoneWithWidget(
                    facade.GetColumnsInPage(userSetup.CurrentPage.ID).First().WidgetZone.ID);

                var widgetInstance = widgets.First();
                wiId = widgetInstance.Id;

                widgetInstance.State = newState;
            });

            "It should preserve the state on the next visit".Assert(() =>
            {
                var userPageSetup = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);

                var widgets = facade.GetWidgetInstancesInZoneWithWidget(
                    facade.GetColumnsInPage(userSetup.CurrentPage.ID).First().WidgetZone.ID);

                var widgetInstance = widgets.Where(wi => wi.Id == wiId).First();
                Assert.Equal(newState, widgetInstance.State);
            });
        }


        [Specification]
        public void User_should_be_add_a_new_widget_on_a_column_which_will_push_down_other_widets()
        {
            var facade = default(Facade);
            var profile = default(UserProfile);
            var userSetup = default(UserSetup);
            var newState = Guid.NewGuid().ToString();
            var addedWidgetInstance = default(WidgetInstance);
            var widgetToAdd = default(Widget);

            "Given a new user".Context(() =>
            {
                profile = MembershipHelper.CreateNewAnonUser();
                facade = new Facade(new AppContext(string.Empty, profile.UserName));

                userSetup = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
            });

            "When user adds a new widget on a column".Do(() =>
            {
                var widgets = facade.GetWidgetList(profile.UserName, Enumerations.WidgetType.PersonalPage);
                widgetToAdd = widgets.First();
                //var columns = facade.GetColumnsInPage(userSetup.CurrentPage.ID);
                //var firstColumn = columns.First();

                //addedWidgetInstance = facade.AddWidgetInstance(widgets.First().ID, 0, firstColumn.ColumnNo, firstColumn.WidgetZone.ID);
                addedWidgetInstance = facade.AddWidgetInstance(widgetToAdd.ID, 0, 0, 0);

            });

            "It should add the widget at the first position and push down other widgets and the widget is visible".Assert(() =>
            {
                var userPageSetup = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);

                var widgets = facade.GetWidgetInstancesInZoneWithWidget(
                    facade.GetColumnsInPage(userSetup.CurrentPage.ID).First().WidgetZone.ID);

                Assert.Equal(addedWidgetInstance.Id, widgets.First().Id);
                Assert.Equal(addedWidgetInstance.Widget.ID, widgetToAdd.ID);
                Assert.Equal(true, addedWidgetInstance.Expanded);
            });
        }
    }


}
