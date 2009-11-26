namespace Dropthings.DataAccess.Repository
{
    using System;

    public interface IRoleRepository
    {
        #region Methods

        System.Collections.Generic.List<Dropthings.DataAccess.aspnet_Role> GetAllRole();

        Dropthings.DataAccess.aspnet_Role GetRoleByRoleName(string roleName);

        Dropthings.DataAccess.aspnet_Role Insert(Action action, string roleName);

        void Delete(Action action, string roleName);

        #endregion Methods
    }
}