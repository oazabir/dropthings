namespace Dropthings.DataAccess.Repository
{
    using System;

    public interface IWidgetsInRolesRepository
    {
        #region Methods

        void Delete(int id);

        void Delete(WidgetsInRole page);

        System.Collections.Generic.List<Dropthings.DataAccess.WidgetsInRole> GetWidgetsInRoleByWidgetId(int widgetId);

        System.Collections.Generic.List<Dropthings.DataAccess.WidgetsInRole> GetWidgetsInRolesByRoleName(int widgetId, string roleName);

        WidgetsInRole Insert(Action<WidgetsInRole> populate);

        void Update(WidgetsInRole page, Action<WidgetsInRole> detach, Action<WidgetsInRole> postAttachUpdate);

        #endregion Methods
    }
}