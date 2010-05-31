using System;
namespace Dropthings.Data.Repository
{
    public interface IWidgetRepository : IDisposable
    {
        System.Collections.Generic.List<Dropthings.Data.Widget> GetAllWidgets();
        System.Collections.Generic.List<Dropthings.Data.Widget> GetAllWidgets(Dropthings.Data.Enumerations.WidgetType widgetType);
        Dropthings.Data.Widget GetWidgetById(int id);
        System.Collections.Generic.List<Dropthings.Data.Widget> GetWidgetByIsDefault(bool isDefault);
        Dropthings.Data.Widget Insert(Dropthings.Data.Widget w);
        void Update(Dropthings.Data.Widget wi);

        void Delete(int widgetId);
    }
}
