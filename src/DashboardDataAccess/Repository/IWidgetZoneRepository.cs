namespace Dropthings.DataAccess.Repository
{
    using System;

    public interface IWidgetZoneRepository
    {
        #region Methods

        void Delete(int id);

        void Delete(WidgetZone page);

        Dropthings.DataAccess.WidgetZone GetWidgetZoneById(int widgetZoneId);

        Dropthings.DataAccess.WidgetZone GetWidgetZoneByPageId_ColumnNo(int pageId, int columnNo);

        WidgetZone Insert(Action<WidgetZone> populate);

        void Update(WidgetZone page, Action<WidgetZone> detach, Action<WidgetZone> postAttachUpdate);

        string GetWidgetZoneOwnerName(int widgetZoneId);

        #endregion Methods
    }
}