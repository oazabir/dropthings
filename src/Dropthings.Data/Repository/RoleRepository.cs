namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class RoleRepository : Dropthings.Data.Repository.IRoleRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public RoleRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        private void RemoveRoleNameDependentItems(string roleName)
        {
            RemoveRoleCollection();
            _cacheResolver.Remove(CacheKeys.RoleKeys.RoleByRoleName(roleName));
        }

        private void RemoveRoleCollection()
        {
            _cacheResolver.Remove(CacheKeys.RoleKeys.AllRoles());
        }

        public List<aspnet_Role> GetAllRole()
        {
            return AspectF.Define
                .Cache<List<aspnet_Role>>(_cacheResolver, CacheKeys.RoleKeys.AllRoles())
                .Return<List<aspnet_Role>>(() =>
                    _database.Query<aspnet_Role>(CompiledQueries.RoleQueries.GetAllRole)
                    .ToList());
        }

        public aspnet_Role GetRoleByRoleName(string roleName)
        {
            return AspectF.Define
                .Cache<aspnet_Role>(_cacheResolver, CacheKeys.RoleKeys.RoleByRoleName(roleName))
                .Return<aspnet_Role>(() =>
                    _database.Query<string, aspnet_Role>(
                        CompiledQueries.RoleQueries.GetRoleByRoleName, roleName)
                    .First());
        }

        #endregion Methods

        #region IDisposable Members

        public void Dispose()
        {
            _database.Dispose();
        }

        #endregion
    }
}