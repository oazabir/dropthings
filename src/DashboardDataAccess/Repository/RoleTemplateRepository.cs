namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class RoleTemplateRepository : Dropthings.DataAccess.Repository.IRoleTemplateRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public RoleTemplateRepository(IDropthingsDataContext database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public void Delete(int id)
        {
            _database.DeleteByPK<RoleTemplate, int>(DropthingsDataContext.SubsystemEnum.User, id);
        }

        public void Delete(RoleTemplate roleTemplate)
        {
            _database.Delete<RoleTemplate>(DropthingsDataContext.SubsystemEnum.User, roleTemplate);
        }

        public List<RoleTemplate> GeAllRoleTemplates()
        {
            return _database.GetList<RoleTemplate>(DropthingsDataContext.SubsystemEnum.User, LinqQueries.CompiledQuery_GetRoleTemplates);
        }

        public RoleTemplate GetRoleTemplateByRoleName(string roleName)
        {
            return AspectF.Define
                .Cache<RoleTemplate>(_cacheResolver, CacheSetup.CacheKeys.RoleTemplateByRoleName(roleName))
                .Return<RoleTemplate>(() =>
                    _database.GetSingle<RoleTemplate, string>(DropthingsDataContext.SubsystemEnum.User, roleName, LinqQueries.CompiledQuery_GetRoleTemplateByRoleName));
        }

        public RoleTemplate GetRoleTemplateByTemplateUserName(string userName)
        {
            return AspectF.Define
                .Cache<RoleTemplate>(_cacheResolver, CacheSetup.CacheKeys.RoleTemplateByUser(userName))
                .Return<RoleTemplate>(() =>
                    _database.GetSingle<RoleTemplate, string>(DropthingsDataContext.SubsystemEnum.User, userName, LinqQueries.CompiledQuery_GetRoleTemplateByTemplateUserName));
        }

        public RoleTemplate GetRoleTemplatesByUserId(Guid userId)
        {
            return AspectF.Define
                .Cache<RoleTemplate>(_cacheResolver, CacheSetup.CacheKeys.RoleTemplateByUser(userId))
                .Return<RoleTemplate>(() =>
                    _database.GetSingle<RoleTemplate, Guid>(DropthingsDataContext.SubsystemEnum.User, userId, LinqQueries.CompiledQuery_GetRoleTemplatesByUserId));
        }

        public RoleTemplate Insert(Action<RoleTemplate> populate)
        {
            return _database.Insert<RoleTemplate>(DropthingsDataContext.SubsystemEnum.User, populate);
        }

        public void Update(RoleTemplate page, Action<RoleTemplate> detach, Action<RoleTemplate> postAttachUpdate)
        {
            _database.UpdateObject<RoleTemplate>(DropthingsDataContext.SubsystemEnum.User, page, detach, postAttachUpdate);
        }

        #endregion Methods
    }
}