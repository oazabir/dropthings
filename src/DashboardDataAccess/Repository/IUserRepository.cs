namespace Dropthings.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;

    public interface IUserRepository
    {
        #region Methods

        Dropthings.DataAccess.aspnet_User GetUserByUserGuid(Guid userGuid);

        Dropthings.DataAccess.aspnet_User GetUserFromUserName(string userName);

        Guid GetUserGuidFromUserName(string userName);

        int GetMemberCount();

        List<aspnet_Membership> GetPagedMember(int startIndex, int maxRows, string sortExpression);

        int GetMemberCountByRole(string roleName);

        List<aspnet_Membership> GetPagedMemberByRole(string roleName, int startIndex, int maxRows);

        #endregion Methods
    }
}