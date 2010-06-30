namespace Dropthings.DataAccess.UnitTest
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Moq;

	using Xunit;
	using SubSpec;
	using Dropthings.Util;
	using OmarALZabir.AspectF;
	using System.Collections;
    using Dropthings.Data;
    using Dropthings.Data.Repository;

	public class TabRepositoryTest
	{
		#region Constructors

		public TabRepositoryTest()
		{
		}

		#endregion Constructors

		#region Methods

		[Specification]
		public void GetTab_Should_Return_A_Tab_from_database_when_cache_is_empty_and_then_caches_it()
		{
			var cache = new Mock<ICache>();
			var database = new Mock<IDatabase>();
			ITabRepository pageRepository = new TabRepository(database.Object, cache.Object);

			const int pageId = 1;
			var page = default(Tab);
			var sampleTab = new Tab() { ID = pageId, Title = "Test Tab", ColumnCount = 3, LayoutType = 3, VersionNo = 1, PageType = (int)Enumerations.PageType.PersonalTab, CreatedDate = DateTime.Now };

			database
					.Expect<IQueryable<Tab>>(d => d.Query<int, Tab>(CompiledQueries.TabQueries.GetTabById, 1))
					.Returns(new Tab[] { sampleTab }.AsQueryable()).Verifiable();

			"Given TabRepository and empty cache".Context(() =>
			{
				// cache is empty
				cache.Expect(c => c.Get(It.IsAny<string>())).Returns(default(object));
				// It will cache the Tab object afte loading from database
				cache.Expect(c => c.Add(It.Is<string>(cacheKey => cacheKey == CacheKeys.TabKeys.TabId(pageId)),
						It.Is<Tab>(cacheTab => object.ReferenceEquals(cacheTab, sampleTab)))).Verifiable();
			});

			"when GetTabById is called".Do(() =>
					page = pageRepository.GetTabById(1));

			"it checks in the cache first and finds nothing and then caches it".Assert(() =>
					cache.VerifyAll());


			"it loads the page from database".Assert(() =>
					database.VerifyAll());

			"it returns the page as expected".Assert(() =>
			{
				Assert.Equal<int>(pageId, page.ID);
			});
		}

		[Specification]
		public void GetTab_Should_Return_A_Tab_from_cache_when_it_is_already_cached()
		{
			var cache = new Mock<ICache>();
			var database = new Mock<IDatabase>();
			ITabRepository pageRepository = new TabRepository(database.Object, cache.Object);

			const int pageId = 1;
			var page = default(Tab);
			var sampleTab = new Tab() { ID = pageId, Title = "Test Tab", ColumnCount = 3, LayoutType = 3, VersionNo = 1, 
                PageType = (int)Enumerations.PageType.PersonalTab, CreatedDate = DateTime.Now };

			"Given TabRepository and the requested page in cache".Context(() =>
			{
				cache.Expect(c => c.Get(CacheKeys.TabKeys.TabId(sampleTab.ID)))
						.Returns(sampleTab).AtMostOnce();
			});

			"when GetTabById is called".Do(() =>
					page = pageRepository.GetTabById(1));

			"it checks in the cache first and finds the object is in cache".Assert(() =>
			{
				cache.VerifyAll();
			});

			"it returns the page as expected".Assert(() =>
			{
				Assert.Equal<int>(pageId, page.ID);
			});
		}


		[Specification]
		public void GetTabIdByUserGuid_should_return_a_list_of_pageId_from_database_if_not_already_cached_and_then_cache_it()
		{
			RepositoryHelper.UseRepository<TabRepository>((pageRepository, database, cache) =>
			{
                Guid userId = Guid.NewGuid();

				List<Tab> userTabs = new List<Tab>();
				userTabs.Add(new Tab() { ID = 1, Title = "Test Tab 1", ColumnCount = 1, LayoutType = 1, VersionNo = 1, PageType = (int)Enumerations.PageType.PersonalTab, CreatedDate = DateTime.Now });
				userTabs.Add(new Tab() { ID = 2, Title = "Test Tab 2", ColumnCount = 2, LayoutType = 2, VersionNo = 1, PageType = (int)Enumerations.PageType.PersonalTab, CreatedDate = DateTime.Now });

				database.Expect<IQueryable<Tab>>(d => d.Query<Guid, Tab>(CompiledQueries.TabQueries.GetTabsByUserId, userId))
						.Returns(userTabs.AsQueryable()).Verifiable();

				List<int> pageIds = userTabs.Select(p => p.ID).ToList();

				var returnedTabIds = default(List<int>);

				"Given TabRepository and empty cache".Context(() =>
				{
					// cache is empty
					cache.Expect(c => c.Get(It.IsAny<string>())).Returns(default(object));
					// allow storing anything in cache
					cache.Expect(c => c.Add(It.IsAny<string>(), It.IsAny<object>()));
					cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<object>()));
				});

				"when GetpageIdByUserGuid is called with a userId".Do(() =>
				{
					returnedTabIds = pageRepository.GetTabIdByUserGuid(userId);
				});

				"it looks up in the cache and finds not in cache".Assert(() =>
						cache.VerifyAll());


				"it returns a collection of pageId from database".Assert(() =>
				{
					database.VerifyAll();
					Assert.Equal<int>(2, returnedTabIds.Count);
				});

				"it returns the pages in exact order as it is returned from database".Assert(() =>
				{
					Assert.Equal<int>(1, returnedTabIds[0]);
					Assert.Equal<int>(2, returnedTabIds[1]);
				});
			});
		}

		[Specification]
		public void GetTabOwnerName_Should_Return_UserName_Given_TabId_from_database_if_not_already_cached()
		{
			RepositoryHelper.UseRepository<TabRepository>((pageRepository, database, cache) =>
			{
				const string ownerName = "some user name";
				const int pageId = 1;
				database.Expect<IQueryable<string>>(d => d.Query<int, string>(CompiledQueries.TabQueries.GetTabOwnerName, 1))
						.Returns(new string[] { ownerName }.AsQueryable()).Verifiable();

				var userName = default(string);

				"Given TabRepository and empty cache".Context(() =>
						{
							// cache is empty
							cache.Expect(c => c.Get(It.IsAny<string>())).Returns(default(object));
							// ensure the page owner name is cached after loading from database
							cache.Expect(c => c.Add(It.Is<string>(cacheKey => cacheKey == CacheKeys.TabKeys.TabOwnerName(pageId)),
									It.Is<string>(cacheOwnerName => cacheOwnerName == ownerName))).Verifiable();
						});

				"when GetTabOwnerName is called with a TabId".Do(() =>
				{
					userName = pageRepository.GetTabOwnerName(pageId);
				});

				"it looks up in the cache first and find nothing and then it caches the owner name".Assert(() =>
						cache.Verify());

				"it returns the owner name of the page".Assert(() =>
				{
					database.VerifyAll();
					Assert.Equal<string>("some user name", userName);
				});
			});
		}

		[Specification]
		public void GetTabsOfUser_Should_Return_List_Of_Tabs()
		{
			RepositoryHelper.UseRepository<TabRepository>((pageRepository, database, cache) =>
			{
                Guid userId = Guid.NewGuid();

				List<Tab> userTabs = new List<Tab>();
                var page1 = new Tab() { ID = 1, Title = "Test Tab 1", ColumnCount = 1, LayoutType = 1, VersionNo = 1, PageType = (int)Enumerations.PageType.PersonalTab, CreatedDate = DateTime.Now, AspNetUser = new AspNetUser { UserId = userId } };
                var page2 = new Tab() { ID = 2, Title = "Test Tab 2", ColumnCount = 2, LayoutType = 2, VersionNo = 1, PageType = (int)Enumerations.PageType.PersonalTab, CreatedDate = DateTime.Now, AspNetUser = new AspNetUser { UserId = userId } };
				userTabs.Add(page1);
				userTabs.Add(page2);

				database.Expect<IQueryable<Tab>>(d => d.Query<Guid, Tab>(CompiledQueries.TabQueries.GetTabsByUserId, userId))                    
						.Returns(userTabs.AsQueryable()).Verifiable();

				var cacheMap = new Dictionary<string, object>();
				var collectionKey = CacheKeys.UserKeys.TabsOfUser(userId);
				cacheMap.Add(collectionKey, userTabs);
				cacheMap.Add(CacheKeys.TabKeys.TabId(1), page1);
				cacheMap.Add(CacheKeys.TabKeys.TabId(2), page2);

				"Given TabRepository and Empty cache".Context(() =>
						{
							cache.Expect(c => c.Get(It.IsAny<string>())).Returns(default(object));
							cache.Expect(c => c.Add(collectionKey, It.IsAny<List<Tab>>())).Verifiable();
							cache.Expect(c =>
											c.Set(It.Is<string>(cacheKey => cacheMap.ContainsKey(cacheKey)),
													It.Is<object>(cacheValue => cacheMap.Values.Contains(cacheValue))))
									.Verifiable();
						});

				var pages = default(List<Tab>);

				"when GetTabsOfUser is called".Do(() =>
						pages = pageRepository.GetTabsOfUser(userId));

				"it first looks into cache for the pages and finds nothing and then it caches it".Assert(() =>
						cache.VerifyAll());

				"it loads the pages from database".Assert(() =>
						database.VerifyAll());

				"it returns the pages of the user".Assert(() =>
				{
					Assert.Equal<int>(userTabs.Count, pages.Count);
					pages.Each(page => Assert.Equal(userId, page.AspNetUser.UserId));
				});
			});
		}

		[Specification]
		public void InsertTab_should_insert_a_page_in_database_and_cache_it()
		{
			var cache = new Mock<ICache>();
			var database = new Mock<IDatabase>();
			ITabRepository pageRepository = new TabRepository(database.Object, cache.Object);

			const int pageId = 1;
            Guid userId = Guid.NewGuid();
			var page = default(Tab);
			var sampleTab = new Tab() { Title = "Test Tab", ColumnCount = 3, 
				LayoutType = 3, AspNetUser = new AspNetUser { UserId = userId }, VersionNo = 1, 
				PageType = (int)Enumerations.PageType.PersonalTab, CreatedDate = DateTime.Now };

			database.Expect<Tab>(d => d.Insert<AspNetUser, Tab>(
                        It.Is<AspNetUser>(u => u.UserId == userId),
                        It.IsAny<Action<AspNetUser, Tab>>(),
                        It.Is<Tab>(p => p.ID == default(int))))
                    .Callback(() => sampleTab.ID = pageId)
					.Returns(sampleTab)
                    .AtMostOnce()
                    .Verifiable();

			"Given TabRepository".Context(() =>
			{
				// It will clear items from cache
				cache.Expect(c => c.Remove(CacheKeys.UserKeys.TabsOfUser(sampleTab.AspNetUser.UserId)));
			});

			"when Insert is called".Do(() =>
					page = pageRepository.Insert(new Tab
					{
						Title = sampleTab.Title,
						ColumnCount = sampleTab.ColumnCount,
						LayoutType = sampleTab.LayoutType,
						AspNetUser = new AspNetUser { UserId = userId },
						VersionNo = sampleTab.VersionNo,
						PageType = sampleTab.PageType
					}));

			("then it should insert the page in database" +
			"and clear any cached collection of pages for the user who gets the new page" +
			"and it returns the newly inserted page").Assert(() =>
			{
				database.VerifyAll();
				cache.VerifyAll();

				Assert.Equal<int>(pageId, page.ID);
			});			
		}



		#endregion Methods
	}
}