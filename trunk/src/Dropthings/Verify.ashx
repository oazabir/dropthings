<%@ WebHandler Language="C#" Class="Logout" %>
// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar


using System;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using Dropthings.Business;
using Dropthings.Data;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;
using Dropthings.Web.Framework;

public class Logout : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        string activationKey = context.Request.QueryString["key"];

        Token token = null;
        
        // -- Workflow way. Obselete.
        
        //var response = Dropthings.Business.Container.ServiceLocator.Resolve<Dropthings.Business.Workflows.IWorkflowHelper>()
        //            .ExecuteWorkflow<
        //                Dropthings.Business.Workflows.UserAccountWorkflow.ActivateAccountWorkflow,
        //                Dropthings.Business.Workflows.UserAccountWorkflows.ActivateAccountWorkflowRequest,
        //                Dropthings.Business.Workflows.UserAccountWorkflows.ActivateAccountWorkflowResponse
        //                >(
        //                    Dropthings.Business.Container.ServiceLocator.Resolve<System.Workflow.Runtime.WorkflowRuntime>(),
        //                    new Dropthings.Business.Workflows.UserAccountWorkflows.ActivateAccountWorkflowRequest { ActivationKey = activationKey, UserName = string.Empty }
        //                );

        using (var facade = new Facade(new AppContext(string.Empty, context.Profile.UserName)))
        {
            token = facade.ActivateUser(activationKey);
        }
        
        if (token != null)
        {
            FormsAuthentication.RedirectFromLoginPage(token.UserName, true);
        }
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}