using System;
namespace Dropthings.Data.Repository
{
    public interface IUserSettingRepository : IDisposable
    {
        void Delete(Dropthings.Data.UserSetting userSetting);
        Dropthings.Data.UserSetting GetUserSettingByUserGuid(Guid userGuid);
        Dropthings.Data.UserSetting Insert(Dropthings.Data.UserSetting setting);
        void Update(Dropthings.Data.UserSetting userSetting);
    }
}
