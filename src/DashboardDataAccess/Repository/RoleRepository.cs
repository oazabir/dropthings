namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class RoleRepository : Dropthings.DataAccess.Repository.IRoleRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public RoleRepository(IDropthingsDataContext database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public List<aspnet_Role> GetAllRole()
        {
            return AspectF.Define
                .Cache<List<aspnet_Role>>(_cacheResolver, CacheSetup.CacheKeys.AllRoles())
                .Return<List<aspnet_Role>>(() =>
                    _database.GetList<aspnet_Role>(DropthingsDataContext.SubsystemEnum.User, LinqQueries.CompiledQuery_GetAllRole));
        }

        public aspnet_Role GetRoleByRoleName(string roleName)
        {
            return AspectF.Define
                .Cache<aspnet_Role>(_cacheResolver, CacheSetup.CacheKeys.RoleByRoleName(roleName))
                .Return<aspnet_Role>(() =>
                    _database.GetSingle<aspnet_Role, string>(DropthingsDataContext.SubsystemEnum.User, roleName, LinqQueries.CompiledQuery_GetRoleByRoleName));
        }

        #endregion Methods
    }
}