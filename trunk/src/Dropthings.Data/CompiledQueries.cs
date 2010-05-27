using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.Objects;
using System.Linq.Expressions;

namespace Dropthings.Data
{
    public class CompiledQueries
    {
        #region Compile Helper

        private static Func<DropthingsDataContext, IQueryable<T>> Compile<T>(
            Expression<Func<DropthingsDataContext, IQueryable<T>>> query)
        {
            return CompiledQuery.Compile<DropthingsDataContext, IQueryable<T>>(query);
        }
        private static Func<DropthingsDataContext, Arg0, IQueryable<T>> Compile<Arg0, T>(
            Expression<Func<DropthingsDataContext, Arg0, IQueryable<T>>> query)
        {
            return CompiledQuery.Compile<DropthingsDataContext, Arg0, IQueryable<T>>(query);
        }
        private static Func<DropthingsDataContext, Arg0, Arg1, IQueryable<T>> Compile<Arg0, Arg1, T>(
            Expression<Func<DropthingsDataContext, Arg0, Arg1, IQueryable<T>>> query)
        {
            return CompiledQuery.Compile<DropthingsDataContext, Arg0, Arg1, IQueryable<T>>(query);
        }
        private static Func<DropthingsDataContext, Arg0, Arg1, Arg2, IQueryable<T>> Compile<Arg0, Arg1, Arg2, T>(
            Expression<Func<DropthingsDataContext, Arg0, Arg1, Arg2, IQueryable<T>>> query)
        {
            return CompiledQuery.Compile<DropthingsDataContext, Arg0, Arg1, Arg2, IQueryable<T>>(query);
        }

        #endregion

        public class WidgetQueries
        {
            public static readonly Func<DropthingsDataContext, int, IQueryable<Widget>> GetWidgetById =
                Compile<int, Widget>((dc, id) =>
                    from widget in dc.Widgets
                    where widget.ID == id
                    select widget
                );

            public static readonly Func<DropthingsDataContext, IQueryable<Widget>> GetAllWidgets =
                Compile(dc =>
                    from widget in dc.Widgets
                    orderby widget.OrderNo
                    select widget
                );

            public static readonly Func<DropthingsDataContext, Enumerations.WidgetTypeEnum, IQueryable<Widget>> GetAllWidgets_ByWidgetType =
                Compile<Enumerations.WidgetTypeEnum, Widget>((dc, widgetType) =>
                    from widget in dc.Widgets
                    where widget.WidgetType == (int)widgetType
                    orderby widget.OrderNo
                    select widget
                );

            public static readonly Func<DropthingsDataContext, bool, IQueryable<Widget>> GetWidgetByIsDefault =
                Compile<bool, Widget>((dc, isDefault) =>
                    from widget in dc.Widgets
                    where widget.IsDefault == isDefault
                    orderby widget.OrderNo
                    select widget
                );

            public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetsInRoles>> GetWidgetsInRoleByWidgetId =
                Compile<int, WidgetsInRoles>((dc, widgetId) =>
                    from widgetsInRole in dc.WidgetsInRolesSet.Include("Widget").Include("aspnet_Roles")
                    where widgetsInRole.Widget.ID == widgetId
                    select widgetsInRole
                );


            public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> GetWidgetInstanceById =
                Compile<int, WidgetInstance>((dc, id) =>
                    from widgetInstance in dc.WidgetInstances.Include("WidgetZone").Include("Widget")
                    where widgetInstance.Id == id
                    select widgetInstance
                );

            // TODO: See the query generated from this and ensure it's optimal
            public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> GetWidgetInstancesByPageId =
                Compile<int, WidgetInstance>((dc, pageId) =>
                    from page in dc.Pages
                    from column in page.Column
                    from widgetInstance in column.WidgetZone.WidgetInstance
                    where page.ID == pageId
                    orderby widgetInstance.OrderNo
                    select widgetInstance
                );

            public static readonly Func<DropthingsDataContext, int, int, IQueryable<WidgetZone>> GetWidgetZoneByPageId_ColumnNo =
                Compile<int, int, WidgetZone>((dc, pageId, columnNo) =>
                    from page in dc.Pages
                    from column in page.Column
                    where page.ID == pageId && column.ColumnNo == columnNo
                    select column.WidgetZone
                );

