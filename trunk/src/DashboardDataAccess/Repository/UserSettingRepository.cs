using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmarALZabir.AspectF;
using Dropthings.Util;

namespace Dropthings.DataAccess.Repository
{
    public class UserSettingRepository : Dropthings.DataAccess.Repository.IUserSettingRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICacheResolver _cacheResolver;

        #endregion Fields

        #region Constructors

        public UserSettingRepository(IDropthingsDataContext database, ICacheResolver cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public UserSetting GetUserSettingByUserGuid(Guid userGuid)
        {
            return AspectF.Define
                .Cache<UserSetting>(_cacheResolver, CacheSetup.CacheKeys.UserSettingByUserGuid(userGuid))
                .Return<UserSetting>(() =>
                    _database.GetSingle<UserSetting, Guid>(DropthingsDataContext.SubsystemEnum.User, userGuid, LinqQueries.CompiledQuery_GetUserSettingByUserGuid));
        }

        public void Delete(int id)
        {
            _database.DeleteByPK<UserSetting, int>(DropthingsDataContext.SubsystemEnum.User, id);
        }

        public void Delete(UserSetting userSetting)
        {
            RemoveUserSettingCacheForUser(userSetting);
            _database.Delete<UserSetting>(DropthingsDataContext.SubsystemEnum.User, userSetting);
        }

	    public UserSetting Insert(Action<UserSetting> populate)
        {
            return _database.Insert<UserSetting>(DropthingsDataContext.SubsystemEnum.User, populate);
        }

        public void Update(UserSetting userSetting, Action<UserSetting> detach, Action<UserSetting> postAttachUpdate)
        {
            RemoveUserSettingCacheForUser(userSetting);
            _database.UpdateObject<UserSetting>(DropthingsDataContext.SubsystemEnum.User, userSetting, detach, postAttachUpdate);
        }

        private void RemoveUserSettingCacheForUser(UserSetting userSetting)
        {
            _cacheResolver.Remove(CacheSetup.CacheKeys.UserSettingByUserGuid(userSetting.UserId));
        }

        #endregion

    }
}
