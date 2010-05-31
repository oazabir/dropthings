using System;
namespace Dropthings.Data.Repository
{
    public interface ITabRepository : IDisposable
    {
        void Delete(Tab Tab);
        Tab GetFirstTabOfUser(Guid userGuid);
        System.Collections.Generic.List<Tab> GetLockedTabsOfUser(Guid userGuid, bool isDownForMaintenenceMode);
        System.Collections.Generic.List<Tab> GetLockedTabsOfUserByMaintenenceMode(Guid userGuid, bool isInMaintenenceMode);
        System.Collections.Generic.List<Tab> GetMaintenenceTabsOfUser(Guid userGuid);
        Tab GetOverridableStartTabOfUser(Guid userGuid);
        Tab GetTabById(int TabId);
        System.Collections.Generic.List<int> GetTabIdByUserGuid(Guid userGuid);
        string GetTabOwnerName(int TabId);
        System.Collections.Generic.List<Tab> GetTabsOfUser(Guid userGuid);
        Tab Insert(Tab Tab);
        void Update(Tab Tab);
        void UpdateList(System.Collections.Generic.IEnumerable<Tab> Tabs);
    }
}
