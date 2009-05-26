using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Dropthings.Business;
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.SystemWorkflows;
using Dropthings.Business.Workflows.UserAccountWorkflow;
using Dropthings.Web.Framework;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;

public partial class Setup : System.Web.UI.Page
{
    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        SetupDefaultSetting();
    }

    private static void SetupDefaultSetting()
    {
        //setup default roles, template user and role template
        using (var facade = new Facade(new AppContext(string.Empty, string.Empty)))
        {
            facade.SetupDefaultRoles();

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<SetupDefaultRolesWorkflow, SetupDefaultRolesWorkflowRequest, SetupDefaultRolesWorkflowResponse>(
            //    new SetupDefaultRolesWorkflowRequest { }
            //);

            Dropthings.Business.UserSettingTemplateSettingsSection settings = (UserSettingTemplateSettingsSection)ConfigurationManager.GetSection(UserSettingTemplateSettingsSection.SectionName);

            foreach (UserSettingTemplateElement setting in settings.UserSettingTemplates)
            {
                facade.CreateTemplateUser(setting.UserName, false, setting.Password, setting.UserName, setting.RoleNames, setting.TemplateRoleName);
                
                // -- Workflow way. Obselete.
                //WorkflowHelper.Run<CreateTemplateUserWorkflow, CreateTemplateUserWorkflowRequest, CreateTemplateUserWorkflowResponse>(
                //    new CreateTemplateUserWorkflowRequest { Email = setting.UserName, IsActivationRequired = false, Password = setting.Password, RequestedUsername = setting.UserName, RoleName = setting.RoleNames, TemplateRoleName = setting.TemplateRoleName }
                //);

            }
        }
    }

    #endregion Methods
}