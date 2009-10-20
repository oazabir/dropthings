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
using Dropthings.DataAccess;

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
            MembershipHelper.UsingNewAnonUser((profile) =>
            {
                using (var facade = new Facade(new AppContext(string.Empty, profile.UserName)))
                {
                    UserSetup userVisitModel = null;
                    
                    // Load the anonymous user pages and widgets
                    string anonUserName = facade.GetUserSettingTemplate().AnonUserSettingTemplate.UserName;
                    var anonPages = facade.GetPagesOfUser(facade.GetUserGuidFromUserName(anonUserName));
                    
                    "Given anonymous user who has never visited the site before".Context(() => {});
         
                    "when the user visits for the first time".Do(() =>
                        {
                            userVisitModel = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
                        });

                    "it creates default pages from template user".Assert(() =>
                        {
                            Assert.Equal(anonPages.Count(), userVisitModel.UserPages.Count());
                            anonPages.Each(page => Assert.True(userVisitModel.UserPages
                                .Where(userPage => 
                                    userPage.Title == page.Title && userPage.OrderNo == page.OrderNo
                                    && userPage.PageType == page.PageType)
                                    .Count() == 1));
                        });

                    "it sets the first page as the default page for the new user".Assert(() =>
                        Assert.NotNull(userVisitModel.CurrentPage));

                    "it creates widgets on the creates page at exact columns and positions as the anon user's pages".Assert(() =>
                        {
                            anonPages.Each(anonPage =>
                                facade.GetColumnsInPage(anonPage.ID).Each(anonColumn =>
                                    {
                                        var userPage = userVisitModel.UserPages.First(page =>
                                            page.Title == anonPage.Title
                                            && page.OrderNo == anonPage.OrderNo
                                            && page.PageType == anonPage.PageType);
                                        var userColumns = facade.GetColumnsInPage(userPage.ID);
                                        var userColumn = userColumns.First(column => 
                                                column.ColumnNo == anonColumn.ColumnNo);

                                        var anonColumnWidgets = facade.GetWidgetInstancesInZone(anonColumn.WidgetZoneId);
                                        var userColumnWidgets = facade.GetWidgetInstancesInZone(userColumn.WidgetZoneId);

                                        anonColumnWidgets.Each(anonWidget => Assert.True(userColumnWidgets.Where(userWidget =>
                                            userWidget.Title == anonWidget.Title 
                                            && userWidget.OrderNo == anonWidget.OrderNo).Count() == 1));                                            
                                    }));
                        });

                    
                }
            });
        }
    }
}
