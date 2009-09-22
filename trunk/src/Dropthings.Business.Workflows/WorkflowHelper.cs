#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Workflows
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Web;
    using System.Workflow.Activities;
    using System.Workflow.ComponentModel;
    using System.Workflow.Runtime;
    using System.Workflow.Runtime.Hosting;

    using Exceptions;
    using Dropthings.Util;

    public class WorkflowHelper : Dropthings.Business.Workflows.IWorkflowHelper
    {
        #region Methods

        public static WorkflowRuntime CreateDefaultRuntime()
        {
            WorkflowRuntime workflowRuntime = new WorkflowRuntime();

            var manualService = new ManualWorkflowSchedulerService();
            workflowRuntime.AddService(manualService);

            var syncCallService = new Activities.CallWorkflowService();
            workflowRuntime.AddService(syncCallService);

            workflowRuntime.StartRuntime();

            return workflowRuntime;
        }

        public static TResponse Run<TWorkflow, TRequest, TResponse>(TRequest request)
            where TResponse : new()
        {
            return ServiceLocator.Resolve<IWorkflowHelper>().ExecuteWorkflow<TWorkflow, TRequest, TResponse>(
                ServiceLocator.Resolve<WorkflowRuntime>(),
                request);
        }

        public static void TerminateDefaultRuntime(WorkflowRuntime workflowRuntime)
        {
            workflowRuntime.StopRuntime();
            workflowRuntime.Dispose();
        }

        public TResponse ExecuteWorkflow<TWorkflow, TRequest, TResponse>(WorkflowRuntime workflowRuntime, TRequest request)
            where TResponse : new()
        {
            var properties = new Dictionary<string, object>();
            properties.Add("Request", request);
            var response = new TResponse();
            properties.Add("Response", response);
            ExecuteWorkflow(workflowRuntime, typeof(TWorkflow), properties);
            return response;
        }

        public void ExecuteWorkflow(WorkflowRuntime workflowRuntime, Type workflowType, Dictionary<string,object> properties)
        {
            ManualWorkflowSchedulerService manualScheduler = workflowRuntime.GetService<ManualWorkflowSchedulerService>();

            WorkflowInstance instance = workflowRuntime.CreateWorkflow(workflowType, properties);
            instance.Start();
            var instanceId = instance.InstanceId;

            EventHandler<WorkflowCompletedEventArgs> completedHandler = null;
            completedHandler = delegate(object o, WorkflowCompletedEventArgs e)
            {
                if (e.WorkflowInstance.InstanceId == instanceId)
                {
                    // copy the output parameters in the specified properties dictionary
                    Dictionary<string,object>.Enumerator enumerator = e.OutputParameters.GetEnumerator();
                    while( enumerator.MoveNext() )
                    {
                        KeyValuePair<string,object> pair = enumerator.Current;
                        if( properties.ContainsKey(pair.Key) )
                        {
                            properties[pair.Key] = pair.Value;
                        }
                    }
                }
            };

            Exception x  = null;
            EventHandler<WorkflowTerminatedEventArgs> terminatedHandler = null;
            terminatedHandler = delegate(object o, WorkflowTerminatedEventArgs e)
            {
                if (e.WorkflowInstance.InstanceId == instanceId)
                {
                    x = e.Exception;
                    Debug.WriteLine(e.Exception);
                }
            };

            workflowRuntime.WorkflowCompleted += completedHandler;
            workflowRuntime.WorkflowTerminated += terminatedHandler;

            manualScheduler.RunWorkflow(instance.InstanceId);

            workflowRuntime.WorkflowTerminated -= terminatedHandler;
            workflowRuntime.WorkflowCompleted -= completedHandler;

            if (null != x)
                throw new WorkflowException(x);
        }

        #endregion Methods
    }
}