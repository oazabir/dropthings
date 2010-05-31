namespace Dropthings.Test.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web.Profile;

    using Dropthings.Business.Facade.Test.Helper;
    using Dropthings.Web.Framework;
    using Xunit;
    using SubSpec;

    /// <summary>
    /// Summary description for TestProfileProvider
    /// </summary>
    public class TestProfileProvider
    {

        #region Methods

        /// <summary>
        /// I have modified some asp.net membership provider SPs and optimized them.
        /// If you don't have those optimizations, then this test will fail.
        /// </summary>
        [Specification]
        public void Profile_LastUpdate_Should_Change_Every_30_Mins()
        {
            var profile = default(UserProfile);
            DateTime firstLastActivityDate = default(DateTime);

            "Given a new user profile".Context(() => 
            {
                profile = MembershipHelper.CreateNewAnonUser();
                firstLastActivityDate = profile.LastActivityDate;
            });

            "when profile is updated within 30 mins".Do(() =>
            {
                TimeSpan diff = DateTime.Now.ToUniversalTime() - profile.LastActivityDate;
                Assert.True(diff.TotalMinutes < 1, "LastActivityDate was not set to current date time");

                Thread.Sleep(5000);
                profile.Fullname = "Changed to some new value";
                profile.Save();
            });

            "it should not change the LastActivityDate in AspNetUsers table".Assert(() =>
            {
                ProfileInfoCollection profiles = ProfileManager.FindProfilesByUserName(ProfileAuthenticationOption.Anonymous, profile.UserName);
                ProfileInfo existingProfile = profiles[profile.UserName];
                Assert.Equal(firstLastActivityDate, existingProfile.LastActivityDate);
            });

        }

        #endregion Methods
    }
}