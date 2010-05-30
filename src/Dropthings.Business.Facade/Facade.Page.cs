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

    /// <summary>
    /// Facade subsystem for Pages, Columns, WidgetZones
    /// </summary>
    partial class Facade
    {
        #region Methods

        public Page GetPage(int pageId)
        {
            return this.pageRepository.GetPageById(pageId);
        }

        public List<Page> GetPagesOfUser(Guid userGuid)
        {
            return this.pageRepository.GetPagesOfUser(userGuid);
        }

        public List<Column> GetColumnsInPage(int pageId)
        {
            return this.columnRepository.GetColumnsByPageId(pageId);
        }

        public Column CloneColumn(Page clonedPage, Column columnToClone)
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
                Page = new Page { ID = clonedPage.ID },
                WidgetZone = new WidgetZone { ID = clonedWidgetZone.ID },
                ColumnNo = columnToClone.ColumnNo,
                ColumnWidth = columnToClone.ColumnWidth
            };

            return this.columnRepository.Insert(newColumn);
        }

        public Page CreatePage(Guid userGuid, string title, int layoutType, int toOrder)
        {
            PushDownPages(0, toOrder);
            
            title = string.IsNullOrEmpty(title) ? DecideUniquePageName() : title;

            var insertedPage = this.pageRepository.Insert(new Page
            {
                aspnet_Users = new aspnet_User { UserId = userGuid },
                Title = title,
                LayoutType = layoutType,
                OrderNo = toOrder,
                ColumnCount = Page.GetColumnWidths(layoutType).Length,
                CreatedDate = DateTime.Now
            });

            var page = this.pageRepository.GetPageById(insertedPage.ID);

            for (int i = 0; i < insertedPage.ColumnCount; ++i)
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
                    ColumnWidth = (100 / insertedPage.ColumnCount),
                    WidgetZone = new WidgetZone { ID = insertedWidgetZone.ID },
                    Page = new Page { ID = insertedPage.ID }
                });
            }
            
            ReorderPagesOfUser();

            return SetCurrentPage(userGuid, page.ID);
        }

        public Page CreatePage(string title, int layoutType)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName); 
            return CreatePage(userGuid, title, layoutType, 9999);
        }

        public Page ClonePage(Guid userGuid, Page pageToClone)
        {
            if (userGuid != Guid.Empty)
            {
                var clonedPage = this.pageRepository.Insert(new Page
                {
                    aspnet_Users = new aspnet_User { UserId = userGuid },
                    CreatedDate = DateTime.Now,
                    Title = pageToClone.Title,
                    LastUpdated = pageToClone.LastUpdated,
                    VersionNo = pageToClone.VersionNo,
                    LayoutType = pageToClone.LayoutType,
                    PageType = pageToClone.PageType,
                    ColumnCount = pageToClone.ColumnCount,
                    OrderNo = pageToClone.OrderNo,
                });

                //ReorderPagesOfUser();

                var columns = this.GetColumnsInPage(pageToClone.ID);
                columns.Each(columnToClone => CloneColumn(clonedPage, columnToClone));

                return clonedPage;
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

        public Page SetCurrentPage(Guid userGuid, int currentPageId)
        {
            var userSetting = GetUserSetting(userGuid);
            var newPage = GetPage(currentPageId);

            userSetting.Page = new Page { ID = newPage.ID };
            userSetting.PageReference = new EntityReference<Page> { EntityKey = newPage.EntityKey };

            this.userSettingRepository.Update(userSetting);

            return newPage;
        }

        public Page DecideCurrentPage(Guid userGuid, string pageTitle, int currentPageId, bool? isAnonymous, bool? isFirstVisitAfterLogin)
        {
            Page currentPage = null;
            var pages = this.GetPagesOfUser(userGuid);
            List<Page> sharedPages = null;
            RoleTemplate roleTemplate = null;
            UserTemplateSetting settingTemplate = GetUserSettingTemplate();

            if((isAnonymous.GetValueOrDefault() && settingTemplate.CloneAnonProfileEnabled) || (!isAnonymous.GetValueOrDefault() && settingTemplate.CloneRegisteredProfileEnabled))
            {
                roleTemplate = GetRoleTemplate(userGuid);

                if (!roleTemplate.aspnet_Users.UserId.IsEmpty())
                {
                    // Get template user pages so that it can be cloned for new user
                    if (roleTemplate.aspnet_Users.UserId != userGuid)
                    {
                        sharedPages = this.pageRepository.GetLockedPagesOfUser(roleTemplate.aspnet_Users.UserId, false);
                    }
                }
            }
            // Find the page that has the specified Page Name and make it as current
            // page. This is needed to make a tab as current tab when the tab name is
            // known
            if (!string.IsNullOrEmpty(pageTitle))
            {
                foreach (Page page in pages)
                {
                    if (string.Equals(page.Title.Replace(' ', '_'), pageTitle))
                    {
                        currentPageId = page.ID;
                        currentPage = page;
                        break;
                    }
                }

                if (sharedPages != null)
                {
                    foreach (Page page in sharedPages)
                    {
                        if (string.Equals(page.Title.Replace(' ', '_') + "_Locked", pageTitle))
                        {
                            currentPageId = page.ID;
                            currentPage = page;
                            break;
                        }
                    }
                }
            }
            else if (roleTemplate != null && settingTemplate.CloneRegisteredProfileEnabled && roleTemplate.aspnet_Users.UserId.Equals(userGuid) && CheckRoleTemplateIsRegisterUserTemplate(roleTemplate))
            {
                foreach (Page page in pages)
                {
                    if (page.ServeAsStartPageAfterLogin.GetValueOrDefault())
                    {
                        currentPageId = page.ID;
                        currentPage = page;
                        break;
                    }
                }
            }
            else if (settingTemplate.CloneRegisteredProfileEnabled && isFirstVisitAfterLogin.GetValueOrDefault() && !isAnonymous.GetValueOrDefault() && !CheckRoleTemplateIsAnonymousUserTemplate(roleTemplate))
            {
                //For register user check for the first load after login and find if there any defined page should load after login set by register template user
                if (sharedPages != null)
                {
                    foreach (Page page in sharedPages)
                    {
                        if(page.ServeAsStartPageAfterLogin.GetValueOrDefault())
                        {
                            currentPageId = page.ID;
                            currentPage = page;
                            break;
                        }
                    }
                }
            }

            // If there's no such page, then the first page user has will be the current
            // page. This happens when a page is deleted.
            currentPage = (currentPageId == 0) ? pages.First() : this.GetPage(currentPageId);



            return currentPage;
        }

        public string DecideUniquePageName()
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            List<Page> pages = this.GetPagesOfUser(userGuid).ToList();
            
            string uniqueNamePrefix = DEFAULT_FIRST_PAGE_NAME;
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

        public bool ChangePageName(string title)
        {
            var success = false;
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.Page.ID > 0)
            {
                var currentPage = this.GetPage(userSetting.Page.ID);

                // Ensure the title is unique and does not match with other pages
                var otherPages = this.GetPagesOfUser(userGuid).Where(p => p.ID != currentPage.ID);
                
                // Keep incrementing the last digit on the page title until there's no 
                // such duplicate
                int loopCounter = 0;
                while (loopCounter++ < 100 
                    && otherPages.FirstOrDefault(p => p.Title == title) != null)
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

                currentPage.Title = title;
                this.pageRepository.Update(currentPage);

                success = true;
            }

            return success;
        }

        public bool LockPage()
        {
            var success = false;
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.Page.ID > 0)
            {
                this.GetPage(userSetting.Page.ID).As(page =>
                    {
                        page.IsLocked = true;
                        page.LastLockedStatusChangedAt = DateTime.Now;
                        this.pageRepository.Update(page);
                    });
                
                success = true;
            }

            return success;
        }

        public bool UnLockPage()
        {
            var success = false;
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.Page.ID > 0)
            {
                this.GetPage(userSetting.Page.ID).As(page =>
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

        public bool ChangePageMaintenenceStatus(bool isInMaintenenceMode)
        {
            var success = false;
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.Page.ID > 0)
            {
                this.GetPage(userSetting.Page.ID).As(page =>
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

        public bool ChangeServeAsStartPageAfterLoginStatus(bool shouldServeAsStartPage)
        {
            var success = false;
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.Page.ID > 0)
            {
                //check if there any previously overridable start page and make it false if request is for changing the start page to true
                if (shouldServeAsStartPage)
                {
                    Page overridablePage = this.pageRepository.GetOverridableStartPageOfUser(userGuid);

                    if (overridablePage != null)
                    {
                        overridablePage.ServeAsStartPageAfterLogin = false;
                        this.pageRepository.Update(overridablePage);
                    }
                }

                //change the overridable start page status
                this.GetPage(userSetting.Page.ID).As(page =>
                    {
                        page.ServeAsStartPageAfterLogin = shouldServeAsStartPage;                    
                        this.pageRepository.Update(page);
                    });

                success = true;
            }

            return success;
        }

        public void DeleteColumn(int pageId, int columnNo)
        {
            var columnToDelete = this.columnRepository.GetColumnByPageId_ColumnNo(pageId, columnNo);
            WidgetZone widgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(pageId, columnNo);

            var widgetInstances = this.GetWidgetInstancesInZoneWithWidget(widgetZone.ID);
            widgetInstances.Each((widgetInstance) => this.widgetInstanceRepository.Delete(widgetInstance.Id));

            this.columnRepository.Delete(columnToDelete);
            this.widgetZoneRepository.Delete(widgetZone);
        }

        public Page DeletePage(int pageId)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            
            var columns = this.GetColumnsInPage(pageId);
            columns.Each((column) => DeleteColumn(pageId, column.ColumnNo));

            this.pageRepository.Delete(new Page { ID = pageId });
            
            var currentPage = DecideCurrentPage(userGuid, string.Empty, 0, null, null);   // 0 - since current page has been deleted.

            ReorderPagesOfUser();

            return SetCurrentPage(userGuid, currentPage.ID);
        }

        public void ModifyPageLayout(int newLayout)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);
            var newColumnDefs = Page.GetColumnWidths(newLayout);
            var existingColumns = GetColumnsInPage(userSetting.Page.ID);
            var columnCounter = existingColumns.Count - 1;
            var newColumnNo = newColumnDefs.Count() - 1;

            if (newColumnDefs.Count() < existingColumns.Count)
            {
                // No of columns decreased. So, we need to move widgets from the deceased column 
                // to the last available column.

                for (var existingColumnNo = newColumnNo + 1; existingColumnNo < existingColumns.Count; existingColumnNo ++)
                {
                    var oldWidgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(userSetting.Page.ID, existingColumnNo);
                    var newWidgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(userSetting.Page.ID, newColumnNo);
                    
                    var widgetInstancesToMove = GetWidgetInstancesInZoneWithWidget(oldWidgetZone.ID);
                    var originalWidgets = GetWidgetInstancesInZoneWithWidget(newWidgetZone.ID);
                    var lastWidgetPosition = originalWidgets.Max(w => w.OrderNo);
                    
                    widgetInstancesToMove.Each((wi) => ChangeWidgetInstancePosition(wi.Id, newWidgetZone.ID, ++lastWidgetPosition));
                    DeleteColumn(userSetting.Page.ID, existingColumnNo);                    
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
                        Page = new Page { ID = userSetting.Page.ID }
                    });
                    
                    ++columnCounter;
                }
            }

            var columns = this.columnRepository.GetColumnsByPageId(userSetting.Page.ID);
            columns.Each(column => column.ColumnWidth = newColumnDefs[column.ColumnNo]);
            this.columnRepository.UpdateList(columns);

            var currentPage = this.GetPage(userSetting.Page.ID);
            currentPage.LayoutType = newLayout;
            currentPage.ColumnCount = Page.GetColumnWidths(newLayout).Length;
            this.pageRepository.Update(currentPage);
        }

        public void MovePage(int pageId, int toOrderNo)
        {
            EnsureOwner(pageId, 0 , 0);
            PushDownPages(pageId, toOrderNo);
            ChangePagePosition(pageId, toOrderNo);
            ReorderPagesOfUser();
        }

        public void PushDownPages(int pageId, int toOrderNo)
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);
            var isMovingDown = toOrderNo > (pageId > 0 ? 
                this.GetPage(pageId).OrderNo.GetValueOrDefault() 
                : 0);

            IEnumerable<Page> list = this.pageRepository.GetPagesOfUser(userGuid)
                .Where(page => (isMovingDown ? page.OrderNo > toOrderNo : page.OrderNo >= toOrderNo));

            //list = isMovingDown ? this.pageRepository.GetPagesOfUserAfterPosition(userGuid, toOrderNo) : this.pageRepository.GetPagesOfUserFromPosition(userGuid, toOrderNo);

            int orderNo = toOrderNo + 1;
            foreach (Page item in list)
            {
                item.OrderNo = ++orderNo;
            }

            this.pageRepository.UpdateList(list);
        }

        public void ChangePagePosition(int pageId, int orderNo)
        {
            var page = this.GetPage(pageId);
            page.OrderNo = orderNo > page.OrderNo.GetValueOrDefault() ? orderNo + 1 : orderNo;
            this.pageRepository.Update(page);
        }

        public void ReorderPagesOfUser()
        {
            var userGuid = this.GetUserGuidFromUserName(Context.CurrentUserName);

            var list = this.GetPagesOfUser(userGuid);

            int orderNo = 0;
            foreach (Page page in list)
            {
                page.OrderNo = orderNo++;
            }

            this.pageRepository.UpdateList(list);
        }

        #endregion Methods
    }
}