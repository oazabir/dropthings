<%@ WebHandler Language="C#" Class="Logout" %>
// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar


using System;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using Dropthings.Business;
using Dropthings.DataAccess;
public class Logout : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        string activationKey = context.Request.QueryString["key"];

        var response = Dropthings.Business.Container.ObjectContainer.Resolve<Dropthings.Business.Workflows.IWorkflowHelper>()
                    .ExecuteWorkflow<
                        Dropthings.Business.Workflows.UserAccountWorkflow.ActivateAccountWorkflow,
                        Dropthings.Business.Workflows.UserAccountWorkflows.ActivateAccountWorkflowRequest,
                        Dropthings.Business.Workflows.UserAccountWorkflows.ActivateAccountWorkflowResponse
                        >(
                            Dropthings.Business.Container.ObjectContainer.Resolve<System.Workflow.Runtime.WorkflowRuntime>(),
                            new Dropthings.Business.Workflows.UserAccountWorkflows.ActivateAccountWorkflowRequest { ActivationKey = activationKey, UserName = string.Empty }
                        );

        if (response.Token != null)
        {
            FormsAuthentication.RedirectFromLoginPage(response.Token.UserName, true);
        }
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}