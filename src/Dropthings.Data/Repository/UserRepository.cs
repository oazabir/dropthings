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

        public List<AspNetRole> GetRolesOfUser(Guid userGuid)
        {
            return AspectF.Define
                .Cache<List<AspNetRole>>(_cacheResolver, CacheKeys.UserKeys.RolesOfUser(userGuid))
                .Return<List<AspNetRole>>(() =>
                    _database.Query(CompiledQueries.UserQueries.GetRolesOfUser, userGuid)
                    .ToList());
        }

        public AspNetUser GetUserByUserGuid(Guid userGuid)
        {
            return AspectF.Define
                .Cache<AspNetUser>(_cacheResolver, CacheKeys.UserKeys.UserFromUserGuid(userGuid))
                .Return<AspNetUser>(() =>
                    _database.Query(CompiledQueries.UserQueries.GetUserByUserGuid, userGuid)
                            .First());
        }

        public AspNetUser GetUserFromUserName(string userName)
        {
            return AspectF.Define
                .Cache<AspNetUser>(_cacheResolver, CacheKeys.UserKeys.UserFromUserName(userName))
                .Return<AspNetUser>(() =>
                    _database.Query(CompiledQueries.UserQueries.GetUserFromUserName, userName)
                    .FirstOrDefault());
        }

        public Guid GetUserGuidFromUserName(string userName)
        {
            return AspectF.Define
                .Cache<Guid>(_cacheResolver, CacheKeys.UserKeys.UserGuidFromUserName(userName))
                .Return<Guid>(() =>
                    _database.Query(CompiledQueries.UserQueries.GetUserGuidFromUserName, userName)
                    .FirstOrDefault());
        }

        //public int GetMemberCount()
        //{
        //    return _database.aspnet_Members.Count();
        //}

        //public List<AspNetMembership> GetPagedMember(int startIndex, int maxRows, string sortExpression)
        //{
        //    List<AspNetMembership> members = new List<AspNetMembership>();
        //    bool asc = !sortExpression.Contains("DESC");

        //    switch (sortExpression.Split(' ')[0])
        //    {
        //        case "Username":
        //            members = Page<string>(startIndex, maxRows, m => m.AspNetUser.UserName, asc);
        //            break;
        //        default:
        //            members = Page<DateTime>(startIndex, maxRows, m => m.CreateDate, false);
        //            break;
        //    }

        //    return members;
        //}

        //public int GetMemberCountByRole(string roleName)
        //{
        //    return _database.GetQueryResult<AspNetMembership, string, int>(
                                    
        //                            roleName,
        //                            CompiledQueries.UserQueries.GetMembersInRoleCount,
        //                            (query) => query.Count());
        //}

        //public List<AspNetMembership> GetPagedMemberByRole(string roleName, int startIndex, int maxRows)
        //{
        //    return _database.GetPagedList<AspNetMembership, string>(
        //                roleName,startIndex, maxRows,
        //                CompiledQueries.UserQueries.GetMembersInRole, LinqQueries.AspNetMembership_Options_With_AspNetUsers);
        //}

        //private List<AspNetMembership> Page<TResult>(int startIndex, int maxRows, Expression<Func<AspNetMembership, TResult>> sortKeySelector, bool asc)
        //{
        //    if (asc)
        //    {
        //        return _database.GetPagedList<AspNetMembership>(
        //                startIndex, maxRows,
        //                ((dc) => dc.AspNetMemberships.OrderBy(sortKeySelector)), LinqQueries.AspNetMembership_Options_With_AspNetUsers);
        //    }
        //    else
        //    {
        //        return _database.GetPagedList<AspNetMembership>(
        //                startIndex, maxRows,
        //                ((dc) => dc.AspNetMemberships.OrderByDescending(sortKeySelector)), LinqQueries.AspNetMembership_Options_With_AspNetUsers);
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