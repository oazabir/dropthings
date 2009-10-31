namespace Dropthings.DataAccess.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess.Repository;
    using Dropthings.Util;

    using Moq;
    using OmarALZabir.AspectF;

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
        public static void UseCache(Action<Mock<ICache>> callback)
        {
            Mock<ICache> cacheResolver = new Mock<ICache>();
            callback(cacheResolver);
        }

        [DebuggerStepThrough]
        public static void UseRepository<TRepository>(Action<TRepository, Mock<IDropthingsDataContext>, Mock<ICache>> callback)
            where TRepository : class
        {
            UseDatabase((database) => 
                UseCache((cache) =>
                    callback(
                        Activator.CreateInstance(typeof(TRepository), database.Object, cache.Object) as TRepository,
                        database,
                        cache)
                    ));
        }

        [DebuggerStepThrough]
        public static void UseRepository<TRepository>(Mock<IDropthingsDataContext> database, Mock<ICache> cacheResolver, Action<TRepository> callback)
            where TRepository : class
        {
            callback(Activator.CreateInstance(typeof(TRepository), database.Object, cacheResolver.Object) as TRepository);
        }

        #endregion Methods
    }
}