using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSpec;
using Dropthings.Business.Facade.Context;
using Dropthings.Data;
using Dropthings.Business.Facade.Test.Helper;
using Xunit;

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
                        (int)Enumerations.WidgetTypeEnum.PersonalPage);
                    facade.AssignWidgetRoles(newWidget.ID, new string[] { GUEST_ROLE });
                });

            "It should be available to the users of that role".Assert(() =>
                {
                    var newUserProfile = MembershipHelper.CreateNewAnonUser();
                    facade.SetUserRoles(newUserProfile.UserName, new string[] { GUEST_ROLE });
                    var widgetsAvailable = facade.GetWidgetList(newUserProfile.UserName, Enumerations.WidgetTypeEnum.PersonalPage);
                    Assert.Equal(1, widgetsAvailable.Where(w => w.ID == newWidget.ID).Count());
                });
        }
    }
}
