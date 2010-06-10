using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;

namespace Dropthings.Test.WatiN.Pages
{
    [Page(UrlRegex = Urls.LoginPage)]
    public class LoginPage : Page
    {
        [FindBy(Name="Email")]
        public TextField Email;

        [FindBy(Name = "Password")]
        public TextField Password;

        [FindBy(Name="RememberMeCheckbox")]
        public CheckBox RememberMe;

        [FindBy(Name="LoginButton")]
        public Button LoginButton;

        [FindBy(Name="RegisterButton")]
        public Button RegisterButton;
    }
}
