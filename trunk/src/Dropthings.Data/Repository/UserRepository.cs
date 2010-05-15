namespace Dropthings.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OmarALZabir.AspectF;
    using Dropthings.Util;
    using System.Linq.Expressions;

    public class UserRepository : Dropthings.Data.Repository.IUserRepository 
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly ICache _cacheResolver;

        #endregion Fields

        #region Constructors

        public UserRepository(IDatabase database, ICache cacheResolver)
        {
            this._database = database;
            this._cacheResolver = cacheResolver;
        }

        #endregion Constructors

        #region Methods

        public List<aspnet_Role> GetRolesOfUser(Guid userGuid)
        {
            return AspectF.Define
                .Cache<List<aspnet_Role>>(_cacheResolver, CacheKeys.UserKeys.RolesOfUser(userGuid))
                .Return<List<aspnet_Role>>(() =>
                    _database.Query<Guid, aspnet_Role>(CompiledQueries.UserQueries.GetRolesOfUser, userGuid)
                    .ToList());
        }

        public aspnet_User GetUserByUserGuid(Guid userGuid)
        {
            return AspectF.Define
                .Cache<aspnet_User>(_cacheResolver, CacheKeys.UserKeys.UserFromUserGuid(userGuid))
                .Return<aspnet_User>(() =>
                    _database.Query<Guid, aspnet_User>(CompiledQueries.UserQueries.GetUserByUserGuid, userGuid)
                            .First());
        }

        public aspnet_User GetUserFromUserName(string userName)
        {
            return AspectF.Define
                .Cache<aspnet_User>(_cacheResolver, CacheKeys.UserKeys.UserFromUserName(userName))
                .Return<aspnet_User>(() =>
                    _database.Query<string, aspnet_User>(CompiledQueries.UserQueries.GetUserFromUserName, userName)
                    .FirstOrDefault());
        }

        public Guid GetUserGuidFromUserName(string userName)
        {
            return AspectF.Define
                .Cache<Guid>(_cacheResolver, CacheKeys.UserKeys.UserGuidFromUserName(userName))
                .Return<Guid>(() =>
                    _database.Query<string, Guid>(CompiledQueries.UserQueries.GetUserGuidFromUserName, userName)
                    .FirstOrDefault());
        }

        //public int GetMemberCount()
        //{
        //    return _database.aspnet_Members.Count();
        //}

        //public List<aspnet_Membership> GetPagedMember(int startIndex, int maxRows, string sortExpression)
        //{
        //    List<aspnet_Membership> members = new List<aspnet_Membership>();
        //    bool asc = !sortExpression.Contains("DESC");

        //    switch (sortExpression.Split(' ')[0])
        //    {
        //        case "Username":
        //            members = Page<string>(startIndex, maxRows, m => m.aspnet_User.UserName, asc);
        //            break;
        //        default:
        //            members = Page<DateTime>(startIndex, maxRows, m => m.CreateDate, false);
        //            break;
        //    }

        //    return members;
        //}

        //public int GetMemberCountByRole(string roleName)
        //{
        //    return _database.GetQueryResult<aspnet_Membership, string, int>(
                                    
        //                            roleName,
        //                            CompiledQueries.UserQueries.GetMembersInRoleCount,
        //                            (query) => query.Count());
        //}

        //public List<aspnet_Membership> GetPagedMemberByRole(string roleName, int startIndex, int maxRows)
        //{
        //    return _database.GetPagedList<aspnet_Membership, string>(
        //                roleName,startIndex, maxRows,
        //                CompiledQueries.UserQueries.GetMembersInRole, LinqQueries.aspnet_Membership_Options_With_aspnet_Users);
        //}

        //private List<aspnet_Membership> Page<TResult>(int startIndex, int maxRows, Expression<Func<aspnet_Membership, TResult>> sortKeySelector, bool asc)
        //{
        //    if (asc)
        //    {
        //        return _database.GetPagedList<aspnet_Membership>(
        //                startIndex, maxRows,
        //                ((dc) => dc.aspnet_Memberships.OrderBy(sortKeySelector)), LinqQueries.aspnet_Membership_Options_With_aspnet_Users);
        //    }
        //    else
        //    {
        //        return _database.GetPagedList<aspnet_Membership>(
        //                startIndex, maxRows,
        //                ((dc) => dc.aspnet_Memberships.OrderByDescending(sortKeySelector)), LinqQueries.aspnet_Membership_Options_With_aspnet_Users);
        //    }
        //}

        #endregion Methods

        #region IDisposable Members

        public void Dispose()
        {
            _database.Dispose();
        }

        #endregion
    }
}