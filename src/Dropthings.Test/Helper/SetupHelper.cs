namespace Dropthings.Test.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Diagnostics;
    using Dropthings.Model;
    using Dropthings.Business.Facade;
    using Dropthings.Business.Facade.Context;

    internal class SetupHelper
    {
        #region Methods

        [DebuggerStepThrough]
        public static void UsingNewAnonSetup(string userName, Action<UserSetup> callback)
        {
            // Create default page setup which is expected to populate all three columns with widgets
            //var response = WorkflowTest.Run<FirstVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
            //    new UserVisitWorkflowRequest { IsAnonymous = true, PageName = "", UserName = userName }
            //    );
            var response = new Facade(new AppContext(Guid.NewGuid().ToString(), userName)).FirstVisitHomePage(userName, string.Empty, true, false);

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

        [DebuggerStepThrough]
        public static void UsingNewAnonSetup_ByFacade(string userName, Action<UserSetup> callback)
        {
            // Create default page setup which is expected to populate all three columns with widgets
            using(var facade = new Facade(new AppContext(string.Empty, userName)))
            {
                var response = facade.FirstVisitHomePage(userName, string.Empty, true, false);
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
        }

        #endregion Methods
    }
}