#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Header : System.Web.UI.UserControl
{
    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        if( Context.Profile.IsAnonymous )
        {
            LoginLinkButton.Visible = true;
            LogoutLinkButton.Visible = false;
            AccountLinkButton.Visible = false;
            StartOverButton.Visible = true;
        }
        else
        {
            LoginLinkButton.Visible = false;
            LogoutLinkButton.Visible = true;
            StartOverButton.Visible = false;
            AccountLinkButton.Visible = true;
            UserNameLabel.Text = Profile.UserName + " | ";
            UserNameLabel.Visible = true;

            string[] roles = Roles.GetRolesForUser(Profile.UserName);
            AdminLink.Visible = Array.Exists(roles, r => r == ConfigurationManager.AppSettings["AdministratorRoleName"]);
        }
    }

    #endregion Methods
}