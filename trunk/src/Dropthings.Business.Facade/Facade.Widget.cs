namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    /// <summary>
    /// Facade subsystem for Widgets and WidgetInstances
    /// </summary>
    partial class Facade
    {
        #region Methods

        public WidgetInstance GetWidgetInstanceById(int widgetInstanceId)
        {
            return this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);
        }

        public string GetWidgetInstanceOwnerName(int widgetInstanceId)
        {
            return this.widgetInstanceRepository.GetWidgetInstanceOwnerName(widgetInstanceId);
        }

        public void CreateDefaultWidgetsOnPage(string userName, int pageId)
        {
            List<Widget> defaultWidgets = null;

            defaultWidgets = (System.Web.Security.Roles.Enabled && !string.IsNullOrEmpty(userName)) ?
                this.widgetRepository.GetDefaultWidgetsByRole(userName, Enumerations.WidgetTypeEnum.PersonalPage, true) :
                this.widgetRepository.GetWidgetByIsDefault(true);

            var widgetsPerColumn = (int)Math.Ceiling((float)defaultWidgets.Count / 3.0);

            var row = 0;
            var col = 0;

            var widgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(pageId, col); 
                
            this.widgetInstanceRepository.InsertList(defaultWidgets, widget =>
            {
                var instance = new WidgetInstance();

                instance.WidgetZoneId = widgetZone.ID;
                instance.OrderNo = row;
                instance.CreatedDate = DateTime.Now;
                instance.Expanded = true;
                instance.Title = widget.Name;
                instance.VersionNo = 1;
                instance.WidgetId = widget.ID;
                instance.State = widget.DefaultState;

                row++;
                if (row >= widgetsPerColumn)
                {
                    row = 0;
                    col++;
                }

                return instance;
            });            
        }

        public List<Widget> GetWidgetList(Dropthings.DataAccess.Enumerations.WidgetTypeEnum widgetType)
        {
            return this.widgetRepository.GetAllWidgets(widgetType);
        }

        public List<Widget> GetWidgetList(string username, Dropthings.DataAccess.Enumerations.WidgetTypeEnum widgetType)
        {
            return this.widgetRepository.GetWidgetsByRole(username, widgetType);
        }

        public bool IsWidgetInRole(int widgetId, string roleName)
        {
            List<WidgetsInRole> widgetsInRole = this.widgetsInRolesRepository.GetWidgetsInRolesByRoleName(widgetId, roleName);

            return widgetsInRole.HasItems();
        }

        public IEnumerable<WidgetInstance> GetWidgetInstancesInZone(int widgetZoneId)
        {
            return this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneIdWithWidget(widgetZoneId);            
        }

        public WidgetInstance ExpandWidget(int widgetInstanceId, bool isExpand)
        {
            EnsureOwner(0, widgetInstanceId, 0);
            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.Expanded = isExpand;
                widgetInstance = wi;
            }, null);

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public void MoveWidgetInstance(int widgetInstanceId, int toZoneId, int toRowId)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            if (widgetInstance.WidgetZoneId != toZoneId)
            {
                // widget moving from one zone to another. Need to clear all cached
                // instances of widget instances on the source zone
                CacheSetup.CacheKeys.WidgetZoneKeys(widgetInstance.WidgetZoneId)
                    .Each(key => Services.Get<ICacheResolver>().Remove(key));
            }
            PushDownWidgetInstancesOnWidgetZoneAfterWidget(toRowId, widgetInstanceId, toZoneId);
            ChangeWidgetInstancePosition(widgetInstanceId, toZoneId, toRowId);

            ReorderWidgetInstancesOnWidgetZone(widgetInstance.WidgetZoneId);

            // The new dropped zone now has more widgets than before. So clear cache.
            CacheSetup.CacheKeys.WidgetZoneKeys(toZoneId)
                    .Each(key => Services.Get<ICacheResolver>().Remove(key));
        }

        private void PushDownWidgetInstancesOnWidgetZoneAfterWidget(int toRowId, int widgetInstanceId, int widgetZoneId)
        {
            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            var isMovingDown = toRowId > widgetInstance.OrderNo;

            PushDownWidgetInstancesOnWidgetZone(toRowId, widgetZoneId, isMovingDown);
        }

        private void PushDownWidgetInstancesOnWidgetZone(int toRowId, int widgetZoneId, bool isMovingDown)
        {
            IEnumerable<WidgetInstance> list = this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneId(widgetZoneId);

            if (isMovingDown)
            {
                list = list.Where(wi => wi.OrderNo > toRowId);
            }
            else
            {
                list = list.Where(wi => wi.OrderNo >= toRowId);
            }

            int orderNo = toRowId + 1;
            foreach (WidgetInstance wi in list)
            {
                wi.OrderNo = ++orderNo;
            }

            this.widgetInstanceRepository.UpdateList((list), null, null);
        }

        private void ChangeWidgetInstancePosition(int widgetInstanceId, int widgetZoneId, int rowNo)
        {
            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.OrderNo = rowNo > wi.OrderNo ? rowNo + 1 : rowNo;
                wi.WidgetZoneId = widgetZoneId;
            }, null);
        }

        private void ReorderWidgetInstancesOnWidgetZone(int widgetZoneId)
        {
            var list = this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneId(widgetZoneId);

            int orderNo = 0;
            foreach (WidgetInstance wi in list)
            {
                wi.OrderNo = orderNo++;
            }

            this.widgetInstanceRepository.UpdateList(list, null, null);
        }

        public string GetWidgetInstanceState(int widgetInstanceId)
        {
            EnsureOwner(0, widgetInstanceId, 0);
            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);

            return widgetInstance.State;
        }

        public WidgetInstance SaveWidgetInstanceState(int widgetInstanceId, string state)
        {
            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);

            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.State = state;
                widgetInstance = wi;
            }, null);

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public WidgetInstance ResizeWidgetInstance(int widgetInstanceId, int width, int height)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.Resized = true;
                wi.Width = width;
                wi.Height = height;
                widgetInstance = wi;
            }, null);

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public WidgetInstance MaximizeWidget(int widgetInstanceId, bool isMaximized)
        {
            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);

            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.Maximized = isMaximized;
                widgetInstance = wi;
            }, null);

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public WidgetInstance ChangeWidgetInstanceTitle(int widgetInstanceId, string newTitle)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);

            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.Title = newTitle;
                widgetInstance = wi;
            }, null);

            return this.GetWidgetInstanceById(widgetInstanceId);
        }

        public void DeleteWidgetInstance(int widgetInstanceId)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            var widgetInstance = this.GetWidgetInstanceById(widgetInstanceId);
            this.widgetInstanceRepository.Delete(widgetInstance);
            ReorderWidgetInstancesOnWidgetZone(widgetInstance.WidgetZoneId);
        }

        public WidgetInstance AddWidget(int widgetId, int toRow, int columnNo, int zoneId)
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
                widgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(userSetting.CurrentPageId, columnNo);
            }

            PushDownWidgetInstancesOnWidgetZone(toRow, widgetZone.ID, true);

            var widget = this.widgetRepository.GetWidgetById(widgetId);

            return this.widgetInstanceRepository.Insert((wi) =>
            {
                ObjectBuilder.BuildDefaultWidgetInstance(wi,
                    widget.Name, widgetZone.ID, toRow, widget.ID, widget.DefaultState);
            });
        }

        #endregion Methods
    }
}