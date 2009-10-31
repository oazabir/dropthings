using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using OmarALZabir.AspectF;

namespace Dropthings.Util
{
    public class CacheSetup
    {
        public class CacheKeys
        {
            public static string WidgetZone(int widgetZoneId)
            {
                return "WidgetZone." + widgetZoneId;
            }
            public static string UserGuidFromUserName(string userName)
            {
                return "UserGuidFromUserName." + userName;
            }

            public static string PageId(int pageId)
            {
                return "Page." + pageId;
            }

            public static string UserTemplateSetting()
            {
                return "UserTemplateSetting";
            }

            public static string UserFromUserName(string userName)
            {
                return "UserFromUserName." + userName;
            }

            public static string UserFromUserGuid(Guid userGuid)
            {
                return "UserFromUserGuid." + userGuid.ToString();
            }

            public static string PagesOfUser(Guid userGuid)
            {
                return "PagesOfUser." + userGuid.ToString();
            }

            public static string RoleTemplateByUser(Guid userGuid)
            {
                return "RoleTemplateByUser." + userGuid.ToString();
            }

            public static string ColumnsInPage(int pageId)
            {
                return "ColumnsInPage." + pageId.ToString();
            }

            public static string WidgetInstancesInWidgetZone(int widgetZoneId)
            {
                return "WidgetInstancesInWidgetZone." + widgetZoneId;
            }

            public static string WidgetInstancesInWidgetZoneWithWidget(int widgetZoneId)
            {
                return "WidgetInstancesInWidgetZoneWithWidget." + widgetZoneId;
            }

            public static string WidgetInstance(int widgetInstanceId)
            {
                return "WidgetInstance." + widgetInstanceId;
            }

            public static string WidgetInstanceOwnerName(int widgetInstanceId)
            {
                return "WidgetInstanceOwnerName." + widgetInstanceId;
            }

            public static string[] WidgetInstanceKeys(int widgetInstanceId)
            {
                return new string[] {
                    WidgetInstance(widgetInstanceId),
                    WidgetInstanceOwnerName(widgetInstanceId),
                    WidgetInstanceWithWidget(widgetInstanceId)
                };
            }

            public static string PageOwnerName(int pageId)
            {
                return "PageOwnerName." + pageId;
            }

            public static string[] PageIdKeys(int pageId)
            {
                return new string[] {
                    PageOwnerName(pageId),
                    PageId(pageId) 
                };
                    
            }

            public static string RoleByRoleName(string roleName)
            {
                return "RoleByRoleName." + roleName;
            }

            public static string AllRoles()
            {
                return "AllRoles";
            }

            public static string RoleTemplateByRoleName(string roleName)
            {
                return "RoleTemplateByRoleName." + roleName;
            }

            public static string RoleTemplateByUser(string userName)
            {
                return "RoleTemplateByUser." + userName;
            }

            public static string UserSettingByUserGuid(Guid userGuid)
            {
                return "UserSettingByUserGuid." + userGuid.ToString();
            }

            public static string AllWidgets()
            {
                return "AllWidgets";
            }

            public static string Widget(int widgetId)
            {
                return "Widget." + widgetId;
            }

            public static string DefaultWidgets()
            {
                return "DefaultWidgets";
            }

            public static string WidgetZoneByPageIdColumnNo(int pageId, int columnNo)
            {
                return "WidgetZoneByPageIdColumnNo.PageId." + pageId + ".ColumnNo." + columnNo;
            }

            public static string[] WidgetZoneKeys(int widgetZoneId)
            {
                return new string[] {
                    WidgetInstancesInWidgetZone(widgetZoneId),
                    WidgetInstancesInWidgetZoneWithWidget(widgetZoneId)
                };
            }

            public static string WidgetInstanceWithWidget(int widgetInstanceId)
            {
                return "WidgetInstanceWithWidget." + widgetInstanceId;
            }

            public static string WidgetsByType(int widgetType)
            {
                return "WidgetsByType." + widgetType;
            }
        }

        public static void Register()
        {

            bool enabled;
            if (bool.TryParse(ConfigurationManager.AppSettings["DisableCache"], out enabled)
                && enabled)
            {
                Services.RegisterType<ICache, NoCacheResolver>();
            }
            else
            {
                if (bool.TryParse(ConfigurationManager.AppSettings["EnableVelocity"], out enabled)
                    && enabled)
                {
                    Services.RegisterType<ICache, VelocityCacheResolver>();
                }
                else
                {
                    Services.RegisterType<ICache, EntlibCacheResolver>();
                }
            }
        }        
    }
}
