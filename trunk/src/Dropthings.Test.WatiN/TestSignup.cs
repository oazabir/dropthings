using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSpec;
using WatiN.Core;
using Dropthings.Test.WatiN.Pages;
using Dropthings.Test.WatiN.Controls;
using Xunit;
using System.Threading;

namespace Dropthings.Test.WatiN
{
    public class TestSignup
    {
        [Specification]
        public void User_can_signup_to_permanently_save_page_setup()
        {
            var browser = default(Browser);
            var homepage = default(HomePage);
            var loginPage = default(LoginPage);

            var loginName = default(string);
            var password = default(string);

            var removedWidgetTitle = default(string);

            "Given a new user who has some widgets".Context(() =>
                {
                    BrowserHelper.ClearCookies();
                    browser = BrowserHelper.OpenNewBrowser(Urls.Homepage);
                    homepage = browser.Page<HomePage>();
                });

            "When the user signs up after making some changes to the page".Do(() =>
                {
                    removedWidgetTitle = homepage.Widgets.First().Title;
                    homepage.Widgets.First().Close();
                    Thread.Sleep(5000);
                    browser.WaitForAsyncPostbackComplete(10000);


                    browser.GoTo(Urls.LoginPage);
                    loginPage = browser.Page<LoginPage>();

                    loginName = Guid.NewGuid().ToString();
                    loginPage.Email.TypeText(loginName);
                    password = Guid.NewGuid().ToString();
                    loginPage.Password.TypeText(password);
                    loginPage.RegisterButton.Click();

                    browser.WaitForComplete();
                    homepage = browser.Page<HomePage>();
                });

            "It should save the changes against that user".Assert(() =>
                {
                    using (browser)
                    {
                        Assert.False(browser.ContainsText(removedWidgetTitle));
                    }
                });

            "It should allow user to login to that account".Assert(() =>
                {
                    using (browser)
                    {
                        BrowserHelper.ClearCookies();
                        browser.GoTo(Urls.LoginPage);

                        loginPage = browser.Page<LoginPage>();
                        loginPage.Email.TypeText(loginName);
                        loginPage.Password.TypeText(password);
                        loginPage.LoginButton.Click();
                        browser.WaitForComplete();

                        Assert.False(browser.ContainsText(removedWidgetTitle));
                    }
                });
        }
    }
}
