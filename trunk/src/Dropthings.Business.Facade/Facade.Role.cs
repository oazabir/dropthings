using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dropthings.DataAccess;

using System.Transactions;
using System.Web.Security;
using Dropthings.Configuration;
using OmarALZabir.AspectF;
using Dropthings.Util;

namespace Dropthings.Business.Facade
{
	partial class Facade
    {
        #region Methods

        public aspnet_Role GetRole(string roleName)
        {
            return roleRepository.GetRoleByRoleName(roleName);
        }

        public List<aspnet_Role> GetAllRole()
        {
            return roleRepository.GetAllRole();
        }

        public aspnet_Role InsertRole(string roleName)
        {
            var insertedRole = this.roleRepository.Insert(() =>
            {
                if (!Roles.RoleExists(roleName))
                {
                    Roles.CreateRole(roleName);
                }
                else
                {
                    throw new ArgumentException("Role with name '{0}' already exists".FormatWith(roleName));
                }
            }, roleName);

            return insertedRole;
        }

        public void DeleteRole(string roleName)
        {
            this.roleRepository.Delete(() =>
            {
                Roles.DeleteRole(roleName);
            }, roleName);
        }

        #endregion
    }
}
