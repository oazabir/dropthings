namespace Dropthings.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ObjectBuilder
    {
        #region Methods

        public static void BuildDefaultPage(Page p, Guid userId, string title, int layoutType, int position)
        {
            p.CreatedDate = DateTime.Now;
            p.ColumnCount = 3;
            p.VersionNo = 1;
            p.PageType = (int)Enumerations.PageTypeEnum.PersonalPage;
            p.UserId = userId;
            p.Title = title;
            p.IsLocked = false;
            p.IsDownForMaintenance = false;
            p.ServeAsStartPageAfterLogin = false;
            p.LayoutType = layoutType;
            p.OrderNo = position;
        }

        public static void BuildDefaultWidget(Widget w, string title, string url, string description, bool isDefault)
        {
            w.CreatedDate = DateTime.Now;
            w.VersionNo = 1;
            w.Name = title;
            w.Url = url;
            w.Description = description;
            w.IsDefault = isDefault;
            w.RoleName = "guest";
            w.IsLocked = false;
            w.Icon = "Widgets/RSS.gif";
            w.DefaultState = string.Empty;
        }

        public static void BuildDefaultWidgetInstance(WidgetInstance wi, string title, int widgetZoneId, int position, int widgetId, string state)
        {
            wi.CreatedDate = DateTime.Now;
            wi.VersionNo = 1;
            wi.State = string.Empty;
            wi.Expanded = true;

            wi.Title = title;
            wi.WidgetZoneId = widgetZoneId;
            wi.WidgetId = widgetId;
            wi.OrderNo = position;
            wi.State = state;
        }

        public static void BuildDefaultWidgetZone(WidgetZone zone, string title, string uniqueID, int orderNo)
        {
            zone.Title = title;
            zone.UniqueID = uniqueID;
            zone.OrderNo = orderNo;
        }

        #endregion Methods
    }
}