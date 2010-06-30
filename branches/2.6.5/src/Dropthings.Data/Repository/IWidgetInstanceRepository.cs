using System;
using System.Collections.Generic;
namespace Dropthings.Data.Repository
{
    public interface IWidgetInstanceRepository : IDisposable
    {
        void Delete(Dropthings.Data.WidgetInstance wi);
        void Delete(int id);
        Dropthings.Data.WidgetInstance GetWidgetInstanceById(int id);
        System.Collections.Generic.List<Dropthings.Data.WidgetInstance> GetWidgetInstanceOnWidgetZoneAfterPosition(int widgetZoneId, int position);
        System.Collections.Generic.List<Dropthings.Data.WidgetInstance> GetWidgetInstanceOnWidgetZoneFromPosition(int widgetZoneId, int position);
        string GetWidgetInstanceOwnerName(int widgetInstanceId);
        System.Collections.Generic.List<Dropthings.Data.WidgetInstance> GetWidgetInstancesByWidgetZoneIdWithWidget(int widgetZoneId);
        Dropthings.Data.WidgetInstance Insert(Dropthings.Data.WidgetInstance wi);
        void Update(Dropthings.Data.WidgetInstance wi);
        void UpdateList(System.Collections.Generic.IEnumerable<Dropthings.Data.WidgetInstance> widgetInstances);
        void InsertList(IEnumerable<WidgetInstance> wis);
    }
}
