namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class WidgetsInRolesRepository : Dropthings.Data.Repository.IWidgetsInRolesRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public WidgetsInRolesRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public void Delete(WidgetsInRoles wr)
        {
            _cacheResolver.Remove(CacheKeys.WidgetsInRolesKeys.WidgetsInRolesByWidgetId(wr.Widget.ID));
            _database.Delete<WidgetsInRoles>(wr);
        }

        public List<WidgetsInRoles> GetWidgetsInRolesByWidgetId(int widgetId)
        {
            return AspectF.Define
                .Cache<List<WidgetsInRoles>>(_cacheResolver, CacheKeys.WidgetsInRolesKeys.WidgetsInRolesByWidgetId(widgetId))
                .Return<List<WidgetsInRoles>>(() =>
                    _database.Query(CompiledQueries.WidgetQueries.GetWidgetsInRoleByWidgetId, widgetId)
                    .ToList());
        }

        //public List<WidgetsInRoles> GetWidgetsInRolessByRoleName(int widgetId, string roleName)
        //{
        //    return _database.GetList<WidgetsInRoles, int, string>(widgetId, roleName, CompiledQueries.WidgetQueries.GetWidgetsInRolessByRoleName);
        //}

        public WidgetsInRoles Insert(WidgetsInRoles wir)
        {
            var widget = wir.Widget;
            var role = wir.aspnet_Roles;

            wir.Widget = null;
            wir.aspnet_Roles = null;

            _database.Insert<Widget, aspnet_Role, WidgetsInRoles>(widget, role, 
                (w, wr) => wr.Widget = w,
                (r, wr) => wr.aspnet_Roles = role,
                wir);

            wir.Widget = widget;
            wir.aspnet_Roles = role;

            _cacheResolver.Remove(CacheKeys.WidgetsInRolesKeys.WidgetsInRolesByWidgetId(wir.Widget.ID));
            return wir;
        }

        public void Update(WidgetsInRoles wr)
        {
            _cacheResolver.Remove(CacheKeys.WidgetsInRolesKeys.WidgetsInRolesByWidgetId(wr.Widget.ID));
            _database.Update<WidgetsInRoles>(wr);
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