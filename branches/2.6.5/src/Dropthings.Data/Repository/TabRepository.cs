namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Linq.Expressions;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class TabRepository : Dropthings.Data.Repository.ITabRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public TabRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public Tab GetTabById(int TabId)
        {
            string cacheKey = CacheKeys.TabKeys.TabId(TabId);
            object cachedTab = _cacheResolver.Get(cacheKey);
            if (null == cachedTab)
            {
                var Tab = _database.Query(
                        CompiledQueries.TabQueries.GetTabById,
                        TabId).First();
                _cacheResolver.Add(cacheKey, Tab);
                return Tab;
            }
            else
            {
                return cachedTab as Tab;
            }
        }

        //public Tab GetTabById(int TabId)
        //{
        //    return AspectF.Define.Cache<Tab>(_cacheResolver, CacheKeys.TabId(TabId))
        //        .Return<Tab>(() =>
        //            _database.GetSingle<Tab, int>(TabId, CompiledQueries.TabQueries.GetTabById).Detach());
        //}        

        private void RemoveTabIdDependentItems(int id)
        {
            RemoveUserTabsCollection(id);
            CacheKeys.TabKeys.TabIdKeys(id).Each(key => _cacheResolver.Remove(key));            
        }

        private void RemoveUserTabsCollection(int TabId)
        {
            var Tab = this.GetTabById(TabId);
            
            if (Tab != null)
            {
                var userGuid = Tab.AspNetUser.UserId;
                _cacheResolver.Remove(CacheKeys.UserKeys.TabsOfUser(userGuid));
            }
        }

		private void RemoveUserTabsCollection(Guid userGuid)
		{
			_cacheResolver.Remove(CacheKeys.UserKeys.TabsOfUser(userGuid));
		}

        public void Delete(Tab Tab)
        {
            RemoveTabIdDependentItems(Tab.ID);
            _database.Delete<Tab>(Tab);
        }

        public List<int> GetTabIdByUserGuid(Guid userGuid)
        {
            return this.GetTabsOfUser(userGuid).Select(Tab => Tab.ID).ToList();
        }

        public string GetTabOwnerName(int TabId)
        {
            return AspectF.Define
                .Cache<string>(_cacheResolver, CacheKeys.TabKeys.TabOwnerName(TabId))
                .Return<string>(() =>
                    _database.Query(
                        CompiledQueries.TabQueries.GetTabOwnerName, TabId)
                        .First());
        }

        public List<Tab> GetTabsOfUser(Guid userGuid)
        {
            return AspectF.Define
                .CacheList<Tab, List<Tab>>(_cacheResolver, CacheKeys.UserKeys.TabsOfUser(userGuid), Tab => CacheKeys.TabKeys.TabId(Tab.ID))
                .Return<List<Tab>>(() =>
                    _database.Query(CompiledQueries.TabQueries.GetTabsByUserId, userGuid)
                    .ToList());
        }

        public List<Tab> GetLockedTabsOfUser(Guid userGuid, bool isDownForMaintenenceMode)
        {
            return isDownForMaintenenceMode ? 
                this.GetTabsOfUser(userGuid).Where(Tab => Tab.IsLocked && Tab.IsDownForMaintenance == isDownForMaintenenceMode).ToList()
                : this.GetTabsOfUser(userGuid).Where(Tab => Tab.IsLocked).ToList();
        }

        // TODO: Remove this
        public List<Tab> GetLockedTabsOfUserByMaintenenceMode(Guid userGuid, bool isInMaintenenceMode)
        {
            return GetTabsOfUser(userGuid).Where(Tab => Tab.IsDownForMaintenance == isInMaintenenceMode && Tab.IsLocked == true).ToList();
                //_database.GetList<Tab, Guid, bool>(userGuid, isInMaintenenceMode, CompiledQueries.TabQueries.GetLockedTabs_ByUserId_DownForMaintenence);
        }

        public List<Tab> GetMaintenenceTabsOfUser(Guid userGuid)
        {
            return GetTabsOfUser(userGuid).Where(Tab => Tab.IsDownForMaintenance == true).ToList(); 
                //_database.GetList<Tab, Guid>(userGuid, CompiledQueries.TabQueries.GetTabsWhichIsDownForMaintenanceByUserId);
        }

        public Tab Insert(Tab Tab)
        {
            var user = Tab.AspNetUser;
            Tab.AspNetUser = null;
            var newTab = _database.Insert<AspNetUser, Tab>(
                user,
                (u, p) => p.AspNetUser = u,
                Tab);
            Tab.AspNetUser = user;
            RemoveUserTabsCollection(newTab.AspNetUser.UserId);
            return newTab;
        }

        public void Update(Tab Tab)
        {
            RemoveTabIdDependentItems(Tab.ID);
            _database.Update<Tab>(Tab);
        }

        public void UpdateList(IEnumerable<Tab> Tabs)
        {
            Tabs.Each(Tab => RemoveTabIdDependentItems(Tab.ID));            
            _database.UpdateList<Tab>(Tabs);
        }

        public Tab GetFirstTabOfUser(Guid userGuid)
        {
            return GetTabsOfUser(userGuid).First();
        }

        public Tab GetOverridableStartTabOfUser(Guid userGuid)
        {
            return GetTabsOfUser(userGuid).Where(Tab => Tab.ServeAsStartPageAfterLogin == true)
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