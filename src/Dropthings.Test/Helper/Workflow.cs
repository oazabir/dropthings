namespace Dropthings.Test.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Workflow.Runtime;

    using Dropthings.Business.Container;
    using Dropthings.Business.Workflows;

    class Workflow
    {
        #region Fields

        private static WorkflowRuntime _Runtime;

        #endregion Fields

        #region Methods

        public static void Init()
        {
            if (null == _Runtime)
            {
                _Runtime = WorkflowHelper.CreateDefaultRuntime();

                ObjectContainer.RegisterInstanceExternalLifetime<WorkflowRuntime>(_Runtime);
                ObjectContainer.RegisterTypePerThread<IWorkflowHelper, WorkflowHelper>();
            }
        }

        public static TResponse Run<TWorkflow, TRequest, TResponse>(TRequest request)
            where TResponse : new()
        {
            Init();
            return ObjectContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow<TWorkflow, TRequest, TResponse>(
                ObjectContainer.Resolve<WorkflowRuntime>(),
                request);
        }

        #endregion Methods
    }
}