namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class RoleTemplateRepository : IRoleTemplateRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public RoleTemplateRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public void Delete(RoleTemplate roleTemplate)
        {
            _database.Delete<RoleTemplate>( roleTemplate);
        }

        public List<RoleTemplate> GeAllRoleTemplates()
        {
            return _database.Query(CompiledQueries.RoleQueries.GetRoleTemplates)
                .ToList();
        }

        public RoleTemplate GetRoleTemplateByRoleName(string roleName)
        {
            return AspectF.Define
                .Cache<RoleTemplate>(_cacheResolver, CacheKeys.RoleKeys.RoleTemplateByRoleName(roleName))
                .Return<RoleTemplate>(() =>
                    _database.Query(CompiledQueries.RoleQueries.GetRoleTemplateByRoleName, roleName)
                    .FirstOrDefault());
        }

        public RoleTemplate GetRoleTemplateByTemplateUserName(string userName)
        {
            return AspectF.Define
                .Cache<RoleTemplate>(_cacheResolver, CacheKeys.UserKeys.RoleTemplateByUser(userName))
                .Return<RoleTemplate>(() =>
                    _database.Query(CompiledQueries.RoleQueries.GetRoleTemplateByTemplateUserName, userName)
                    .FirstOrDefault());
        }

        public RoleTemplate GetRoleTemplatesByUserId(Guid userId)
        {
            return AspectF.Define
                .Cache<RoleTemplate>(_cacheResolver, CacheKeys.TemplateKeys.RoleTemplateByUser(userId))
                .Return<RoleTemplate>(() =>
                    _database.Query(CompiledQueries.RoleQueries.GetRoleTemplatesByUserId, userId)
                    .FirstOrDefault());
        }

        public RoleTemplate Insert(RoleTemplate roleTemplate)
        {
            var user = roleTemplate.aspnet_Users;
            var role = roleTemplate.aspnet_Roles;

            roleTemplate.aspnet_Users = null;
            roleTemplate.aspnet_Roles = null;

            var result = _database.Insert<aspnet_User, aspnet_Role, RoleTemplate>(
                user, role,    
                (u, rt) => rt.aspnet_Users = u,
                (r, rt) => rt.aspnet_Roles = r,
                roleTemplate);

            result.aspnet_Users = user;
            result.aspnet_Roles = role;
            
            return result;
        }

        public void Update(RoleTemplate roleTemplate)
        {
            _database.Update<RoleTemplate>(roleTemplate);
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