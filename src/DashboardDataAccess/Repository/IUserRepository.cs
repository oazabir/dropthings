namespace Dropthings.DataAccess.Repository
{
    using System;

    public interface IUserRepository
    {
        #region Methods

        Dropthings.DataAccess.aspnet_User GetUserByUserGuid(Guid userGuid);

        Dropthings.DataAccess.aspnet_User GetUserFromUserName(string userName);

        Guid GetUserGuidFromUserName(string userName);

        #endregion Methods
    }
}