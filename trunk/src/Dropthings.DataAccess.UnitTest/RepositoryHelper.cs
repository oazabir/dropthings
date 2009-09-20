namespace Dropthings.DataAccess.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess.Repository;

    using Moq;

    internal class RepositoryHelper
    {
        #region Methods

        [DebuggerStepThrough]
        public static void UseDatabase(Action<Mock<IDropthingsDataContext>> callback)
        {
            Mock<IDropthingsDataContext> database = new Mock<IDropthingsDataContext>();
            callback(database);
        }

        [DebuggerStepThrough]
        public static void UseRepository<TRepository>(Action<TRepository, Mock<IDropthingsDataContext>> callback)
            where TRepository : class
        {
            UseDatabase((database) => callback(
                Activator.CreateInstance(typeof(TRepository), database.Object) as TRepository,
                database));
        }

        [DebuggerStepThrough]
        public static void UseRepository<TRepository>(Mock<IDropthingsDataContext> database, Action<TRepository> callback)
            where TRepository : class
        {
            callback(Activator.CreateInstance(typeof(TRepository), database.Object) as TRepository);
        }

        #endregion Methods
    }
}