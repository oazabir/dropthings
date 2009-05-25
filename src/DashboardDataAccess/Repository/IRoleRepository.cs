namespace Dropthings.DataAccess.Repository
{
    using System;

    public interface IRoleRepository
    {
        #region Methods

        System.Collections.Generic.List<Dropthings.DataAccess.aspnet_Role> GetAllRole();

        Dropthings.DataAccess.aspnet_Role GetRoleByRoleName(string roleName);

        #endregion Methods
    }
}