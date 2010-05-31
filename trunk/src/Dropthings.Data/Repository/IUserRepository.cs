using System;
namespace Dropthings.Data.Repository
{
    public interface IUserRepository : IDisposable
    {
        System.Collections.Generic.List<Dropthings.Data.AspNetRole> GetRolesOfUser(Guid userGuid);
        Dropthings.Data.AspNetUser GetUserByUserGuid(Guid userGuid);
        Dropthings.Data.AspNetUser GetUserFromUserName(string userName);
        Guid GetUserGuidFromUserName(string userName);
    }
}
