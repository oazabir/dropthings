namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;
    using System.Linq.Expressions;

    public class UserRepository : Dropthings.DataAccess.Repository.IUserRepository
    {
        #region Fields

        private readonly IDropthingsDataContext _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public UserRepository(IDropthingsDataContext database, ICache cacheResolver)
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

        public int GetMemberCount()
        {
            return _database.aspnet_MembersSource.Count();
        }

        public List<aspnet_Membership> GetPagedMember(int startIndex, int maxRows, string sortExpression)
        {
            List<aspnet_Membership> members = new List<aspnet_Membership>();
            bool asc = !sortExpression.Contains("DESC");

            switch (sortExpression.Split(' ')[0])
            {
                case "Username":
                    members = Page<string>(startIndex, maxRows, m => m.aspnet_User.UserName, asc);
                    break;
                default:
                    members = Page<DateTime>(startIndex, maxRows, m => m.CreateDate, false);
                    break;
            }

            return members;
        }

        public int GetMemberCountByRole(string roleName)
        {
            return _database.GetQueryResult<aspnet_Membership, string, int>(
                                    DropthingsDataContext.SubsystemEnum.User,
                                    roleName,
                                    LinqQueries.CompiledQuery_GetMembersInRoleCount,
                                    (query) => query.Count());
        }

        public List<aspnet_Membership> GetPagedMemberByRole(string roleName, int startIndex, int maxRows)
        {
            return _database.GetPagedList<aspnet_Membership, string>(DropthingsDataContext.SubsystemEnum.User,
                        roleName,startIndex, maxRows,
                        LinqQueries.CompiledQuery_GetMembersInRole, LinqQueries.aspnet_Membership_Options_With_aspnet_Users);
        }

        private List<aspnet_Membership> Page<TResult>(int startIndex, int maxRows, Expression<Func<aspnet_Membership, TResult>> sortKeySelector, bool asc)
        {
            if (asc)
            {
                return _database.GetPagedList<aspnet_Membership>(DropthingsDataContext.SubsystemEnum.User,
                        startIndex, maxRows,
                        ((dc) => dc.aspnet_Memberships.OrderBy(sortKeySelector)), LinqQueries.aspnet_Membership_Options_With_aspnet_Users);
            }
            else
            {
                return _database.GetPagedList<aspnet_Membership>(DropthingsDataContext.SubsystemEnum.User,
                        startIndex, maxRows,
                        ((dc) => dc.aspnet_Memberships.OrderByDescending(sortKeySelector)), LinqQueries.aspnet_Membership_Options_With_aspnet_Users);
            }
        }

        #endregion Methods
    }
}