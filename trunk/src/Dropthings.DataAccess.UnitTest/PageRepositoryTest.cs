namespace Dropthings.DataAccess.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess.Repository;

    using Moq;

    using Xunit;

    public class PageRepositoryTest
    {
        #region Constructors

        public PageRepositoryTest()
        {
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public void GetPageIdByUserGuid()
        {
            RepositoryHelper.UseRepository<PageRepository>((pageRepository, database) =>
            {
                List<int> pageIds = new List<int>();
                pageIds.Add(1);
                pageIds.Add(2);

                database.Expect<List<int>>(d => d.GetList<int, Guid>(DropthingsDataContext.SubsystemEnum.Page, Guid.Empty, LinqQueries.CompiledQuery_GetPageIdByUserGuid))
                    .Returns(pageIds);

                var returnedPageIds = pageRepository.GetPageIdByUserGuid(Guid.Empty);

                Assert.Equal<int>(2, returnedPageIds.Count);
                Assert.Equal<int>(1, returnedPageIds[0]);
                Assert.Equal<int>(2, returnedPageIds[1]);
            });
        }

        [Fact]
        public void GetPageOwnerName_Should_Return_UserName_Given_PageId()
        {
            RepositoryHelper.UseRepository<PageRepository>((pageRepository, database) =>
            {
                database.Expect<string>(d => d.GetSingle<string, int>(DropthingsDataContext.SubsystemEnum.Page, 1, LinqQueries.CompiledQuery_GetPageOwnerName))
                    .Returns("some user name");

                var userName = pageRepository.GetPageOwnerName(1);
                Assert.Equal<string>("some user name", userName);
            });
        }

        [Fact]
        public void GetPage_Should_Return_A_Page()
        {
            RepositoryHelper.UseRepository<PageRepository>((pageRepository, database) =>
            {
                database
                    .Expect<Page>(d => d.GetSingle<Page, int>(DropthingsDataContext.SubsystemEnum.Page, 1, LinqQueries.CompiledQuery_GetPageById))
                    .Returns(new Page() { ID = 1, Title = "Test Page", ColumnCount = 3, LayoutType = 3, UserId = Guid.Empty, VersionNo = 1, PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now });

                var page = pageRepository.GetPageById(1);

                Assert.Equal<int>(1, page.ID);
            });
        }

        [Fact]
        public void GetPagesOfUser_Should_Return_List_Of_Pages()
        {
            RepositoryHelper.UseRepository<PageRepository>((pageRepository, database) =>
            {
                List<Page> userPages = new List<Page>();
                userPages.Add(new Page() { ID = 1, Title = "Test Page 1", ColumnCount = 1, LayoutType = 1, UserId = Guid.Empty, VersionNo = 1, PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now });
                userPages.Add(new Page() { ID = 2, Title = "Test Page 2", ColumnCount = 2, LayoutType = 2, UserId = Guid.Empty, VersionNo = 1, PageType = Enumerations.PageTypeEnum.PersonalPage, CreatedDate = DateTime.Now });

                database.Expect<List<Page>>(d => d.GetList<Page, Guid>(DropthingsDataContext.SubsystemEnum.Page, Guid.Empty, LinqQueries.CompiledQuery_GetPagesByUserId))
                    .Returns(userPages);

                var pages = pageRepository.GetPagesOfUser(Guid.Empty);

                Assert.Equal<int>(2, pages.Count);
                Assert.Equal<int>(1, pages[0].ID);
                Assert.Equal<int>(2, pages[1].ID);
            });
        }

        #endregion Methods
    }
}