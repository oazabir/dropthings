using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dropthings.Business.Container;
using System.Workflow.Runtime;
using Dropthings.Business.Workflows;

/// <summary>
/// Summary description for RunWorkflow
/// </summary>
public class RunWorkflow
{
    public static TResponse Run<TWorkflow, TRequest, TResponse>(TRequest request) where TResponse : new()
    {
        return ObjectContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow<TWorkflow, TRequest, TResponse>(
            ObjectContainer.Resolve<WorkflowRuntime>(),
            request);
    }
}
