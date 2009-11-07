namespace Dropthings.Business.Facade.Test.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Profile;

    using Dropthings.Web.Framework;
    using System.Diagnostics;

    internal class MembershipHelper
    {
        #region Methods

        [DebuggerStepThrough]
        public static void UsingNewAnonUser(Action<UserProfile> callback)
        {
            UserProfile profile = UserProfile.Create(Guid.NewGuid().ToString(), false) as UserProfile;
            profile.IsFirstVisit = false;
            profile.Save();

            try
            {
                callback(profile);
            }
            finally
            {
                ProfileManager.DeleteProfile(profile.UserName);
            }
        }

        #endregion Methods
    }
}