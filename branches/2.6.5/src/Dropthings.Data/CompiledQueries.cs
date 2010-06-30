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

            public static readonly Func<DropthingsDataContext, Enumerations.WidgetType, IQueryable<Widget>> GetAllWidgets_ByWidgetType =
                Compile<Enumerations.WidgetType, Widget>((dc, widgetType) =>
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
                    from widgetsInRole in dc.WidgetsInRolesSet.Include("Widget").Include("AspNetRole")
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
            public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> GetWidgetInstancesByTabId =
                Compile<int, WidgetInstance>((dc, TabId) =>
                    from Tab in dc.Tabs
                    from column in Tab.Column
                    from widgetInstance in column.WidgetZone.WidgetInstance
                    where Tab.ID == TabId
                    orderby widgetInstance.OrderNo
                    select widgetInstance
                );

            public static readonly Func<DropthingsDataContext, int, int, IQueryable<WidgetZone>> GetWidgetZoneByTabId_ColumnNo =
                Compile<int, int, WidgetZone>((dc, TabId, columnNo) =>
                    from Tab in dc.Tabs
                    from column in Tab.Column
                    where Tab.ID == TabId && column.ColumnNo == columnNo
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
                    select widgetInstance.WidgetZone.Column.FirstOrDefault().Tab.AspNetUser.LoweredUserName
                );


            public static readonly Func<DropthingsDataContext, int, IQueryable<string>> GetWidgetZoneOwnerName =
                Compile<int, string>((dc, widgetZoneId) =>
                    from widgetZone in dc.WidgetZones
                    where widgetZone.ID == widgetZoneId
                    select widgetZone.Column.FirstOrDefault().Tab.AspNetUser.LoweredUserName
                );

        }

        public class TabQueries
        {
            public static readonly Func<DropthingsDataContext, int, IQueryable<string>> GetTabOwnerName =
                Compile<int, string>((dc, TabId) =>
                    from p in dc.Tabs
                    where p.ID == TabId
                    select p.AspNetUser.LoweredUserName
                );

            public static readonly Func<DropthingsDataContext, int, IQueryable<Column>> GetColumnsByTabId =
                Compile<int, Column>((dc, TabId) =>
                    from column in dc.Columns.Include("WidgetZone").Include("Tab")
                    where column.Tab.ID == TabId
                    orderby column.ColumnNo
                    select column
                );

            public static readonly Func<DropthingsDataContext, int, IQueryable<Column>> GetColumnById =
                Compile<int, Column>((dc, columnId) =>
                    from column in dc.Columns.Include("WidgetZone").Include("Tab")
                    where column.ID == columnId
                    select column
                );

            public static readonly Func<DropthingsDataContext, int, int, IQueryable<Column>> GetColumnByTabId_ColumnNo =
                Compile<int, int, Column>((dc, TabId, columnNo) =>
                    from column in dc.Columns.Include("WidgetZone").Include("Tab")
                    where column.Tab.ID == TabId && column.ColumnNo == columnNo 
                    select column
                );

            public static readonly Func<DropthingsDataContext, int, IQueryable<Tab>> GetTabById =
                Compile<int, Tab>((dc, id) =>
                    from Tab in dc.Tabs.Include("AspNetUser")
                    where Tab.ID == id
                    select Tab
                );

            //public static readonly Func<DropthingsDataContext, Guid, IQueryable<Tab>> GetOverridableStartTabByUser =
            //    Compile<Guid, Tab>((dc, userId) =>
            //        from user in dc.AspNetUsers
            //        from Tab in dc.Tabs.Include("AspNetUsers")
            //        where user.UserId == userId && Tab.ServeAsStartTabAfterLogin.GetValueOrDefault()
            //        select Tab
            //    );
            
            public static readonly Func<DropthingsDataContext, Guid, IQueryable<Tab>> GetTabsByUserId =
                Compile<Guid, Tab>((dc, userId) =>
                    from Tab in dc.Tabs.Include("AspNetUser")
                    where Tab.AspNetUser.UserId == userId
                    orderby Tab.OrderNo
                    select Tab
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
            public static readonly Func<DropthingsDataContext, IQueryable<AspNetRole>> GetAllRole =
                Compile<AspNetRole>((dc) =>
                    from role in dc.AspNetRoles
                    select role
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<AspNetRole>> GetRoleByRoleName =
                Compile<string, AspNetRole>((dc, roleName) =>
                    from role in dc.AspNetRoles
                    where role.RoleName == roleName
                    select role
                );

            public static readonly Func<DropthingsDataContext, Guid, IQueryable<AspNetRole>> GetRoleByRoleId =
                Compile<Guid, AspNetRole>((dc, roleId) =>
                    from role in dc.AspNetRoles
                    where role.RoleId == roleId
                    select role
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<RoleTemplate>> GetRoleTemplateByRoleName =
                Compile<string, RoleTemplate>((dc, roleName) =>
                    from roleTemplate in dc.RoleTemplates.Include("AspNetUser")
                    where roleTemplate.AspNetRole.RoleName == roleName
                    select roleTemplate
                );

            public static readonly Func<DropthingsDataContext, IQueryable<RoleTemplate>> GetRoleTemplates =
                Compile<RoleTemplate>((dc) =>
                    from roleTemplate in dc.RoleTemplates.Include("AspNetUser")
                    select roleTemplate
                );

            public static readonly Func<DropthingsDataContext, Guid, IQueryable<RoleTemplate>> GetRoleTemplatesByUserId =
                Compile<Guid, RoleTemplate>((dc, userId) =>
                    from roleTemplate in dc.RoleTemplates.Include("AspNetUser")
                    where roleTemplate.AspNetUser.UserId == userId
                    orderby roleTemplate.Priority descending
                    select roleTemplate
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<RoleTemplate>> GetRoleTemplateByTemplateUserName =
                Compile<string, RoleTemplate>((dc, userName) =>
                    from roleTemplate in dc.RoleTemplates.Include("AspNetUser")
                    where roleTemplate.AspNetUser.LoweredUserName == userName
                    && roleTemplate.AspNetUser.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    select roleTemplate
                );
        }



        public class UserQueries
        {
            public static readonly Func<DropthingsDataContext, Guid, IQueryable<AspNetRole>> GetRolesOfUser =
                Compile<Guid, AspNetRole>((dc, userGuid) =>
                    from user in dc.AspNetUsers
                    from roles in user.AspNetRoles
                    where user.UserId == userGuid
                    select roles
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<Guid>> GetUserGuidFromUserName =
                Compile<string, Guid>((dc, userName) =>
                    from u in dc.AspNetUsers
                    where u.LoweredUserName == userName && u.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    select u.UserId
                );

            public static readonly Func<DropthingsDataContext, string, IQueryable<AspNetUser>> GetUserFromUserName =
                Compile<string, AspNetUser>((dc, userName) =>
                    from u in dc.AspNetUsers
                    where u.LoweredUserName == userName && u.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    select u
                );

            public static readonly Func<DropthingsDataContext, Guid, IQueryable<AspNetUser>> GetUserByUserGuid =
                Compile<Guid, AspNetUser>((dc, userGuid) =>
                    from u in dc.AspNetUsers
                    where u.UserId == userGuid && u.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    select u
                );

            public static readonly Func<DropthingsDataContext, Guid, IQueryable<UserSetting>> GetUserSettingByUserGuid =
                Compile<Guid, UserSetting>((dc, userGuid) =>
                    from u in dc.UserSettings.Include("AspNetUser").Include("CurrentTab")
                    where u.AspNetUser.UserId == userGuid 
                    select u
                );
        }


        //public static readonly Func<string, Enumerations.WidgetTypeEnum, IQueryable<Widget>> GetWidgetsByRole =
        //        Compile<string, Enumerations.WidgetTypeEnum, Widget>((dc, username, widgetType) =>
        //            from widget in dc.Widget
        //            join widgetsInRoles in dc.WidgetsInRoles on widget.ID equals widgetsInRoles.WidgetId
        //            join roles in dc.AspNetRoles on widgetsInRoles.RoleId equals roles.RoleId
        //            join userinrole in dc.AspNetUsersInRoles on roles.RoleId equals userinrole.RoleId
        //            join users in dc.AspNetUsers on userinrole.UserId equals users.UserId                    
        //            where (users.LoweredUserName == username && users.ApplicationId == DropthingsDataContext2.ApplicationGuid) && (widget.WidgetType == (int)widgetType)
        //            orderby widget.OrderNo
        //            group widget by widget.ID into w                    
        //            select w.First()
        //        );

        //public static readonly Func<DropthingsDataContext, string, Enumerations.WidgetTypeEnum, bool, IQueryable<Widget>> GetDefaultWidgetsByRole =
        //        Compile<string, Enumerations.WidgetTypeEnum, bool, IQueryable<Widget>>((dc, username, widgetType, isDefault) =>
        //            from widget in dc.Widgets
        //            join widgetsInRoles in dc.WidgetsInRoles on widget.ID equals widgetsInRoles.WidgetId
        //            join roles in dc.AspNetRoles on widgetsInRoles.RoleId equals roles.RoleId
        //            join userinrole in dc.AspNetUsersInRoles on roles.RoleId equals userinrole.RoleId
        //            join users in dc.AspNetUsers on userinrole.UserId equals users.UserId                    
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
        //        join user in dc.AspNetUsers 
        //            on column.Tab.AspNetUser.LoweredUserName equals user.LoweredUserName
        //        join usersInRoles in dc.AspNetUsersInRoles 
        //            on new {user.UserId, widgetsInRoles.RoleId} equals new {usersInRoles.UserId, usersInRoles.RoleId}
        //        where widgetInstance.Id == widgetInstanceId 
        //            && widgetsInRoles.RoleId != roleId
        //            && column.Tab.AspNetUser.ApplicationId == DropthingsDataContext2.ApplicationGuid
                    
        //        select widgetInstance.Id
        //);

        /*
        public static readonly Func<DropthingsDataContext, int, int, IQueryable<WidgetInstance>> GetWidgetInstancesByTabId_ColumnNo =
            Compile<int, int, IQueryable<WidgetInstance>>((dc, TabId, columnNo) =>
                from widgetInstance in dc.WidgetInstances
                join column in dc.Columns on widgetInstance.WidgetZoneId equals column.WidgetZoneId
                where column.TabId == TabId && column.ColumnNo == columnNo
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
        public static readonly Func<DropthingsDataContext, int, IQueryable<WidgetInstance>> GetWidgetInstancesByTabId_With_Widget =
            Compile<int, IQueryable<WidgetInstance>>((dc, TabId) =>
                from widgetInstance in dc.WidgetInstances
                join column in dc.Columns on widgetInstance.WidgetZoneId equals column.WidgetZoneId
                where column.TabId == TabId
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

        

        #region Redundant Tab related queries

        // OMAR: Do not create queries for every possible WHERE clause combination.
        // All such queries results in database hits. Does not benefit from the caching.
        // We get all users Tabs cached and we can retrive those Tabs from cache and
        // do further filtering. That's faster than hitting database.
        //public static readonly Func<DropthingsDataContext, Guid, IQueryable<Tab>> GetLockedTabsByUserId =
        //    Compile<Guid, Tab>((dc, userId) =>
        //        from Tab in dc.Tab
        //        where Tab.AspNetUser.UserId == userId && Tab.IsLocked
        //        orderby Tab.OrderNo
        //        select Tab
        //    );

        //public static readonly Func<DropthingsDataContext, Guid, bool, IQueryable<Tab>> GetLockedTabs_ByUserId_DownForMaintenence =
        //    Compile<Guid, bool, Tab>((dc, userId, isDownForMaintenance) =>
        //        from Tab in dc.Tab
        //        where Tab.AspNetUser.UserId == userId && Tab.IsLocked && Tab.IsDownForMaintenance == isDownForMaintenance
        //        orderby Tab.OrderNo
        //        select Tab
        //    );

        //public static readonly Func<DropthingsDataContext, Guid, IQueryable<Tab>> GetTabsWhichIsDownForMaintenanceByUserId =
        //    Compile<Guid, Tab>((dc, userId) =>
        //        from Tab in dc.Tab
        //        where Tab.AspNetUser.UserId == userId && Tab.IsDownForMaintenance
        //        orderby Tab.OrderNo
        //        select Tab
        //    );

        //public static readonly Func<DropthingsDataContext, Guid, int, IQueryable<Tab>> GetTabsOfUserAfterPosition =
        //    Compile<Guid, int, Tab>((dc, userId, position) =>
        //        from Tab in dc.Tab
        //        where Tab.UserId == userId && Tab.OrderNo > position
        //        orderby Tab.OrderNo
        //        select Tab
        //    );

        //public static readonly Func<DropthingsDataContext, Guid, int, IQueryable<Tab>> GetTabsOfUserFromPosition =
        //    Compile<Guid, int, IQueryable<Tab>>((dc, userId, position) =>
        //        from Tab in dc.Tabs
        //        where Tab.UserId == userId && Tab.OrderNo >= position
        //        orderby Tab.OrderNo
        //        select Tab
        //    );

        
        #endregion

        //public static readonly Func<DropthingsDataContext, Guid, IQueryable<int>> GetTabIdByUserGuid =
        //    Compile<Guid, IQueryable<int>>((dc, userGuid) =>
        //        from Tab in dc.Tabs
        //        where Tab.UserId == userGuid
        //        select Tab.ID
        //    );


        public struct TabIdColumnNoPosition
        {
            public int TabId;
            public int ColumnNo;
            public int Position;
        }

        public struct WidgetZonePosition
        {
            public int WidgetZoneId;
            public int Position;
        }

        /*
        public static readonly Func<DropthingsDataContext, TabIdColumnNoPosition, IQueryable<WidgetInstance>> GetWidgetInstanceOnTabColumnAfterPosition =
            Compile<TabIdColumnNoPosition, IQueryable<WidgetInstance>>((dc, param) =>
                from wi in dc.WidgetInstances
                join column in dc.Columns on wi.WidgetZoneId equals column.WidgetZoneId
                where column.TabId == param.TabId && column.ColumnNo == param.ColumnNo && wi.OrderNo > param.Position
                orderby wi.OrderNo
                select wi
            );

        public static readonly Func<DropthingsDataContext, TabIdColumnNoPosition, IQueryable<WidgetInstance>> GetWidgetInstanceOnTabColumnFromPosition =
            Compile<TabIdColumnNoPosition, IQueryable<WidgetInstance>>((dc, param) =>
                from wi in dc.WidgetInstances
                join column in dc.Columns on wi.WidgetZoneId equals column.WidgetZoneId
                where column.TabId == param.TabId && column.ColumnNo == param.ColumnNo && wi.OrderNo >= param.Position
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
        //            join roles in dc.AspNetRoles on w.RoleId equals roles.RoleId
        //            where (w.WidgetId == widgetId) && (roles.RoleName == roleName)
        //            select w
        //        );

        // OMAR: These are evil queries. System will blow up when you will call these queries.
        // You can never think of loading the entire user table in memory to do anything.
        // It's a web application man! 
        //public static readonly Func<DropthingsDataContext, Expression<Func<AspNetMembership, object>>, IQueryable<AspNetMembership>> GetAspnetMember =
        //    Compile<Expression<Func<AspNetMembership, object>>, IQueryable<AspNetMembership>>((dc, orderSelector) =>
        //        dc.AspNetMemberships.OrderBy(orderSelector)
        //    );

        //public static readonly Func<DropthingsDataContext, string, IQueryable<AspNetMembership>> GetMembersInRole =
        //        Compile<string, IQueryable<AspNetMembership>>((dc, rolename) =>
        //            from member in dc.AspNetMemberships
        //            join user in dc.AspNetUsers on member.UserId equals user.UserId
        //            join usersInRole in dc.AspNetUsersInRoles on user.UserId equals usersInRole.UserId
        //            join roles in dc.AspNetRoles on usersInRole.RoleId equals roles.RoleId
        //            where rolename == roles.RoleName
        //            select member
        //        );
        //public static readonly Func<DropthingsDataContext, string, IQueryable<AspNetMembership>> GetMembersInRoleCount =
        //        Compile<string, IQueryable<AspNetMembership>>((dc, rolename) =>
        //            from member in dc.AspNetMemberships
        //            join user in dc.AspNetUsers on member.UserId equals user.UserId
        //            join usersInRole in dc.AspNetUsersInRoles on user.UserId equals usersInRole.UserId
        //            join roles in dc.AspNetRoles on usersInRole.RoleId equals roles.RoleId
        //            where rolename == roles.RoleName
        //            select member
        //        );
    }
}
