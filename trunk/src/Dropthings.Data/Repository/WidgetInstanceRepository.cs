namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class WidgetInstanceRepository : Dropthings.Data.Repository.IWidgetInstanceRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public WidgetInstanceRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        private void RemoveWidgetInstanceCacheEntries(int widgetInstanceId)
        {
            CacheKeys.WidgetInstanceKeys.AllWidgetInstanceIdBasedKeys(widgetInstanceId).Each(key => _cacheResolver.Remove(key));
        }

        private void RemoveWidgetZoneCacheEntries(int widgetZoneId)
        {
            CacheKeys.WidgetZoneKeys.AllWidgetZoneIdBasedKeys(widgetZoneId).Each(key => _cacheResolver.Remove(key));
        }

        public void Delete(int id)
        {
            RemoveWidgetInstanceCacheEntries(id);
            var widgetInstance = this.GetWidgetInstanceById(id);
            if (null != widgetInstance)
            {
                RemoveWidgetZoneCacheEntries(widgetInstance.WidgetZone.ID);
                _database.Delete<WidgetInstance>(widgetInstance);
            }
        }

        public void Delete(WidgetInstance wi)
        {
            RemoveWidgetInstanceCacheEntries(wi.Id);
            RemoveWidgetZoneCacheEntries(wi.WidgetZone.ID);
            _database.Delete<WidgetInstance>(wi);
        }

        public WidgetInstance GetWidgetInstanceById(int id)
        {
            return AspectF.Define
                .Cache<WidgetInstance>(_cacheResolver, CacheKeys.WidgetInstanceKeys.WidgetInstance(id))
                .Return<WidgetInstance>(() =>
                    _database.Query<int, WidgetInstance>(CompiledQueries.WidgetQueries.GetWidgetInstanceById, id)
                        .First());
        }

        public List<WidgetInstance> GetWidgetInstanceOnWidgetZoneAfterPosition(int widgetZoneId, int position)
        {
            return this.GetWidgetInstancesByWidgetZoneIdWithWidget(widgetZoneId)
                .Where(wi => wi.OrderNo > position).ToList();
        }

        public List<WidgetInstance> GetWidgetInstanceOnWidgetZoneFromPosition(int widgetZoneId, int position)
        {
            return this.GetWidgetInstancesByWidgetZoneIdWithWidget(widgetZoneId)
                .Where(wi => wi.OrderNo >= position).ToList();
        }

        public string GetWidgetInstanceOwnerName(int widgetInstanceId)
        {
            return AspectF.Define
                .Cache<string>(_cacheResolver, CacheKeys.WidgetInstanceKeys.WidgetInstanceOwnerName(widgetInstanceId))
                .Return<string>(() =>
                    _database.Query<int, string>(CompiledQueries.WidgetQueries.GetWidgetInstanceOwnerName, widgetInstanceId)
                    .First());
        }

        //public List<int> GetWidgetInstancesByRole(int widgetInstanceId, Guid roleId)
        //{            
        //    return _database.GetList<int, int, Guid>(widgetInstanceId, roleId, CompiledQueries.WidgetQueries.GetWidgetInstancesByRole);
        //}

        //public List<WidgetInstance> GetWidgetInstancesByWidgetAndRole(int widgetId, Guid roleId)
        //{
        //    return _database.GetList<WidgetInstance, int, Guid>(widgetId, roleId, CompiledQueries.WidgetQueries.GetAllWidgetInstancesByWidgetAndRole);
        //}

        //public List<WidgetInstance> GetWidgetInstancesByWidgetZoneId(int widgetZoneId)
        //{
        //    return AspectF.Define
        //        .CacheList<WidgetInstance, List<WidgetInstance>>(_cacheResolver, CacheKeys.WidgetZoneKeys.WidgetInstancesInWidgetZone(widgetZoneId),
        //        wi => CacheKeys.WidgetInstanceKeys.WidgetInstance(wi.Id))
        //        .Return<List<WidgetInstance>>(() =>
        //            _database.GetList<WidgetInstance, int>(widgetZoneId, CompiledQueries.WidgetQueries.GetWidgetInstancesByWidgetZoneId));
        //}

        public List<WidgetInstance> GetWidgetInstancesByWidgetZoneIdWithWidget(int widgetZoneId)
        {
            return AspectF.Define
                .CacheList<WidgetInstance, List<WidgetInstance>>(_cacheResolver, CacheKeys.WidgetZoneKeys.WidgetInstancesInWidgetZoneWithWidget(widgetZoneId),
                    wi => CacheKeys.WidgetInstanceKeys.WidgetInstanceWithWidget(wi.Id))
                .Return<List<WidgetInstance>>(() =>
                    _database.Query<int, WidgetInstance>(CompiledQueries.WidgetQueries.GetWidgetInstancesByWidgetZoneId,
                        widgetZoneId)
                   .ToList());
        }

        public WidgetInstance Insert(WidgetInstance wi)
        {
            var zone = wi.WidgetZone;
            var widget = wi.Widget;

            wi.WidgetZone = null;
            wi.Widget = null;
            
            var widgetInstance = _database.Insert<WidgetZone, Widget, WidgetInstance>(
                zone, widget,
                (z, me) => me.WidgetZone = z,
                (w, me) => me.Widget = w,
                wi);

            wi.WidgetZone = zone;
            wi.Widget = widget;

            RemoveWidgetZoneCacheEntries(widgetInstance.WidgetZone.ID);
            return widgetInstance;
        }

        public void InsertList(IEnumerable<WidgetInstance> wis)
        {
            wis.Each(wi =>
                {
                    _database.Insert<WidgetZone, Widget, WidgetInstance>(
                        wi.WidgetZone,
                        wi.Widget,
                        (zone, me) => zone.WidgetInstance.Add(me),
                        (w, me) => w.WidgetInstance.Add(me),
                        wi);
                    RemoveWidgetZoneCacheEntries(wi.WidgetZone.ID);
                });
        }

        public void Update(WidgetInstance wi)
        {
            RemoveWidgetInstanceCacheEntries(wi.Id);
            _database.Update<WidgetInstance>(wi);
        }

        public void UpdateList(IEnumerable<WidgetInstance> widgetInstances)
        {
            widgetInstances.Each(wi => 
                {
                    RemoveWidgetInstanceCacheEntries(wi.Id);
                    RemoveWidgetZoneCacheEntries(wi.WidgetZone.ID);
                });            
            
            _database.UpdateList<WidgetInstance>(widgetInstances);
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