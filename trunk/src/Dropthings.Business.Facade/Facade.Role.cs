using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dropthings.Data;

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
            // TODO: Facade is not supposed to do this. It's the job of repositorys
            Roles.CreateRole(roleName);
            return this.roleRepository.GetRoleByRoleName(roleName);
        }

        public void DeleteRole(string roleName)
        {
            Roles.DeleteRole(roleName);
        }

        #endregion
    }
}
