namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    /// <summary>
    /// Facade subsystem for Widgets and WidgetInstances
    /// </summary>
    partial class Facade
    {
        #region Methods

        public void CreateDefaultWidgetsOnPage(string userName, int pageId)
        {
            List<Widget> defaultWidgets = null;

            defaultWidgets = (System.Web.Security.Roles.Enabled && !string.IsNullOrEmpty(userName)) ?
                this.widgetRepository.GetDefaultWidgetsByRole(userName, Enumerations.WidgetTypeEnum.PersonalPage, true) :
                this.widgetRepository.GetWidgetByIsDefault(true);

            var widgetsPerColumn = (int)Math.Ceiling((float)defaultWidgets.Count / 3.0);

            var row = 0;
            var col = 0;

            this.widgetInstanceRepository.InsertList(defaultWidgets, widget =>
            {
                var instance = new WidgetInstance();

                var widgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(pageId, col); 

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

        public List<WidgetInstance> GetWidgetInstancesInZone(int widgetZoneId)
        {
            List<WidgetInstance> list;
            list = this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneIdWithWidget(widgetZoneId);
            return list;
        }

        public WidgetInstance ExpandWidget(int widgetInstanceId, bool isExpand)
        {
            EnsureOwner(0, widgetInstanceId, 0);
            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);
            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.Expanded = isExpand;
                widgetInstance = wi;
            }, null);

            return widgetInstance;
        }

        public void MoveWidgetInstance(int widgetInstanceId, int toZoneId, int toRowId)
        {
            EnsureOwner(0, widgetInstanceId, 0);
            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);
            PushDownWidgetInstancesOnWidgetZone(toRowId, widgetInstanceId, toZoneId);
            ChangeWidgetInstancePosition(widgetInstanceId, toZoneId, toRowId);
            ReorderWidgetInstancesOnWidgetZone(widgetInstance.WidgetZoneId);
        }

        public void PushDownWidgetInstancesOnWidgetZone(int toRowId, int widgetInstanceId, int widgetZoneId)
        {
            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);
            var isMovingDown = toRowId > (widgetInstance != null ? widgetInstance.OrderNo : 0);

            List<WidgetInstance> list;

            if (isMovingDown)
            {
                list = this.widgetInstanceRepository.GetWidgetInstanceOnWidgetZoneAfterPosition(widgetZoneId, toRowId);
            }
            else
            {
                list = this.widgetInstanceRepository.GetWidgetInstanceOnWidgetZoneFromPosition(widgetZoneId, toRowId);
            }

            int orderNo = toRowId + 1;
            foreach (WidgetInstance wi in list)
            {
                wi.OrderNo = ++orderNo;
            }

            this.widgetInstanceRepository.UpdateList((list), null, null);
        }

        public void ChangeWidgetInstancePosition(int widgetInstanceId, int widgetZoneId, int rowNo)
        {
            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);
            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.OrderNo = rowNo > wi.OrderNo ? rowNo + 1 : rowNo;
                wi.WidgetZoneId = widgetZoneId;
            }, null);
        }

        public void ReorderWidgetInstancesOnWidgetZone(int widgetZoneId)
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
            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);

            return widgetInstance.State;
        }

        public WidgetInstance SaveWidgetInstanceState(int widgetInstanceId, string state)
        {
            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);

            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.State = state;
                widgetInstance = wi;
            }, null);

            return widgetInstance;
        }

        public WidgetInstance ResizeWidgetInstance(int widgetInstanceId, int width, int height)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);
            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.Resized = true;
                wi.Width = width;
                wi.Height = height;
                widgetInstance = wi;
            }, null);

            return widgetInstance;
        }

        public WidgetInstance MaximizeWidget(int widgetInstanceId, bool isMaximized)
        {
            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);

            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.Maximized = isMaximized;
                widgetInstance = wi;
            }, null);

            return widgetInstance;
        }

        public WidgetInstance ChangeWidgetInstanceTitle(int widgetInstanceId, string newTitle)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);

            this.widgetInstanceRepository.Update(widgetInstance, (wi) =>
            {
                wi.Title = newTitle;
                widgetInstance = wi;
            }, null);

            return widgetInstance;
        }

        public void DeleteWidgetInstance(int widgetInstanceId)
        {
            EnsureOwner(0, widgetInstanceId, 0);

            var widgetInstance = this.widgetInstanceRepository.GetWidgetInstanceById(widgetInstanceId);
            this.widgetInstanceRepository.Delete(widgetInstance);
            ReorderWidgetInstancesOnWidgetZone(widgetInstance.WidgetZoneId);
        }

        public WidgetInstance AddWidget(int widgetId, int toRow, int columnNo, int zoneId)
        {
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
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

            PushDownWidgetInstancesOnWidgetZone(toRow, 0, widgetZone.ID);

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