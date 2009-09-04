namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PageRepository : IPageRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;

        #endregion Fields

        #region Constructors

        public PageRepository(IDropthingsDataContext database)
        {
            this._database = database;
        }

        #endregion Constructors

        #region Methods

        public void Delete(int id)
        {
            _database.DeleteByPK<Page, int>(DropthingsDataContext.SubsystemEnum.Page, id);
        }

        public void Delete(Page page)
        {
            _database.Delete<Page>(DropthingsDataContext.SubsystemEnum.Page, page);
        }

        public Page GetPageById(int pageId)
        {
            return _database.GetSingle<Page, int>(DropthingsDataContext.SubsystemEnum.Page, pageId, LinqQueries.CompiledQuery_GetPageById);
        }

        public List<int> GetPageIdByUserGuid(Guid userGuid)
        {
            return _database.GetList<int, Guid>(DropthingsDataContext.SubsystemEnum.Page, userGuid, LinqQueries.CompiledQuery_GetPageIdByUserGuid);
        }

        public string GetPageOwnerName(int pageId)
        {
            return _database.GetSingle<string, int>(DropthingsDataContext.SubsystemEnum.Page, pageId, LinqQueries.CompiledQuery_GetPageOwnerName);
        }

        public List<Page> GetPagesOfUser(Guid userGuid)
        {
            return _database.GetList<Page, Guid>(DropthingsDataContext.SubsystemEnum.Page, userGuid, LinqQueries.CompiledQuery_GetPagesByUserId);
        }

        public List<Page> GetLockedPagesOfUser(Guid userGuid, bool? isDownForMaintenenceMode)
        {
            return isDownForMaintenenceMode.HasValue ? _database.GetList<Page, Guid, bool>(DropthingsDataContext.SubsystemEnum.Page, userGuid, isDownForMaintenenceMode.Value, LinqQueries.CompiledQuery_GetLockedPages_ByUserId_DownForMaintenence) 
                                                : _database.GetList<Page, Guid>(DropthingsDataContext.SubsystemEnum.Page, userGuid, LinqQueries.CompiledQuery_GetLockedPagesByUserId);
        }

        public List<Page> GetLockedPagesOfUserByMaintenenceMode(Guid userGuid, bool isInMaintenenceMode)
        {
            return _database.GetList<Page, Guid, bool>(DropthingsDataContext.SubsystemEnum.Page, userGuid, isInMaintenenceMode, LinqQueries.CompiledQuery_GetLockedPages_ByUserId_DownForMaintenence);
        }

        public List<Page> GetMaintenencePagesOfUser(Guid userGuid)
        {
            return _database.GetList<Page, Guid>(DropthingsDataContext.SubsystemEnum.Page, userGuid, LinqQueries.CompiledQuery_GetPagesWhichIsDownForMaintenanceByUserId);
        }

        public Page Insert(Action<Page> populate)
        {
            return _database.Insert<Page>(DropthingsDataContext.SubsystemEnum.Page, populate);
        }

        public void Update(Page page, Action<Page> detach, Action<Page> postAttachUpdate)
        {
            _database.UpdateObject<Page>(DropthingsDataContext.SubsystemEnum.Page, page, detach, postAttachUpdate);
        }

        public void UpdateList(List<Page> pages, Action<Page> detach, Action<Page> postAttachUpdate)
        {
            _database.UpdateList<Page>(DropthingsDataContext.SubsystemEnum.Page, pages, detach, postAttachUpdate);
        }

        public Page GetFirstPageOfUser(Guid userGuid)
        {
            return _database.GetQueryResult<Page, Guid, Page>(DropthingsDataContext.SubsystemEnum.Page, userGuid, LinqQueries.CompiledQuery_GetPagesByUserId, (query) => query.First<Page>());
        }

        public Page GetOverridableStartPageOfUser(Guid userGuid)
        {
            return _database.GetSingle<Page, Guid>(DropthingsDataContext.SubsystemEnum.Page, userGuid, LinqQueries.CompiledQuery_GetOverridableStartPageByUser);
        }

        #endregion Methods
    }
}