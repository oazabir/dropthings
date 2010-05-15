namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class WidgetRepository : Dropthings.Data.Repository.IWidgetRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public WidgetRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods


        public void Delete(int widgetId)
        {
            _database.Delete<Widget>(new Widget { ID = widgetId });
        }


        public List<Widget> GetAllWidgets()
        {
            return AspectF.Define
                .CacheList<Widget, List<Widget>>(_cacheResolver, CacheKeys.WidgetKeys.AllWidgets(),
                w => CacheKeys.WidgetKeys.Widget(w.ID))
                .Return<List<Widget>>(() =>
                    _database.Query<Widget>(CompiledQueries.WidgetQueries.GetAllWidgets)
                    .ToList());
        }

        public List<Widget> GetAllWidgets(Enumerations.WidgetTypeEnum widgetType)
        {
            return this.GetAllWidgets().Where(w => w.WidgetType == (int)widgetType).ToList();
            //return AspectF.Define
            //    .CacheList<Widget, List<Widget>>(_cacheResolver, CacheKeys.WidgetKeys.WidgetsByType((int)widgetType),
            //    w => CacheKeys.WidgetKeys.Widget(w.ID))
            //    .Return<List<Widget>>(() =>
            //        _database.GetList<Widget, Enumerations.WidgetTypeEnum>(widgetType, CompiledQueries.WidgetQueries.GetAllWidgets_ByWidgetType));
        }

        //public List<Widget> GetDefaultWidgetsByRole(string userName, Enumerations.WidgetTypeEnum widgetType, bool isDefault)
        //{
        //    // TODO: Cache this call. But cache key is complicated, it needs to be based on user's Roles and Widget Type+IsDefault
        //    return _database.GetList<Widget, string, Enumerations.WidgetTypeEnum, bool>(
        //        userName, widgetType, isDefault, 
        //        CompiledQueries.WidgetQueries.GetDefaultWidgetsByRole)
        //        .Select(w => w.Detach()).ToList();
        //}

        public Widget GetWidgetById(int id)
        {
            return AspectF.Define
                .Cache<Widget>(_cacheResolver, CacheKeys.WidgetKeys.Widget(id))
                .Return<Widget>(() =>
                    _database.Query<int, Widget>(CompiledQueries.WidgetQueries.GetWidgetById, id)
                    .First());
        }

        public List<Widget> GetWidgetByIsDefault(bool isDefault)
        {
            return GetAllWidgets().Where(w => w.IsDefault == isDefault).ToList();
        }

        public Widget Insert(Widget w)
        {
            return _database.Insert<Widget>(w);
        }

        public void Update(Widget wi)
        {
            _database.Update<Widget>(wi);
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