using System;
namespace Dropthings.Data
{
    public interface IDatabase : IDisposable
    {
        void AddTo<TEntity>(TEntity entity);
        void Attach<TEntity>(TEntity entity) where TEntity : System.Data.Objects.DataClasses.EntityObject;
        void Delete<TEntity>(TEntity entity) where TEntity : System.Data.Objects.DataClasses.EntityObject;
        TEntity Insert<TEntity>(TEntity entity) where TEntity : System.Data.Objects.DataClasses.EntityObject;
        TEntity Insert<TParent, TEntity>(TParent parent, Action<TParent, TEntity> addChildToParent, TEntity entity)
            where TParent : System.Data.Objects.DataClasses.EntityObject
            where TEntity : System.Data.Objects.DataClasses.EntityObject;
        TEntity Insert<TParent1, TParent2, TEntity>(TParent1 parent1, TParent2 parent2, Action<TParent1, TEntity> addChildToParent1, Action<TParent2, TEntity> addChildToParent2, TEntity entity)
            where TParent1 : System.Data.Objects.DataClasses.EntityObject
            where TParent2 : System.Data.Objects.DataClasses.EntityObject
            where TEntity : System.Data.Objects.DataClasses.EntityObject;
        void InsertList<TEntity>(System.Collections.Generic.IEnumerable<TEntity> entities) where TEntity : System.Data.Objects.DataClasses.EntityObject;
        void InsertList<TParent, TEntity>(TParent parent, Action<TParent, TEntity> addChildToParent, System.Collections.Generic.IEnumerable<TEntity> entities)
            where TParent : System.Data.Objects.DataClasses.EntityObject
            where TEntity : System.Data.Objects.DataClasses.EntityObject;
        void InsertList<TParent1, TParent2, TEntity>(TParent1 parent1, TParent2 parent2, Action<TParent1, TEntity> addChildToParent1, Action<TParent2, TEntity> addChildToParent2, System.Collections.Generic.IEnumerable<TEntity> entities)
            where TParent1 : System.Data.Objects.DataClasses.EntityObject
            where TParent2 : System.Data.Objects.DataClasses.EntityObject
            where TEntity : System.Data.Objects.DataClasses.EntityObject;
        System.Linq.IQueryable<TReturnType> Query<Arg0, Arg1, Arg2, TReturnType>(Func<DropthingsDataContext, Arg0, Arg1, Arg2, System.Linq.IQueryable<TReturnType>> query, Arg0 arg0, Arg1 arg1, Arg2 arg2);
        System.Linq.IQueryable<TReturnType> Query<Arg0, Arg1, TReturnType>(Func<DropthingsDataContext, Arg0, Arg1, System.Linq.IQueryable<TReturnType>> query, Arg0 arg0, Arg1 arg1);
        System.Linq.IQueryable<TReturnType> Query<Arg0, TReturnType>(Func<DropthingsDataContext, Arg0, System.Linq.IQueryable<TReturnType>> query, Arg0 arg0);
        System.Linq.IQueryable<TReturnType> Query<TReturnType>(Func<DropthingsDataContext, System.Linq.IQueryable<TReturnType>> query);
        TEntity Update<TEntity>(TEntity entity) where TEntity : System.Data.Objects.DataClasses.EntityObject;
        void UpdateList<TEntity>(System.Collections.Generic.IEnumerable<TEntity> entities) where TEntity : System.Data.Objects.DataClasses.EntityObject;
    }
}
