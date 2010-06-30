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
            var user = roleTemplate.AspNetUser;
            var role = roleTemplate.AspNetRole;

            roleTemplate.AspNetUser = null;
            roleTemplate.AspNetRole = null;

            var result = _database.Insert<AspNetUser, AspNetRole, RoleTemplate>(
                user, role,    
                (u, rt) => rt.AspNetUser = u,
                (r, rt) => rt.AspNetRole = r,
                roleTemplate);

            result.AspNetUser = user;
            result.AspNetRole = role;
            
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