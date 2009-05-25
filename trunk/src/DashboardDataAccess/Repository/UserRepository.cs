namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UserRepository : Dropthings.DataAccess.Repository.IUserRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;

        #endregion Fields

        #region Constructors

        public UserRepository(IDropthingsDataContext database)
        {
            this._database = database;
        }

        #endregion Constructors

        #region Methods

        public aspnet_User GetUserByUserGuid(Guid userGuid)
        {
            return _database.GetSingle<aspnet_User, Guid>(DropthingsDataContext.SubsystemEnum.User, userGuid, LinqQueries.CompiledQuery_GetUserByUserGuid);
        }

        public aspnet_User GetUserFromUserName(string userName)
        {
            return _database.GetSingle<aspnet_User, string>(DropthingsDataContext.SubsystemEnum.User, userName, LinqQueries.CompiledQuery_GetUserFromUserName);
        }

        public Guid GetUserGuidFromUserName(string userName)
        {
            return _database.GetSingle<Guid, string>(DropthingsDataContext.SubsystemEnum.User, userName, LinqQueries.CompiledQuery_GetUserGuidFromUserName);
        }

        public UserSetting GetUserSettingByUserGuid(Guid userGuid)
        {
            return _database.GetSingle<UserSetting, Guid>(DropthingsDataContext.SubsystemEnum.User, userGuid, LinqQueries.CompiledQuery_GetUserSettingByUserGuid);
        }

        #endregion Methods
    }
}