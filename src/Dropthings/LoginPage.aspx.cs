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
using Dropthings.Business.Container;
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.UserAccountWorkflow;
using Dropthings.Business.Workflows.UserAccountWorkflows;
using Dropthings.Web.Framework;
using Dropthings.Web.Util;
using Dropthings.Business.Facade;
using Dropthings.Model;
using Dropthings.Business.Facade.Context;

public partial class LoginPage : System.Web.UI.Page
{
    #region Methods

    protected void LoginButton_Click( object sender, EventArgs e)
    {
        if( Membership.ValidateUser( Email.Text, Password.Text ) )
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.OverrideCurrentPageIfTemplateUser(Email.Text);
            }
            
            FormsAuthentication.RedirectFromLoginPage( Email.Text, RememberMeCheckbox.Checked );
        }
        else
        {
            InvalidLoginLabel.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void RegisterButton_Click( object sender, EventArgs e)
    {
        try
        {
            bool activationRequired = Convert.ToBoolean(ConfigurationManager.AppSettings["ActivationRequired"]);

            RegisterUserResponse registerUserResponse = null;
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                registerUserResponse = facade.RegisterUser(Email.Text, Password.Text, Email.Text, activationRequired);
            }

            if (registerUserResponse != null)
            {
                if (activationRequired)
                {
                    MailManager.SendSignupMail(Email.Text.Trim(), Password.Text.Trim(), activationRequired, registerUserResponse.UnlockKey);
                    CookieHelper.Clear();
                    Response.Redirect("~/Confirm.aspx");
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(Email.Text, RememberMeCheckbox.Checked);
                    MailManager.SendSignupMail(Email.Text.Trim(), Password.Text.Trim(), activationRequired, string.Empty);
                    Response.Redirect("~/Default.aspx");
                }
            }
            else
            {
                InvalidLoginLabel.Visible = true;
            }

            // -- Workflow way. Obselete.
            //var response = WorkflowHelper.Run<UserRegistrationWorkflow,UserRegistrationWorkflowRequest,UserRegistrationWorkflowResponse>(
            //                new UserRegistrationWorkflowRequest { Email = Email.Text, RequestedUsername = Email.Text, Password = Password.Text, IsActivationRequired = activationRequired, UserName = Profile.UserName }
            //            );

            //if (response != null)
            //{
            //    if (activationRequired)
            //    {
            //        MailManager.SendSignupMail(Email.Text.Trim(), Password.Text.Trim(), activationRequired, response.UnlockKey);
            //        CookieHelper.Clear();
            //        Response.Redirect("~/Confirm.aspx");
            //    }
            //    else
            //    {
            //        FormsAuthentication.RedirectFromLoginPage( Email.Text, RememberMeCheckbox.Checked );
            //        MailManager.SendSignupMail(Email.Text.Trim(), Password.Text.Trim(), activationRequired, string.Empty);
            //        Response.Redirect( "~/Default.aspx" );
            //    }
            //}
            //else
            //{
            //    InvalidLoginLabel.Visible = true;
            //}
        }
        catch(Exception x )
        {
            Debug.WriteLine(x);
            InvalidLoginLabel.Visible = true;
            InvalidLoginLabel.Text = x.Message;
        }
    }

    protected void SendPasswordButton_Click(object sender, EventArgs e)
    {
        try
        {
            string newPassword = string.Empty;

            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                newPassword = facade.ResetPassword(ForgotEmail.Text.Trim());
            }

            MailManager.SendPasswordMail(ForgotEmail.Text.Trim(), newPassword);
            InvalidLoginLabel.Text = "Password has been sent to your provided email.";

            // -- Workflow way. Obselete.
            //var response = ServiceLocator.Resolve<IWorkflowHelper>()
            //        .ExecuteWorkflow<
            //            ResetPasswordWorkflow,
            //            ResetPasswordWorkflowRequest,
            //            ResetPasswordWorkflowResponse
            //            >(
            //                ServiceLocator.Resolve<WorkflowRuntime>(),
            //                new ResetPasswordWorkflowRequest { Email = ForgotEmail.Text.Trim(), UserName = Profile.UserName }
            //            );
            ////var password = new DashboardFacade(Profile.UserName).ResetPassword(ForgotEmail.Text);
            //MailManager.SendPasswordMail(ForgotEmail.Text.Trim(), response.NewPassword);
            //InvalidLoginLabel.Text = "Password has been sent to your provided email.";
        }
        catch (Exception x)
        {
            Debug.WriteLine(x);
            InvalidLoginLabel.Visible = true;
            InvalidLoginLabel.Text = x.Message;
        }
    }

    #endregion Methods
}