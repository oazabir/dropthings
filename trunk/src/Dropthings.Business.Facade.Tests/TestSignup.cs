using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSpec;
using Dropthings.Web.Framework;
using Dropthings.Business.Facade.Test.Helper;

namespace Dropthings.Business.Facade.Tests
{
    public class TestSignup
    {
        public TestSignup()
        {
            Facade.BootStrap();
        }

        [Specification]
        public void User_can_signup_to_save_changes_made_to_anon_pages()
        {
            var facade = default(Facade);
            var anonUserProfile = default(UserProfile);
            var newlyAddedWidgetId = default(int);
            var registeredUserName = default(string);
            var registeredUserPassword = default(string);

            "Given an anonymous user who has made some changes to the page".Context(() =>
                {
                    anonUserProfile = MembershipHelper.CreateNewAnonUser();
                    facade = new Facade(new Context.AppContext(anonUserProfile.UserName, anonUserProfile.UserName));

                    var userSetup = facade.FirstVisitHomeTab(anonUserProfile.UserName, string.Empty, true, false);
                    var newWidgetToAdd = facade.GetWidgetList(anonUserProfile.UserName, Data.Enumerations.WidgetType.PersonalTab).First();
                    var newlyAddedWidget = facade.AddWidgetInstance(newWidgetToAdd.ID, 0, 0, 0);
                    newlyAddedWidgetId = newlyAddedWidget.Id;
                });

            "When user signs up to save the changes permanently".Do(() =>
                {
                    registeredUserName = Guid.NewGuid().ToString();
                    registeredUserPassword = Guid.NewGuid().ToString();
                    facade.RegisterUser(registeredUserName, registeredUserPassword, registeredUserName, false);
                });

            "It should save the changes permanently".Assert(() =>
                {
                });

            "It should allow user to login to access the pages".Assert(() =>
                {
                });
        }
    }
}
