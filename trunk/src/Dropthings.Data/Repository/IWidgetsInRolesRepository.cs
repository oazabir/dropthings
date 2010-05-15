using System;
namespace Dropthings.Data.Repository
{
    public interface IWidgetsInRolesRepository : IDisposable
    {
        void Delete(Dropthings.Data.WidgetsInRoles wr);
        void Dispose();
        System.Collections.Generic.List<Dropthings.Data.WidgetsInRoles> GetWidgetsInRolesByWidgetId(int widgetId);
        Dropthings.Data.WidgetsInRoles Insert(Dropthings.Data.WidgetsInRoles wir);
        void Update(Dropthings.Data.WidgetsInRoles wr);
    }
}
