namespace Dropthings.Configuration
{
    using System.Configuration;
    using System.Diagnostics;

    public class UserSettingTemplateElement : ConfigurationElement
    {
        #region Properties

        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            [DebuggerStepThrough]
            get { return (string)this["key"]; }
            [DebuggerStepThrough]
            set { this["key"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            [DebuggerStepThrough]
            get { return (string)this["password"]; }
            [DebuggerStepThrough]
            set { this["password"] = value; }
        }

        [ConfigurationProperty("roleNames", IsRequired = true)]
        public string RoleNames
        {
            [DebuggerStepThrough]
            get { return (string)this["roleNames"]; }
            [DebuggerStepThrough]
            set { this["roleNames"] = value; }
        }

        [ConfigurationProperty("templateRoleName", IsRequired = true)]
        public string TemplateRoleName
        {
            [DebuggerStepThrough]
            get { return (string)this["templateRoleName"]; }
            [DebuggerStepThrough]
            set { this["templateRoleName"] = value; }
        }

        [ConfigurationProperty("userName", IsRequired = true, IsKey = true)]
        public string UserName
        {
            [DebuggerStepThrough]
            get { return (string)this["userName"]; }
            [DebuggerStepThrough]
            set { this["userName"] = value; }
        }

        #endregion Properties
    }
}