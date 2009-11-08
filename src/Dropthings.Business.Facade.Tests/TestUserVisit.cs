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

					"Given anonymous user who has never visited the site before".Context(() => { });

					"when the user visits for the first time".Do(() =>
					{
						userVisitModel = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
					});

					"it creates widgets on the creates page at exact columns and positions as the anon user's pages".Assert(() =>
					{
						anonPages.Each(anonPage =>
						{
							var userPage = userVisitModel.UserPages.First(page =>
											page.Title == anonPage.Title
											&& page.OrderNo == anonPage.OrderNo
											&& page.PageType == anonPage.PageType);

							facade.GetColumnsInPage(anonPage.ID).Each(anonColumn =>
							{
								var userColumns = facade.GetColumnsInPage(userPage.ID);
								var userColumn = userColumns.First(column =>
												column.ColumnNo == anonColumn.ColumnNo);

								var anonColumnWidgets = facade.GetWidgetInstancesInZoneWithWidget(anonColumn.WidgetZoneId);
								var userColumnWidgets = facade.GetWidgetInstancesInZoneWithWidget(userColumn.WidgetZoneId);

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
				}
			});
		}

		[Specification]
		public void Revisit_should_load_the_pages_and_widgets_exactly_the_same()
		{
			MembershipHelper.UsingNewAnonUser((profile) =>
			{
				using (var facade = new Facade(new AppContext(string.Empty, profile.UserName)))
				{
					UserSetup userVisitModel = null;
					UserSetup userRevisitModel = null;

					"Given an anonymous user who visited first".Context(() =>
					{
						userVisitModel = facade.FirstVisitHomePage(profile.UserName, string.Empty, true, false);
					});

					"when the same user visits again".Do(() =>
					{
						userRevisitModel = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, false);
					});

					"it should load the exact same pages, column and widgets as the first visit produced".Assert(() =>
					{
						userVisitModel.UserPages.Each(firstVisitPage =>
						{
							Assert.True(userRevisitModel.UserPages.Exists(page => page.ID == firstVisitPage.ID));

							var revisitPage = userRevisitModel.UserPages.First(page => page.ID == firstVisitPage.ID);
							var revisitPageColumns = facade.GetColumnsInPage(revisitPage.ID);

							facade.GetColumnsInPage(firstVisitPage.ID).Each(firstVisitColumn =>
							{
								var revisitColumn = revisitPageColumns.First(column => column.ID == firstVisitColumn.ID);

								var firstVisitWidgets = facade.GetWidgetInstancesInZoneWithWidget(firstVisitColumn.WidgetZoneId);
								var revisitWidgets = facade.GetWidgetInstancesInZoneWithWidget(revisitColumn.WidgetZoneId);

								firstVisitWidgets.Each(firstVisitWidget =>
										Assert.True(revisitWidgets.Where(revisitWidget =>
												revisitWidget.Id == firstVisitWidget.Id).Count() == 1));
							});
						});
					});
				}
			});
		}
	}
}
