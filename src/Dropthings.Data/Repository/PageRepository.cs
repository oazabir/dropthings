namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Linq.Expressions;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class PageRepository : Dropthings.Data.Repository.IPageRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public PageRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public Page GetPageById(int pageId)
        {
            string cacheKey = CacheKeys.PageKeys.PageId(pageId);
            object cachedPage = _cacheResolver.Get(cacheKey);
            if (null == cachedPage)
            {
                var page = _database.Query<int, Page>(
                        CompiledQueries.PageQueries.GetPageById,
                        pageId).First();
                _cacheResolver.Add(cacheKey, page);
                return page;
            }
            else
            {
                return cachedPage as Page;
            }
        }

        //public Page GetPageById(int pageId)
        //{
        //    return AspectF.Define.Cache<Page>(_cacheResolver, CacheKeys.PageId(pageId))
        //        .Return<Page>(() =>
        //            _database.GetSingle<Page, int>(pageId, CompiledQueries.PageQueries.GetPageById).Detach());
        //}        

        private void RemovePageIdDependentItems(int id)
        {
            RemoveUserPagesCollection(id);
            CacheKeys.PageKeys.PageIdKeys(id).Each(key => _cacheResolver.Remove(key));            
        }

        private void RemoveUserPagesCollection(int pageId)
        {
            var page = this.GetPageById(pageId);
            
            if (page != null)
            {
                var userGuid = page.aspnet_Users.UserId;
                _cacheResolver.Remove(CacheKeys.UserKeys.PagesOfUser(userGuid));
            }
        }

		private void RemoveUserPagesCollection(Guid userGuid)
		{
			_cacheResolver.Remove(CacheKeys.UserKeys.PagesOfUser(userGuid));
		}

        public void Delete(Page page)
        {
            RemovePageIdDependentItems(page.ID);
            _database.Delete<Page>(page);
        }

        public List<int> GetPageIdByUserGuid(Guid userGuid)
        {
            return this.GetPagesOfUser(userGuid).Select(page => page.ID).ToList();
        }

        public string GetPageOwnerName(int pageId)
        {
            return AspectF.Define
                .Cache<string>(_cacheResolver, CacheKeys.PageKeys.PageOwnerName(pageId))
                .Return<string>(() =>
                    _database.Query<int, string>(
                        CompiledQueries.PageQueries.GetPageOwnerName, pageId)
                        .First());
        }

        public List<Page> GetPagesOfUser(Guid userGuid)
        {
            return AspectF.Define
                .CacheList<Page, List<Page>>(_cacheResolver, CacheKeys.UserKeys.PagesOfUser(userGuid), page => CacheKeys.PageKeys.PageId(page.ID))
                .Return<List<Page>>(() =>
                    _database.Query<Guid, Page>(CompiledQueries.PageQueries.GetPagesByUserId, userGuid)
                    .ToList());
        }

        public List<Page> GetLockedPagesOfUser(Guid userGuid, bool isDownForMaintenenceMode)
        {
            return isDownForMaintenenceMode ? 
                this.GetPagesOfUser(userGuid).Where(page => page.IsLocked && page.IsDownForMaintenance == isDownForMaintenenceMode).ToList()
                : this.GetPagesOfUser(userGuid).Where(page => page.IsLocked).ToList();
        }

        // TODO: Remove this
        public List<Page> GetLockedPagesOfUserByMaintenenceMode(Guid userGuid, bool isInMaintenenceMode)
        {
            return GetPagesOfUser(userGuid).Where(page => page.IsDownForMaintenance == isInMaintenenceMode && page.IsLocked == true).ToList();
                //_database.GetList<Page, Guid, bool>(userGuid, isInMaintenenceMode, CompiledQueries.PageQueries.GetLockedPages_ByUserId_DownForMaintenence);
        }

        public List<Page> GetMaintenencePagesOfUser(Guid userGuid)
        {
            return GetPagesOfUser(userGuid).Where(page => page.IsDownForMaintenance == true).ToList(); 
                //_database.GetList<Page, Guid>(userGuid, CompiledQueries.PageQueries.GetPagesWhichIsDownForMaintenanceByUserId);
        }

        public Page Insert(Page page)
        {
            var user = page.aspnet_Users;
            page.aspnet_Users = null;
            var newPage = _database.Insert<aspnet_User, Page>(
                user,
                (u, p) => p.aspnet_Users = u,
                page);
            page.aspnet_Users = user;
            RemoveUserPagesCollection(newPage.aspnet_Users.UserId);
            return newPage;
        }

        public void Update(Page page)
        {
            RemovePageIdDependentItems(page.ID);
            _database.Update<Page>(page);
        }

        public void UpdateList(IEnumerable<Page> pages)
        {
            pages.Each(page => RemovePageIdDependentItems(page.ID));            
            _database.UpdateList<Page>(pages);
        }

        public Page GetFirstPageOfUser(Guid userGuid)
        {
            return GetPagesOfUser(userGuid).First();
        }

        public Page GetOverridableStartPageOfUser(Guid userGuid)
        {
            return GetPagesOfUser(userGuid).Where(page => page.ServeAsStartPageAfterLogin == true)
                .FirstOrDefault();
        }

        #endregion Methods

        #region IDisposable Members

        public void Dispose()
        {
            _database.Dispose();
        }

        #endregion
    }
}