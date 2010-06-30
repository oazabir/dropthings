namespace Dropthings.Configuration
{
    using System.Configuration;
    using System.Diagnostics;

    public class UserSettingTemplateElementCollection : ConfigurationElementCollection
    {
        #region Indexers

        public new UserSettingTemplateElement this[string name]
        {
            [DebuggerStepThrough]
            get
            {
                return BaseGet(name) as UserSettingTemplateElement;
            }
        }

        #endregion Indexers

        #region Methods

        public void Add(UserSettingTemplateElement element)
        {
            BaseAdd(element);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new UserSettingTemplateElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UserSettingTemplateElement)element).Key;
        }

        #endregion Methods
    }
}