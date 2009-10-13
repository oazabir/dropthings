namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class WidgetZoneRepository : Dropthings.DataAccess.Repository.IWidgetZoneRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICacheResolver _cacheResolver;

        #endregion Fields

        #region Constructors

        public WidgetZoneRepository(IDropthingsDataContext database, ICacheResolver cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public string GetWidgetZoneOwnerName(int widgetZoneId)
        {            
            return _database.GetSingle<string, int>(DropthingsDataContext.SubsystemEnum.WidgetZone,
                    widgetZoneId, LinqQueries.CompiledQuery_GetWidgetZoneOwnerName);
        }

        public void Delete(int id)
        {
            _database.DeleteByPK<WidgetZone, int>(DropthingsDataContext.SubsystemEnum.WidgetZone, id);
        }

        public void Delete(WidgetZone widgetZone)
        {
            Services.Get<ICacheResolver>().Remove(CacheSetup.CacheKeys.WidgetInstancesInWidgetZone(widgetZone.ID));            
            _database.Delete<WidgetZone>(DropthingsDataContext.SubsystemEnum.WidgetZone, widgetZone);
        }

        public WidgetZone GetWidgetZoneById(int widgetZoneId)
        {
            return AspectF.Define
                .Cache<WidgetZone>(_cacheResolver, CacheSetup.CacheKeys.WidgetZone(widgetZoneId))
                .Return<WidgetZone>(() =>
                    _database.GetSingle<WidgetZone, int>(DropthingsDataContext.SubsystemEnum.WidgetZone, widgetZoneId, LinqQueries.CompiledQuery_GetWidgetZoneById));
        }

        public WidgetZone GetWidgetZoneByPageId_ColumnNo(int pageId, int columnNo)
        {
            return AspectF.Define
                .Cache<WidgetZone>(_cacheResolver, CacheSetup.CacheKeys.WidgetZoneByPageIdColumnNo(pageId, columnNo))
                .Return<WidgetZone>(() =>
                    _database.GetSingle<WidgetZone, int, int>(DropthingsDataContext.SubsystemEnum.WidgetZone, pageId, columnNo, LinqQueries.CompiledQuery_GetWidgetZoneByPageId_ColumnNo));
        }

        public WidgetZone Insert(Action<WidgetZone> populate)
        {
            return _database.Insert<WidgetZone>(DropthingsDataContext.SubsystemEnum.WidgetZone, populate);
        }

        public void Update(WidgetZone page, Action<WidgetZone> detach, Action<WidgetZone> postAttachUpdate)
        {
            _database.UpdateObject<WidgetZone>(DropthingsDataContext.SubsystemEnum.WidgetZone, page, detach, postAttachUpdate);
        }

        #endregion Methods
    }
}