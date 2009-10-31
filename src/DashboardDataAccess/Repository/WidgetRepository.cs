namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class WidgetRepository : Dropthings.DataAccess.Repository.IWidgetRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public WidgetRepository(IDropthingsDataContext database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public List<Widget> GetAllWidgets(Enumerations.WidgetTypeEnum widgetType)
        {
            return AspectF.Define
                .CacheList<Widget, List<Widget>>(_cacheResolver, CacheSetup.CacheKeys.WidgetsByType((int)widgetType),
                w => CacheSetup.CacheKeys.Widget(w.ID))
                .Return<List<Widget>>(() =>
                    _database.GetList<Widget, Enumerations.WidgetTypeEnum>(DropthingsDataContext.SubsystemEnum.Widget, widgetType, LinqQueries.CompiledQuery_GetAllWidgets));
        }

        public List<Widget> GetDefaultWidgetsByRole(string userName, Enumerations.WidgetTypeEnum widgetType, bool isDefault)
        {
            // TODO: Cache this call. But cache key is complicated, it needs to be based on user's Roles and Widget Type+IsDefault
            return _database.GetList<Widget, string, Enumerations.WidgetTypeEnum, bool>(DropthingsDataContext.SubsystemEnum.Widget, userName, widgetType, isDefault, LinqQueries.CompiledQuery_GetDefaultWidgetsByRole);
        }

        public Widget GetWidgetById(int id)
        {
            return AspectF.Define
                .Cache<Widget>(_cacheResolver, CacheSetup.CacheKeys.Widget(id))
                .Return<Widget>(() =>
                    _database.GetSingle<Widget, int>(DropthingsDataContext.SubsystemEnum.Widget, id, LinqQueries.CompiledQuery_GetWidgetById));
        }

        public List<Widget> GetWidgetByIsDefault(bool isDefault)
        {
            return AspectF.Define
                .CacheList<Widget, List<Widget>>(_cacheResolver, CacheSetup.CacheKeys.DefaultWidgets(),
                w => CacheSetup.CacheKeys.Widget(w.ID))
                .Return<List<Widget>>(() =>
                    _database.GetList<Widget, bool>(DropthingsDataContext.SubsystemEnum.Widget, isDefault, LinqQueries.CompiledQuery_GetWidgetByIsDefault));
        }

        public List<Widget> GetWidgetsByRole(string userName, Enumerations.WidgetTypeEnum widgetType)
        {
            // TODO: Cache this call. But cache key is complicated, it needs to be based on user's Roles and Widget Type+IsDefault            
            return _database.GetList<Widget, string, Enumerations.WidgetTypeEnum>(DropthingsDataContext.SubsystemEnum.Widget, userName, widgetType, LinqQueries.CompiledQuery_GetWidgetsByRole);
        }

        #endregion Methods
    }
}