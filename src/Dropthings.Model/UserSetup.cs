namespace Dropthings.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class UserSetup
    {
        #region Properties

        public Page CurrentPage
        {
            get;
            set;
        }

        public List<Page> UserPages
        {
            get;
            set;
        }

        public List<Page> UserSharedPages
        {
            get;
            set;
        }

        public UserSetting UserSetting
        {
            get;
            set;
        }

        public Guid CurrentUserId
        {
            get;
            set;
        }

        public RoleTemplate RoleTemplate
        {
            get; 
            set;
        }

        #endregion Properties
    }
}