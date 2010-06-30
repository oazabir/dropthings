namespace Dropthings.Business.Activities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Configuration;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.DataAccess;

    public partial class GetUserSettingTemplatesActivity : Activity
    {
        #region Fields

        public static readonly DependencyProperty CloneAnonProfileEnabledProperty = 
            DependencyProperty.Register("CloneAnonProfileEnabled", typeof(bool), typeof(GetUserSettingTemplatesActivity));
        public static readonly DependencyProperty CloneRegisteredProfileEnabledProperty = 
            DependencyProperty.Register("CloneRegisteredProfileEnabled", typeof(bool), typeof(GetUserSettingTemplatesActivity));

        private static DependencyProperty AllUserSettingTemplateProperty = 
            DependencyProperty.Register("AllUserSettingTemplate", typeof(List<UserSettingTemplateElement>), typeof(GetUserSettingTemplatesActivity));
        private static DependencyProperty AnonUserSettingTemplateProperty = 
            DependencyProperty.Register("AnonUserSettingTemplate", typeof(UserSettingTemplateElement), typeof(GetUserSettingTemplatesActivity));
        private static DependencyProperty RegisteredUserSettingTemplateProperty = 
            DependencyProperty.Register("RegisteredUserSettingTemplate", typeof(UserSettingTemplateElement), typeof(GetUserSettingTemplatesActivity));

        #endregion Fields

        #region Constructors

        public GetUserSettingTemplatesActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public List<UserSettingTemplateElement> AllUserSettingTemplate
        {
            get { return (List<UserSettingTemplateElement>)base.GetValue(AllUserSettingTemplateProperty); }
            set { base.SetValue(AllUserSettingTemplateProperty, value); }
        }

        public UserSettingTemplateElement AnonUserSettingTemplate
        {
            get { return (UserSettingTemplateElement)base.GetValue(AnonUserSettingTemplateProperty); }
            set { base.SetValue(AnonUserSettingTemplateProperty, value); }
        }

        public bool CloneAnonProfileEnabled
        {
            get { return (bool)GetValue(CloneAnonProfileEnabledProperty); }
            set { SetValue(CloneAnonProfileEnabledProperty, value); }
        }

        public bool CloneRegisteredProfileEnabled
        {
            get { return (bool)GetValue(CloneRegisteredProfileEnabledProperty); }
            set { SetValue(CloneRegisteredProfileEnabledProperty, value); }
        }

        public UserSettingTemplateElement RegisteredUserSettingTemplate
        {
            get { return (UserSettingTemplateElement)base.GetValue(RegisteredUserSettingTemplateProperty); }
            set { base.SetValue(RegisteredUserSettingTemplateProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            UserSettingTemplateSettingsSection settings = (UserSettingTemplateSettingsSection)ConfigurationManager.GetSection(UserSettingTemplateSettingsSection.SectionName);

            this.CloneAnonProfileEnabled = settings.CloneAnonProfileEnabled;
            this.CloneRegisteredProfileEnabled = settings.CloneRegisteredProfileEnabled;

            this.AnonUserSettingTemplate = settings.UserSettingTemplates[UserSettingTemplateSettingsSection.AnonTemplateKey];
            this.RegisteredUserSettingTemplate = settings.UserSettingTemplates[UserSettingTemplateSettingsSection.RegTemplateKey];

            this.AllUserSettingTemplate = new List<UserSettingTemplateElement>();

            foreach (UserSettingTemplateElement setting in settings.UserSettingTemplates)
            {
                this.AllUserSettingTemplate.Add(setting);
            }

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}