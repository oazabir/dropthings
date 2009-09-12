namespace Dropthings.Test.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Profile;
    using System.Web.Security;

    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.Business.Workflows.TabWorkflows;
    using Dropthings.DataAccess;
    using Dropthings.Test.Helper;
    using Dropthings.Web.Framework;
    using Dropthings.Business.Facade;
    using Dropthings.Business.Facade.Context;

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
        public void Layout_Change_Should_Move_Widgets_To_Last_Available_Column_ByFacade()
        {
            const int ColumnsBeforeChange = 3;
            const int ColumnsAfterChange = 2;
            const int LayoutType = 2;

            Facade.BootStrap();

            MembershipHelper.UsingNewAnonUser((profile) =>
                SetupHelper.UsingNewAnonSetup_ByFacade(profile.UserName, (setup) =>
                {
                    using (var facade = new Facade(new AppContext(string.Empty, profile.UserName)))
                    {
                        int originalLayoutType = setup.CurrentPage.LayoutType;

                        // Ensure there's widgets on the last column
                        WidgetZone lastZone = DatabaseHelper.GetSingle<WidgetZone, int, int>(DatabaseHelper.SubsystemEnum.WidgetZone,
                            setup.CurrentPage.ID, ColumnsBeforeChange - 1,
                            LinqQueries.CompiledQuery_GetWidgetZoneByPageId_ColumnNo);
                        List<WidgetInstance> widgetsOnLastZone = DatabaseHelper.GetList<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                            lastZone.ID,
                            LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneId);
                        Assert.AreNotEqual(0, widgetsOnLastZone.Count, "No widget found on last column to move");

                        // Change to 25%, 75% two column layout
                        facade.ModifyPageLayout(LayoutType);

                        // Get the page setup again to ensure the number of columns are changed
                        var userSetup = facade.RepeatVisitHomePage(profile.UserName, string.Empty, true, DateTime.Now, false);

                        Assert.AreEqual(ColumnsAfterChange, userSetup.CurrentPage.ColumnCount, "Number of columns did not change");

                        // Get the columns to verify the column number are same and each column has the expected width set
                        List<Column> columns = DatabaseHelper.GetList<Column, int>(DatabaseHelper.SubsystemEnum.Column,
                            userSetup.CurrentPage.ID,
                            LinqQueries.CompiledQuery_GetColumnsByPageId);
                        Assert.AreEqual(ColumnsAfterChange, columns.Count, "There are still more columns in database");

                        int[] columnWidths = Page.GetColumnWidths(LayoutType);
                        foreach (Column col in columns)
                            Assert.AreEqual(columnWidths[col.ColumnNo], col.ColumnWidth, "Column width is not as expected for Column No: " + col.ColumnNo);

                        // Ensure the last column does not have any widgets
                        List<WidgetInstance> remainingWidgetsOnLastZone = DatabaseHelper.GetList<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                            lastZone.ID,
                            LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneId);
                        Assert.AreEqual(0, remainingWidgetsOnLastZone.Count, "Widgets are still in the last column. {0}".FormatWith(lastZone.ID));

                        // Now change back to 3 column layout and ensure the last column is added
                        facade.ModifyPageLayout(originalLayoutType);

                        List<Column> originalColumns = DatabaseHelper.GetList<Column, int>(DatabaseHelper.SubsystemEnum.Column,
                            setup.CurrentPage.ID,
                            LinqQueries.CompiledQuery_GetColumnsByPageId);
                        Assert.AreEqual(ColumnsBeforeChange, originalColumns.Count, "There are still more columns in database");

                        // and the column width distribution must have changed as well
                        int[] originalColumnWidths = Page.GetColumnWidths(originalLayoutType);
                        foreach (Column col in originalColumns)
                            Assert.AreEqual(originalColumnWidths[col.ColumnNo], col.ColumnWidth, "Column width is not as expected for Column No: " + col.ColumnNo);
                    }
                })
            );
        }

        [TestMethod]
        public void Layout_Change_Should_Move_Widgets_To_Last_Available_Column()
        {
            const int ColumnsBeforeChange = 3;
            const int ColumnsAfterChange = 2;
            const int LayoutType = 2;

            WorkflowTest.UsingWorkflowRuntime(() =>
                MembershipHelper.UsingNewAnonUser((profile) =>
                    SetupHelper.UsingNewAnonSetup(profile.UserName, (setup) =>
                    {
                        int originalLayoutType = setup.CurrentPage.LayoutType;

                        // Ensure there's widgets on the last column
                        WidgetZone lastZone = DatabaseHelper.GetSingle<WidgetZone, int, int>(DatabaseHelper.SubsystemEnum.WidgetZone,
                            setup.CurrentPage.ID, ColumnsBeforeChange-1,
                            LinqQueries.CompiledQuery_GetWidgetZoneByPageId_ColumnNo);
                        List<WidgetInstance> widgetsOnLastZone = DatabaseHelper.GetList<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                            lastZone.ID,
                            LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneId);
                        Assert.AreNotEqual(0, widgetsOnLastZone.Count, "No widget found on last column to move");

                        // Change to 25%, 75% two column layout
                        WorkflowTest.Run<ModifyPageLayoutWorkflow, ModifyTabLayoutWorkflowRequest, ModifyTabLayoutWorkflowResponse>(
                            new ModifyTabLayoutWorkflowRequest { UserName = profile.UserName, LayoutType = LayoutType }
                            );

                        // Get the page setup again to ensure the number of columns are changed
                        var newSetup = WorkflowTest.Run<UserVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
                            new UserVisitWorkflowRequest { IsAnonymous = true, PageName = "", UserName = profile.UserName }
                            );
                        Assert.AreEqual(ColumnsAfterChange, newSetup.CurrentPage.ColumnCount, "Number of columns did not change");

                        // Get the columns to verify the column number are same and each column has the expected width set
                        List<Column> columns = DatabaseHelper.GetList<Column, int>(DatabaseHelper.SubsystemEnum.Column,
                            newSetup.CurrentPage.ID,
                            LinqQueries.CompiledQuery_GetColumnsByPageId);
                        Assert.AreEqual(ColumnsAfterChange, columns.Count, "There are still more columns in database");

                        int[] columnWidths = Page.GetColumnWidths(LayoutType);
                        foreach (Column col in columns)
                            Assert.AreEqual(columnWidths[col.ColumnNo], col.ColumnWidth, "Column width is not as expected for Column No: " + col.ColumnNo);

                        // Ensure the last column does not have any widgets
                        List<WidgetInstance> remainingWidgetsOnLastZone = DatabaseHelper.GetList<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                            lastZone.ID,
                            LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneId);
                        Assert.AreEqual(0, remainingWidgetsOnLastZone.Count, "Widgets are still in the last column. {0}".FormatWith(lastZone.ID));

                        // Now change back to 3 column layout and ensure the last column is added
                        WorkflowTest.Run<ModifyPageLayoutWorkflow, ModifyTabLayoutWorkflowRequest, ModifyTabLayoutWorkflowResponse>(
                            new ModifyTabLayoutWorkflowRequest { UserName = profile.UserName, LayoutType = originalLayoutType }
                            );
                        List<Column> originalColumns = DatabaseHelper.GetList<Column, int>(DatabaseHelper.SubsystemEnum.Column,
                            setup.CurrentPage.ID,
                            LinqQueries.CompiledQuery_GetColumnsByPageId);
                        Assert.AreEqual(ColumnsBeforeChange, originalColumns.Count, "There are still more columns in database");

                        // and the column width distribution must have changed as well
                        int[] originalColumnWidths = Page.GetColumnWidths(originalLayoutType);
                        foreach (Column col in originalColumns)
                            Assert.AreEqual(originalColumnWidths[col.ColumnNo], col.ColumnWidth, "Column width is not as expected for Column No: " + col.ColumnNo);

                    })
                )
            );
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