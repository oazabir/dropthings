namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;

    public class UserRepository : Dropthings.DataAccess.Repository.IUserRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICacheResolver _cacheResolver;

        #endregion Fields

        #region Constructors

        public UserRepository(IDropthingsDataContext database, ICacheResolver cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public aspnet_User GetUserByUserGuid(Guid userGuid)
        {
            return AspectF.Define
                .Cache<aspnet_User>(_cacheResolver, CacheSetup.CacheKeys.UserFromUserGuid(userGuid))
                .Return<aspnet_User>(() =>
                    _database.GetSingle<aspnet_User, Guid>(DropthingsDataContext.SubsystemEnum.User, userGuid, LinqQueries.CompiledQuery_GetUserByUserGuid));
        }

        public aspnet_User GetUserFromUserName(string userName)
        {
            return AspectF.Define
                .Cache<aspnet_User>(_cacheResolver, CacheSetup.CacheKeys.UserFromUserName(userName))
                .Return<aspnet_User>(() =>
                    _database.GetSingle<aspnet_User, string>(DropthingsDataContext.SubsystemEnum.User, userName, LinqQueries.CompiledQuery_GetUserFromUserName));
        }

        public Guid GetUserGuidFromUserName(string userName)
        {
            return AspectF.Define
                .Cache<Guid>(_cacheResolver, CacheSetup.CacheKeys.UserGuidFromUserName(userName))
                .Return<Guid>(() =>
                    _database.GetSingle<Guid, string>(DropthingsDataContext.SubsystemEnum.User, userName, LinqQueries.CompiledQuery_GetUserGuidFromUserName));
        }

        #endregion Methods
    }
}