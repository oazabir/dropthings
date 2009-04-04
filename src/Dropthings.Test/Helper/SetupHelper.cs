namespace Dropthings.Test.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Business;
    using Dropthings.Business.Workflows.EntryPointWorkflows;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Diagnostics;

    internal class SetupHelper
    {
        #region Methods

        [DebuggerStepThrough]
        public static void UsingNewAnonSetup(string userName, Action<UserVisitWorkflowResponse> callback)
        {
            // Create default page setup which is expected to populate all three columns with widgets
            //var response = WorkflowTest.Run<FirstVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
            //    new UserVisitWorkflowRequest { IsAnonymous = true, PageName = "", UserName = userName }
            //    );
            var response = new DashboardFacade(userName).SetupNewUser(userName);
            Assert.IsNotNull(response.CurrentPage, "First time visit did not create pages");
            Assert.AreNotEqual(0, response.UserPages.Count, "No page returned");
            Assert.IsNotNull(response.UserSetting, "User setting not returned");

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