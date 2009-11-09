namespace Dropthings.DataAccess.UnitTest
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Dropthings.DataAccess.Repository;

	using Moq;

	using Xunit;
	using SubSpec;
	using Dropthings.Util;
	using OmarALZabir.AspectF;
	using System.Collections;

	public class PageRepositoryTest
	{
		#region Constructors

		public PageRepositoryTest()
		{
		}

		#endregion Constructors

		#region Methods

		[Specification]
		public void GetPage_Should_Return_A_Page_from_database_when_cache_is_empty_and_then_caches_it()
		{
			var cache = new Mock<ICache>();
			var database = new Mock<IDropthingsDataContext>();
			IPageRepository pageRepository = new PageRepository(database.Object, cache.Object);

			const int pageId = 1;
			var page = default(Page);
			var samplePage = new Page() { ID = pageId, Title = "Test Page", ColumnCount = 3, LayoutType = 3, UserId = Guid.Empty, VersionNo = 1, PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now };

			database
					.Expect<Page>(d => d.GetSingle<Page, int>(DropthingsDataContext.SubsystemEnum.Page, 1, LinqQueries.CompiledQuery_GetPageById))
					.Returns(samplePage);

			"Given PageRepository and empty cache".Context(() =>
			{
				// cache is empty
				cache.Expect(c => c.Get(It.IsAny<string>())).Returns(default(object));
				// It will cache the Page object afte loading from database
				cache.Expect(c => c.Add(It.Is<string>(cacheKey => cacheKey == CacheSetup.CacheKeys.PageId(pageId)),
						It.Is<Page>(cachePage => object.ReferenceEquals(cachePage, samplePage)))).AtMostOnce().Verifiable();
			});

			"when GetPageById is called".Do(() =>
					page = pageRepository.GetPageById(1));

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
		public void GetPage_Should_Return_A_Page_from_cache_when_it_is_already_cached()
		{
			var cache = new Mock<ICache>();
			var database = new Mock<IDropthingsDataContext>();
			IPageRepository pageRepository = new PageRepository(database.Object, cache.Object);

			const int pageId = 1;
			var page = default(Page);
			var samplePage = new Page() { ID = pageId, Title = "Test Page", ColumnCount = 3, LayoutType = 3, UserId = Guid.Empty, VersionNo = 1, PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now };

			"Given PageRepository and the requested page in cache".Context(() =>
			{
				cache.Expect(c => c.Get(CacheSetup.CacheKeys.PageId(samplePage.ID)))
						.Returns(samplePage);
			});

			"when GetPageById is called".Do(() =>
					page = pageRepository.GetPageById(1));

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
		public void GetPageIdByUserGuid_should_return_a_list_of_pageId_from_database_if_not_already_cached_and_then_cache_it()
		{
			RepositoryHelper.UseRepository<PageRepository>((pageRepository, database, cache) =>
			{
				Guid userId = Guid.Empty;

				List<Page> userPages = new List<Page>();
				userPages.Add(new Page() { ID = 1, Title = "Test Page 1", ColumnCount = 1, LayoutType = 1, UserId = userId, VersionNo = 1, PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now });
				userPages.Add(new Page() { ID = 2, Title = "Test Page 2", ColumnCount = 2, LayoutType = 2, UserId = userId, VersionNo = 1, PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now });

				database.Expect<List<Page>>(d => d.GetList<Page, Guid>(DropthingsDataContext.SubsystemEnum.Page, Guid.Empty, LinqQueries.CompiledQuery_GetPagesByUserId))
						.Returns(userPages);

				List<int> pageIds = userPages.Select(p => p.ID).ToList();

				var returnedPageIds = default(List<int>);

				"Given PageRepository and empty cache".Context(() =>
				{
					// cache is empty
					cache.Expect(c => c.Get(It.IsAny<string>())).Returns(default(object));
					// allow storing anything in cache
					cache.Expect(c => c.Add(It.IsAny<string>(), It.IsAny<object>()));
					cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<object>()));
				});

				"when GetpageIdByUserGuid is called with a userId".Do(() =>
				{
					returnedPageIds = pageRepository.GetPageIdByUserGuid(userId);
				});

				"it looks up in the cache and finds not in cache".Assert(() =>
						cache.VerifyAll());


				"it returns a collection of pageId from database".Assert(() =>
				{
					database.VerifyAll();
					Assert.Equal<int>(2, returnedPageIds.Count);
				});

				"it returns the pages in exact order as it is returned from database".Assert(() =>
				{
					Assert.Equal<int>(1, returnedPageIds[0]);
					Assert.Equal<int>(2, returnedPageIds[1]);
				});
			});
		}

		[Specification]
		public void GetPageOwnerName_Should_Return_UserName_Given_PageId_from_database_if_not_already_cached()
		{
			RepositoryHelper.UseRepository<PageRepository>((pageRepository, database, cache) =>
			{
				const string ownerName = "some user name";
				const int pageId = 1;
				database.Expect<string>(d => d.GetSingle<string, int>(DropthingsDataContext.SubsystemEnum.Page, 1, LinqQueries.CompiledQuery_GetPageOwnerName))
						.Returns(ownerName);

				var userName = default(string);

				"Given PageRepository and empty cache".Context(() =>
						{
							// cache is empty
							cache.Expect(c => c.Get(It.IsAny<string>())).Returns(default(object));
							// ensure the page owner name is cached after loading from database
							cache.Expect(c => c.Add(It.Is<string>(cacheKey => cacheKey == CacheSetup.CacheKeys.PageOwnerName(pageId)),
									It.Is<string>(cacheOwnerName => cacheOwnerName == ownerName))).AtMostOnce().Verifiable();
						});

				"when GetPageOwnerName is called with a PageId".Do(() =>
				{
					userName = pageRepository.GetPageOwnerName(pageId);
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
		public void GetPagesOfUser_Should_Return_List_Of_Pages()
		{
			RepositoryHelper.UseRepository<PageRepository>((pageRepository, database, cache) =>
			{
				Guid userId = Guid.NewGuid();

				List<Page> userPages = new List<Page>();
				var page1 = new Page() { ID = 1, Title = "Test Page 1", ColumnCount = 1, LayoutType = 1, UserId = userId, VersionNo = 1, PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now };
				var page2 = new Page() { ID = 2, Title = "Test Page 2", ColumnCount = 2, LayoutType = 2, UserId = userId, VersionNo = 1, PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now };
				userPages.Add(page1);
				userPages.Add(page2);

				database.Expect<List<Page>>(d => d.GetList<Page, Guid>(DropthingsDataContext.SubsystemEnum.Page, userId, LinqQueries.CompiledQuery_GetPagesByUserId))
						.Returns(userPages);

				var cacheMap = new Dictionary<string, object>();
				var collectionKey = CacheSetup.CacheKeys.PagesOfUser(userId);
				cacheMap.Add(collectionKey, userPages);
				cacheMap.Add(CacheSetup.CacheKeys.PageId(1), page1);
				cacheMap.Add(CacheSetup.CacheKeys.PageId(2), page2);

				"Given PageRepository and Empty cache".Context(() =>
						{
							cache.Expect(c => c.Get(It.IsAny<string>())).Returns(default(object));
							cache.Expect(c => c.Add(collectionKey, It.IsAny<List<Page>>())).Verifiable();
							cache.Expect(c =>
											c.Set(It.Is<string>(cacheKey => cacheMap.ContainsKey(cacheKey)),
													It.Is<object>(cacheValue => cacheMap.Values.Contains(cacheValue))))
									.Verifiable();
						});

				var pages = default(List<Page>);

				"when GetPagesOfUser is called".Do(() =>
						pages = pageRepository.GetPagesOfUser(userId));

				"it first looks into cache for the pages and finds nothing and then it caches it".Assert(() =>
						cache.VerifyAll());

				"it loads the pages from database".Assert(() =>
						database.VerifyAll());

				"it returns the pages of the user".Assert(() =>
				{
					Assert.Equal<int>(userPages.Count, pages.Count);
					pages.Each(page => Assert.Equal(userId, page.UserId));
				});
			});
		}

		[Specification]
		public void InsertPage_should_insert_a_page_in_database_and_cache_it()
		{
			var cache = new Mock<ICache>();
			var database = new Mock<IDropthingsDataContext>();
			IPageRepository pageRepository = new PageRepository(database.Object, cache.Object);

			const int pageId = 1;
			var page = default(Page);
			var samplePage = new Page() { ID = pageId, Title = "Test Page", ColumnCount = 3, 
				LayoutType = 3, UserId = Guid.NewGuid(), VersionNo = 1, 
				PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now };

			database
					.Expect<Page>(d => d.Insert<Page>(DropthingsDataContext.SubsystemEnum.Page, It.IsAny<Action<Page>>()))
					.Returns(samplePage);

			"Given PageRepository".Context(() =>
			{
				// It will clear items from cache
				cache.Expect(c => c.Remove(CacheSetup.CacheKeys.PagesOfUser(samplePage.UserId)));
			});

			"when Insert is called".Do(() =>
					page = pageRepository.Insert((newPage) =>
					{
						newPage.Title = samplePage.Title;
						newPage.ColumnCount = samplePage.ColumnCount;
						newPage.LayoutType = samplePage.LayoutType;
						newPage.UserId = samplePage.UserId;
						newPage.VersionNo = samplePage.VersionNo;
						newPage.PageType = samplePage.PageType;
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