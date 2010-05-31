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

        public List<AspNetRole> GetAllRole()
        {
            return AspectF.Define
                .Cache<List<AspNetRole>>(_cacheResolver, CacheKeys.RoleKeys.AllRoles())
                .Return<List<AspNetRole>>(() =>
                    _database.Query(CompiledQueries.RoleQueries.GetAllRole)
                    .ToList());
        }

        public AspNetRole GetRoleByRoleName(string roleName)
        {
            return AspectF.Define
                .Cache<AspNetRole>(_cacheResolver, CacheKeys.RoleKeys.RoleByRoleName(roleName))
                .Return<AspNetRole>(() =>
                    _database.Query(
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