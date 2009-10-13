namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;

    public interface IWidgetInstanceRepository
    {
        #region Methods

        void Delete(int id);

        void Delete(WidgetInstance page);

        Dropthings.DataAccess.WidgetInstance GetWidgetInstanceById(int id);

        //System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance> GetWidgetInstanceOnWidgetZoneAfterPosition(int widgetZoneId, int position);

        //System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance> GetWidgetInstanceOnWidgetZoneFromPosition(int widgetZoneId, int position);

        string GetWidgetInstanceOwnerName(int widgetInstanceId);

        System.Collections.Generic.List<int> GetWidgetInstancesByRole(int widgetInstanceId, Guid roleId);

        System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance> GetWidgetInstancesByWidgetAndRole(int widgetId, Guid roleId);

        System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance> GetWidgetInstancesByWidgetZoneId(int widgetZoneId);

        System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance> GetWidgetInstancesByWidgetZoneIdWithWidget(int widgetZoneId);

        WidgetInstance Insert(Action<WidgetInstance> populate);

        void InsertList(List<Widget> widgets, Converter<Widget, WidgetInstance> populate);

        void Update(WidgetInstance page, Action<WidgetInstance> detach, Action<WidgetInstance> postAttachUpdate);

        void UpdateList(IEnumerable<WidgetInstance> widgetInstances, Action<WidgetInstance> detach, Action<WidgetInstance> postAttachUpdate);

        #endregion Methods
    }
}