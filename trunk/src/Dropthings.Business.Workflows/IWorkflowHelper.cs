namespace Dropthings.Business.Workflows
{
    using System;
    using System.Workflow.Runtime;

    public interface IWorkflowHelper
    {
        #region Methods

        void ExecuteWorkflow(WorkflowRuntime runtime, Type workflowType, System.Collections.Generic.Dictionary<string, object> properties);

        TResponse ExecuteWorkflow<TWorkflow, TRequest, TResponse>(WorkflowRuntime runtime, TRequest request)
            where TResponse : new();

        #endregion Methods
    }
}