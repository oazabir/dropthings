namespace Dropthings.Test.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Business.Workflows.EntryPointWorkflows;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal class SetupHelper
    {
        #region Methods

        public static void UsingNewAnonSetup(string userName, Action<UserVisitWorkflowResponse> callback)
        {
            // Create default page setup which is expected to populate all three columns with widgets
            var response = WorkflowTest.Run<FirstVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
                new UserVisitWorkflowRequest { IsAnonymous = true, PageName = "", UserName = userName }
                );
            Assert.IsNotNull(response.CurrentPage, "First time visit did not create pages");

            try
            {
                callback(response);
            }
            finally
            {
                // TODO: Clear the setup
            }
        }

        #endregion Methods
    }
}