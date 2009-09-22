namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;
    
    using Dropthings.Util;
    using Dropthings.Configuration;

    /// <summary>
    /// Facade subsystem for Pages, Columns, WidgetZones
    /// </summary>
    partial class Facade
    {
        #region Methods

        public List<Column> GetColumnsInPage(int pageId)
        {
            return this.columnRepository.GetColumnsByPageId(pageId);
        }

        public Column CloneColumn(Page clonedPage, Column columnToClone)
        {
            var widgetZoneToClone = this.widgetZoneRepository.GetWidgetZoneById(columnToClone.WidgetZoneId);

            var clonedWidgetZone = this.widgetZoneRepository.Insert((newWidgetZone) =>
            {
                newWidgetZone.Title = widgetZoneToClone.Title;
                newWidgetZone.UniqueID = Guid.NewGuid().ToString();
            });

            var widgetInstancesToClone = this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneId(widgetZoneToClone.ID);
            widgetInstancesToClone.Each(widgetInstanceToClone => CloneWidgetInstance(clonedWidgetZone.ID, widgetInstanceToClone));                

            return this.columnRepository.Insert((col) =>
            {
                col.PageId = clonedPage.ID;
                col.WidgetZoneId = clonedWidgetZone.ID;
                col.ColumnNo = columnToClone.ColumnNo;
                col.ColumnWidth = columnToClone.ColumnWidth;
            });
        }

        public Page CreatePage(Guid userGuid, string title, string layoutType, int toOrder)
        {
            PushDownPages(0, toOrder);
            
            title = string.IsNullOrEmpty(title) ? DecideUniquePageName() : title;

            var insertedPage = this.pageRepository.Insert((pageToBeCreated) =>
            {
                ObjectBuilder.BuildDefaultPage(pageToBeCreated, userGuid, title, Convert.ToInt32(layoutType), toOrder);
            });

            var page = this.pageRepository.GetPageById(insertedPage.ID);

            for (int i = 0; i < insertedPage.ColumnCount; ++i)
            {
                var insertedWidgetZone = this.widgetZoneRepository.Insert((widgetZone) =>
                {
                    var columnTitle = "Column " + (i + 1);
                    ObjectBuilder.BuildDefaultWidgetZone(widgetZone, columnTitle, columnTitle, 0);
                });

                var insertedColumn = this.columnRepository.Insert((column) =>
                {
                    column.ColumnNo = i;
                    column.ColumnWidth = (100 / insertedPage.ColumnCount);
                    column.WidgetZoneId = insertedWidgetZone.ID;
                    column.PageId = insertedPage.ID;
                });
            }

            ReorderPagesOfUser();

            return SetCurrentPage(userGuid, page.ID);
        }

        public Page CreatePage(string title, string layoutType)
        {
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName); 
            return CreatePage(userGuid, title, layoutType, 9999);
        }

        public Page ClonePage(Guid userGuid, Page pageToClone)
        {
            if (userGuid != Guid.Empty)
            {
                var clonedPage = this.pageRepository.Insert((page) =>
                {
                    page.CreatedDate = DateTime.Now;
                    page.Title = pageToClone.Title;
                    page.UserId = userGuid;
                    page.LastUpdated = pageToClone.LastUpdated;
                    page.VersionNo = pageToClone.VersionNo;
                    page.LayoutType = pageToClone.LayoutType;
                    page.PageType = pageToClone.PageType;
                    page.ColumnCount = pageToClone.ColumnCount;
                    page.OrderNo = pageToClone.OrderNo;
                });

                ReorderPagesOfUser();

                var columns = this.columnRepository.GetColumnsByPageId(pageToClone.ID);
                columns.Each(columnToClone => CloneColumn(clonedPage, columnToClone));

                return clonedPage;
            }

            return null;
        }

        public WidgetInstance CloneWidgetInstance(int widgetZoneId, WidgetInstance wiToClone)
        {
            return this.widgetInstanceRepository.Insert((wi) =>
            {
                wi.CreatedDate = wiToClone.CreatedDate;
                wi.Expanded = wiToClone.Expanded;
                wi.Height = wiToClone.Height;
                wi.LastUpdate = wiToClone.LastUpdate;
                wi.Maximized = wiToClone.Maximized;
                wi.OrderNo = wiToClone.OrderNo;
                wi.Resized = wiToClone.Resized;
                wi.State = wiToClone.State;
                wi.Title = wiToClone.Title;
                wi.VersionNo = wiToClone.VersionNo;
                wi.WidgetId = wiToClone.WidgetId;
                wi.WidgetZoneId = widgetZoneId;
                wi.Width = wiToClone.Width;
            });
        }

        public Page SetCurrentPage(Guid userGuid, int currentPageId)
        {
            var userSetting = GetUserSetting(userGuid);
            userSetting.CurrentPageId = currentPageId;
            return this.pageRepository.GetPageById(currentPageId);
        }

        public Page DecideCurrentPage(Guid userGuid, string pageTitle, int currentPageId, bool? isAnonymous, bool? isFirstVisitAfterLogin)
        {
            Page currentPage = null;
            var pages = this.pageRepository.GetPagesOfUser(userGuid);
            List<Page> sharedPages = null;
            RoleTemplate roleTemplate = null;
            UserTemplateSetting settingTemplate = GetUserSettingTemplate();

            if((isAnonymous.GetValueOrDefault() && settingTemplate.CloneAnonProfileEnabled) || (!isAnonymous.GetValueOrDefault() && settingTemplate.CloneRegisteredProfileEnabled))
            {
                roleTemplate = GetRoleTemplate(userGuid);

                if (!roleTemplate.TemplateUserId.IsEmpty())
                {
                    // Get template user pages so that it can be cloned for new user
                    if (roleTemplate.TemplateUserId != userGuid)
                    {
                        sharedPages = this.pageRepository.GetLockedPagesOfUser(roleTemplate.TemplateUserId, false);
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
            else if (roleTemplate != null && settingTemplate.CloneRegisteredProfileEnabled && roleTemplate.TemplateUserId.Equals(userGuid) && CheckRoleTemplateIsRegisterUserTemplate(roleTemplate))
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
            currentPage = (currentPageId == 0) ? pages[0] : this.pageRepository.GetPageById(currentPageId);

            return currentPage;
        }

        public string DecideUniquePageName()
        {
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
            List<Page> pages = DatabaseHelper.GetList<Page, Guid>(DatabaseHelper.SubsystemEnum.Page, userGuid, LinqQueries.CompiledQuery_GetPagesByUserId);

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
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.CurrentPageId > 0)
            {
                var currentPage = this.pageRepository.GetPageById(userSetting.CurrentPageId);

                this.pageRepository.Update(currentPage, (page) =>
                {
                    page.Title = title;
                }, null);

                success = true;
            }

            return success;
        }

        public bool LockPage()
        {
            var success = false;
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.CurrentPageId > 0)
            {
                var currentPage = this.pageRepository.GetPageById(userSetting.CurrentPageId);

                this.pageRepository.Update(currentPage, (page) =>
                {
                    page.IsLocked = true;
                    page.LastLockedStatusChangedAt = DateTime.Now;
                }, null);
                
                success = true;
            }

            return success;
        }

        public bool UnLockPage()
        {
            var success = false;
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.CurrentPageId > 0)
            {
                var currentPage = this.pageRepository.GetPageById(userSetting.CurrentPageId);

                this.pageRepository.Update(currentPage, (page) =>
                {
                    page.IsLocked = false;
                    page.IsDownForMaintenance = false;
                    page.LastLockedStatusChangedAt = DateTime.Now;
                }, null);

                success = true;
            }

            return success;
        }

        public bool ChangePageMaintenenceStatus(bool isInMaintenenceMode)
        {
            var success = false;
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.CurrentPageId > 0)
            {
                var currentPage = this.pageRepository.GetPageById(userSetting.CurrentPageId);

                this.pageRepository.Update(currentPage, (page) =>
                {
                    page.IsDownForMaintenance = isInMaintenenceMode;

                    if (isInMaintenenceMode)
                    {
                        page.LastDownForMaintenanceAt = DateTime.Now;    
                    }
                }, null);

                success = true;
            }

            return success;
        }

        public bool ChangeServeAsStartPageAfterLoginStatus(bool shouldServeAsStartPage)
        {
            var success = false;
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);

            if (userSetting != null && userSetting.CurrentPageId > 0)
            {
                //check if there any previously overridable start page and make it false if request is for changing the start page to true
                if (shouldServeAsStartPage)
                {
                    Page overridablePage = this.pageRepository.GetOverridableStartPageOfUser(userGuid);

                    if (overridablePage != null)
                    {
                        this.pageRepository.Update(overridablePage, (page) =>
                        {
                            page.ServeAsStartPageAfterLogin = false;
                        }, null);    
                    }
                }

                //change the overridable start page status
                this.pageRepository.Update(this.pageRepository.GetPageById(userSetting.CurrentPageId), (page) =>
                {
                    page.ServeAsStartPageAfterLogin = shouldServeAsStartPage;
                }, null);

                success = true;
            }

            return success;
        }

        public void DeleteColumn(int pageId, int columnId, int columnNo)
        {
            WidgetZone widgetZone;

            if (columnId > 0)
            {
                widgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(pageId, columnNo);
            }
            else
            {
                var columnToDelete = this.columnRepository.GetColumnByPageId_ColumnNo(pageId, columnNo);
                columnId = columnToDelete.ID;
                widgetZone = this.widgetZoneRepository.GetWidgetZoneById(columnToDelete.WidgetZoneId);
            }

            var widgetInstances = this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneId(widgetZone.ID);
            widgetInstances.Each((widgetInstance) => this.widgetInstanceRepository.Delete(widgetInstance.Id));
            this.columnRepository.Delete(columnId);
            this.widgetZoneRepository.Delete(widgetZone.ID);
        }

        public Page DeletePage(int pageId)
        {
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
            var columns = this.columnRepository.GetColumnsByPageId(pageId);
            columns.Each((column) => DeleteColumn(pageId, column.ID, column.ColumnNo));
            this.pageRepository.Delete(pageId);
            var currentPage = DecideCurrentPage(userGuid, string.Empty, 0, null, null);   // 0 - since current page has been deleted.
            ReorderPagesOfUser();
            return SetCurrentPage(userGuid, currentPage.ID);
        }

        public void ModifyPageLayout(int newLayout)
        {
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
            var userSetting = GetUserSetting(userGuid);
            var newColumnDefs = Page.GetColumnWidths(newLayout);
            var existingColumns = GetColumnsInPage(userSetting.CurrentPageId);
            var columnCounter = existingColumns.Count - 1;
            var newColumnNo = newColumnDefs.Count() - 1;

            if (newColumnDefs.Count() < existingColumns.Count)
            {
                while (columnCounter + 1 > newColumnDefs.Length)
                {
                    var oldWidgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(userSetting.CurrentPageId, columnCounter);
                    var newWidgetZone = this.widgetZoneRepository.GetWidgetZoneByPageId_ColumnNo(userSetting.CurrentPageId, newColumnNo);
                    var widgetInstancesToMove = GetWidgetInstancesInZone(oldWidgetZone.ID);
                    widgetInstancesToMove.Each((wi) => ChangeWidgetInstancePosition(wi.Id, newWidgetZone.ID, 0));
                    DeleteColumn(userSetting.CurrentPageId, 0, columnCounter);
                    --columnCounter;
                }
            }
            else
            {
                while (columnCounter + 1 < newColumnDefs.Length)
                {
                    var newColumnWidth = newColumnDefs[columnCounter];

                    // Add Column
                    var insertedWidgetZone = this.widgetZoneRepository.Insert((newWidgetZone) =>
                    {
                        string title = "Column " + (newColumnNo + 1);
                        ObjectBuilder.BuildDefaultWidgetZone(newWidgetZone, title, title, 0);
                    });


                    try
                    {
                        var insertedColumn = this.columnRepository.Insert((newColumn) =>
                        {
                            newColumn.ColumnNo = newColumnNo;
                            newColumn.ColumnWidth = newColumnWidth;
                            newColumn.WidgetZoneId = insertedWidgetZone.ID;
                            newColumn.PageId = userSetting.CurrentPageId;
                        });
                    }
                    catch (Exception)
                    {

                    }

                    ++columnCounter;
                }
            }

            var columns = GetColumnsInPage(userSetting.CurrentPageId);
            this.columnRepository.UpdateList(columns, (column) =>
            {
                column.ColumnWidth = newColumnDefs[column.ColumnNo];
            }, null);

            var currentPage = this.pageRepository.GetPageById(userSetting.CurrentPageId);
            this.pageRepository.Update(currentPage, (page) =>
            {
                page.LayoutType = newLayout;
                page.ColumnCount = Page.GetColumnWidths(newLayout).Length;
            }, null);
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
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);
            var isMovingDown = toOrderNo > (pageId > 0 ? this.pageRepository.GetPageById(pageId).OrderNo.GetValueOrDefault() : 0);

            List<Page> list = null;

            list = isMovingDown ? this.pageRepository.GetPagesOfUserAfterPosition(userGuid, toOrderNo) : this.pageRepository.GetPagesOfUserFromPosition(userGuid, toOrderNo);

            int orderNo = toOrderNo + 1;
            foreach (Page item in list)
            {
                item.OrderNo = ++orderNo;
            }

            this.pageRepository.UpdateList((list), null, null);
        }

        public void ChangePagePosition(int pageId, int orderNo)
        {
            this.pageRepository.Update(this.pageRepository.GetPageById(pageId), (p) =>
            {
                p.OrderNo = orderNo > p.OrderNo.GetValueOrDefault() ? orderNo + 1 : orderNo;
            }, null);
        }

        public void ReorderPagesOfUser()
        {
            var userGuid = this.userRepository.GetUserGuidFromUserName(Context.CurrentUserName);

            var list = this.pageRepository.GetPagesOfUser(userGuid);

            int orderNo = 0;
            foreach (Page page in list)
            {
                page.OrderNo = orderNo++;
            }

            this.pageRepository.UpdateList(list, null, null);
        }

        #endregion Methods
    }
}