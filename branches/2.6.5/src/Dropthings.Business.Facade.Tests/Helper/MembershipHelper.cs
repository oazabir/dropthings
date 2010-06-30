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
        public static UserProfile CreateNewAnonUser()
        {
            UserProfile profile = UserProfile.Create(Guid.NewGuid().ToString(), false) as UserProfile;
            profile.IsFirstVisit = false;
            profile.Save();
            return profile;
        }

        #endregion Methods
    }
}