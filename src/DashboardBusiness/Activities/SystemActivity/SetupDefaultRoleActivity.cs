#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Activities
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Configuration;
    using System.Data.Linq;
    using System.Drawing;
    using System.Linq;
    using System.Web.Security;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.DataAccess;

    public partial class SetupDefaultRoleActivity : System.Workflow.ComponentModel.Activity
    {
        #region Constructors

        public SetupDefaultRoleActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            UserSettingTemplateSettingsSection settings = (UserSettingTemplateSettingsSection)ConfigurationManager.GetSection(UserSettingTemplateSettingsSection.SectionName);

            foreach (UserSettingTemplateElement setting in settings.UserSettingTemplates)
            {
                SetupRoles(setting.RoleNames);
            }

            return ActivityExecutionStatus.Closed;
        }

        private void SetupRoles(string roleNames)
        {
            if (!string.IsNullOrEmpty(roleNames))
            {
                string[] roles = roleNames.Split(new char[] { ',', ':' });

                for (int i = 0; i < roles.Length; i++)
                {
                    if (!System.Web.Security.Roles.RoleExists(roles[i]))
                    {
                        if (!Roles.RoleExists(roles[i]))
                        {
                            Roles.CreateRole(roles[i]);

                            var aspnet_Role = DatabaseHelper.GetSingle<aspnet_Role, string>(DatabaseHelper.SubsystemEnum.User, roles[i], LinqQueries.CompiledQuery_GetRoleByRoleName);
                            var defaultWidgets = DatabaseHelper.GetList<Widget, bool>(DatabaseHelper.SubsystemEnum.Widget,
                                true, LinqQueries.CompiledQuery_GetWidgetByIsDefault);

                            if (defaultWidgets != null && defaultWidgets.Count > 0)
                            {
                                foreach (var defaultWidget in defaultWidgets)
                                {
                                    DatabaseHelper.Insert<WidgetsInRole>(DatabaseHelper.SubsystemEnum.Widget,
                                       (newWidgetsInRole) =>
                                       {
                                           newWidgetsInRole.WidgetId = defaultWidget.ID;
                                           newWidgetsInRole.RoleId = aspnet_Role.RoleId;
                                       });
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}