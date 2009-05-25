namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RoleRepository : Dropthings.DataAccess.Repository.IRoleRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;

        #endregion Fields

        #region Constructors

        public RoleRepository(IDropthingsDataContext database)
        {
            this._database = database;
        }

        #endregion Constructors

        #region Methods

        public List<aspnet_Role> GetAllRole()
        {
            return _database.GetList<aspnet_Role>(DropthingsDataContext.SubsystemEnum.User, LinqQueries.CompiledQuery_GetAllRole);
        }

        public aspnet_Role GetRoleByRoleName(string roleName)
        {
            return _database.GetSingle<aspnet_Role, string>(DropthingsDataContext.SubsystemEnum.User, roleName, LinqQueries.CompiledQuery_GetRoleByRoleName);
        }

        #endregion Methods
    }
}