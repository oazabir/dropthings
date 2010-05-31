namespace Dropthings.Business.Facade.Test.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using System.Diagnostics;
    using Dropthings.Model;
    using Dropthings.Business.Facade;
    using Dropthings.Business.Facade.Context;
    using Xunit;

    internal class SetupHelper
    {
        #region Methods

        [DebuggerStepThrough]
        public static void UsingNewAnonSetup(string userName, Action<UserSetup> callback)
        {
            // Create default page setup which is expected to populate all three columns with widgets
            //var response = WorkflowTest.Run<FirstVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
            //    new UserVisitWorkflowRequest { IsAnonymous = true, TabName = "", UserName = userName }
            //    );
            var response = new Facade(new AppContext(Guid.NewGuid().ToString(), userName)).FirstVisitHomeTab(userName, string.Empty, true, false);

            Assert.NotNull(response.CurrentTab);
            Assert.NotEqual(0, response.UserTabs.Count());
            Assert.NotNull(response.UserSetting);

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
                var response = facade.FirstVisitHomeTab(userName, string.Empty, true, false);
                Assert.NotNull(response.CurrentTab);
                Assert.NotEqual(0, response.UserTabs.Count());
                Assert.NotNull(response.UserSetting);

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