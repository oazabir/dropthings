using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using OmarALZabir.AspectF;

namespace Dropthings.Util
{
    public class CacheKeys
    {
        public static class WidgetKeys
        {
            public static string AllWidgets()
            {
                return "AllWidgets";
            }

            public static string[] AllWidgetIdBasedKeys(int widgetId)
            {
                return new string[] { Widget(widgetId), WidgetsInRolesKeys.WidgetsInRolesByWidgetId(widgetId) };
            }

            public static string Widget(int widgetId)
            {
                return "Widget." + widgetId;
            }

            public static string DefaultWidgets()
            {
                return "DefaultWidgets";
            }

            public static string[] AllWidgetsKeys()
            {
                return new string[] 
                { 
                    AllWidgets(), 
                    DefaultWidgets() 
                };
            }
        }

        public static class WidgetInstanceKeys
        {
            public static string WidgetInstance(int widgetInstanceId)
            {
                return "WidgetInstance." + widgetInstanceId;
            }

            public static string WidgetInstanceOwnerName(int widgetInstanceId)
            {
                return "WidgetInstanceOwnerName." + widgetInstanceId;
            }

            public static string WidgetInstanceWithWidget(int widgetInstanceId)
            {
                return "WidgetInstanceWithWidget." + widgetInstanceId;
            }
            public static string[] AllWidgetInstanceIdBasedKeys(int widgetInstanceId)
            {
                return new string[] {
                WidgetInstance(widgetInstanceId),
                WidgetInstanceOwnerName(widgetInstanceId),
                WidgetInstanceWithWidget(widgetInstanceId)
            };
            }
        }

        public static class WidgetsInRolesKeys
        {
            public static string WidgetsInRolesByWidgetId(int widgetId)
            {
                return "WidgetsInRoles." + widgetId;
            }
        }

        public static class WidgetZoneKeys
        {
            public static string WidgetZone(int widgetZoneId)
            {
                return "WidgetZone." + widgetZoneId;
            }
            public static string WidgetInstancesInWidgetZone(int widgetZoneId)
            {
                return "WidgetInstancesInWidgetZone." + widgetZoneId;
            }

            public static string WidgetInstancesInWidgetZoneWithWidget(int widgetZoneId)
            {
                return "WidgetInstancesInWidgetZoneWithWidget." + widgetZoneId;
            }

            public static string[] AllWidgetZoneIdBasedKeys(int widgetZoneId)
            {
                return new string[] {
                    WidgetZone(widgetZoneId),
                    WidgetInstancesInWidgetZone(widgetZoneId),
                    WidgetInstancesInWidgetZoneWithWidget(widgetZoneId)
            };
            }

        }

        public class PageKeys
        {
            // TODO: This should not be used, find out who uses this
            public static string WidgetZoneByPageIdColumnNo(int pageId, int columnNo)
            {
                return "WidgetZoneByPageIdColumnNo.PageId." + pageId + ".ColumnNo." + columnNo;
            }

            public static string PageId(int pageId)
            {
                return "Page." + pageId;
            }
            public static string ColumnsInPage(int pageId)
            {
                return "ColumnsInPage." + pageId.ToString();
            }
            public static string PageOwnerName(int pageId)
            {
                return "PageOwnerName." + pageId;
            }

            public static string[] PageIdKeys(int pageId)
            {
                return new string[] 
                {
                    PageOwnerName(pageId),
                    PageId(pageId),
                    ColumnsInPage(pageId)
                };

            }
        }

        public class UserKeys
        {
            public static string UserGuidFromUserName(string userName)
            {
                return "UserGuidFromUserName." + userName;
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
            public static string RoleTemplateByUser(string userName)
            {
                return "RoleTemplateByUser." + userName;
            }
            public static string UserSettingByUserGuid(Guid userGuid)
            {
                return "UserSettingByUserGuid." + userGuid.ToString();
            }


            public static string[] AllUserNameBasedKeys(string userName)
            {
                return new string[] 
                { 
                    RoleTemplateByUser(userName),
                    UserGuidFromUserName(userName), 
                    UserFromUserName(userName) 
                };
            }


            public static string[] AllUserGuidBasedKeys(Guid userGuid)
            {
                return new string[] 
                {
                    UserSettingByUserGuid(userGuid),
                    UserFromUserGuid(userGuid), 
                    PagesOfUser(userGuid),
                    TemplateKeys.RoleTemplateByUser(userGuid),
                    RolesOfUser(userGuid)
                };
            }

            public static string RolesOfUser(Guid userGuid)
            {
                return "RolesOfUser." + userGuid;
            }
        }

        public class TemplateKeys
        {
            public static string UserTemplateSetting()
            {
                return "UserTemplateSetting";
            }

            public static string RoleTemplateByUser(Guid userGuid)
            {
                return "RoleTemplateByUser." + userGuid.ToString();
            }
        }


        public class RoleKeys
        {
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

            public static string[] AllRoleNameBasedKeys(string roleName)
            {
                return new string[] 
                {
                    RoleByRoleName(roleName),
                    RoleTemplateByRoleName(roleName)
                };
            }
        }
        
    }

    public static class CacheSetup
    {

        public static void Register()
        {

            if (ConstantHelper.DisableCache)
            {
#if MUNQ
                Services.RegisterInstance<ICache>(r => new NoCacheResolver());
#else
                Services.RegisterType<ICache, NoCacheResolver>();
#endif
            }
            else
            {
                if (ConstantHelper.EnableVelocity)
                {
#if MUNQ
                    Services.RegisterInstance<ICache>(r => new VelocityCacheResolver());
#else
                    Services.RegisterType<ICache, VelocityCacheResolver>();
#endif
                }
                else
                {
#if MUNQ
                    Services.RegisterInstance<ICache>(r => new EntlibCacheResolver());
#else
                    Services.RegisterType<ICache, EntlibCacheResolver>();
#endif
                }
            }
        }
    }
    
}
