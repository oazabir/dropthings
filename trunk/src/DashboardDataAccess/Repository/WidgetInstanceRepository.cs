namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class WidgetInstanceRepository : Dropthings.DataAccess.Repository.IWidgetInstanceRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public WidgetInstanceRepository(IDropthingsDataContext database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        private void RemoveWidgetInstanceCacheEntries(int widgetInstanceId)
        {
            CacheSetup.CacheKeys.WidgetInstanceKeys(widgetInstanceId).Each(key => _cacheResolver.Remove(key));
        }

        private void RemoveWidgetZoneCacheEntries(int widgetZoneId)
        {
            CacheSetup.CacheKeys.WidgetZoneKeys(widgetZoneId).Each(key => _cacheResolver.Remove(key));
        }

        public void Delete(int id)
        {
            RemoveWidgetInstanceCacheEntries(id);
            var widgetInstance = this.GetWidgetInstanceById(id);
            if (null != widgetInstance)
            {
                RemoveWidgetZoneCacheEntries(widgetInstance.WidgetZoneId);
                _database.DeleteByPK<WidgetInstance, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, id);
            }
        }

        public void Delete(WidgetInstance wi)
        {
            RemoveWidgetInstanceCacheEntries(wi.Id);
            RemoveWidgetZoneCacheEntries(wi.WidgetZoneId);
            _database.Delete<WidgetInstance>(DropthingsDataContext.SubsystemEnum.WidgetInstance, wi);
        }

        public WidgetInstance GetWidgetInstanceById(int id)
        {
            return AspectF.Define
                .Cache<WidgetInstance>(_cacheResolver, CacheSetup.CacheKeys.WidgetInstance(id))
                .Return<WidgetInstance>(() =>
                    _database.GetSingle<WidgetInstance, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, id, LinqQueries.CompiledQuery_GetWidgetInstanceById));
        }

        public List<WidgetInstance> GetWidgetInstanceOnWidgetZoneAfterPosition(int widgetZoneId, int position)
        {
            return this.GetWidgetInstancesByWidgetZoneId(widgetZoneId)
                .Where(wi => wi.OrderNo > position).ToList();
        }

        public List<WidgetInstance> GetWidgetInstanceOnWidgetZoneFromPosition(int widgetZoneId, int position)
        {
            return this.GetWidgetInstancesByWidgetZoneId(widgetZoneId)
                .Where(wi => wi.OrderNo >= position).ToList();
        }

        public string GetWidgetInstanceOwnerName(int widgetInstanceId)
        {
            return AspectF.Define
                .Cache<string>(_cacheResolver, CacheSetup.CacheKeys.WidgetInstanceOwnerName(widgetInstanceId))
                .Return<string>(() =>
                    _database.GetSingle<string, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceOwnerName));
        }

        public List<int> GetWidgetInstancesByRole(int widgetInstanceId, Guid roleId)
        {            
            return _database.GetList<int, int, Guid>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetInstanceId, roleId, LinqQueries.CompiledQuery_GetWidgetInstancesByRole);
        }

        public List<WidgetInstance> GetWidgetInstancesByWidgetAndRole(int widgetId, Guid roleId)
        {
            return _database.GetList<WidgetInstance, int, Guid>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetId, roleId, LinqQueries.CompiledQuery_GetAllWidgetInstancesByWidgetAndRole);
        }

        public List<WidgetInstance> GetWidgetInstancesByWidgetZoneId(int widgetZoneId)
        {
            return AspectF.Define
                .CacheList<WidgetInstance, List<WidgetInstance>>(_cacheResolver, CacheSetup.CacheKeys.WidgetInstancesInWidgetZone(widgetZoneId),
                wi => CacheSetup.CacheKeys.WidgetInstance(wi.Id))
                .Return<List<WidgetInstance>>(() =>
                    _database.GetList<WidgetInstance, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetZoneId, LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneId));
        }

        public List<WidgetInstance> GetWidgetInstancesByWidgetZoneIdWithWidget(int widgetZoneId)
        {
            return AspectF.Define
                .CacheList<WidgetInstance, List<WidgetInstance>>(_cacheResolver, CacheSetup.CacheKeys.WidgetInstancesInWidgetZoneWithWidget(widgetZoneId),
                    wi => CacheSetup.CacheKeys.WidgetInstanceWithWidget(wi.Id))
                .Return<List<WidgetInstance>>(() => _database.GetList<WidgetInstance, int>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetZoneId, LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneIdWithWidget, LinqQueries.WidgetInstance_Options_With_Widget));
        }

        public WidgetInstance Insert(Action<WidgetInstance> populate)
        {            
            var widgetInstance = _database.Insert<WidgetInstance>(DropthingsDataContext.SubsystemEnum.WidgetInstance, populate);
            RemoveWidgetZoneCacheEntries(widgetInstance.WidgetZoneId);
            return widgetInstance;
        }

        public void InsertList(List<Widget> widgets, Converter<Widget, WidgetInstance> populate)
        {
            _database.InsertList<WidgetInstance, Widget>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgets.AsEnumerable<Widget>(), populate);
        }

        public void Update(WidgetInstance wi, Action<WidgetInstance> detach, Action<WidgetInstance> postAttachUpdate)
        {
            RemoveWidgetInstanceCacheEntries(wi.Id);
            _database.UpdateObject<WidgetInstance>(DropthingsDataContext.SubsystemEnum.WidgetInstance, wi, detach, postAttachUpdate);
        }

        public void UpdateList(IEnumerable<WidgetInstance> widgetInstances, Action<WidgetInstance> detach, Action<WidgetInstance> postAttachUpdate)
        {
            widgetInstances.Each(wi => RemoveWidgetInstanceCacheEntries(wi.Id));
            _database.UpdateList<WidgetInstance>(DropthingsDataContext.SubsystemEnum.WidgetInstance, widgetInstances, detach, postAttachUpdate);
        }

        #endregion Methods
    }
}