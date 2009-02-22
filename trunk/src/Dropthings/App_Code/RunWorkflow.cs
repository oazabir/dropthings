using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dropthings.Business.Container;
using System.Workflow.Runtime;
using Dropthings.Business.Workflows;
using Dropthings.DataAccess;

/// <summary>
/// Summary description for RunWorkflow
/// </summary>
public class RunWorkflow
{
    public static TResponse Run<TWorkflow, TRequest, TResponse>(TRequest request) where TResponse : new()
    {
        using (new TimedLog(HttpContext.Current.User.Identity.Name, "Workflow: " + typeof(TWorkflow).Name))
        {
            return ObjectContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow<TWorkflow, TRequest, TResponse>(
                ObjectContainer.Resolve<WorkflowRuntime>(),
                request);
        }
    }
}
