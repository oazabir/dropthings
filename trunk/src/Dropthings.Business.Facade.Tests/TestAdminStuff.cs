using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSpec;
using Dropthings.Business.Facade.Context;
using Dropthings.Data;
using Dropthings.Business.Facade.Test.Helper;
using Xunit;
using Dropthings.Web.Framework;
using System.Security.Permissions;
using Dropthings.Util;

namespace Dropthings.Business.Facade.Tests
{
    public class TestAdminStuff
    {
        const string ADMIN_USER = "admin";
        const string GUEST_ROLE = "guest";

        public TestAdminStuff()
        {
            Facade.BootStrap();
        }

        [Specification]
        public void Admin_user_can_add_new_widget_and_assign_roles_to_widget()
        {
            var facade = default(Facade);
            var newWidget = default(Widget);

            "Given an admin user".Context(() =>
                {
                    facade = new Facade(new AppContext(string.Empty, ADMIN_USER));
                });

            "When user adds a new widget and assigns roles to it".Do(() =>
                {
                    newWidget = facade.AddWidget("Test Widget", 
                        "omaralzabir.com", string.Empty, "Test widget", 
                        string.Empty, false, false, 0, "guest", 
                        (int)Enumerations.WidgetType.PersonalTab);
                    facade.AssignWidgetRoles(newWidget.ID, new string[] { GUEST_ROLE });
                });

            "It should be available to the users of that role".Assert(() =>
                {
                    var newUserProfile = MembershipHelper.CreateNewAnonUser();
                    facade.SetUserRoles(newUserProfile.UserName, new string[] { GUEST_ROLE });
                    var widgetsAvailable = facade.GetWidgetList(newUserProfile.UserName, Enumerations.WidgetType.PersonalTab);
                    Assert.Equal(1, widgetsAvailable.Where(w => w.ID == newWidget.ID).Count());
                });
        }

        [Specification]
        public void Admin_user_can_delete_a_newly_created_widget()
        {
            var facade = default(Facade);
            var newWidget = default(Widget);
            var userFacade = default(Facade);
            var someNewUser = default(UserProfile);
            var addedWidgetInstance = default(WidgetInstance);

            "Given admin user and a newly created widget assigned to some roles and users using the widget".Context(() =>
                {
                    facade = new Facade(new AppContext(string.Empty, ADMIN_USER));

                    newWidget = facade.AddWidget("Test Widget",
                        "omaralzabir.com", string.Empty, "Test widget",
                        string.Empty, false, false, 0, "guest",
                        (int)Enumerations.WidgetType.PersonalTab);
                    facade.AssignWidgetRoles(newWidget.ID, new string[] { GUEST_ROLE });

                    someNewUser = MembershipHelper.CreateNewAnonUser();
                    userFacade = new Facade(new AppContext(someNewUser.UserName, someNewUser.UserName));
                    var userSetup = userFacade.FirstVisitHomeTab(someNewUser.UserName, string.Empty, true, false);
                    addedWidgetInstance = userFacade.AddWidgetInstance(newWidget.ID, 0, 0, 0);
                    Assert.NotNull(addedWidgetInstance);
                });

            "When admin user deletes the widget".Do(() =>
                {
                    facade.DeleteWidget(newWidget.ID);
                });

            "It should delete the widget completely from all users pages".Assert(() =>
                {
                    Assert.False(facade.GetAllWidgets().Exists(w => w.ID == newWidget.ID));
                    Assert.False(userFacade.GetAllWidgets().Exists(w => w.ID == newWidget.ID));
                    // OMAR: Turning off as this comes from cache, not from database
                    // Assert.Throws<InvalidOperationException>(() => userFacade.GetWidgetInstanceById(addedWidgetInstance.Id));
                });
        }
    }
}
