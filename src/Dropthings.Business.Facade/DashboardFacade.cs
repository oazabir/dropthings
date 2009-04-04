#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Linq;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Transactions;
    using System.Web.Security;
    using System.Workflow.ComponentModel;

    using Dropthings.Business.Activities;
    using Dropthings.Business.Activities.PageActivities;
    using Dropthings.Business.Activities.UserAccountActivities;
    using Dropthings.Business.Activities.WidgetActivities;
    using Dropthings.Business.Container;
    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.DataAccess;
    using Dropthings.Business.Workflows;
    using Dropthings.Business.Workflows.SystemWorkflows;
    using Dropthings.Business.Workflows.UserAccountWorkflow;

    public class DashboardFacade
    {
        #region Fields

        private static readonly Regex EmailExpression = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled | RegexOptions.Singleline);

        private string _UserName;

        #endregion Fields

        #region Constructors

        public DashboardFacade(string userName)
        {
            this._UserName = userName;
        }

        #endregion Constructors

        #region Methods

        public List<Widget> GetWidgetList(Dropthings.DataAccess.Enumerations.WidgetTypeEnum widgetType)
        {
            return DatabaseHelper.GetList<Widget, Dropthings.DataAccess.Enumerations.WidgetTypeEnum>(DatabaseHelper.SubsystemEnum.Widget, widgetType,
                LinqQueries.CompiledQuery_GetAllWidgets);
        }

        public List<Widget> GetWidgetList(string username, Dropthings.DataAccess.Enumerations.WidgetTypeEnum widgetType)
        {
            return DatabaseHelper.GetList<Widget, string, Dropthings.DataAccess.Enumerations.WidgetTypeEnum>(DatabaseHelper.SubsystemEnum.Widget, username, widgetType,
                LinqQueries.CompiledQuery_GetWidgetsByRole).Distinct().ToList();
        }

        public bool IsWidgetInRole(int widgetId, string roleName)
        {
            WidgetsInRole widgetsInRole = DatabaseHelper.GetSingle<WidgetsInRole, int, string>(DatabaseHelper.SubsystemEnum.Widget,
                widgetId, roleName, LinqQueries.CompiledQuery_GetWidgetsInRolesByRoleName);

            return widgetsInRole != null;
        }

        /// <summary>
        /// Creates user setting and page setup for a brand new user. If there's a user template, it is cloned for the new user.
        /// If there's no user template, then some default tabs and widgets are created.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Response object holding user setting and created pages</returns>
        public UserVisitWorkflowResponse SetupNewUser(string userName)
        {
            var response = new UserVisitWorkflowResponse();

            // Get template setting that so that we can create pages from templates
            var getUserActivity = RunActivity<GetUserGuidActivity>((activity) => activity.UserName = userName);
            var getUserSettingTemplateActivity = RunActivity<GetUserSettingTemplatesActivity>((activity) => { });
            RunActivity<SetUserRolesActivity>((activity) =>
            {
                activity.RoleName = getUserSettingTemplateActivity.AnonUserSettingTemplate.RoleNames;
                activity.UserName = userName;
            });

            if (getUserSettingTemplateActivity.CloneAnonProfileEnabled)
            {
                // Get the template user so that its page setup can be cloned for new user
                var getRoleTemplateActivity = RunActivity<GetRoleTemplateActivity>((activity) => activity.UserGuid = getUserActivity.UserGuid);
                if (getRoleTemplateActivity.RoleTemplate.TemplateUserId != Guid.Empty)
                {
                    // Get template user pages so that it can be cloned for new user
                    var getTemplateUserPages = RunActivity<GetUserPagesActivity>((activity) => activity.UserGuid = getRoleTemplateActivity.RoleTemplate.TemplateUserId);
                    foreach (Page page in getTemplateUserPages.Pages)
                    {
                        var clonePageActivity = RunActivity<ClonePageActivity>((activity) =>
                            {
                                activity.PageToClone = page;
                                activity.UserId = getUserActivity.UserGuid;
                            });

                        var getColumnsOfPageActivity = RunActivity<GetColumnsOfPageActivity>((activity) => activity.PageId = page.ID);
                        foreach (Column column in getColumnsOfPageActivity.Columns)
                        {
                            var getWidgetZoneActivity = RunActivity<GetWidgetZoneActivity>((activity) => activity.ZoneId = column.WidgetZoneId);
                            var cloneWidgetZoneActivity = RunActivity<AddWidgetZoneActivity>((activity) => activity.WidgetZoneTitle = getWidgetZoneActivity.WidgetZone.Title);
                            RunActivity<CloneColumnActivity>((activity) =>
                                {
                                    activity.ColumnToClone = column;
                                    activity.PageId = clonePageActivity.NewPage.ID;
                                    activity.WidgetZoneId = cloneWidgetZoneActivity.NewWidgetZone.ID;
                                });

                            var getWidgetInstancesActivity = RunActivity<GetWidgetInstancesInZoneActivity>((activity) => activity.WidgetZoneId = column.WidgetZoneId);
                            foreach (WidgetInstance widgetInstance in getWidgetInstancesActivity.WidgetInstances)
                            {
                                RunActivity<CloneWidgetInstanceActivity>((activity) =>
                                    {
                                        activity.WidgetInstance = widgetInstance;
                                        activity.WidgetZoneId = cloneWidgetZoneActivity.NewWidgetZone.ID;
                                    });
                            }
                        }
                    }
                }
            }
            else
            {
                // Setup some default pages
            }

            var getUserSettingActivity = RunActivity<GetUserSettingActivity>((activity) => activity.UserGuid = getUserActivity.UserGuid);
            response.UserSetting = getUserSettingActivity.UserSetting;
            response.CurrentPage = getUserSettingActivity.CurrentPage;

            var getUserPagesActivity = RunActivity<GetUserPagesActivity>((activity) => activity.UserGuid = getUserActivity.UserGuid);
            response.UserPages = getUserPagesActivity.Pages;

            return response;
        }

        public void SetupDefaultSetting()
        {
            //setup default roles, template user and role template
            WorkflowHelper.Run<SetupDefaultRolesWorkflow, SetupDefaultRolesWorkflowRequest, SetupDefaultRolesWorkflowResponse>(
                new SetupDefaultRolesWorkflowRequest { }
            );

            Dropthings.Business.UserSettingTemplateSettingsSection settings = (UserSettingTemplateSettingsSection)ConfigurationManager.GetSection(UserSettingTemplateSettingsSection.SectionName);

            foreach (UserSettingTemplateElement setting in settings.UserSettingTemplates)
            {
                WorkflowHelper.Run<CreateTemplateUserWorkflow, CreateTemplateUserWorkflowRequest, CreateTemplateUserWorkflowResponse>(
                    new CreateTemplateUserWorkflowRequest { Email = setting.UserName, IsActivationRequired = false, Password = setting.Password, RequestedUsername = setting.UserName, RoleName = setting.RoleNames, TemplateRoleName = setting.TemplateRoleName }
                );
            }
        }

        private static bool IsValidEmail(string email)
        {
            return EmailExpression.IsMatch(email);
        }

        [DebuggerStepThrough]
        private TActivity RunActivity<TActivity>(
            Action<TActivity> bind)
            where TActivity : Activity, new()
        {
            var activityType = typeof(TActivity);
            var activity = new TActivity();
            bind(activity);

            MethodInfo executeMethod = activityType.GetMethod("Execute",
                        BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance);
            // execute the Execute method but do not send any argument.
            executeMethod.Invoke(activity, new object[] { null });

            return activity;
        }

        #endregion Methods
    }
}