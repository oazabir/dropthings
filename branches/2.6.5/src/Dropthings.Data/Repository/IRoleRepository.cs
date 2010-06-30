using System;
namespace Dropthings.Data.Repository
{
    public interface IRoleRepository : IDisposable
    {
        System.Collections.Generic.List<Dropthings.Data.AspNetRole> GetAllRole();
        Dropthings.Data.AspNetRole GetRoleByRoleName(string roleName);
    }
}
