namespace Dropthings.Test.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Workflow.Runtime;

    using Dropthings.Business.Container;
    using Dropthings.Business.Workflows;

    internal class WorkflowTest
    {
        #region Fields

        private static WorkflowRuntime _Runtime;

        #endregion Fields

        #region Methods

        public static void Dispose()
        {
            if (null != _Runtime)
            {
                _Runtime.StopRuntime();
                _Runtime.Dispose();
                _Runtime = null;
            }
        }

        public static void Init()
        {
            if (null == _Runtime)
            {
                _Runtime = WorkflowHelper.CreateDefaultRuntime();

                ServiceLocator.RegisterInstanceExternalLifetime<WorkflowRuntime>(_Runtime);
                ServiceLocator.RegisterTypePerThread<IWorkflowHelper, WorkflowHelper>();
            }
        }

        public static TResponse Run<TWorkflow, TRequest, TResponse>(TRequest request)
            where TResponse : new()
        {
            Init();
            return ServiceLocator.Resolve<IWorkflowHelper>().ExecuteWorkflow<TWorkflow, TRequest, TResponse>(
                ServiceLocator.Resolve<WorkflowRuntime>(),
                request);
        }

        [DebuggerStepThrough]
        public static void UsingWorkflowRuntime(Action callback)
        {
            Init();
            try
            {
                callback();
            }
            finally
            {
                Dispose();
            }
        }

        #endregion Methods
    }
}