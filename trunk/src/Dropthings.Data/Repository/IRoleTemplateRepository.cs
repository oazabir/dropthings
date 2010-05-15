using System;
namespace Dropthings.Data.Repository
{
    public interface IRoleTemplateRepository : IDisposable
    {
        void Delete(Dropthings.Data.RoleTemplate roleTemplate);
        void Dispose();
        System.Collections.Generic.List<Dropthings.Data.RoleTemplate> GeAllRoleTemplates();
        Dropthings.Data.RoleTemplate GetRoleTemplateByRoleName(string roleName);
        Dropthings.Data.RoleTemplate GetRoleTemplateByTemplateUserName(string userName);
        Dropthings.Data.RoleTemplate GetRoleTemplatesByUserId(Guid userId);
        Dropthings.Data.RoleTemplate Insert(Dropthings.Data.RoleTemplate roleTemplate);
        void Update(Dropthings.Data.RoleTemplate roleTemplate);
    }
}
