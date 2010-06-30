namespace Dropthings.Configuration
{
    using System.Configuration;
    using System.Diagnostics;

    public class UserSettingTemplateSettingsSection : ConfigurationSection
    {
        #region Fields

        public const string AnonTemplateKey = "anon_template";
        public const string RegTemplateKey = "registered_template";
        public const string SectionName = "userSettingTemplates";

        internal const bool DefaultAnonEnabled = true;
        internal const bool DefaultRegisteredEnabled = true;

        #endregion Fields

        #region Properties

        [ConfigurationProperty("cloneAnonProfileEnabled", DefaultValue = DefaultAnonEnabled)]
        public bool CloneAnonProfileEnabled
        {
            [DebuggerStepThrough]
            get { return (bool)this["cloneAnonProfileEnabled"]; }
            [DebuggerStepThrough]
            set { this["cloneAnonProfileEnabled"] = value; }
        }

        [ConfigurationProperty("cloneRegisteredProfileEnabled", DefaultValue = DefaultRegisteredEnabled)]
        public bool CloneRegisteredProfileEnabled
        {
            [DebuggerStepThrough]
            get { return (bool)this["cloneRegisteredProfileEnabled"]; }
            [DebuggerStepThrough]
            set { this["cloneRegisteredProfileEnabled"] = value; }
        }

        [ConfigurationProperty("templates", IsDefaultCollection = true, IsRequired = true)]
        public UserSettingTemplateElementCollection UserSettingTemplates
        {
            [DebuggerStepThrough]
            get
            {
                return (UserSettingTemplateElementCollection)base["templates"] ?? new UserSettingTemplateElementCollection();
            }
        }

        #endregion Properties
    }
}