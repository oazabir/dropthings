namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;

    public interface IWidgetRepository
    {
        #region Methods

        List<Widget> GetAllWidgets();

        System.Collections.Generic.List<Dropthings.DataAccess.Widget> GetAllWidgets(Dropthings.DataAccess.Enumerations.WidgetTypeEnum widgetType);

        System.Collections.Generic.List<Dropthings.DataAccess.Widget> GetDefaultWidgetsByRole(string userName, Dropthings.DataAccess.Enumerations.WidgetTypeEnum widgetType, bool isDefault);

        Dropthings.DataAccess.Widget GetWidgetById(int id);

        System.Collections.Generic.List<Dropthings.DataAccess.Widget> GetWidgetByIsDefault(bool isDefault);

        System.Collections.Generic.List<Dropthings.DataAccess.Widget> GetWidgetsByRole(string userName, Dropthings.DataAccess.Enumerations.WidgetTypeEnum widgetType);

        Widget Insert(Action<Widget> populate);

        void Update(Widget widget, Action<Widget> detach, Action<Widget> postAttachUpdate);
        
        #endregion Methods
    }
}