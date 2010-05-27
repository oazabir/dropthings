using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmarALZabir.AspectF;
using Dropthings.Util;

namespace Dropthings.Data.Repository
{
    public class UserSettingRepository : Dropthings.Data.Repository.IUserSettingRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public UserSettingRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public UserSetting GetUserSettingByUserGuid(Guid userGuid)
        {
            return AspectF.Define
                .Cache<UserSetting>(_cacheResolver, CacheKeys.UserKeys.UserSettingByUserGuid(userGuid))
                .Return<UserSetting>(() => _database.Query(
                            CompiledQueries.UserQueries.GetUserSettingByUserGuid, userGuid)
                            .FirstOrDefault());
        }

        public void Delete(UserSetting userSetting)
        {
            RemoveUserSettingCacheForUser(userSetting);
            _database.Delete<UserSetting>( userSetting);
        }

	    public UserSetting Insert(UserSetting setting)
        {
            var user = setting.aspnet_Users;
            var page = setting.Page;
            
            setting.aspnet_Users = null;
            setting.Page = null;

            _database.Insert<aspnet_User, Page, UserSetting>(user, page,
                (u, s) => s.aspnet_Users = u,
                (p, s) => s.Page = p,
                setting);
            
            setting.aspnet_Users = user;
            setting.Page = page;
            return setting;
        }

        public void Update(UserSetting userSetting)
        {
            RemoveUserSettingCacheForUser(userSetting);
            _database.Update<UserSetting>(userSetting);
        }

        private void RemoveUserSettingCacheForUser(UserSetting userSetting)
        {
            _cacheResolver.Remove(CacheKeys.UserKeys.UserSettingByUserGuid(userSetting.UserId));
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            _database.Dispose();
        }

        #endregion
    }
}
