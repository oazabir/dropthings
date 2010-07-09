namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Data;
    
    using Dropthings.Util;
    using Dropthings.Configuration;
    using OmarALZabir.AspectF;
    using System.Data.Objects.DataClasses;
    using System.Text.RegularExpressions;
    using System.Web.Security;

    /// <summary>
    /// Facade subsystem for Tabs, Columns, WidgetZones
    /// </summary>
    partial class Facade
    {
        #region Methods

        public Tab GetTab(int pageId)
        {
            return this.pageRepository.GetTabById(pageId);
        }

        public List<Tab> GetTabsOfUser(Guid userGuid)
        {
            return this.pageRepository.GetTabsOfUser(userGuid);
        }

        public List<Column> GetColumnsInTab(int pageId)
        {
            return this.columnRepository.GetColumnsByTabId(pageId);
        }

        public Column CloneColumn(Tab clonedTab, Column columnToClone)
        {
            var widgetZoneToClone = this.widgetZoneRepository.GetWidgetZoneById(columnToClone.WidgetZone.ID);

            var clonedWidgetZone = this.widgetZoneRepository.Insert(new WidgetZone
            {
                Title = widgetZoneToClone.Title,
                UniqueID = Guid.NewGuid().ToString()
            });

            var widgetInstancesToClone = this.GetWidgetInstancesInZoneWithWidget(widgetZoneToClone.ID);
            widgetInstancesToClone.Each(widgetInstanceToClone => CloneWidgetInstance(clonedWidgetZone.ID, widgetInstanceToClone));
            var newColumn = new Column
            {
                Tab = new Tab { ID = clonedTab.ID },
                WidgetZone = new WidgetZone { ID = clonedWidgetZone.ID },
                ColumnNo = columnToClone.ColumnNo,
                ColumnWidth = columnToClone.ColumnWidth
            };

            return this.columnRepository.Insert(newColumn);
        }

        public Tab CreateTab(Guid userGuid, string title, int layoutType, int toOrder)
        {
            PushDownTabs(0, toOrder);
            
            title = string.IsNullOrEmpty(title) ? DecideUniqueTabName(title) : title;

            var insertedTab = this.pageRepository.Insert(new Tab
            {
                AspNetUser = new AspNetUser { UserId = userGuid },
                Title = title,
                LayoutType = layoutType,
                OrderNo = toOrder,
                ColumnCount = Tab.GetColumnWidths(layoutType).Length,
                CreatedDate = DateTime.Now
            });

            var page = this.pageRepository.GetTabById(insertedTab.ID);

            for (int i = 0; i < insertedTab.ColumnCount; ++i)
            {
                var insertedWidgetZone = this.widgetZoneRepository.Insert(new WidgetZone
                {
                    Title = "Column " + (i + 1),
                    OrderNo = 0,
                    UniqueID = "Column " + (i + 1)
                });

                var insertedColumn = this.columnRepository.Insert(new Column
                {
                    ColumnNo = i,
                    ColumnWidth = (100 / insertedTab.ColumnCount),
                    WidgetZone = new WidgetZone { ID = insertedWidgetZone.ID },
                    Tab = new Tab { ID = insertedTab.ID }
                });
            }
            
            ReorderTabsOfUser();

            return SetCurrentTab(userGuid, page.ID);
        }

        public Tab CreateTab(string title, int layoutType)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName); 
            return CreateTab(userGuid, title, layoutType, 9999);
        }

        public Tab CloneTab(Guid userGuid, Tab pageToClone)
        {
            if (userGuid != Guid.Empty)
            {
                var clonedTab = this.pageRepository.Insert(new Tab
                {
                    AspNetUser = new AspNetUser { UserId = userGuid },
                    CreatedDate = DateTime.Now,
                    Title = pageToClone.Title,
                    LastUpdated = pageToClone.LastUpdated,
                    VersionNo = pageToClone.VersionNo,
                    LayoutType = pageToClone.LayoutType,
                    PageType = pageToClone.PageType,
                    ColumnCount = pageToClone.ColumnCount,
                    OrderNo = pageToClone.OrderNo,
                });

                //ReorderTabsOfUser();

                var columns = this.GetColumnsInTab(pageToClone.ID);
                columns.Each(columnToClone => CloneColumn(clonedTab, columnToClone));

                return clonedTab;
            }

            return null;
        }

        public WidgetInstance CloneWidgetInstance(int widgetZoneId, WidgetInstance wiToClone)
        {
            var newWidgetInstance = this.widgetInstanceRepository.Insert(new WidgetInstance
            {
                CreatedDate = wiToClone.CreatedDate,
                Expanded = wiToClone.Expanded,
                Height = wiToClone.Height,
                LastUpdate = wiToClone.LastUpdate,
                Maximized = wiToClone.Maximized,
                OrderNo = wiToClone.OrderNo,
                Resized = wiToClone.Resized,
                State = wiToClone.State,
                Title = wiToClone.Title,
                VersionNo = wiToClone.VersionNo,
                Widget = new Widget { ID = wiToClone.Widget.ID },
                WidgetZone = new WidgetZone { ID = widgetZoneId },
                Width = wiToClone.Width
            });           

            return newWidgetInstance;
        }

        public Tab SetCurrentTab(Guid userGuid, int currentTabId)
        {
            var userSetting = GetUserSetting(userGuid);
            var newTab = GetTab(currentTabId);

            userSetting.CurrentTab = new Tab { ID = newTab.ID };
            userSetting.CurrentTabReference = new EntityReference<Tab> { EntityKey = newTab.EntityKey };

            this.userSettingRepository.Update(userSetting);

            return newTab;
        }


        /// <summary>
        /// Returns the shared tabs the user can see as read-only.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<Tab> GetSharedTabs(string userName)
        {
            if (this.IsUserAnonymous(userName))
            {
                var anonUserName = GetUserSettingTemplate().AnonUserSettingTemplate.UserName;
                return this.pageRepository.GetLockedTabsOfUser(this.GetUserGuidFromUserName(anonUserName), false);
            }
            else
            {
                var registeredUserName = GetUserSettingTemplate().RegisteredUserSettingTemplate.UserName;
                var userGuid = this.GetUserGuidFromUserName(registeredUserName);
                if (userGuid != null)
                    return this.pageRepository.GetLockedTabsOfUser(userGuid, false);
                else
                    return new List<Tab>();
            }
        }

        
        public Tab DecideCurrentTab(Guid userGuid, string pageTitle, List<Tab> ownTabs, List<Tab> sharedTabs)
        {
            // Find the page that has the specified Tab Name and make it as current
            // page. This is needed to make a tab as current tab when the tab name is
            // known
            if (!string.IsNullOrEmpty(pageTitle))
            {
                foreach (Tab tab in ownTabs)
                {
                    if (string.Equals(tab.Title.Replace(' ', '_'), pageTitle))
                    {
                        return tab;
                    }
                }

                if (sharedTabs != null)
                {
                    foreach (Tab tab in sharedTabs)
                    {
                        if (string.Equals(tab.Title.Replace(' ', '_') + "_Locked", pageTitle))
                        {
                            return tab;
                        }
                    }
                }
            }
            
            // If we are here, then we haven't found a tab to show yet. so return the
            // current default tab
            return this.GetTab(this.GetUserSetting(userGuid).CurrentTab.ID);
        }

        public string DecideUniqueTabName(string title)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            List<Tab> pages = this.pageRepository.GetTabsOfUser(userGuid);
            
            string uniqueNamePrefix = title;
            string pageUniqueName = uniqueNamePrefix;
            for (int counter = 0; counter < 100; counter++)
            {
                if (counter > 0)
                    pageUniqueName = uniqueNamePrefix + " " + counter;
                if (pages.Exists((page) => page.Title == pageUniqueName) == false)
                    break;
            }

            if (pages.Exists((page) => page.Title == pageUniqueName))
                pageUniqueName = uniqueNamePrefix + "" + DateTime.Now.Ticks;

            return pageUniqueName;
        }

        public void ChangeTabName(string title)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            var currentTab = this.GetTab(userSetting.CurrentTab.ID);
            // Ensure the title is unique and does not match with other pages
            var otherTabs = this.pageRepository.GetTabsOfUser(userGuid).Where(p => p.ID != currentTab.ID);
                
            // Keep incrementing the last digit on the page title until there's no 
            // such duplicate
            int loopCounter = 0;
            while (loopCounter++ < 100 
                && otherTabs.FirstOrDefault(p => p.Title == title) != null)
            {
                var match = Regex.Match(title, "\\d+$");
                if (match.Success)
                {
                    var existingNumber = default(int);
                    if (int.TryParse(match.Value, out existingNumber))
                    {
                        title = Regex.Replace(title, "\\d+$", (existingNumber + 1).ToString());
                    }
                    else
                    {
                        title = title + " " + (existingNumber + 1);
                    }
                }
                else
                {
                    title = title + " 1";
                }
            }

            currentTab.Title = title;
            this.pageRepository.Update(currentTab);
        }

        public bool LockTab()
        {
            var success = false;
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.CurrentTab.ID > 0)
            {
                this.GetTab(userSetting.CurrentTab.ID).As(page =>
                    {
                        page.IsLocked = true;
                        page.LastLockedStatusChangedAt = DateTime.Now;
                        this.pageRepository.Update(page);
                    });
                
                success = true;
            }

            return success;
        }

        public bool UnLockTab()
        {
            var success = false;
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.CurrentTab.ID > 0)
            {
                this.GetTab(userSetting.CurrentTab.ID).As(page =>
                    {
                        page.IsLocked = false;
                        page.IsDownForMaintenance = false;
                        page.LastLockedStatusChangedAt = DateTime.Now;
                        this.pageRepository.Update(page);
                    });

                success = true;
            }

            return success;
        }

        public bool ChangeTabMaintenenceStatus(bool isInMaintenenceMode)
        {
            var success = false;
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.CurrentTab.ID > 0)
            {
                this.GetTab(userSetting.CurrentTab.ID).As(page =>
                    {
                        page.IsDownForMaintenance = isInMaintenenceMode;

                        if (isInMaintenenceMode)
                        {
                            page.LastDownForMaintenanceAt = DateTime.Now;
                        }
                        this.pageRepository.Update(page);
                    });

                success = true;
            }

            return success;
        }

        public bool ChangeServeAsStartPageAfterLoginStatus(bool shouldServeAsStartTab)
        {
            var success = false;
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.CurrentTab.ID > 0)
            {
                //check if there any previously overridable start page and make it false if request is for changing the start page to true
                if (shouldServeAsStartTab)
                {
                    Tab overridableTab = this.pageRepository.GetOverridableStartTabOfUser(userGuid);

                    if (overridableTab != null)
                    {
                        overridableTab.ServeAsStartPageAfterLogin = false;
                        this.pageRepository.Update(overridableTab);
                    }
                }

                //change the overridable start page status
                this.GetTab(userSetting.CurrentTab.ID).As(page =>
                    {
                        page.ServeAsStartPageAfterLogin = shouldServeAsStartTab;                    
                        this.pageRepository.Update(page);
                    });

                success = true;
            }

            return success;
        }

        public void DeleteColumn(int pageId, int columnNo)
        {
            var columnToDelete = this.columnRepository.GetColumnByTabId_ColumnNo(pageId, columnNo);
            WidgetZone widgetZone = this.widgetZoneRepository.GetWidgetZoneByTabId_ColumnNo(pageId, columnNo);

            var widgetInstances = this.GetWidgetInstancesInZoneWithWidget(widgetZone.ID);
            widgetInstances.Each((widgetInstance) => this.widgetInstanceRepository.Delete(widgetInstance.Id));

            this.columnRepository.Delete(columnToDelete);
            this.widgetZoneRepository.Delete(widgetZone);
        }
        /// <summary>
        /// Delete the specified page. If the specified page is the current
        /// page, then select the page before or after it as the current page
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public Tab DeleteTab(int pageId)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            
            var columns = this.GetColumnsInTab(pageId);
            columns.Each((column) => DeleteColumn(pageId, column.ColumnNo));

            var userSetting = GetUserSetting(userGuid);
            if (pageId == userSetting.CurrentTab.ID)
            {
                // Choose either the page before or after as the current page
                var pagesOfUser = this.pageRepository.GetTabsOfUser(userGuid);
                if (pagesOfUser.Count == 1)
                    throw new ApplicationException("Cannot delete the only page.");

                var index = pagesOfUser.FindIndex(p => p.ID == pageId);
                if (index == 0)
                    index++;
                else
                    index--;

                var newCurrentTab = pagesOfUser[index];
                SetCurrentTab(userGuid, newCurrentTab.ID);
            }

            this.pageRepository.Delete(new Tab { ID = pageId });
            
            ReorderTabsOfUser();

            return GetUserSetting(userGuid).CurrentTab;
        }

        public void ModifyTabLayout(int newLayout)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);
            var newColumnDefs = Tab.GetColumnWidths(newLayout);
            var existingColumns = GetColumnsInTab(userSetting.CurrentTab.ID);
            var columnCounter = existingColumns.Count - 1;
            var newColumnNo = newColumnDefs.Count() - 1;

            if (newColumnDefs.Count() < existingColumns.Count)
            {
                // No of columns decreased. So, we need to move widgets from the deceased column 
                // to the last available column.

                for (var existingColumnNo = newColumnNo + 1; existingColumnNo < existingColumns.Count; existingColumnNo ++)
                {
                    var oldWidgetZone = this.widgetZoneRepository.GetWidgetZoneByTabId_ColumnNo(userSetting.CurrentTab.ID, existingColumnNo);
                    var newWidgetZone = this.widgetZoneRepository.GetWidgetZoneByTabId_ColumnNo(userSetting.CurrentTab.ID, newColumnNo);
                    
                    var widgetInstancesToMove = GetWidgetInstancesInZoneWithWidget(oldWidgetZone.ID);
                    var originalWidgets = GetWidgetInstancesInZoneWithWidget(newWidgetZone.ID);
                    if (originalWidgets.Count() > 0)
                    {
                        var lastWidgetPosition = originalWidgets.Max(w => w.OrderNo);

                        widgetInstancesToMove.Each((wi) => ChangeWidgetInstancePosition(wi.Id, newWidgetZone.ID, ++lastWidgetPosition));
                    }

                    DeleteColumn(userSetting.CurrentTab.ID, existingColumnNo);                    
                }                
            }
            else
            {
                while (columnCounter + 1 < newColumnDefs.Length)
                {
                    var newColumnWidth = newColumnDefs[columnCounter];
                    // OMAR: Fix provided in http://code.google.com/p/dropthings/issues/detail?id=42#makechanges
                    //string title = "Column " + (newColumnNo + 1);                       
                    string title = "Column " + (columnCounter + 2);
                        
                    // Add Column
                    var insertedWidgetZone = this.widgetZoneRepository.Insert(new WidgetZone
                    {
                        Title = title,
                        UniqueID = title,
                        OrderNo = 0
                    });
                    
                    var insertedColumn = this.columnRepository.Insert(new Column
                    {
                        // OMAR: Fix provided in http://code.google.com/p/dropthings/issues/detail?id=42#makechanges
                        //newColumn.ColumnNo = newColumnNo;
                        ColumnNo = columnCounter + 1,
                        ColumnWidth = newColumnWidth,
                        WidgetZone = new WidgetZone { ID = insertedWidgetZone.ID },
                        Tab = new Tab { ID = userSetting.CurrentTab.ID }
                    });
                    
                    ++columnCounter;
                }
            }

            var columns = this.columnRepository.GetColumnsByTabId(userSetting.CurrentTab.ID);
            columns.Each(column => column.ColumnWidth = newColumnDefs[column.ColumnNo]);
            this.columnRepository.UpdateList(columns);

            var currentTab = this.GetTab(userSetting.CurrentTab.ID);
            currentTab.LayoutType = newLayout;
            currentTab.ColumnCount = Tab.GetColumnWidths(newLayout).Length;
            this.pageRepository.Update(currentTab);
        }

        public void MoveTab(int pageId, int toOrderNo)
        {
            EnsureOwner(pageId, 0 , 0);
            PushDownTabs(pageId, toOrderNo);
            ChangeTabPosition(pageId, toOrderNo);
            ReorderTabsOfUser();
        }

        public void PushDownTabs(int pageId, int toOrderNo)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var isMovingDown = toOrderNo > (pageId > 0 ? 
                this.GetTab(pageId).OrderNo.GetValueOrDefault() 
                : 0);

            IEnumerable<Tab> list = this.pageRepository.GetTabsOfUser(userGuid)
                .Where(page => (isMovingDown ? page.OrderNo > toOrderNo : page.OrderNo >= toOrderNo));

            //list = isMovingDown ? this.pageRepository.GetTabsOfUserAfterPosition(userGuid, toOrderNo) : this.pageRepository.GetTabsOfUserFromPosition(userGuid, toOrderNo);

            int orderNo = toOrderNo + 1;
            foreach (Tab item in list)
            {
                item.OrderNo = ++orderNo;
            }

            this.pageRepository.UpdateList(list);
        }

        public void ChangeTabPosition(int pageId, int orderNo)
        {
            var page = this.GetTab(pageId);
            page.OrderNo = orderNo > page.OrderNo.GetValueOrDefault() ? orderNo + 1 : orderNo;
            this.pageRepository.Update(page);
        }

        public void ReorderTabsOfUser()
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);

            var list = this.pageRepository.GetTabsOfUser(userGuid);

            int orderNo = 0;
            foreach (Tab page in list)
            {
                page.OrderNo = orderNo++;
            }

            this.pageRepository.UpdateList(list);
        }

        #endregion Methods
    }
}