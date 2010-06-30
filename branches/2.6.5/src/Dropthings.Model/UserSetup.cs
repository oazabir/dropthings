namespace Dropthings.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Data;

    public class UserSetup
    {
        #region Properties

        public Tab CurrentTab
        {
            get;
            set;
        }

        public List<Tab> UserTabs
        {
            get;
            set;
        }

        public List<Tab> UserSharedTabs
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

        //public RoleTemplate RoleTemplate
        //{
        //    get; 
        //    set;
        //}

        //public bool IsRoleTemplateForRegisterUser
        //{
        //    get;
        //    set;
        //}

        #endregion Properties

        public bool IsTemplateUser { get; set; }
    }
}