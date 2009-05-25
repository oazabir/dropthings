using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dropthings.DataAccess.Repository
{
    public class UserSettingRepository : Dropthings.DataAccess.Repository.IUserSettingRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;

        #endregion Fields

        #region Constructors

        public UserSettingRepository(IDropthingsDataContext database)
        {
            this._database = database;
        }

        #endregion Constructors

        #region Methods

        public UserSetting GetUserSettingByUserGuid(Guid userGuid)
        {
            return _database.GetSingle<UserSetting, Guid>(DropthingsDataContext.SubsystemEnum.User, userGuid, LinqQueries.CompiledQuery_GetUserSettingByUserGuid);
        }

        public void Delete(int id)
        {
            _database.DeleteByPK<UserSetting, int>(DropthingsDataContext.SubsystemEnum.User, id);
        }

        public void Delete(UserSetting page)
        {
            _database.Delete<UserSetting>(DropthingsDataContext.SubsystemEnum.User, page);
        }

	    public UserSetting Insert(Action<UserSetting> populate)
        {
            return _database.Insert<UserSetting>(DropthingsDataContext.SubsystemEnum.User, populate);
        }

        public void Update(UserSetting page, Action<UserSetting> detach, Action<UserSetting> postAttachUpdate)
        {
            _database.UpdateObject<UserSetting>(DropthingsDataContext.SubsystemEnum.User, page, detach, postAttachUpdate);
        }

        #endregion

    }
}
