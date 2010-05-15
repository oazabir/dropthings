namespace Dropthings.DataAccess.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Dropthings.Util;

    using Moq;
    using OmarALZabir.AspectF;
    using Dropthings.Data;

    internal class RepositoryHelper
    {
        #region Methods

        [DebuggerStepThrough]
        public static void UseDatabase(Action<Mock<IDatabase>> callback)
        {
            Mock<IDatabase> database = new Mock<IDatabase>();
            callback(database);
        }

        [DebuggerStepThrough]
        public static void UseCache(Action<Mock<ICache>> callback)
        {
            Mock<ICache> cacheResolver = new Mock<ICache>();
            callback(cacheResolver);
        }

        [DebuggerStepThrough]
        public static void UseRepository<TRepository>(Action<TRepository, Mock<IDatabase>, Mock<ICache>> callback)
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
        public static void UseRepository<TRepository>(Mock<IDatabase> database, Mock<ICache> cacheResolver, Action<TRepository> callback)
            where TRepository : class
        {
            callback(Activator.CreateInstance(typeof(TRepository), database.Object, cacheResolver.Object) as TRepository);
        }

        #endregion Methods
    }
}