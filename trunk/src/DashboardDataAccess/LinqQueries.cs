using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Linq.Expressions;

namespace Dropthings.DataAccess
{
    public class LinqQueries
    {
        public static readonly Func<DropthingsDataContext, int, IQueryable<Widget>> CompiledQuery_GetWidgetById =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<Widget>>((dc, id) =>
                from widget in dc.Widgets
                where widget.ID == id
                select widget
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<Column>> CompiledQuery_GetColumnsByPageId =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<Column>>((dc, pageId) =>
                from column in dc.Columns
                where column.PageId == pageId
                select column
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<Column>> CompiledQuery_GetColumnById =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<Column>>((dc, columnId) =>
                from column in dc.Columns
                where column.ID == columnId
                select column
            );        

        public static readonly Func<DropthingsDataContext, int, int, IQueryable<Column>> CompiledQuery_GetColumnByPageId_ColumnNo =
            CompiledQuery.Compile<DropthingsDataContext, int, int, IQueryable<Column>>((dc, pageId, columnNo) =>
                from column in dc.Columns
                where column.PageId == pageId && column.ColumnNo == columnNo
                select column
            );

        public static readonly Func<DropthingsDataContext, Enumerations.WidgetTypeEnum, IQueryable<Widget>> CompiledQuery_GetAllWidgets =
            CompiledQuery.Compile<DropthingsDataContext, Enumerations.WidgetTypeEnum, IQueryable<Widget>>((dc, widgetType) =>
                from widget in dc.Widgets
                where widget.WidgetType == (int)widgetType
                orderby widget.OrderNo
                select widget
            );



        public static readonly Func<DropthingsDataContext, string, Enumerations.WidgetTypeEnum, IQueryable<Widget>> CompiledQuery_GetWidgetsByRole =
                CompiledQuery.Compile<DropthingsDataContext, string, Enumerations.WidgetTypeEnum, IQueryable<Widget>>((dc, username, widgetType) =>
                    from widget in dc.Widgets
                    join widgetsInRoles in dc.WidgetsInRoles on widget.ID equals widgetsInRoles.WidgetId
                    join roles in dc.aspnet_Roles on widgetsInRoles.RoleId equals roles.RoleId
                    join userinrole in dc.aspnet_UsersInRoles on roles.RoleId equals userinrole.RoleId
                    join users in dc.aspnet_Users on userinrole.UserId equals users.UserId                    
                    where (users.UserName == username) && (widget.WidgetType == (int)widgetType)
                    orderby widget.OrderNo
                    select widget
                );

        public static readonly Func<DropthingsDataContext, string, Enumerations.WidgetTypeEnum, bool, IQueryable<Widget>> CompiledQuery_GetDefaultWidgetsByRole =
                CompiledQuery.Compile<DropthingsDataContext, string, Enumerations.WidgetTypeEnum, bool, IQueryable<Widget>>((dc, username, widgetType, isDefault) =>
                    from widget in dc.Widgets
                    join widgetsInRoles in dc.WidgetsInRoles on widget.ID equals widgetsInRoles.WidgetId
                    join roles in dc.aspnet_Roles on widgetsInRoles.RoleId equals roles.RoleId
                    join userinrole in dc.aspnet_UsersInRoles on roles.RoleId equals userinrole.RoleId
                    join users in dc.aspnet_Users on userinrole.UserId equals users.UserId                    
                    where (users.UserName == username) && (widget.WidgetType == (int)widgetType) && (widget.IsDefault == isDefault)
                    orderby widget.OrderNo
                    select widget
                );

        public static readonly Func<DropthingsDataContext, bool, IQueryable<Widget>> CompiledQuery_GetWidgetByIsDefault =
            CompiledQuery.Compile<DropthingsDataContext, bool, IQueryable<Widget>>((dc, isDefault) =>
                from widget in dc.Widgets
                where widget.IsDefault == isDefault
                orderby widget.OrderNo
                select widget
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstanceById =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<WidgetInstance>>((dc, id) =>
                from widgetInstance in dc.WidgetInstances
                where widgetInstance.Id == id
                select widgetInstance
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstancesByPageId =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<WidgetInstance>>((dc, pageId) =>
                from widgetInstance in dc.WidgetInstances
                join column in dc.Columns on widgetInstance.WidgetZoneId equals column.WidgetZoneId
                where column.PageId == pageId
                orderby widgetInstance.OrderNo
                select widgetInstance
            );

        public static readonly Func<DropthingsDataContext, int, Guid, IQueryable<WidgetInstance>> CompiledQuery_GetAllWidgetInstancesByWidgetAndRole =
            CompiledQuery.Compile<DropthingsDataContext, int, Guid, IQueryable<WidgetInstance>>((dc, widgetId, roleId) =>
                from widgetInstance in dc.WidgetInstances
                join widgetsInRoles in dc.WidgetsInRoles on widgetInstance.WidgetId equals widgetsInRoles.WidgetId
                where widgetsInRoles.WidgetId == widgetId && widgetsInRoles.RoleId == roleId
                select widgetInstance
            );

        public static readonly Func<DropthingsDataContext, int, Guid, IQueryable<int>> CompiledQuery_GetWidgetInstancesByRole =
           CompiledQuery.Compile<DropthingsDataContext, int, Guid, IQueryable<int>>((dc, widgetInstanceId, roleId) =>
               from widgetInstance in dc.WidgetInstances
               join widgetsInRoles in dc.WidgetsInRoles on widgetInstance.WidgetId equals widgetsInRoles.WidgetId
               join column in dc.Columns on widgetInstance.WidgetZoneId equals column.WidgetZoneId
               join user in dc.aspnet_Users on column.Page.aspnet_User.UserName equals user.UserName
               join usersInRoles in dc.aspnet_UsersInRoles on new {user.UserId, widgetsInRoles.RoleId} equals new {usersInRoles.UserId, usersInRoles.RoleId}
               where widgetInstance.Id == widgetInstanceId && widgetsInRoles.RoleId != roleId
               select widgetInstance.Id
           );

        /*
        public static readonly Func<DropthingsDataContext, int, int, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstancesByPageId_ColumnNo =
            CompiledQuery.Compile<DropthingsDataContext, int, int, IQueryable<WidgetInstance>>((dc, pageId, columnNo) =>
                from widgetInstance in dc.WidgetInstances
                join column in dc.Columns on widgetInstance.WidgetZoneId equals column.WidgetZoneId
                where column.PageId == pageId && column.ColumnNo == columnNo
                orderby widgetInstance.OrderNo
                select widgetInstance
            );
        */

        public static readonly Func<DropthingsDataContext, int, int, IQueryable<WidgetZone>> CompiledQuery_GetWidgetZoneByPageId_ColumnNo =
            CompiledQuery.Compile<DropthingsDataContext, int, int, IQueryable<WidgetZone>>((dc, pageId, columnNo) =>
                from widgetZone in dc.WidgetZones
                join column in dc.Columns on widgetZone.ID equals column.WidgetZoneId
                where column.PageId == pageId && column.ColumnNo == columnNo
                select widgetZone
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetZone>> CompiledQuery_GetWidgetZoneById =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<WidgetZone>>((dc, zoneId) =>
                from widgetZone in dc.WidgetZones
                where widgetZone.ID == zoneId
                select widgetZone
            );
        public static readonly DataLoadOptions WidgetInstance_Options_With_Widget = 
            (new Func<DataLoadOptions>(() =>
            {
                DataLoadOptions option = new DataLoadOptions();
                option.LoadWith<WidgetInstance>(w => w.Widget);
                return option;
            }))();

        /*
        public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstancesByPageId_With_Widget =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<WidgetInstance>>((dc, pageId) =>
                from widgetInstance in dc.WidgetInstances
                join column in dc.Columns on widgetInstance.WidgetZoneId equals column.WidgetZoneId
                where column.PageId == pageId
                orderby widgetInstance.OrderNo
                select widgetInstance
            );
        */
        public static readonly Func<DropthingsDataContext, int, IQueryable<Page>> CompiledQuery_GetPageById =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<Page>>((dc, id) =>
                from page in dc.Pages
                where page.ID == id
                select page
            );

        public static readonly Func<DropthingsDataContext, Guid, IQueryable<Page>> CompiledQuery_GetOverridableStartPageByUser =
            CompiledQuery.Compile<DropthingsDataContext, Guid, IQueryable<Page>>((dc, userId) =>
                from page in dc.Pages
                where page.UserId == userId && page.ServeAsStartPageAfterLogin.GetValueOrDefault()
                select page
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstancesByWidgetZoneId =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<WidgetInstance>>((dc, widgetZoneId) =>
                from widgetInstance in dc.WidgetInstances
                where widgetInstance.WidgetZoneId == widgetZoneId
                orderby widgetInstance.OrderNo
                select widgetInstance
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstancesByWidgetZoneIdWithWidget =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<WidgetInstance>>((dc, widgetZoneId) =>
                from widgetInstance in dc.WidgetInstances
                where widgetInstance.WidgetZoneId == widgetZoneId
                orderby widgetInstance.OrderNo
                select widgetInstance
            );

        public static readonly Func<DropthingsDataContext, Guid, IQueryable<Page>> CompiledQuery_GetPagesByUserId =
            CompiledQuery.Compile<DropthingsDataContext, Guid, IQueryable<Page>>((dc, userId) =>
                from page in dc.Pages
                where page.UserId == userId
                orderby page.OrderNo
                select page                
            );

        public static readonly Func<DropthingsDataContext, Guid, IQueryable<Page>> CompiledQuery_GetLockedPagesByUserId =
            CompiledQuery.Compile<DropthingsDataContext, Guid, IQueryable<Page>>((dc, userId) =>
                from page in dc.Pages
                where page.UserId == userId && page.IsLocked
                orderby page.OrderNo
                select page
            );

        public static readonly Func<DropthingsDataContext, Guid, bool, IQueryable<Page>> CompiledQuery_GetLockedPages_ByUserId_DownForMaintenence =
            CompiledQuery.Compile<DropthingsDataContext, Guid, bool, IQueryable<Page>>((dc, userId, isDownForMaintenance) =>
                from page in dc.Pages
                where page.UserId == userId && page.IsLocked && page.IsDownForMaintenance == isDownForMaintenance
                orderby page.OrderNo
                select page
            );

        public static readonly Func<DropthingsDataContext, Guid, IQueryable<Page>> CompiledQuery_GetPagesWhichIsDownForMaintenanceByUserId =
            CompiledQuery.Compile<DropthingsDataContext, Guid, IQueryable<Page>>((dc, userId) =>
                from page in dc.Pages
                where page.UserId == userId && page.IsDownForMaintenance
                orderby page.OrderNo
                select page
            );

        public static readonly Func<DropthingsDataContext, Guid, int, IQueryable<Page>> CompiledQuery_GetPagesOfUserAfterPosition =
            CompiledQuery.Compile<DropthingsDataContext, Guid, int, IQueryable<Page>>((dc, userId, position) =>
                from page in dc.Pages
                where page.UserId == userId && page.OrderNo > position
                orderby page.OrderNo
                select page
            );

        public static readonly Func<DropthingsDataContext, Guid, int, IQueryable<Page>> CompiledQuery_GetPagesOfUserFromPosition =
            CompiledQuery.Compile<DropthingsDataContext, Guid, int, IQueryable<Page>>((dc, userId, position) =>
                from page in dc.Pages
                where page.UserId == userId && page.OrderNo >= position
                orderby page.OrderNo
                select page
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<string>> CompiledQuery_GetWidgetInstanceOwnerName =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<string>>((dc, widgetInstanceId) =>
                from column in dc.Columns
                join wi in dc.WidgetInstances on column.WidgetZoneId equals wi.WidgetZoneId 
                where wi.Id == widgetInstanceId
                select column.Page.aspnet_User.LoweredUserName
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<string>> CompiledQuery_GetWidgetZoneOwnerName =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<string>>((dc, widgetZoneId) =>
                from column in dc.Columns
                join wz in dc.WidgetZones on column.WidgetZoneId equals wz.ID
                where wz.ID == widgetZoneId
                select column.Page.aspnet_User.LoweredUserName
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<string>> CompiledQuery_GetPageOwnerName =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<string>>((dc, pageId) =>
                from p in dc.Pages
                where p.ID == pageId
                select p.aspnet_User.LoweredUserName
            );

        public static readonly Func<DropthingsDataContext, string, IQueryable<Guid>> CompiledQuery_GetUserGuidFromUserName =
            CompiledQuery.Compile<DropthingsDataContext, string, IQueryable<Guid>>((dc, userName) =>
                from u in dc.aspnet_Users
                where u.LoweredUserName == userName && u.ApplicationId == DropthingsDataContext.ApplicationGuid
                select u.UserId
            );

        public static readonly Func<DropthingsDataContext, string, IQueryable<aspnet_User>> CompiledQuery_GetUserFromUserName =
            CompiledQuery.Compile<DropthingsDataContext, string, IQueryable<aspnet_User>>((dc, userName) =>
                from u in dc.aspnet_Users
                where u.LoweredUserName == userName && u.ApplicationId == DropthingsDataContext.ApplicationGuid
                select u
            );

        public static readonly Func<DropthingsDataContext, Guid, IQueryable<aspnet_User>> CompiledQuery_GetUserByUserGuid =
            CompiledQuery.Compile<DropthingsDataContext, Guid, IQueryable<aspnet_User>>((dc, userGuid) =>
                from u in dc.aspnet_Users
                where u.UserId == userGuid && u.ApplicationId == DropthingsDataContext.ApplicationGuid
                select u
            );

        public static readonly Func<DropthingsDataContext, Guid, IQueryable<UserSetting>> CompiledQuery_GetUserSettingByUserGuid =
            CompiledQuery.Compile<DropthingsDataContext, Guid, IQueryable<UserSetting>>((dc, userGuid) =>
                from u in dc.UserSettings
                where u.UserId == userGuid
                select u
            );

        public static readonly Func<DropthingsDataContext, Guid, IQueryable<int>> CompiledQuery_GetPageIdByUserGuid =
            CompiledQuery.Compile<DropthingsDataContext, Guid, IQueryable<int>>((dc, userGuid) =>
                from page in dc.Pages
                where page.UserId == userGuid
                select page.ID
            );


        public struct PageIdColumnNoPosition
        {
            public int PageId;
            public int ColumnNo;
            public int Position;
        }

        public struct WidgetZonePosition
        {
            public int WidgetZoneId;
            public int Position;
        }

        /*
        public static readonly Func<DropthingsDataContext, PageIdColumnNoPosition, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstanceOnPageColumnAfterPosition =
            CompiledQuery.Compile<DropthingsDataContext, PageIdColumnNoPosition, IQueryable<WidgetInstance>>((dc, param) =>
                from wi in dc.WidgetInstances
                join column in dc.Columns on wi.WidgetZoneId equals column.WidgetZoneId
                where column.PageId == param.PageId && column.ColumnNo == param.ColumnNo && wi.OrderNo > param.Position
                orderby wi.OrderNo
                select wi
            );

        public static readonly Func<DropthingsDataContext, PageIdColumnNoPosition, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstanceOnPageColumnFromPosition =
            CompiledQuery.Compile<DropthingsDataContext, PageIdColumnNoPosition, IQueryable<WidgetInstance>>((dc, param) =>
                from wi in dc.WidgetInstances
                join column in dc.Columns on wi.WidgetZoneId equals column.WidgetZoneId
                where column.PageId == param.PageId && column.ColumnNo == param.ColumnNo && wi.OrderNo >= param.Position
                orderby wi.OrderNo
                select wi
            );
        */

        public static readonly Func<DropthingsDataContext, WidgetZonePosition, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstanceOnWidgetZoneAfterPosition =
            CompiledQuery.Compile<DropthingsDataContext, WidgetZonePosition, IQueryable<WidgetInstance>>((dc, param) =>
                from wi in dc.WidgetInstances
                where wi.WidgetZoneId == param.WidgetZoneId && wi.OrderNo > param.Position
                orderby wi.OrderNo
                select wi
            );

        public static readonly Func<DropthingsDataContext, WidgetZonePosition, IQueryable<WidgetInstance>> CompiledQuery_GetWidgetInstanceOnWidgetZoneFromPosition =
            CompiledQuery.Compile<DropthingsDataContext, WidgetZonePosition, IQueryable<WidgetInstance>>((dc, param) =>
                from wi in dc.WidgetInstances
                where wi.WidgetZoneId == param.WidgetZoneId && wi.OrderNo >= param.Position
                orderby wi.OrderNo
                select wi
            );

        public static readonly Func<DropthingsDataContext, Guid, IQueryable<Token>> CompiledQuery_GetTokenByUniqueId =
            CompiledQuery.Compile<DropthingsDataContext, Guid, IQueryable<Token>>((dc, guid) =>
                from token in dc.Tokens
                where token.UniqueID == guid
                select token
            );

        public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetsInRole>> CompiledQuery_GetWidgetsInRoleByWidgetId =
            CompiledQuery.Compile<DropthingsDataContext, int, IQueryable<WidgetsInRole>>((dc, widgetId) =>
                from widgetsInRole in dc.WidgetsInRoles
                where widgetsInRole.WidgetId == widgetId
                select widgetsInRole
            );

        public static readonly Func<DropthingsDataContext, int, string, IQueryable<WidgetsInRole>> CompiledQuery_GetWidgetsInRolesByRoleName =
                CompiledQuery.Compile<DropthingsDataContext, int, string, IQueryable<WidgetsInRole>>((dc, widgetId, roleName) =>
                    from w in dc.WidgetsInRoles
                    join roles in dc.aspnet_Roles on w.RoleId equals roles.RoleId
                    where (w.WidgetId == widgetId) && (roles.RoleName == roleName)
                    select w
                );

        public static readonly Func<DropthingsDataContext, IQueryable<aspnet_Role>> CompiledQuery_GetAllRole =
            CompiledQuery.Compile<DropthingsDataContext, IQueryable<aspnet_Role>>((dc) =>
                from role in dc.aspnet_Roles
                select role
            );

        public static readonly Func<DropthingsDataContext, string, IQueryable<aspnet_Role>> CompiledQuery_GetRoleByRoleName =
            CompiledQuery.Compile<DropthingsDataContext, string, IQueryable<aspnet_Role>>((dc, roleName) =>
                from role in dc.aspnet_Roles
                where role.RoleName == roleName
                select role
            );

        public static readonly Func<DropthingsDataContext, string, IQueryable<RoleTemplate>> CompiledQuery_GetRoleTemplateByRoleName =
            CompiledQuery.Compile<DropthingsDataContext, string, IQueryable<RoleTemplate>>((dc, roleName) =>
                from roleTemplate in dc.RoleTemplates
                join roles in dc.aspnet_Roles on roleTemplate.RoleId equals roles.RoleId
                where roles.RoleName == roleName
                select roleTemplate
            );

        public static readonly Func<DropthingsDataContext, IQueryable<RoleTemplate>> CompiledQuery_GetRoleTemplates =
            CompiledQuery.Compile<DropthingsDataContext, IQueryable<RoleTemplate>>((dc) =>
                from roleTemplate in dc.RoleTemplates
                select roleTemplate
            );

        public static readonly Func<DropthingsDataContext, Guid, IQueryable<RoleTemplate>> CompiledQuery_GetRoleTemplatesByUserId =
            CompiledQuery.Compile<DropthingsDataContext, Guid, IQueryable<RoleTemplate>>((dc, userId) =>
                from roleTemplate in dc.RoleTemplates
                join usersInRole in dc.aspnet_UsersInRoles on roleTemplate.RoleId equals usersInRole.RoleId
                where usersInRole.UserId == userId
                orderby roleTemplate.Priority descending
                select roleTemplate
            );

        public static readonly Func<DropthingsDataContext, string, IQueryable<RoleTemplate>> CompiledQuery_GetRoleTemplateByTemplateUserName =
            CompiledQuery.Compile<DropthingsDataContext, string, IQueryable<RoleTemplate>>((dc, userName) =>
                from roleTemplate in dc.RoleTemplates
                join users in dc.aspnet_Users on roleTemplate.TemplateUserId equals users.UserId
                where users.UserName == userName
                select roleTemplate
            );

    }
}
