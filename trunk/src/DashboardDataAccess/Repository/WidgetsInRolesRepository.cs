namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;

    public class WidgetsInRolesRepository : Dropthings.DataAccess.Repository.IWidgetsInRolesRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public WidgetsInRolesRepository(IDropthingsDataContext database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public void Delete(int id)
        {
            _database.DeleteByPK<WidgetsInRole, int>(DropthingsDataContext.SubsystemEnum.Widget, id);
        }

        public void Delete(WidgetsInRole page)
        {
            _database.Delete<WidgetsInRole>(DropthingsDataContext.SubsystemEnum.Widget, page);
        }

        public List<WidgetsInRole> GetWidgetsInRoleByWidgetId(int widgetId)
        {
            return _database.GetList<WidgetsInRole, int>(DropthingsDataContext.SubsystemEnum.Widget, widgetId, LinqQueries.CompiledQuery_GetWidgetsInRoleByWidgetId);
        }

        public List<WidgetsInRole> GetWidgetsInRolesByRoleName(int widgetId, string roleName)
        {
            return _database.GetList<WidgetsInRole, int, string>(DropthingsDataContext.SubsystemEnum.Widget, widgetId, roleName, LinqQueries.CompiledQuery_GetWidgetsInRolesByRoleName);
        }

        public WidgetsInRole Insert(Action<WidgetsInRole> populate)
        {
            return _database.Insert<WidgetsInRole>(DropthingsDataContext.SubsystemEnum.Widget, populate);
        }

        public void Update(WidgetsInRole page, Action<WidgetsInRole> detach, Action<WidgetsInRole> postAttachUpdate)
        {
            _database.UpdateObject<WidgetsInRole>(DropthingsDataContext.SubsystemEnum.Widget, page, detach, postAttachUpdate);
        }

        #endregion Methods
    }
}