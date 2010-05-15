using System;
namespace Dropthings.Data.Repository
{
    public interface IUserRepository : IDisposable
    {
        void Dispose();
        System.Collections.Generic.List<Dropthings.Data.aspnet_Role> GetRolesOfUser(Guid userGuid);
        Dropthings.Data.aspnet_User GetUserByUserGuid(Guid userGuid);
        Dropthings.Data.aspnet_User GetUserFromUserName(string userName);
        Guid GetUserGuidFromUserName(string userName);
    }
}
