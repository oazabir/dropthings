namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RoleTemplateRepository : Dropthings.DataAccess.Repository.IRoleTemplateRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;

        #endregion Fields

        #region Constructors

        public RoleTemplateRepository(IDropthingsDataContext database)
        {
            this._database = database;
        }

        #endregion Constructors

        #region Methods

        public void Delete(int id)
        {
            _database.DeleteByPK<RoleTemplate, int>(DropthingsDataContext.SubsystemEnum.User, id);
        }

        public void Delete(RoleTemplate page)
        {
            _database.Delete<RoleTemplate>(DropthingsDataContext.SubsystemEnum.User, page);
        }

        public List<RoleTemplate> GeAllRoleTemplates()
        {
            return _database.GetList<RoleTemplate>(DropthingsDataContext.SubsystemEnum.User, LinqQueries.CompiledQuery_GetRoleTemplates);
        }

        public RoleTemplate GetRoleTemplateByRoleName(string roleName)
        {
            return _database.GetSingle<RoleTemplate, string>(DropthingsDataContext.SubsystemEnum.User, roleName, LinqQueries.CompiledQuery_GetRoleTemplateByRoleName);
        }

        public RoleTemplate GetRoleTemplateByTemplateUserName(string userName)
        {
            return _database.GetSingle<RoleTemplate, string>(DropthingsDataContext.SubsystemEnum.User, userName, LinqQueries.CompiledQuery_GetRoleTemplateByTemplateUserName);
        }

        public RoleTemplate GetRoleTemplatesByUserId(Guid userId)
        {
            return _database.GetSingle<RoleTemplate, Guid>(DropthingsDataContext.SubsystemEnum.User, userId, LinqQueries.CompiledQuery_GetRoleTemplatesByUserId);
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