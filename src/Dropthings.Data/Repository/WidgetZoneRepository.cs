namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class WidgetZoneRepository : Dropthings.Data.Repository.IWidgetZoneRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public WidgetZoneRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public string GetWidgetZoneOwnerName(int widgetZoneId)
        {            
            return _database.Query(
                CompiledQueries.WidgetQueries.GetWidgetZoneOwnerName, 
                widgetZoneId)
                .First();
        }

        public void Delete(WidgetZone widgetZone)
        {
            _cacheResolver.Remove(CacheKeys.WidgetZoneKeys.WidgetInstancesInWidgetZone(widgetZone.ID));            
            _database.Delete<WidgetZone>(widgetZone);
        }

        public WidgetZone GetWidgetZoneById(int widgetZoneId)
        {
            return AspectF.Define
                .Cache<WidgetZone>(_cacheResolver, CacheKeys.WidgetZoneKeys.WidgetZone(widgetZoneId))
                .Return<WidgetZone>(() =>
                    _database.Query(
                        CompiledQueries.WidgetQueries.GetWidgetZoneById, widgetZoneId)
                        .First());
        }

        public WidgetZone GetWidgetZoneByPageId_ColumnNo(int pageId, int columnNo)
        {
            return AspectF.Define
                .Cache<WidgetZone>(_cacheResolver, CacheKeys.PageKeys.WidgetZoneByPageIdColumnNo(pageId, columnNo))
                .Return<WidgetZone>(() =>
                    _database.Query(
                        CompiledQueries.WidgetQueries.GetWidgetZoneByPageId_ColumnNo, pageId, columnNo)
                        .First()
                    );
        }

        public WidgetZone Insert(WidgetZone zone)
        {
            return _database.Insert<WidgetZone>(zone);
        }

        public void Update(WidgetZone zone)
        {
            _database.Update<WidgetZone>(zone);
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