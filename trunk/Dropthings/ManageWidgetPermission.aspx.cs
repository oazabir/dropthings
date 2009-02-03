// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Diagnostics;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using Dropthings.Business;
using Dropthings.Web.Util;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.UserAccountWorkflow;
using Dropthings.Business.Workflows.UserAccountWorkflows;
using System.Workflow.Runtime;
using System.Collections.Generic;
using Dropthings.DataAccess;

public partial class ManageWidgetPermissionPage : System.Web.UI.Page
{
    public static void ShowMessage(Label label, string message, bool isError)
    {
        label.ForeColor = System.Drawing.Color.DodgerBlue;
        label.Text = message;
        
        if (isError)
        {
            label.ForeColor = System.Drawing.Color.Red;
            label.Font.Bold = true;
        }
    }

    protected List<Widget> Widgets
    {
        get;
        set;
    }

    protected string[] Roles
    {
        get
        {
            return System.Web.Security.Roles.GetAllRoles();
        }
    }

    protected bool IsWidgetInRole(int widgetId, string roleName)
    {
        return new DashboardFacade(Profile.UserName).IsWidgetInRole(widgetId, roleName);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Widgets = new DashboardFacade(Profile.UserName).GetWidgetList(Enumerations.WidgetTypeEnum.PersonalPage);
        }
    }
}
