using System;
namespace Dropthings.DataAccess.Repository
{
    public interface IUserSettingRepository
    {
        Dropthings.DataAccess.UserSetting GetUserSettingByUserGuid(Guid userGuid);

        void Delete(int id);
void Delete(UserSetting page);
UserSetting Insert(Action<UserSetting> populate);
void Update(UserSetting page, Action<UserSetting> detach, Action<UserSetting> postAttachUpdate);

    }
}