            public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetZone>> GetWidgetZoneById =
                Compile<int, WidgetZone>((dc, zoneId) =>
                    from widgetZone in dc.WidgetZones
                    where widgetZone.ID == zoneId
                    select widgetZone
                );

            public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> GetWidgetInstancesByWidgetZoneId =
                Compile<int, WidgetInstance>((dc, widgetZoneId) =>
                    from widgetInstance in dc.WidgetInstances.Include("Widget").Include("WidgetZone")
                    where widgetInstance.WidgetZone.ID == widgetZoneId
                    orderby widgetInstance.OrderNo
                    select widgetInstance
            );

            public static readonly Func<DropthingsDataContext, int, IQueryable<string>> GetWidgetInstanceOwnerName =
                Compile<int, string>((dc, widgetInstanceId) =>
                    from widgetInstance in dc.WidgetInstances.Include("Widget").Include("WidgetZone")
                    where widgetInstance.Id == widgetInstanceId
                    select widgetInstance.WidgetZone.Column.FirstOrDefault().Page.aspnet_Users.LoweredUserName
                );


            public static readonly Func<DropthingsDataContext, int, IQueryable<string>> GetWidgetZoneOwnerName =
                Compile<int, string>((dc, widgetZoneId) =>
                    from widgetZone in dc.WidgetZones
                    where widgetZone.ID == widgetZoneId
                    select widgetZone.Column.FirstOrDefault().Page.aspnet_Users.LoweredUserName
                );

        }

        public class PageQueries
        {
            public static readonly Func<DropthingsDataContext, int, IQueryable<string>> GetPageOwnerName =
                Compile<int, string>((dc, pageId) =>
                    from p in dc.Pages
                    where p.ID == pageId
                    select p.aspnet_Users.LoweredUserName
                );

            public static readonly Func<DropthingsDataContext, int, IQueryable<Column>> GetColumnsByPageId =
                Compile<int, Column>((dc, pageId) =>
                    from column in dc.Columns.Include("WidgetZone").Include("Page")
                    where column.Page.ID == pageId
                    orderby column.ColumnNo
                    select column
                );

            public static readonly Func<DropthingsDataContext, int, IQueryable<Column>> GetColumnById =
                Compile<int, Column>((dc, columnId) =>
                    from column in dc.Columns.Include("WidgetZone").Include("Page")
                    where column.ID == columnId
                    select column
                );

            public static readonly Func<DropthingsDataContext, int, int, IQueryable<Column>> GetColumnByPageId_ColumnNo =
                Compile<int, int, Column>((dc, pageId, columnNo) =>
                    from column in dc.Columns.Include("WidgetZone").Include("Page")
                    where column.Page.ID == pageId && column.ColumnNo == columnNo 
                    select column
                );

            public static readonly Func<DropthingsDataContext, int, IQueryable<Page>> GetPageById =
                Compile<int, Page>((dc, id) =>
                    from page in dc.Pages.Include("aspnet_Users")
                    where page.ID == id
                    select page
                );

            //public static readonly Func<DropthingsDataContext, Guid, IQueryable<Page>> GetOverridableStartPageByUser =
            //    Compile<Guid, Page>((dc, userId) =>
            //        from user in dc.aspnet_Users
            //        from page in dc.Pages.Include("aspnet_Users")
            //        where user.UserId == userId && page.ServeAsStartPageAfterLogin.GetValueOrDefault()
            //        select page
            //    );
            
            public static readonly Func<DropthingsDataContext, Guid, IQueryable<Page>> GetPagesByUserId =
                Compile<Guid, Page>((dc, userId) =>
                    from page in dc.Pages.Include("aspnet_Users")
                    where page.aspnet_Users.UserId == userId
                    orderby page.OrderNo
                    select page
                );

        
        }

        public class MiscQueries
        {
            public static readonly Func<DropthingsDataContext, Guid, IQueryable<Token>> GetTokenByUniqueId =
                Compile<Guid, Token>((dc, guid) =>
                    from token in dc.Tokens
                    where token.UniqueID == guid
                    select token
                );
        }

