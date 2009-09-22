namespace Dropthings.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UserTemplateSetting
    {
        #region Properties

        public List<UserSettingTemplateElement> AllUserSettingTemplate
        {
            get;
            set;
        }

        public UserSettingTemplateElement AnonUserSettingTemplate
        {
            get;
            set;
        }

        public bool CloneAnonProfileEnabled
        {
            get;
            set;
        }

        public bool CloneRegisteredProfileEnabled
        {
            get;
            set;
        }

        public UserSettingTemplateElement RegisteredUserSettingTemplate
        {
            get;
            set;
        }

        #endregion Properties
    }
}