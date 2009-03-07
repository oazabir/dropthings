namespace Dropthings.Test.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Profile;
    using System.Web.Security;

    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.Test.Helper;
    using Dropthings.Web.Framework;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for TestPageColumnChange
    /// </summary>
    [TestClass]
    public class TestPageColumnChange
    {
        #region Fields

        private TestContext testContextInstance;

        #endregion Fields

        #region Constructors

        public TestPageColumnChange()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion Properties

        #region Methods

        [TestMethod]
        public void TestColumn_Change_Should_Move_Widgets_To_Last_Available_Column()
        {
            UserProfile profile = DefaultProfile.Create(Guid.NewGuid().ToString(), false) as UserProfile;
            profile.IsFirstVisit = false;
            profile.Save();

            var setup = Workflow.Run<FirstVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
                new UserVisitWorkflowRequest { IsAnonymous = true, PageName = "", UserName = profile.UserName }
                );

            Assert.IsNotNull(setup.CurrentPage, "First time visit did not create pages");
        }

        #endregion Methods

        #region Other

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion Other
    }
}