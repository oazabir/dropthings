namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Data;
    using OmarALZabir.AspectF;
    using Dropthings.Util;
    using System.Web.Security;
    using System.Data.Objects.DataClasses;

    /// <summary>
    /// Facade subsystem for Widgets and WidgetInstances
    /// </summary>
    partial class Facade
    {
        #region Methods

        public List<Widget> GetAllWidgets()
        {
            return this.widgetRepository.GetAllWidgets();
        }

        public WidgetInstance GetWidgetInstanceById(int widgetInstanceId)
        {
            return this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);
        }

        //public string GetWidgetInstanceOwnerName(int widgetInstanceId)
        //{
        //    return this.widgetInstanceRepository.GetWidgetInstanceOwnerName(widgetInstanceId);
        //}

        public void CreateDefaultWidgetsOnPage(string userName, int pageId)
        {
            List<Widget> defaultWidgets = null;

            defaultWidgets = (System.Web.Security.Roles.Enabled && !string.IsNullOrEmpty(userName)) ?
                this.GetWidgetList(userName, Enumerations.WidgetTypeEnum.PersonalPage).Where(w => w.IsDefault).ToList() :
                this.widgetRepository.GetWidgetByIsDefault(true);
            
            var widgetsPerColumn = (int)Math.Ceiling((float)defaultWidgets.Count / 3.0);

            var row = 0;
            var col = 0;

            var widgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(pageId, col);
            List<WidgetInstance> wis = defaultWidgets.ConvertAll<WidgetInstance>(widget =>
                {
                    var instance = new WidgetInstance();

                    instance.WidgetZone = new WidgetZone { ID = widgetZone.ID };
                    instance.OrderNo = row;
                    instance.CreatedDate = DateTime.Now;
                    instance.Expanded = true;
                    instance.Title = widget.Name;
                    instance.VersionNo = 1;
                    instance.Widget = new Widget { ID = widget.ID };
                    instance.State = widget.DefaultState;

                    row++;
                    if (row >= widgetsPerColumn)
                    {
                        row = 0;
                        col++;
                    }

                    return instance;
                });
            this.widgetInstanceRepository.InsertList(wis);
        }

        public List<Widget> GetWidgetList(Enumerations.WidgetTypeEnum widgetType)
        {
            return this.widgetRepository.GetAllWidgets(widgetType);
        }

        public List<Widget> GetWidgetList(string username, Enumerations.WidgetTypeEnum widgetType)
        {            
            var userGuid = this.GetUserGuidFromUserName(username);
            var userRoles = this.userRepository.GetRolesOfUser(userGuid);
            var widgets = this.widgetRepository.GetAllWidgets(widgetType);
            return widgets.Where(w => this.widgetsInRolesRepository.GetWidgetsInRolesByWidgetId(w.ID)
                .Exists(wr => userRoles.Exists(role => role.RoleId == wr.aspnet_Roles.RoleId))).ToList();
        }

        public bool IsWidgetInRole(int widgetId, string roleName)
        {
            List<WidgetsInRoles> widgetsInRole = this.widgetsInRolesRepository.GetWidgetsInRolesByWidgetId(widgetId);
            var roleId = this.roleRepository.GetRoleByRoleName(roleName).RoleId;
            return widgetsInRole.Exists(r => r.aspnet_Roles.RoleId == roleId);            
        }

        public IEnumerable<WidgetInstance> GetWidgetInstancesInZoneWithWidget(int widgetZoneId)
        {
            var widgetInstances = this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneIdWithWidget(widgetZoneId);
            var widgetInstacesToRemove = new List<WidgetInstance>();

            var widgetsForUser = GetWidgetList(Context.CurrentUserName, Enumerations.WidgetTypeEnum.PersonalPage);
            widgetInstances.Each(wi =>
                {
                    if (wi.Widget == default(Widget))
                        wi.Widget = this.widgetRepository.GetWidgetById(wi.Widget.ID);
                    
                    // Ensure the user has permission to see all the widgets. It's possible that
                    // roles have been revoked from the widgets after it was added on the user's page
                    if (!widgetsForUser.Exists(w => wi.Id == wi.Widget.ID))
                    {
                        // user can no longer have this widget
                        
                    }
                });

            
            

            return widgetInstances;
        }

        public WidgetInstance ExpandWidget(int widgetInstanceId, bool isExpand)
        {
            EnsureOwner(0, widgetInstanceId, 0);
            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            widgetInstance.Expanded = isExpand;

            this.widgetInstanceRepository.Update(widgetInstance);

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public void MoveWidgetInstance(int widgetInstanceId, int toZoneId, int toRowId)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            var fromZoneId = widgetInstance.WidgetZone.ID;
            
            PushDownWidgetInstancesOnWidgetZoneAfterWidget(toRowId, widgetInstanceId, toZoneId);
            ChangeWidgetInstancePosition(widgetInstanceId, toZoneId, toRowId);

            // Refresh the order numbers of all widgets on the zones
            // to reset their sequence number from 0
            ReorderWidgetInstancesOnWidgetZone(fromZoneId);
            ReorderWidgetInstancesOnWidgetZone(toZoneId);            
        }

        private void PushDownWidgetInstancesOnWidgetZoneAfterWidget(int toRowId, int widgetInstanceId, int widgetZoneId)
        {
            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            var isMovingDown = toRowId > widgetInstance.OrderNo;

            PushDownWidgetInstancesOnWidgetZone(toRowId, widgetZoneId, isMovingDown);
        }

        private void PushDownWidgetInstancesOnWidgetZone(int toRowId, int widgetZoneId, bool isMovingDown)
        {
            IEnumerable<WidgetInstance> list = this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneIdWithWidget(widgetZoneId);

            //if (isMovingDown)
            //{
            //    list = list.Where(wi => wi.OrderNo > toRowId);
            //}
            //else
            //{
            //    list = list.Where(wi => wi.OrderNo >= toRowId);
            //}

            int orderNo = toRowId + 1;

            list.Where(wi => wi.OrderNo >= toRowId)
                .OrderBy(wi => wi.OrderNo)
                .Each(wi => 
                    {
                        wi.OrderNo = orderNo++;
                    });

            this.widgetInstanceRepository.UpdateList(list);
        }

        private void ChangeWidgetInstancePosition(int widgetInstanceId, int toZoneId, int rowNo)
        {
            this.GetWidgetInstanceById(widgetInstanceId).As(wi =>
                {
                    var fromZoneId = wi.WidgetZone.ID;
                    if (fromZoneId != toZoneId)
                    {
                        // widget moving from one zone to another. Need to clear all cached
                        // instances of widgets on the source zone
                        CacheKeys.WidgetZoneKeys.AllWidgetZoneIdBasedKeys(fromZoneId)
                            .Each(key => Services.Get<ICache>().Remove(key));

                        // Change the widget zone reference to reflect the new widgetzone for the
                        // widget instance
                        var newWidgetZone = this.widgetZoneRepository.GetWidgetZoneById(toZoneId);
                        wi.WidgetZone = new WidgetZone { ID = newWidgetZone.ID };
                        wi.WidgetZoneReference = new EntityReference<WidgetZone> { EntityKey = newWidgetZone.EntityKey };

                        // The new dropped zone now has more widgets than before. So clear cache for widgets
                        // on that zone
                        CacheKeys.WidgetZoneKeys.AllWidgetZoneIdBasedKeys(toZoneId)
                                .Each(key => Services.Get<ICache>().Remove(key));
                    }

                    wi.OrderNo = rowNo; 
                    
                    this.widgetInstanceRepository.Update(wi);
                });
        }

        private void ReorderWidgetInstancesOnWidgetZone(int widgetZoneId)
        {
            var list = this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneIdWithWidget(widgetZoneId);

            int orderNo = 0;
            foreach (WidgetInstance wi in list)
            {
                wi.OrderNo = orderNo++;
            }

            this.widgetInstanceRepository.UpdateList(list);
        }

        public string GetWidgetInstanceState(int widgetInstanceId)
        {
            EnsureOwner(0, widgetInstanceId, 0);
            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);

            return widgetInstance.State;
        }

        public WidgetInstance SaveWidgetInstanceState(int widgetInstanceId, string state)
        {
            this.GetWidgetInstanceById(widgetInstanceId).As(wi =>
                {
                    wi.State = state;
                    this.widgetInstanceRepository.Update(wi);
                });

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public WidgetInstance ResizeWidgetInstance(int widgetInstanceId, int width, int height)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            this.GetWidgetInstanceById(widgetInstanceId).As(wi =>
                {
                    wi.Resized = true;
                    wi.Width = width;
                    wi.Height = height;
                    this.widgetInstanceRepository.Update(wi);
                });

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public WidgetInstance MaximizeWidget(int widgetInstanceId, bool isMaximized)
        {
            this.GetWidgetInstanceById(widgetInstanceId).As(wi =>
                {
                    wi.Maximized = isMaximized;

                    this.widgetInstanceRepository.Update(wi);
                });

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public WidgetInstance ChangeWidgetInstanceTitle(int widgetInstanceId, string newTitle)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            this.GetWidgetInstanceById(widgetInstanceId).As(wi =>
                {
                    wi.Title = newTitle;
                    this.widgetInstanceRepository.Update(wi);
                });

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public void DeleteWidgetInstance(int widgetInstanceId)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            var widgetZoneId = widgetInstance.WidgetZone.ID;
            this.widgetInstanceRepository.Delete(widgetInstance);
            ReorderWidgetInstancesOnWidgetZone(widgetZoneId);
        }

        public WidgetInstance AddWidgetInstance(int widgetId, int toRow, int columnNo, int zoneId)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);
            
            WidgetZone widgetZone;

            if (zoneId > 0)
            {
                widgetZone = this.widgetZoneRepository.GetWidgetZoneById(zoneId);
            }
            else
            {
                widgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(userSetting.Page.ID, columnNo);
            }

            PushDownWidgetInstancesOnWidgetZone(toRow, widgetZone.ID, true);

            var widget = this.widgetRepository.GetWidgetById(widgetId);

            return this.widgetInstanceRepository.Insert(new WidgetInstance
            {
                Title = widget.Name,
                WidgetZone = new WidgetZone { ID = widgetZone.ID },
                OrderNo = toRow,
                Widget = new Widget { ID = widget.ID },
                State = widget.DefaultState,
                CreatedDate = DateTime.Now,
                Expanded = true
            });
        }

        public Widget AddWidget(
            string name, 
            string url, 
            string icon,
            string description, 
            string defaultState,
            bool isDefault, 
            bool isLocked,
            int orderNo,
            string roleName,
            int widgetType)
        {
            return this.widgetRepository.Insert(new Widget
            {
                Name = name,
                Url = url,
                Description = description,
                IsDefault = isDefault,
                CreatedDate = DateTime.Now,
                Icon = icon,
                DefaultState = defaultState,
                IsLocked = isLocked,
                OrderNo = orderNo,
                RoleName = roleName,
                WidgetType = widgetType
            });
        }

        public void UpdateWidget(
            int widgetId,
            string name,
            string url,
            string icon,
            string description,
            string defaultState,
            bool isDefault,
            bool isLocked,
            int orderNo,
            string roleName,
            int widgetType)
        {
            var widget = this.widgetRepository.GetWidgetById(widgetId);
            
            widget.Icon = icon;
            widget.DefaultState = defaultState;
            widget.IsLocked = isLocked;
            widget.OrderNo = orderNo;
            widget.RoleName = roleName;
            widget.WidgetType = widgetType;
            widget.Name = name;
            widget.Url = url;
            widget.Description = description;
            widget.IsDefault = isDefault;
            widget.OrderNo = orderNo;

            this.widgetRepository.Update(widget);
        }

        public void DeleteWidget(int widgetId)
        {
            this.widgetRepository.Delete(widgetId);
        }

        #endregion Methods
    }
}