        public class RoleQueries
        {
            public static readonly Func<DropthingsDataContext, IQueryable<aspnet_Role>> GetAllRole =
                Compile<aspnet_Role>((dc) =>
                    from role in dc.aspnet_Roles
                    select role
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<aspnet_Role>> GetRoleByRoleName =
                Compile<string, aspnet_Role>((dc, roleName) =>
                    from role in dc.aspnet_Roles
                    where role.RoleName == roleName
                    select role
                );

            public static readonly Func<DropthingsDataContext, Guid, IQueryable<aspnet_Role>> GetRoleByRoleId =
                Compile<Guid, aspnet_Role>((dc, roleId) =>
                    from role in dc.aspnet_Roles
                    where role.RoleId == roleId
                    select role
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<RoleTemplate>> GetRoleTemplateByRoleName =
                Compile<string, RoleTemplate>((dc, roleName) =>
                    from roleTemplate in dc.RoleTemplates.Include("aspnet_Users")
                    where roleTemplate.aspnet_Roles.RoleName == roleName
                    select roleTemplate
                );

            public static readonly Func<DropthingsDataContext, IQueryable<RoleTemplate>> GetRoleTemplates =
                Compile<RoleTemplate>((dc) =>
                    from roleTemplate in dc.RoleTemplates.Include("aspnet_Users")
                    select roleTemplate
                );

            public static readonly Func<DropthingsDataContext, Guid, IQueryable<RoleTemplate>> GetRoleTemplatesByUserId =
                Compile<Guid, RoleTemplate>((dc, userId) =>
                    from roleTemplate in dc.RoleTemplates.Include("aspnet_Users")
                    where roleTemplate.aspnet_Users.UserId == userId
                    orderby roleTemplate.Priority descending
                    select roleTemplate
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<RoleTemplate>> GetRoleTemplateByTemplateUserName =
                Compile<string, RoleTemplate>((dc, userName) =>
                    from roleTemplate in dc.RoleTemplates.Include("aspnet_Users")
                    where roleTemplate.aspnet_Users.LoweredUserName == userName
                    && roleTemplate.aspnet_Users.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    select roleTemplate
                );
        }



        public class UserQueries
        {
            public static readonly Func<DropthingsDataContext, Guid, IQueryable<aspnet_Role>> GetRolesOfUser =
                Compile<Guid, aspnet_Role>((dc, userGuid) =>
                    from user in dc.aspnet_Users
                    from roles in user.aspnet_Roles
                    where user.UserId == userGuid
                    select roles
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<Guid>> GetUserGuidFromUserName =
                Compile<string, Guid>((dc, userName) =>
                    from u in dc.aspnet_Users
                    where u.LoweredUserName == userName && u.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    select u.UserId
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<aspnet_User>> GetUserFromUserName =
                Compile<string, aspnet_User>((dc, userName) =>
                    from u in dc.aspnet_Users
                    where u.LoweredUserName == userName && u.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    select u
                );

            public static readonly Func<DropthingsDataContext, Guid, IQueryable<aspnet_User>> GetUserByUserGuid =
                Compile<Guid, aspnet_User>((dc, userGuid) =>
                    from u in dc.aspnet_Users
                    where u.UserId == userGuid && u.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    select u
                );

            public static readonly Func<DropthingsDataContext, Guid, IQueryable<UserSetting>> GetUserSettingByUserGuid =
                Compile<Guid, UserSetting>((dc, userGuid) =>
                    from u in dc.UserSettings.Include("aspnet_Users").Include("Page")
                    where u.aspnet_Users.UserId == userGuid 
                    select u
                );
        }


        //public static readonly Func<string, Enumerations.WidgetTypeEnum, IQueryable<Widget>> GetWidgetsByRole =
        //        Compile<string, Enumerations.WidgetTypeEnum, Widget>((dc, username, widgetType) =>
        //            from widget in dc.Widget
        //            join widgetsInRoles in dc.WidgetsInRoles on widget.ID equals widgetsInRoles.WidgetId
        //            join roles in dc.aspnet_Roles on widgetsInRoles.RoleId equals roles.RoleId
        //            join userinrole in dc.aspnet_UsersInRoles on roles.RoleId equals userinrole.RoleId
        //            join users in dc.aspnet_Users on userinrole.UserId equals users.UserId                    
        //            where (users.LoweredUserName == username && users.ApplicationId == DropthingsDataContext2.ApplicationGuid) && (widget.WidgetType == (int)widgetType)
        //            orderby widget.OrderNo
        //            group widget by widget.ID into w                    
        //            select w.First()
        //        );

        //public static readonly Func<DropthingsDataContext, string, Enumerations.WidgetTypeEnum, bool, IQueryable<Widget>> GetDefaultWidgetsByRole =
        //        Compile<string, Enumerations.WidgetTypeEnum, bool, IQueryable<Widget>>((dc, username, widgetType, isDefault) =>
        //            from widget in dc.Widgets
        //            join widgetsInRoles in dc.WidgetsInRoles on widget.ID equals widgetsInRoles.WidgetId
        //            join roles in dc.aspnet_Roles on widgetsInRoles.RoleId equals roles.RoleId
        //            join userinrole in dc.aspnet_UsersInRoles on roles.RoleId equals userinrole.RoleId
        //            join users in dc.aspnet_Users on userinrole.UserId equals users.UserId                    
        //            where (users.LoweredUserName == username && users.ApplicationId == DropthingsDataContext2.ApplicationGuid) && (widget.WidgetType == (int)widgetType) && (widget.IsDefault == isDefault)
        //            orderby widget.OrderNo
        //            group widget by widget.ID into w
        //            select w.First()
        //        );

        
        // TODO: such a query should never exist in code and should never be 
        // implemented anywhere outside database. Retrieving thousands of rows
        // and then doing any operation on them is the shortest way to get yourself
        // fired.
        //public static readonly Func<DropthingsDataContext, int, Guid, IQueryable<WidgetInstance>> GetAllWidgetInstancesByWidgetAndRole =
        //    Compile<int, Guid, WidgetInstance>((dc, widgetId, roleId) =>
        //        from widgetInstance in dc.WidgetInstances
        //        join widgetsInRoles in dc.WidgetsInRoles on widgetInstance.WidgetId equals widgetsInRoles.WidgetId
        //        where widgetsInRoles.WidgetId == widgetId && widgetsInRoles.RoleId == roleId
        //        select widgetInstance
        //    );

        // TODO: such a query should never exist in code and should never be 
        // implemented anywhere outside database. Retrieving thousands of rows
        // and then doing any operation on them is the shortest way to get yourself
        // fired.
        //public static readonly Func<DropthingsDataContext, int, Guid, IQueryable<int>> GetWidgetInstancesByRole =
        //    Compile<int, Guid, IQueryable<int>>((dc, widgetInstanceId, roleId) =>
        //        from widgetInstance in dc.WidgetInstances
        //        join widgetsInRoles in dc.WidgetsInRoles 
        //            on widgetInstance.WidgetId equals widgetsInRoles.WidgetId
        //        join column in dc.Columns 
        //            on widgetInstance.WidgetZoneId equals column.WidgetZoneId
        //        join user in dc.aspnet_Users 
        //            on column.Page.aspnet_User.LoweredUserName equals user.LoweredUserName
        //        join usersInRoles in dc.aspnet_UsersInRoles 
        //            on new {user.UserId, widgetsInRoles.RoleId} equals new {usersInRoles.UserId, usersInRoles.RoleId}
        //        where widgetInstance.Id == widgetInstanceId 
        //            && widgetsInRoles.RoleId != roleId
        //            && column.Page.aspnet_User.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    
        //        select widgetInstance.Id
        //);

        /*
        public static readonly Func<DropthingsDataContext, int, int, IQueryable<WidgetInstance>> GetWidgetInstancesByPageId_ColumnNo =
            Compile<int, int, IQueryable<WidgetInstance>>((dc, pageId, columnNo) =>
                from widgetInstance in dc.WidgetInstances
                join column in dc.Columns on widgetInstance.WidgetZoneId equals column.WidgetZoneId
                where column.PageId == pageId && column.ColumnNo == columnNo
                orderby widgetInstance.OrderNo
                select widgetInstance
            );
        */

        //public static readonly DataLoadOptions WidgetInstance_Options_With_Widget = 
        //    (new Func<DataLoadOptions>(() =>
        //    {
        //        DataLoadOptions option = new DataLoadOptions();
        //        option.LoadWith<WidgetInstance>(w => w.Widget);
        //        return option;
        //    }))();

        /*
        public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> GetWidgetInstancesByPageId_With_Widget =
            Compile<int, IQueryable<WidgetInstance>>((dc, pageId) =>
                from widgetInstance in dc.WidgetInstances
                join column in dc.Columns on widgetInstance.WidgetZoneId equals column.WidgetZoneId
                where column.PageId == pageId
                orderby widgetInstance.OrderNo
                select widgetInstance
            );
        */
        
        // OMAR: Do not create different queries for each variable of WHERE clause or load option.
        // This hurts cacheability. Just have one query that loads everything you need and store it
        // in cache. It's faster to retrive it from cache than making another database query to get
        // subset of what's already in cache.

        //public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> GetWidgetInstancesByWidgetZoneIdWithWidget =
        //    Compile<int, WidgetInstance>((dc, widgetZoneId) =>
        //        from widgetInstance in dc.WidgetInstances
        //        where widgetInstance.WidgetZoneId == widgetZoneId
        //        orderby widgetInstance.OrderNo
        //        select widgetInstance
        //    );

        

        #region Redundant Page related queries

        // OMAR: Do not create queries for every possible WHERE clause combination.
        // All such queries results in database hits. Does not benefit from the caching.
        // We get all users pages cached and we can retrive those pages from cache and
        // do further filtering. That's faster than hitting database.
        //public static readonly Func<DropthingsDataContext, Guid, IQueryable<Page>> GetLockedPagesByUserId =
        //    Compile<Guid, Page>((dc, userId) =>
        //        from page in dc.Page
        //        where page.aspnet_Users.UserId == userId && page.IsLocked
        //        orderby page.OrderNo
        //        select page
        //    );

        //public static readonly Func<DropthingsDataContext, Guid, bool, IQueryable<Page>> GetLockedPages_ByUserId_DownForMaintenence =
        //    Compile<Guid, bool, Page>((dc, userId, isDownForMaintenance) =>
        //        from page in dc.Page
        //        where page.aspnet_Users.UserId == userId && page.IsLocked && page.IsDownForMaintenance == isDownForMaintenance
        //        orderby page.OrderNo
        //        select page
        //    );

        //public static readonly Func<DropthingsDataContext, Guid, IQueryable<Page>> GetPagesWhichIsDownForMaintenanceByUserId =
        //    Compile<Guid, Page>((dc, userId) =>
        //        from page in dc.Page
        //        where page.aspnet_Users.UserId == userId && page.IsDownForMaintenance
        //        orderby page.OrderNo
        //        select page
        //    );

        //public static readonly Func<DropthingsDataContext, Guid, int, IQueryable<Page>> GetPagesOfUserAfterPosition =
        //    Compile<Guid, int, Page>((dc, userId, position) =>
        //        from page in dc.Page
        //        where page.UserId == userId && page.OrderNo > position
        //        orderby page.OrderNo
        //        select page
        //    );

        //public static readonly Func<DropthingsDataContext, Guid, int, IQueryable<Page>> GetPagesOfUserFromPosition =
        //    Compile<Guid, int, IQueryable<Page>>((dc, userId, position) =>
        //        from page in dc.Pages
        //        where page.UserId == userId && page.OrderNo >= position
        //        orderby page.OrderNo
        //        select page
        //    );

        
        #endregion

        //public static readonly Func<DropthingsDataContext, Guid, IQueryable<int>> GetPageIdByUserGuid =
        //    Compile<Guid, IQueryable<int>>((dc, userGuid) =>
        //        from page in dc.Pages
        //        where page.UserId == userGuid
        //        select page.ID
        //    );


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
        public static readonly Func<DropthingsDataContext, PageIdColumnNoPosition, IQueryable<WidgetInstance>> GetWidgetInstanceOnPageColumnAfterPosition =
            Compile<PageIdColumnNoPosition, IQueryable<WidgetInstance>>((dc, param) =>
                from wi in dc.WidgetInstances
                join column in dc.Columns on wi.WidgetZoneId equals column.WidgetZoneId
                where column.PageId == param.PageId && column.ColumnNo == param.ColumnNo && wi.OrderNo > param.Position
                orderby wi.OrderNo
                select wi
            );

        public static readonly Func<DropthingsDataContext, PageIdColumnNoPosition, IQueryable<WidgetInstance>> GetWidgetInstanceOnPageColumnFromPosition =
            Compile<PageIdColumnNoPosition, IQueryable<WidgetInstance>>((dc, param) =>
                from wi in dc.WidgetInstances
                join column in dc.Columns on wi.WidgetZoneId equals column.WidgetZoneId
                where column.PageId == param.PageId && column.ColumnNo == param.ColumnNo && wi.OrderNo >= param.Position
                orderby wi.OrderNo
                select wi
            );
        */

        //public static readonly Func<DropthingsDataContext, WidgetZonePosition, IQueryable<WidgetInstance>> GetWidgetInstanceOnWidgetZoneAfterPosition =
        //    Compile<WidgetZonePosition, IQueryable<WidgetInstance>>((dc, param) =>
        //        from wi in dc.WidgetInstances
        //        where wi.WidgetZoneId == param.WidgetZoneId && wi.OrderNo > param.Position
        //        orderby wi.OrderNo
        //        select wi
        //    );

        //public static readonly Func<DropthingsDataContext, WidgetZonePosition, IQueryable<WidgetInstance>> GetWidgetInstanceOnWidgetZoneFromPosition =
        //    Compile<WidgetZonePosition, IQueryable<WidgetInstance>>((dc, param) =>
        //        from wi in dc.WidgetInstances
        //        where wi.WidgetZoneId == param.WidgetZoneId && wi.OrderNo >= param.Position
        //        orderby wi.OrderNo
        //        select wi
        //    );

        //public static readonly Func<DropthingsDataContext, int, string, IQueryable<WidgetsInRole>> GetWidgetsInRolesByRoleName =
        //        Compile<int, string, IQueryable<WidgetsInRole>>((dc, widgetId, roleName) =>
        //            from w in dc.WidgetsInRoles
        //            join roles in dc.aspnet_Roles on w.RoleId equals roles.RoleId
        //            where (w.WidgetId == widgetId) && (roles.RoleName == roleName)
        //            select w
        //        );

        // OMAR: These are evil queries. System will blow up when you will call these queries.
        // You can never think of loading the entire user table in memory to do anything.
        // It's a web application man! 
        //public static readonly Func<DropthingsDataContext, Expression<Func<aspnet_Membership, object>>, IQueryable<aspnet_Membership>> GetAspnetMember =
        //    Compile<Expression<Func<aspnet_Membership, object>>, IQueryable<aspnet_Membership>>((dc, orderSelector) =>
        //        dc.aspnet_Memberships.OrderBy(orderSelector)
        //    );

        //public static readonly Func<DropthingsDataContext, string, IQueryable<aspnet_Membership>> GetMembersInRole =
        //        Compile<string, IQueryable<aspnet_Membership>>((dc, rolename) =>
        //            from member in dc.aspnet_Memberships
        //            join user in dc.aspnet_Users on member.UserId equals user.UserId
        //            join usersInRole in dc.aspnet_UsersInRoles on user.UserId equals usersInRole.UserId
        //            join roles in dc.aspnet_Roles on usersInRole.RoleId equals roles.RoleId
        //            where rolename == roles.RoleName
        //            select member
        //        );
        //public static readonly Func<DropthingsDataContext, string, IQueryable<aspnet_Membership>> GetMembersInRoleCount =
        //        Compile<string, IQueryable<aspnet_Membership>>((dc, rolename) =>
        //            from member in dc.aspnet_Memberships
        //            join user in dc.aspnet_Users on member.UserId equals user.UserId
        //            join usersInRole in dc.aspnet_UsersInRoles on user.UserId equals usersInRole.UserId
        //            join roles in dc.aspnet_Roles on usersInRole.RoleId equals roles.RoleId
        //            where rolename == roles.RoleName
        //            select member
        //        );
    }
}
