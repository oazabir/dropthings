namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class PageRepository : IPageRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public PageRepository(IDropthingsDataContext database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        private void RemovePageIdDependentItems(int id)
        {
            RemoveUserPagesCollection(id);
            CacheSetup.CacheKeys.PageIdKeys(id).Each(key => _cacheResolver.Remove(key));            
        }

        private void RemoveUserPagesCollection(int pageId)
        {
            var page = this.GetPageById(pageId);
            if (page != null)
            {
                var userGuid = page.UserId;
                _cacheResolver.Remove(CacheSetup.CacheKeys.PagesOfUser(userGuid));
            }
        }

        public void Delete(int id)
        {
            RemovePageIdDependentItems(id);            
            _database.DeleteByPK<Page, int>(DropthingsDataContext.SubsystemEnum.Page, id);
        }

        public void Delete(Page page)
        {
            RemovePageIdDependentItems(page.ID);
            _database.Delete<Page>(DropthingsDataContext.SubsystemEnum.Page, page);
        }

        public Page GetPageById(int pageId)
        {
            return AspectF.Define.Cache<Page>(_cacheResolver, CacheSetup.CacheKeys.PageId(pageId))
                .Return<Page>(() =>
                    _database.GetSingle<Page, int>(DropthingsDataContext.SubsystemEnum.Page, pageId, LinqQueries.CompiledQuery_GetPageById).Detach());
        }

        public List<int> GetPageIdByUserGuid(Guid userGuid)
        {
            return this.GetPagesOfUser(userGuid).Select(page => page.ID).ToList();
        }

        public string GetPageOwnerName(int pageId)
        {
            return AspectF.Define
                .Cache<string>(_cacheResolver, CacheSetup.CacheKeys.PageOwnerName(pageId))
                .Return<string>(() =>
                    _database.GetSingle<string, int>(DropthingsDataContext.SubsystemEnum.Page, pageId, LinqQueries.CompiledQuery_GetPageOwnerName));
        }

        public List<Page> GetPagesOfUser(Guid userGuid)
        {
            return AspectF.Define
                .CacheList<Page, List<Page>>(_cacheResolver, CacheSetup.CacheKeys.PagesOfUser(userGuid), page => CacheSetup.CacheKeys.PageId(page.ID))
                .Return<List<Page>>(() => 
                    _database.GetList<Page, Guid>(DropthingsDataContext.SubsystemEnum.Page, userGuid, LinqQueries.CompiledQuery_GetPagesByUserId)
                    .Select(p => p.Detach()).ToList());
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
                //_database.GetList<Page, Guid, bool>(DropthingsDataContext.SubsystemEnum.Page, userGuid, isInMaintenenceMode, LinqQueries.CompiledQuery_GetLockedPages_ByUserId_DownForMaintenence);
        }

        public List<Page> GetMaintenencePagesOfUser(Guid userGuid)
        {
            return GetPagesOfUser(userGuid).Where(page => page.IsDownForMaintenance == true).ToList(); 
                //_database.GetList<Page, Guid>(DropthingsDataContext.SubsystemEnum.Page, userGuid, LinqQueries.CompiledQuery_GetPagesWhichIsDownForMaintenanceByUserId);
        }

        public Page Insert(Action<Page> populate)
        {
            var newPage = _database.Insert<Page>(DropthingsDataContext.SubsystemEnum.Page, populate);
            RemoveUserPagesCollection(newPage.ID);
            return newPage.Detach();
        }

        public void Update(Page page, Action<Page> detach, Action<Page> postAttachUpdate)
        {
            RemovePageIdDependentItems(page.ID);
            _database.UpdateObject<Page>(DropthingsDataContext.SubsystemEnum.Page, page, detach, postAttachUpdate);
        }

        public void UpdateList(IEnumerable<Page> pages, Action<Page> detach, Action<Page> postAttachUpdate)
        {
            pages.Each(page => RemovePageIdDependentItems(page.ID));            
            _database.UpdateList<Page>(DropthingsDataContext.SubsystemEnum.Page, pages, detach, postAttachUpdate);
        }

        public Page GetFirstPageOfUser(Guid userGuid)
        {
            return GetPagesOfUser(userGuid).First();
        }

        public Page GetOverridableStartPageOfUser(Guid userGuid)
        {
            return GetPagesOfUser(userGuid).Where(page => page.ServeAsStartPageAfterLogin == true).FirstOrDefault();
        }

        #endregion Methods
    }
}