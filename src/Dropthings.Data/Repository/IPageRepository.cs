using System;
namespace Dropthings.Data.Repository
{
    public interface IPageRepository : IDisposable
    {
        void Delete(Dropthings.Data.Page page);
        Dropthings.Data.Page GetFirstPageOfUser(Guid userGuid);
        System.Collections.Generic.List<Dropthings.Data.Page> GetLockedPagesOfUser(Guid userGuid, bool isDownForMaintenenceMode);
        System.Collections.Generic.List<Dropthings.Data.Page> GetLockedPagesOfUserByMaintenenceMode(Guid userGuid, bool isInMaintenenceMode);
        System.Collections.Generic.List<Dropthings.Data.Page> GetMaintenencePagesOfUser(Guid userGuid);
        Dropthings.Data.Page GetOverridableStartPageOfUser(Guid userGuid);
        Dropthings.Data.Page GetPageById(int pageId);
        System.Collections.Generic.List<int> GetPageIdByUserGuid(Guid userGuid);
        string GetPageOwnerName(int pageId);
        System.Collections.Generic.List<Dropthings.Data.Page> GetPagesOfUser(Guid userGuid);
        Dropthings.Data.Page Insert(Dropthings.Data.Page page);
        void Update(Dropthings.Data.Page page);
        void UpdateList(System.Collections.Generic.IEnumerable<Dropthings.Data.Page> pages);
    }
}
