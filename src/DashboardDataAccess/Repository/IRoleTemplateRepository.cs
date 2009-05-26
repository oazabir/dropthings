namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;

    public interface IRoleTemplateRepository
    {
        #region Methods

        void Delete(int id);

        void Delete(RoleTemplate page);

        List<RoleTemplate> GeAllRoleTemplates();

        Dropthings.DataAccess.RoleTemplate GetRoleTemplateByRoleName(string roleName);

        Dropthings.DataAccess.RoleTemplate GetRoleTemplateByTemplateUserName(string userName);

        Dropthings.DataAccess.RoleTemplate GetRoleTemplatesByUserId(Guid userId);

        RoleTemplate Insert(Action<RoleTemplate> populate);

        void Update(RoleTemplate page, Action<RoleTemplate> detach, Action<RoleTemplate> postAttachUpdate);

        #endregion Methods
    }
}