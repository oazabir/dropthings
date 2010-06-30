namespace Dropthings.Test.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Business.Workflows.TabWorkflows;
    using Dropthings.Business.Workflows.WidgetWorkflows;
    using Dropthings.Test.Helper;
    using Dropthings.Web.Framework;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.SqlClient;
    using System.Configuration;
    using Dropthings.Business;
    using Dropthings.Business.Facade;
    using Dropthings.Business.Facade.Context;

    /// <summary>
    /// Summary description for TestVisit
    /// </summary>
    [TestClass]
    public class TestVisit
    {
        #region Fields

        private TestContext testContextInstance;

        #endregion Fields

        #region Constructors

        public TestVisit()
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
        public void First_Visit_Should_Return_Pages_And_Settings_ByFacade()
        {
            Facade.BootStrap();

            MembershipHelper.UsingNewAnonUser((profile) =>
                SetupHelper.UsingNewAnonSetup_ByFacade(profile.UserName, (response) =>
                {
                    response.UserPages.Each((page) =>
                    {
                        var columns = WorkflowTest.Run<GetColumnsInPageWorkflow, GetColumnsInPageWorkflowRequest, GetColumnsInPageWorkflowResponse>(
                            new GetColumnsInPageWorkflowRequest { PageId = page.ID, UserName = profile.UserName }
                        ).Columns;

                        Assert.AreNotEqual(0, page.ColumnCount, "Page must have at least one column");
                        Assert.AreEqual(page.ColumnCount, columns.Count, "Not same columns available in the page");

                        columns.Each((column) =>
                        {
                            var widgets = WorkflowTest.Run<LoadWidgetInstancesInZoneWorkflow, LoadWidgetInstancesInZoneRequest, LoadWidgetInstancesInZoneResponse>(
                                new LoadWidgetInstancesInZoneRequest { WidgetZoneId = column.WidgetZoneId, UserName = profile.UserName }
                            ).WidgetInstances;

                            Assert.AreNotEqual(0, widgets.Count, "No widgets found on page " + page.Title + ", column " + column.ColumnNo);
                        });
                    });
                })
            );
        }

        [TestMethod]
        public void First_Visit_Should_Return_Pages_And_Settings()
        {
            WorkflowTest.UsingWorkflowRuntime(() =>
                MembershipHelper.UsingNewAnonUser((profile) =>
                    SetupHelper.UsingNewAnonSetup(profile.UserName, (response) =>
                    {
                        response.UserPages.Each((page) =>
                        {
                            var columns = WorkflowTest.Run<GetColumnsInPageWorkflow, GetColumnsInPageWorkflowRequest, GetColumnsInPageWorkflowResponse>(
                                new GetColumnsInPageWorkflowRequest { PageId = page.ID, UserName = profile.UserName }
                            ).Columns;

                            Assert.AreNotEqual(0, page.ColumnCount, "Page must have at least one column");
                            Assert.AreEqual(page.ColumnCount, columns.Count, "Not same columns available in the page");

                            columns.Each((column) =>
                            {
                                var widgets = WorkflowTest.Run<LoadWidgetInstancesInZoneWorkflow, LoadWidgetInstancesInZoneRequest, LoadWidgetInstancesInZoneResponse>(
                                    new LoadWidgetInstancesInZoneRequest { WidgetZoneId = column.WidgetZoneId, UserName = profile.UserName }
                                ).WidgetInstances;

                                Assert.AreNotEqual(0, widgets.Count, "No widgets found on page " + page.Title + ", column " + column.ColumnNo);
                            });
                        });
                    })
                )
            );
        }

        [TestMethod]
        public void Very_First_Vist_With_Empty_Database_Should_Setup_Default_Pages()
        {
            // Initialize database to empty state so that we can test the first time initalization process
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("EXEC Resurrection", con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Prepare the database for use
            WorkflowTest.UsingWorkflowRuntime(() => new DashboardFacade(string.Empty).SetupDefaultSetting());

            // Do the first visit test
            First_Visit_Should_Return_Pages_And_Settings();
        }

        [TestMethod]
        public void Very_First_Vist_With_Empty_Database_Should_Setup_Default_Pages_ByFacade()
        {
            Facade.BootStrap();

            // Initialize database to empty state so that we can test the first time initalization process
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("EXEC Resurrection", con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Prepare the database for use
            using (var facade = new Facade(new AppContext(string.Empty, string.Empty)))
            {
                facade.SetupDefaultSetting();
            }

            // Do the first visit test
            First_Visit_Should_Return_Pages_And_Settings_ByFacade();
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