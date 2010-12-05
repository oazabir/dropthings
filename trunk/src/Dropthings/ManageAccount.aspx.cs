#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Workflow.Runtime;

using Dropthings.Business;
using Dropthings.Web.Framework;
using Dropthings.Web.Util;
using Resources;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;
using Dropthings.Util;

public partial class ManageAccountPage : BasePage
{
    #region Methods

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

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void ChangePasswordButton_Click(object sender, EventArgs e)
    {
        try
        {
            if(IsValid)
            {
                MembershipUser user = Membership.GetUser(Profile.UserName);

                if (string.IsNullOrEmpty(OldPasswordTextbox.Text.Trim()))
                {
                    ShowMessage(Message, SharedResources.OldPasswordBlankMessage, true);
                    return;
                }

                if (string.IsNullOrEmpty(NewPasswordTextbox.Text.Trim()))
                {
                    ShowMessage(Message, SharedResources.PasswordBlankMessage, true);
                    return;
                }

                if(!string.Equals(user.GetPassword(), OldPasswordTextbox.Text.Trim()))
                {
                    ShowMessage(Message, SharedResources.ProvideOldPasswordMessage, true);
                    return;
                }

                if (NewPasswordTextbox.Text.Trim() != ConfirmPasswordTextBox.Text.Trim())
                {
                    ShowMessage(Message, SharedResources.PasswordDoesNotMatch, true);
                    return;
                }

                user.ChangePassword(OldPasswordTextbox.Text.Trim(), NewPasswordTextbox.Text.Trim());
                ShowMessage(Message, SharedResources.PasswordChangedMessage, false);
            }

        }
        catch (Exception x)
        {
            Debug.WriteLine(x);
            ShowMessage(Message, x.Message, true);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PrepareView();
        }
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            //new DashboardFacade(Profile.UserName).UpdateAccount(EmailTextbox.Text.Trim());

            //WorkflowHelper.Run<UpdateAccountWorkflow,UpdateAccountWorkflowRequest,UpdateAccountWorkflowResponse>(
            //                new UpdateAccountWorkflowRequest { Email = EmailTextbox.Text.Trim(), UserName = Profile.UserName }
            //            );
            Facade facade = Services.Get<Facade>();
            {
                facade.UpdateAccount(EmailTextbox.Text.Trim(), Profile.UserName);
            }

            FormsAuthentication.SignOut();
            FormsAuthentication.SetAuthCookie(EmailTextbox.Text.Trim(), true);
            ShowMessage(Message, SharedResources.AccountUpdatedMessage, false);
        }
        catch(Exception x )
        {
            Debug.WriteLine(x);
            ShowMessage(Message, x.Message, true);
        }
    }

    private void PrepareView()
    {
        MembershipUser user = Membership.GetUser(Profile.UserName);

        if (user != null)
        {
            EmailTextbox.Text = user.UserName;
        }
    }

    #endregion Methods
}