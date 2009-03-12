namespace Dropthings.Test.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web.Profile;

    using Dropthings.Test.Helper;
    using Dropthings.Web.Framework;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for TestProfileProvider
    /// </summary>
    [TestClass]
    public class TestProfileProvider
    {
        #region Fields

        private TestContext testContextInstance;

        #endregion Fields

        #region Constructors

        public TestProfileProvider()
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
        public void TestProfile_LastUpdate_Should_Change_Every_30_Mins()
        {
            MembershipHelper.UsingNewAnonUser((profile) =>
            {
                DateTime firstLastActivityDate = profile.LastActivityDate;
                TimeSpan diff = DateTime.Now.ToUniversalTime() - profile.LastActivityDate;
                Assert.IsTrue(diff.TotalMinutes < 1, "LastActivityDate was not set to current date time");

                Thread.Sleep(5000);
                profile.Fullname = "Changed to some new value";
                profile.Save();

                ProfileInfoCollection profiles = ProfileManager.FindProfilesByUserName(ProfileAuthenticationOption.Anonymous, profile.UserName);
                ProfileInfo existingProfile = profiles[profile.UserName];
                Assert.AreEqual(firstLastActivityDate, existingProfile.LastActivityDate, "LastActivityDate should not change in repeated save within short duration");
            });
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